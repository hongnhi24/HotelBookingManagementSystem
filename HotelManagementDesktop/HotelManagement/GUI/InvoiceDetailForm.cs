using HotelManagement.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class InvoiceDetailForm : Form
    {
        private int _bookingId;
        // Biến dùng để giữ giá trị số thực tế, tránh lỗi parse từ Label
        private string _tenNhanVienHienTai; // Thêm biến giữ tên NV
        private string _maNhanVienHienTai;  // Thêm biến giữ mã NV
        private decimal _tongCongCuoiCung = 0;

        public InvoiceDetailForm(int bookingId, string maNV, string tenNV)
        {
            InitializeComponent();
            _bookingId = bookingId;
            _maNhanVienHienTai = maNV;
            _tenNhanVienHienTai = tenNV;
        }

        private void InvoiceDetailForm_Load(object sender, EventArgs e)
        {
            LoadInvoiceDetails();
        }

        private void LoadInvoiceDetails()
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    // 1. TRUY VẤN DỮ LIỆU
                    var info = db.BookingDetails
                                 .Include(bd => bd.Booking.Customer)
                                 .Include(bd => bd.Room.RoomType)
                                 .Include(bd => bd.Booking.Invoices)
                                 .FirstOrDefault(bd => bd.BookingID == _bookingId);

                    if (info == null) return;

                    // 2. HIỂN THỊ THÔNG TIN KHÁCH HÀNG
                    lblMaHD.Text = "INV-" + _bookingId.ToString("D4");
                    lblTenKH.Text = info.Booking?.Customer?.TenKH ?? "N/A";
                    lblSDT.Text = info.Booking?.Customer?.SDT ?? "N/A";
                    lblCCCD.Text = info.Booking?.Customer?.CCCD ?? "N/A";
                    lblGioiTinh.Text = info.Booking?.Customer?.GioiTinh ?? "N/A";
                    lblQuocTich.Text = info.Booking?.Customer?.QuocTich ?? "N/A";
                    lblPhong.Text = info.Room?.RoomName ?? "N/A";
                   
                   
                    lblTenNV.Text = _tenNhanVienHienTai;
                    // Trạng thái từ cột PaymentStatus
                    string status = info.Booking.PaymentStatus ?? "Chưa thanh toán";
                    lblTrangThai.Text = status;
                    lblTrangThai.ForeColor = (status == "Đã thanh toán") ? Color.Green : Color.Red;

                    // 3. HIỂN THỊ CHI TIẾT DỊCH VỤ
                    var dsDichVu = db.BookingServices
                                     .Where(bs => bs.BookingID == _bookingId)
                                     .Select(bs => new {
                                         TenDichVu = bs.Service.ServiceName,
                                         SoLuong = bs.Quantity,
                                         DonGia = bs.Price,
                                         ThanhTien = bs.Quantity * bs.Price
                                     }).ToList();

                    dgvChiTiet.DataSource = dsDichVu;
                    FormatGridView();

                    decimal tongTienDV = dsDichVu.Sum(x => (decimal?)x.ThanhTien) ?? 0;
                    lblTongTienDV.Text = string.Format("{0:N0} VNĐ", tongTienDV);

                    // 4. LOGIC HIỂN THỊ TIỀN 
                    var hoaDonDaLuu = info.Booking.Invoices.FirstOrDefault();
                    if (hoaDonDaLuu != null)
                    {
                        
                        lblNgayLap.Text = hoaDonDaLuu.PaymentDate?.ToString("dd/MM/yyyy HH:mm");
                        decimal tongHoaDon = (decimal)hoaDonDaLuu.TotalAmount;

                        lblTongTienPhong.Text = string.Format("{0:N0} VNĐ", tongHoaDon - tongTienDV);
                        lblTongTien.Text = string.Format("{0:N0} VNĐ", tongHoaDon);
                        _tongCongCuoiCung = tongHoaDon;

                        btnIn.Enabled = false; // Đã thanh toán thì không cho bấm In/Lưu lại
                        btnIn.Text = "Đã lưu HD";
                    }
                    else
                    {
                        // TRƯỜNG HỢP: ĐANG TÍNH TIỀN ĐỂ THANH TOÁN (MỚI)
                        lblNgayLap.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                        // Tính tiền phòng thực tế
                        decimal tongTienPhongTamTinh = TinhTienPhongHienTai(info);

                        // Lấy tiền cọc để trừ ra
                        decimal tienCoc = info.Booking.Deposit != null ? Convert.ToDecimal(info.Booking.Deposit) : 0;

                        // Tổng cộng = Tiền phòng + Tiền dịch vụ - Tiền cọc
                        decimal tongCongTamTinh = tongTienPhongTamTinh + tongTienDV - tienCoc;

                        lblTongTienPhong.Text = string.Format("{0:N0} VNĐ", tongTienPhongTamTinh);
                        lblTongTien.Text = string.Format("{0:N0} VNĐ", tongCongTamTinh);
                        _tongCongCuoiCung = tongCongTamTinh; // Gán vào biến số để lưu DB không bị lỗi format

                        btnIn.Enabled = true;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load chi tiết: " + ex.Message); }
        }

        // Hàm phụ để định dạng bảng
        private void FormatGridView()
        {
            if (dgvChiTiet.Columns["TenDichVu"] != null) dgvChiTiet.Columns["TenDichVu"].HeaderText = "Tên dịch vụ";
            if (dgvChiTiet.Columns["SoLuong"] != null) dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
            if (dgvChiTiet.Columns["DonGia"] != null)
            {
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            }
            if (dgvChiTiet.Columns["ThanhTien"] != null)
            {
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.RowHeadersVisible = false;
        }

 
        private decimal TinhTienPhongHienTai(BookingDetail info)
        {
            if (info == null || info.Booking == null || info.Room == null || info.Room.RoomType == null)
                return 0;

            
            decimal giaTheoDem = Convert.ToDecimal(info.Room.RoomType.Price);
            int soNguoiThucTe = info.SoNguoi ?? 1;
            int maxGuests = info.Room.RoomType.MaxGuests == 0 ? 2 : info.Room.RoomType.MaxGuests;

            decimal tongTienPhong = 0;

            // Thời gian khách ở
            DateTime checkIn = info.Booking.CheckInDate ?? DateTime.Now;
            DateTime checkOut = DateTime.Now;
            TimeSpan thoiGianO = checkOut - checkIn;

            string loaiHinhThue = info.Booking.LoaiHinhThue?.ToLower() ?? "";

            // LUỒNG 1: KHÁCH THUÊ THEO GIỜ
            if (loaiHinhThue.Contains("giờ") || loaiHinhThue.Contains("gio"))
            {
                int soGio = (int)Math.Ceiling(thoiGianO.TotalHours);
                if (soGio <= 0) soGio = 1;

                // Qua đêm -> Tính giá 1 ngày
                if (checkIn.Hour >= 22 || checkIn.Hour <= 4)
                {
                    tongTienPhong = giaTheoDem;
                }
                else
                {
                    // Dưới 5 tiếng -> Tính theo giờ đầu & giờ sau
                    if (soGio <= 5)
                    {
                        decimal giaGioDau = Convert.ToDecimal(info.Room.RoomType.FirstHourPrice ?? 0);
                        decimal giaCacGioSau = Convert.ToDecimal(info.Room.RoomType.NextHourPrice ?? 0);
                        tongTienPhong = soGio == 1 ? giaGioDau : giaGioDau + ((soGio - 1) * giaCacGioSau);
                    }
                    // Lố 5 tiếng -> Tính giá 1 ngày
                    else
                    {
                        tongTienPhong = giaTheoDem;
                    }
                }
            }
            // LUỒNG 2: KHÁCH THUÊ THEO ĐÊM
            else
            {
                int soDem = (checkOut.Date - checkIn.Date).Days;
                if (soDem <= 0) soDem = 1;
                tongTienPhong = giaTheoDem * soDem;
            }

            // PHỤ THU NGƯỜI LỚN
            decimal phuThuNguoiLon = soNguoiThucTe > maxGuests ? (soNguoiThucTe - maxGuests) * 100000 : 0;
            tongTienPhong += phuThuNguoiLon;

            // TRỪ GIẢM GIÁ
            decimal giamGia = info.Discount != null ? Convert.ToDecimal(info.Discount) : 0;
            tongTienPhong -= giamGia;

            return tongTienPhong;
        }

        private void ThanhToanVaLuuHoaDon()
        {
            try
            {
                // BẬT HỘP THOẠI KIỂM TRA DỮ LIỆU
           
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var checkExist = db.Invoices.FirstOrDefault(i => i.BookingID == _bookingId);

                    if (checkExist == null)
                    {
                        Invoice inv = new Invoice();
                        inv.BookingID = _bookingId;
                        inv.PaymentDate = DateTime.Now;
                        inv.TotalAmount = (double)_tongCongCuoiCung;

                      
                        inv.MaNV = _maNhanVienHienTai;

                        db.Invoices.Add(inv);
                    }

                    var booking = db.Bookings.Find(_bookingId);
                    if (booking != null)
                    {
                        booking.Status = "Hoàn thành";
                        booking.PaymentStatus = "Đã thanh toán";
                    }

                    db.SaveChanges();

                    lblTrangThai.Text = "Đã thanh toán";
                    lblTrangThai.ForeColor = Color.Green;
                    btnIn.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi Database");
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            ThanhToanVaLuuHoaDon();

            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
                printDoc.PrintController = new StandardPrintController();

                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDoc;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                }
                MessageBox.Show("Thanh toán và Lưu hóa đơn thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            
                Font fontTieuDe = new Font("Arial", 14, FontStyle.Bold);
                Font fontNormal = new Font("Arial", 10, FontStyle.Regular);
                Font fontDam = new Font("Arial", 10, FontStyle.Bold);

                int y = e.MarginBounds.Top;
                int x = e.MarginBounds.Left;
                int khoangCach = 25;

                // Tiêu đề
                string tieuDe = "HÓA ĐƠN THANH TOÁN";
                SizeF kichThuocTieuDe = e.Graphics.MeasureString(tieuDe, fontTieuDe);
                e.Graphics.DrawString(tieuDe, fontTieuDe, Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width - kichThuocTieuDe.Width) / 2, y);
                y += khoangCach + 10;

                // Thông tin chung
                e.Graphics.DrawString($"Mã HD: {lblMaHD.Text}", fontNormal, Brushes.Black, x, y);
                y += khoangCach;
                e.Graphics.DrawString($"Ngày lập: {lblNgayLap.Text}", fontNormal, Brushes.Black, x, y);
                y += khoangCach;
                e.Graphics.DrawString($"Nhân viên: {lblTenNV.Text}", fontNormal, Brushes.Black, x, y);
                y += khoangCach;
                e.Graphics.DrawString($"Phòng: {lblPhong.Text}", fontNormal, Brushes.Black, x, y);
                y += khoangCach + 10;

                // Thông tin khách hàng
                e.Graphics.DrawString("--- THÔNG TIN KHÁCH HÀNG ---", fontDam, Brushes.Black, x, y);
                y += khoangCach;
                e.Graphics.DrawString($"Tên KH: {lblTenKH.Text}", fontNormal, Brushes.Black, x, y);
                y += khoangCach;
                e.Graphics.DrawString($"SDT: {lblSDT.Text}", fontNormal, Brushes.Black, x, y);
                y += khoangCach + 10;

                // Chi tiết dịch vụ
                e.Graphics.DrawString("--- CHI TIẾT DỊCH VỤ ---", fontDam, Brushes.Black, x, y);
                y += khoangCach;

                // Tiêu đề bảng dịch vụ
                e.Graphics.DrawString("Dịch vụ", fontDam, Brushes.Black, x, y);
                e.Graphics.DrawString("SL", fontDam, Brushes.Black, x + 200, y);
                e.Graphics.DrawString("Đơn giá", fontDam, Brushes.Black, x + 270, y);
                e.Graphics.DrawString("Thành tiền", fontDam, Brushes.Black, x + 400, y);
                y += khoangCach - 5;
                e.Graphics.DrawLine(Pens.Black, x, y, x + 550, y);
                y += 10;

                // Danh sách dịch vụ
                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Đã cập nhật lại tên cột tiếng Việt cho khớp với code mới
                    string dichVu = row.Cells["TenDichVu"].Value?.ToString() ?? "";
                    string soLuong = row.Cells["SoLuong"].Value?.ToString() ?? "";
                    decimal donGia = Convert.ToDecimal(row.Cells["DonGia"].Value);
                    decimal thanhTien = Convert.ToDecimal(row.Cells["ThanhTien"].Value);

                    e.Graphics.DrawString(dichVu, fontNormal, Brushes.Black, x, y);
                    e.Graphics.DrawString(soLuong, fontNormal, Brushes.Black, x + 200, y);
                    e.Graphics.DrawString(donGia.ToString("N0"), fontNormal, Brushes.Black, x + 270, y);
                    e.Graphics.DrawString(thanhTien.ToString("N0"), fontNormal, Brushes.Black, x + 400, y);
                    y += khoangCach;
                }

                y += 10;
                e.Graphics.DrawLine(Pens.Black, x, y, x + 550, y);
                y += khoangCach;

                // Tổng kết tiền
                e.Graphics.DrawString($"Tiền phòng: {lblTongTienPhong.Text}", fontNormal, Brushes.Black, x, y);
                y += khoangCach;
                e.Graphics.DrawString($"Tiền dịch vụ: {lblTongTienDV.Text}", fontNormal, Brushes.Black, x, y);
                y += khoangCach;
                e.Graphics.DrawString($"TỔNG CỘNG: {lblTongTien.Text}", fontDam, Brushes.Black, x, y);
                y += khoangCach + 10;

                // Chữ ký
                e.Graphics.DrawString("Khách hàng", fontNormal, Brushes.Black, x, e.MarginBounds.Bottom - 30);
                e.Graphics.DrawString("(Ký, ghi rõ họ tên)", fontNormal, Brushes.Black, x, e.MarginBounds.Bottom - 15);
                e.Graphics.DrawString("Nhân viên", fontNormal, Brushes.Black, x + 350, e.MarginBounds.Bottom - 30);
                e.Graphics.DrawString("(Ký, ghi rõ họ tên)", fontNormal, Brushes.Black, x + 350, e.MarginBounds.Bottom - 15);
            }
        }
    }

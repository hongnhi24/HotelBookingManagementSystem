using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class FrmDatPhong : Form
    {
        // 1. Biến toàn cục giữ thông tin người đăng nhập
        private string _maNhanVien = "";
        private string _tenNhanVien = "";

        // ========================================================
        // HÀM KHỞI TẠO
        // ========================================================

        public FrmDatPhong(string username)
        {
            InitializeComponent();
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var acc = db.EmployeeAccounts.FirstOrDefault(a => a.Username == username);
                if (acc != null && acc.Employee != null)
                {
                    _maNhanVien = acc.MaNV;
                    _tenNhanVien = acc.Employee.HoTen;
                }
                else { _tenNhanVien = "Admin"; }
            }
            CaiDatBanDau();
        }

        public FrmDatPhong(string maNV, string tenNV)
        {
            InitializeComponent();
            _maNhanVien = maNV;
            _tenNhanVien = tenNV;
            CaiDatBanDau();
        }

        private void CaiDatBanDau()
        {
            dgvDanhSach.EnableHeadersVisualStyles = false;
            dgvDanhSach.CellFormatting += DgvDanhSach_CellFormatting;
            LoadDanhSachDatPhong();
        }

        // ========================================================
        // 1. HÀM LOAD DỮ LIỆU TỪ DATABASE
        // ========================================================
        private void LoadDanhSachDatPhong()
        {
            string tuKhoa = txtTimCCCD.Text.Trim().ToLower();

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var query = db.Bookings.AsQueryable();

                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query = query.Where(b => b.Customer != null &&
                                            (b.Customer.CCCD.StartsWith(tuKhoa) || b.Customer.TenKH.ToLower().Contains(tuKhoa)));
                }

                var rawData = query.Select(b => new
                {
                    b.BookingID,
                    TenKhach = b.Customer.TenKH,
                    CCCD = b.Customer.CCCD,
                    b.CheckInDate,
                    b.CheckOutDate,
                    b.LoaiHinhThue,
                    b.Status,
                    DanhSachPhong = b.BookingDetails.Select(bd => bd.Room.RoomName),
                    DanhSachLoaiPhong = b.BookingDetails.Select(bd => bd.Room.RoomType.TypeName)
                }).ToList();

                var danhSachHienThi = rawData.Select(b => new
                {
                    Mã_Đơn = b.BookingID,
                    Tên_Khách = b.TenKhach,
                    CCCD = b.CCCD,
                    Phòng = string.Join(", ", b.DanhSachPhong),
                    Loại_Phòng = string.Join(", ", b.DanhSachLoaiPhong.Distinct()),
                    Ngày_Giờ_Vào = b.CheckInDate,
                    Ngày_Giờ_Ra = b.CheckOutDate,
                    Loại_Hình = b.LoaiHinhThue,
                    Trạng_Thái = b.Status
                }).ToList();

                dgvDanhSach.DataSource = danhSachHienThi;

                if (dgvDanhSach.Columns.Contains("Ngày_Giờ_Vào"))
                {
                    dgvDanhSach.Columns["Ngày_Giờ_Vào"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvDanhSach.Columns["Ngày_Giờ_Ra"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }

                CauHinhCotChucNang();
                FormatGrid();
            }
        }

        // ========================================================
        // 2. TẠO CÁC NÚT CHỨC NĂNG TRÊN BẢNG
        // ========================================================
        private void CauHinhCotChucNang()
        {
            if (!dgvDanhSach.Columns.Contains("ColDuyetDon"))
            {
                DataGridViewButtonColumn btnDuyet = new DataGridViewButtonColumn();
                btnDuyet.Name = "ColDuyetDon";
                btnDuyet.HeaderText = "Duyệt Web";
                btnDuyet.Text = "Duyệt";
                btnDuyet.UseColumnTextForButtonValue = true;
                btnDuyet.FlatStyle = FlatStyle.Flat;
                dgvDanhSach.Columns.Add(btnDuyet);
            }

            if (!dgvDanhSach.Columns.Contains("ColCheckIn"))
            {
                DataGridViewButtonColumn btnCheckIn = new DataGridViewButtonColumn();
                btnCheckIn.Name = "ColCheckIn";
                btnCheckIn.HeaderText = "Nhận Phòng";
                btnCheckIn.Text = "Check-In";
                btnCheckIn.UseColumnTextForButtonValue = true;
                btnCheckIn.FlatStyle = FlatStyle.Flat;
                dgvDanhSach.Columns.Add(btnCheckIn);
            }
            if (!dgvDanhSach.Columns.Contains("ColCheckOut"))
            {
                DataGridViewButtonColumn btnCheckOut = new DataGridViewButtonColumn();
                btnCheckOut.Name = "ColCheckOut";
                btnCheckOut.HeaderText = "Trả Phòng";
                btnCheckOut.Text = "Check-Out";
                btnCheckOut.UseColumnTextForButtonValue = true;
                btnCheckOut.FlatStyle = FlatStyle.Flat;
                dgvDanhSach.Columns.Add(btnCheckOut);
            }
        }

        // ========================================================
        // 3. TÔ MÀU CHO CÁC NÚT (Chỉ tô màu nút, KHÔNG tô màu hàng)
        // ========================================================
        private void DgvDanhSach_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDanhSach.Columns.Count == 0) return;

            string trangThai = dgvDanhSach.Rows[e.RowIndex].Cells["Trạng_Thái"].Value?.ToString();

            if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColDuyetDon")
            {
                if (trangThai == "Chờ duyệt" || trangThai == "Pending")
                {
                    e.CellStyle.BackColor = Color.MediumSeaGreen;
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Gainsboro;
                    e.CellStyle.ForeColor = Color.DarkGray;
                }
            }
            else if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColCheckIn")
            {
                if (trangThai == "Đã đặt")
                {
                    e.CellStyle.BackColor = Color.DodgerBlue;
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Gainsboro;
                    e.CellStyle.ForeColor = Color.DarkGray;
                }
            }
            else if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColCheckOut")
            {
                if (trangThai == "Đang Thuê")
                {
                    e.CellStyle.BackColor = Color.DarkOrange;
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Gainsboro;
                    e.CellStyle.ForeColor = Color.DarkGray;
                }
            }
        }

        // ========================================================
        // 4. ĐỊNH DẠNG TÊN CỘT VÀ TÔ MÀU HÀNG TRÊN CÙNG (HEADER)
        // ========================================================
        private void FormatGrid()
        {
            // Sửa tên cột dễ nhìn
            if (dgvDanhSach.Columns["Mã_Đơn"] != null) dgvDanhSach.Columns["Mã_Đơn"].HeaderText = "Mã Đơn";
            if (dgvDanhSach.Columns["Tên_Khách"] != null) dgvDanhSach.Columns["Tên_Khách"].HeaderText = "Tên Khách Hàng";
            if (dgvDanhSach.Columns["CCCD"] != null) dgvDanhSach.Columns["CCCD"].HeaderText = "Số CCCD";
            if (dgvDanhSach.Columns["Phòng"] != null) dgvDanhSach.Columns["Phòng"].HeaderText = "Phòng Số";
            if (dgvDanhSach.Columns["Loại_Phòng"] != null) dgvDanhSach.Columns["Loại_Phòng"].HeaderText = "Loại Phòng";
            if (dgvDanhSach.Columns["Ngày_Giờ_Vào"] != null) dgvDanhSach.Columns["Ngày_Giờ_Vào"].HeaderText = "Giờ Vào";
            if (dgvDanhSach.Columns["Ngày_Giờ_Ra"] != null) dgvDanhSach.Columns["Ngày_Giờ_Ra"].HeaderText = "Giờ Ra";
            if (dgvDanhSach.Columns["Loại_Hình"] != null) dgvDanhSach.Columns["Loại_Hình"].HeaderText = "Hình Thức";
            if (dgvDanhSach.Columns["Trạng_Thái"] != null) dgvDanhSach.Columns["Trạng_Thái"].HeaderText = "Trạng Thái";

            // Định dạng kích thước bảng
            dgvDanhSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSach.BackgroundColor = Color.White;
            dgvDanhSach.BorderStyle = BorderStyle.None;
            dgvDanhSach.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // ---- CHỈ TÔ MÀU HÀNG TRÊN CÙNG (HEADER) ----
            dgvDanhSach.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 45, 70); // Màu xanh đen
            dgvDanhSach.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDanhSach.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvDanhSach.ColumnHeadersHeight = 40;

            // Đưa các hàng dữ liệu về màu trắng sạch sẽ
            dgvDanhSach.RowsDefaultCellStyle.BackColor = Color.White;
            dgvDanhSach.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvDanhSach.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245); // Sọc xám cực nhạt cho dễ nhìn dòng

            dgvDanhSach.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            dgvDanhSach.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvDanhSach.RowTemplate.Height = 35;
        }

        // ========================================================
        // 5. CÁC SỰ KIỆN CLICK 
        // ========================================================
        private void btDatPhong_Click(object sender, EventArgs e)
        {
            ttDatPhong fm = new ttDatPhong();
            fm.ShowDialog();
            LoadDanhSachDatPhong();
        }

        private void txtTimCCCD_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachDatPhong();
        }

        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];
            string trangThaiHienTai = row.Cells["Trạng_Thái"].Value?.ToString();

            if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColDuyetDon")
            {
                if (trangThaiHienTai != "Chờ duyệt" && trangThaiHienTai != "Pending") return;

                int bookingId = Convert.ToInt32(row.Cells["Mã_Đơn"].Value);
                string tenKhach = row.Cells["Tên_Khách"].Value.ToString();

                DialogResult rs = MessageBox.Show($"Bạn muốn Duyệt đơn cho khách {tenKhach} không?", "Duyệt Đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    using (HotelManagementEntities db = new HotelManagementEntities())
                    {
                        var phieuDat = db.Bookings.Find(bookingId);
                        if (phieuDat != null)
                        {
                            phieuDat.Status = "Đã đặt";
                            db.SaveChanges();
                            MessageBox.Show("Duyệt đơn thành công!");
                            LoadDanhSachDatPhong();
                        }
                    }
                }
            }
            else if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColCheckIn")
            {
                if (trangThaiHienTai != "Đã đặt") return;

                int bookingId = Convert.ToInt32(row.Cells["Mã_Đơn"].Value);
                string tenKhach = row.Cells["Tên_Khách"].Value.ToString();

                DialogResult rs = MessageBox.Show($"Xác nhận Giao phòng cho khách {tenKhach}?", "Check-in", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    using (HotelManagementEntities db = new HotelManagementEntities())
                    {
                        var phieuDat = db.Bookings.Find(bookingId);
                        if (phieuDat != null)
                        {
                            phieuDat.Status = "Đang Thuê";
                            phieuDat.CheckInDate = DateTime.Now;
                            db.SaveChanges();
                            MessageBox.Show("Check-in thành công!");
                            LoadDanhSachDatPhong();
                        }
                    }
                }
            }
            else if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColCheckOut")
            {
                if (trangThaiHienTai != "Đang Thuê") return;

                int bookingId = Convert.ToInt32(row.Cells["Mã_Đơn"].Value);
                string maNV = _maNhanVien;
                string tenNV = _tenNhanVien;

                InvoiceDetailForm frmThanhToan = new InvoiceDetailForm(bookingId, maNV, tenNV);
                frmThanhToan.ShowDialog();

                LoadDanhSachDatPhong();
            }
        }
    }
}
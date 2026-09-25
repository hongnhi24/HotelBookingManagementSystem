using HotelManagement.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class InvoiceForm : Form
    {
        private string _maNVHienTai = "";
        private string _tenNVHienTai = "";
        public InvoiceForm(string usernameHienTai)
        {
            InitializeComponent();
            // 3. Tự động tìm Mã NV và Tên NV dựa vào username
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var acc = db.EmployeeAccounts.FirstOrDefault(a => a.Username == usernameHienTai);
                if (acc != null && acc.Employee != null)
                {
                    _maNVHienTai = acc.MaNV;

                   
                    _tenNVHienTai = acc.Employee.HoTen;
                }
                else
                {
                    _tenNVHienTai = "Admin/Quản lý"; // Đề phòng trường hợp đăng nhập bằng tk không có trong bảng NhanVien
                }
            }
            this.Load += InvoiceForm_Load;

            // --- ĐĂNG KÝ SỰ KIỆN THỜI GIAN THỰC ---
            // 1. Nhập chữ đến đâu lọc đến đó
            txtTimKiem.TextChanged += (s, e) => LoadInvoices();

            // 2. Ấn tick lọc theo ngày là load lại luôn
            chkLocNgay.CheckedChanged += (s, e) => {
                // Mở/Khóa dtpNgay tùy theo trạng thái tick
                dtpNgay.Enabled = chkLocNgay.Checked;
                LoadInvoices();
            };

            // 3. Thay đổi ngày trên DateTimePicker cũng load lại luôn
            dtpNgay.ValueChanged += (s, e) => {
                if (chkLocNgay.Checked) LoadInvoices();
            };
        }

        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            LoadComboSearch();
            dtpNgay.Enabled = false; // Mặc định chưa tick thì khóa chọn ngày
            LoadInvoices();
        }

        private void LoadComboSearch()
        {
            cboLoc.Items.Clear();
            cboLoc.Items.Add("Mã hóa đơn");
            cboLoc.Items.Add("Tên khách hàng");
            cboLoc.SelectedIndex = 0;
        }

        private void LoadInvoices()
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    string searchKey = txtTimKiem.Text.Trim().ToLower();
                    string selectedFilter = cboLoc.SelectedItem?.ToString();

                    var query = db.Invoices
                                  .Include(inv => inv.Booking)
                                  .Include(inv => inv.Booking.Customer)
                                  .Include("Booking.BookingDetails.Room")
                                  .AsQueryable();

                    if (chkLocNgay.Checked)
                    {
                        DateTime targetDate = dtpNgay.Value.Date;
                        query = query.Where(inv => DbFunctions.TruncateTime(inv.PaymentDate) == targetDate);
                    }

                    if (!string.IsNullOrEmpty(searchKey))
                    {
                        if (selectedFilter == "Mã hóa đơn")
                            query = query.Where(inv => inv.InvoiceID.ToString().Contains(searchKey));
                        else if (selectedFilter == "Tên khách hàng")
                            query = query.Where(inv => inv.Booking.Customer.TenKH.ToLower().Contains(searchKey));
                    }

                   // Trong hàm LoadInvoices của InvoiceForm
var dsHoaDon = query.OrderByDescending(inv => inv.InvoiceID) // Sắp xếp mã hóa đơn mới nhất lên đầu
                    .ToList()
                    .Select(inv => new
                    {
                        inv.InvoiceID,
                        NgayLap = inv.PaymentDate,
                        // Truy vấn lấy tên khách từ bảng liên kết
                        TenKH = inv.Booking?.Customer?.TenKH ?? "N/A",
                        RoomName = inv.Booking?.BookingDetails?.FirstOrDefault()?.Room?.RoomName ?? "N/A",
                        inv.TotalAmount,
                        // Hiển thị trạng thái thanh toán
                        TrangThai = inv.Booking?.PaymentStatus ?? "Đã thanh toán",
                        ChiTiet = "📋 Xem chi tiết"
                    }).ToList();

dgvHoaDon.DataSource = dsHoaDon;
                    FormatGrid();
                }
            }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); }
        }

        private void FormatGrid()
        {
            
            if (dgvHoaDon.Columns["InvoiceID"] != null) dgvHoaDon.Columns["InvoiceID"].HeaderText = "Mã HĐ";
            if (dgvHoaDon.Columns["NgayLap"] != null) dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
            if (dgvHoaDon.Columns["TenKH"] != null) dgvHoaDon.Columns["TenKH"].HeaderText = "Khách Hàng";
            if (dgvHoaDon.Columns["RoomName"] != null) dgvHoaDon.Columns["RoomName"].HeaderText = "Phòng";
            if (dgvHoaDon.Columns["TrangThai"] != null) dgvHoaDon.Columns["TrangThai"].HeaderText = "Trạng Thái";
            if (dgvHoaDon.Columns["ChiTiet"] != null) dgvHoaDon.Columns["ChiTiet"].HeaderText = "Thao Tác";

            if (dgvHoaDon.Columns["TotalAmount"] != null)
            {
                dgvHoaDon.Columns["TotalAmount"].HeaderText = "Tổng Tiền (VNĐ)";
                dgvHoaDon.Columns["TotalAmount"].DefaultCellStyle.Format = "N0"; // Có dấu phẩy phân cách hàng nghìn
                dgvHoaDon.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Căn phải cho cột tiền
            }

            // ======================================================
            // CHỈNH MÀU SẮC ĐỒNG BỘ VỚI FORM MAIN
            // ======================================================
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.BackgroundColor = Color.White;
            dgvHoaDon.BorderStyle = BorderStyle.None; // Xóa viền xám mặc định
            dgvHoaDon.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            //  Màu của Thanh Tiêu Đề (Cùng tông Xanh Đen)
            dgvHoaDon.EnableHeadersVisualStyles = false; 
            dgvHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 45, 70);
            dgvHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvHoaDon.ColumnHeadersHeight = 40;

            // Màu xen kẽ của các dòng 
            dgvHoaDon.RowsDefaultCellStyle.BackColor = Color.White;
            dgvHoaDon.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 250); 

            // Màu khi Click chọn 1 dòng
            dgvHoaDon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185); 
            dgvHoaDon.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvHoaDon.RowTemplate.Height = 35;
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvHoaDon.Columns[e.ColumnIndex].Name == "ChiTiet")
            {
                int currentInvoiceId = Convert.ToInt32(dgvHoaDon.Rows[e.RowIndex].Cells["InvoiceID"].Value);
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var hoaDon = db.Invoices.FirstOrDefault(inv => inv.InvoiceID == currentInvoiceId);
                    if (hoaDon?.BookingID != null)
                    {
                        
                        InvoiceDetailForm detailForm = new InvoiceDetailForm(hoaDon.BookingID.Value, _maNVHienTai, _tenNVHienTai);
                        detailForm.ShowDialog();
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            chkLocNgay.Checked = false;
            cboLoc.SelectedIndex = 0;
            LoadInvoices();
        }
    }
}
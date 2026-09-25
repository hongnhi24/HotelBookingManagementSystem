using HotelManagement.Models;
using System;
using System.Data.Entity;
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
    public partial class FrmChiTietPhong : Form
    {
        private int _bookingId;
        private string _maNhanVien;
        private string _tenNhanVien;

       
        public FrmChiTietPhong(int bookingId, string maNhanVien, string tenNhanVien)
        {
            InitializeComponent();
            _bookingId = bookingId;
            _maNhanVien = maNhanVien;
            _tenNhanVien = tenNhanVien;
        }

        private void FrmChiTietPhong_Load(object sender, EventArgs e)
        {
            LoadThongTinKhach();
            LoadDichVuDaDung();
        }

        // 1. Load thông tin khách hàng và phòng
        private void LoadThongTinKhach()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var info = db.BookingDetails
                             .Include(bd => bd.Booking.Customer)
                             .Include(bd => bd.Room)
                             .FirstOrDefault(bd => bd.BookingID == _bookingId);

                if (info != null)
                {
                    lblPhong.Text = info.Room.RoomName;
                    lblTenKhach.Text = info.Booking.Customer.TenKH;
                    lblNgayDen.Text = info.Booking.CheckInDate?.ToString("dd/MM/yyyy HH:mm");

                    // Tính số ngày ở tạm tính
                    var soNgay = (DateTime.Now - info.Booking.CheckInDate.Value).Days;
                    lblSoNgay.Text = (soNgay == 0 ? 1 : soNgay).ToString() + " ngày";
                    //số người
                    lblSoNguoi.Text = info.SoNguoi.ToString() + " người";
                }
            }
        }

        // 2. Load danh sách dịch vụ khách đã gọi
        public void LoadDichVuDaDung()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var ds = db.BookingServices
                           .Where(bs => bs.BookingID == _bookingId)
                           .Select(bs => new
                           {
                               bs.Service.ServiceName,
                               bs.Quantity,
                               ThanhTien = bs.Quantity * bs.Price
                           }).ToList();

                dgvDichVuLuu.DataSource = ds;

                if (dgvDichVuLuu.Columns["ServiceName"] != null)
                    dgvDichVuLuu.Columns["ServiceName"].HeaderText = "Dịch vụ";

                if (dgvDichVuLuu.Columns["Quantity"] != null)
                    dgvDichVuLuu.Columns["Quantity"].HeaderText = "Số lượng";

                if (dgvDichVuLuu.Columns["ThanhTien"] != null)
                {
                    dgvDichVuLuu.Columns["ThanhTien"].HeaderText = "Thành tiền";
                    dgvDichVuLuu.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                }

                dgvDichVuLuu.RowHeadersVisible = false;
                dgvDichVuLuu.AllowUserToAddRows = false;
                dgvDichVuLuu.ReadOnly = true;
                dgvDichVuLuu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // 3. NÚT THÊM DỊCH VỤ
        private void btnThemDV_Click(object sender, EventArgs e)
        {
            FrmThemDichVu frm = new FrmThemDichVu(_bookingId);
            frm.ShowDialog();
            LoadDichVuDaDung();
        }

        // 4. NÚT THANH TOÁN (Check-out)
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            // Truyền tiếp 3 thông tin sang Form in hóa đơn
            InvoiceDetailForm frm = new InvoiceDetailForm(_bookingId, _maNhanVien, _tenNhanVien);
            frm.ShowDialog();
        }
    }
}
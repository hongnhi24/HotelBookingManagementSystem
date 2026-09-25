using HotelManagement.Models; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class FrmThemDichVu : Form
    {
        private int _bookingId; // Biến lưu mã Đơn đặt phòng

        public FrmThemDichVu(int bookingId)
        {
            InitializeComponent();
            _bookingId = bookingId;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // ========================================================
        // 1. SỰ KIỆN LOAD FORM
        // ========================================================
        private void FrmThemDichVu_Load(object sender, EventArgs e)
        {
            CauHinhGiaoDienBang();
            LoadLoaiDichVu();
            LoadDanhSachDichVuBenTrai();
            LoadDichVuKhachDaGoi();
        }

        // ========================================================
        // 2. CẤU HÌNH CÁC CỘT CHO 2 BẢNG 
        // ========================================================
        private void CauHinhGiaoDienBang()
        {
            //CẤU HÌNH BẢNG TRÁI 
            dgvDsDichVu.Columns.Clear();
            dgvDsDichVu.AllowUserToAddRows = false;
            dgvDsDichVu.ReadOnly = true;
            dgvDsDichVu.RowHeadersVisible = false;

            dgvDsDichVu.Columns.Add("ServiceId", "Mã DV"); 
            dgvDsDichVu.Columns["ServiceId"].Visible = false;

            dgvDsDichVu.Columns.Add("Category", "Loại DV");
            dgvDsDichVu.Columns.Add("ServiceName", "Tên dịch vụ");
            dgvDsDichVu.Columns.Add("Unit", "Đơn vị");
            dgvDsDichVu.Columns.Add("Price", "Giá");
            dgvDsDichVu.Columns["Price"].DefaultCellStyle.Format = "N0";

            DataGridViewButtonColumn btnThem = new DataGridViewButtonColumn();
            btnThem.Name = "ColThem";
            btnThem.HeaderText = "Thêm";
            btnThem.Text = "+";
            btnThem.UseColumnTextForButtonValue = true;
            btnThem.DefaultCellStyle.BackColor = Color.ForestGreen;
            btnThem.DefaultCellStyle.ForeColor = Color.White;
            btnThem.FlatStyle = FlatStyle.Flat;
            dgvDsDichVu.Columns.Add(btnThem);

            //  CẤU HÌNH BẢNG PHẢI
            dgvDsDaChon.Columns.Clear();
            dgvDsDaChon.AllowUserToAddRows = false;
            dgvDsDaChon.RowHeadersVisible = false;

            dgvDsDaChon.Columns.Add("ServiceId", "Mã DV");
            dgvDsDaChon.Columns["ServiceId"].Visible = false;

            dgvDsDaChon.Columns.Add("ServiceName", "Dịch vụ");
            dgvDsDaChon.Columns["ServiceName"].ReadOnly = true;

            dgvDsDaChon.Columns.Add("Unit", "Đơn vị"); 
            dgvDsDaChon.Columns["Unit"].ReadOnly = true;

            dgvDsDaChon.Columns.Add("Price", "Đơn giá");
            dgvDsDaChon.Columns["Price"].Visible = false;

            DataGridViewTextBoxColumn colSoLuong = new DataGridViewTextBoxColumn();
            colSoLuong.Name = "Quantity";
            colSoLuong.HeaderText = "Số Lượng";
            colSoLuong.DefaultCellStyle.BackColor = Color.LightYellow;
            dgvDsDaChon.Columns.Add(colSoLuong);

            dgvDsDaChon.Columns.Add("Total", "Thành tiền");
            dgvDsDaChon.Columns["Total"].ReadOnly = true;
            dgvDsDaChon.Columns["Total"].DefaultCellStyle.Format = "N0";

            DataGridViewButtonColumn btnXoa = new DataGridViewButtonColumn();
            btnXoa.Name = "ColXoa";
            btnXoa.HeaderText = "Xóa";
            btnXoa.Text = "X";
            btnXoa.UseColumnTextForButtonValue = true;
            btnXoa.DefaultCellStyle.BackColor = Color.Tomato;
            btnXoa.DefaultCellStyle.ForeColor = Color.White;
            btnXoa.FlatStyle = FlatStyle.Flat;
            dgvDsDaChon.Columns.Add(btnXoa);
        }

        // ========================================================
        // 3. LOAD DỮ LIỆU
        // ========================================================
        private void LoadLoaiDichVu()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                // Vì không có bảng loại riêng, ta gộp nhóm (Distinct) trực tiếp từ cột Category
                var categories = db.Services
                                   .Where(s => s.Category != null && s.Category != "") // Bỏ qua null
                                   .Select(s => s.Category)
                                   .Distinct()
                                   .ToList();

                categories.Insert(0, "Tất cả"); // Thêm dòng "Tất cả" lên đầu

             
                cbbLoaiDichVu.DataSource = categories;
            }
        }

        private void LoadDanhSachDichVuBenTrai()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var query = db.Services.AsQueryable();

                // Lọc theo ComboBox (kiểu chuỗi)
                if (cbbLoaiDichVu.SelectedItem != null && cbbLoaiDichVu.SelectedItem.ToString() != "Tất cả")
                {
                    string selectedCategory = cbbLoaiDichVu.SelectedItem.ToString();
                    query = query.Where(s => s.Category == selectedCategory);
                }

                string tuKhoa = txtTimKiem.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query = query.Where(s => s.ServiceName.ToLower().Contains(tuKhoa));
                }

                var dsDV = query.Select(s => new
                {
                    s.ServiceId,
                    s.Category,
                    s.ServiceName,
                    s.Unit, 
                    s.Price
                }).ToList();

                dgvDsDichVu.Rows.Clear();
                foreach (var item in dsDV)
                {
                    // Đưa dữ liệu lên theo đúng thứ tự cấu hình cột
                    dgvDsDichVu.Rows.Add(item.ServiceId, item.Category, item.ServiceName, item.Unit, item.Price);
                }
            }
        }

        private void LoadDichVuKhachDaGoi()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var dsDaGoi = db.BookingServices.Where(bs => bs.BookingID == _bookingId).ToList();
                dgvDsDaChon.Rows.Clear();
                foreach (var item in dsDaGoi)
                {
                    dgvDsDaChon.Rows.Add(
                        item.ServiceId,
                        item.Service.ServiceName,
                        item.Service.Unit, // Kéo cột Unit từ bảng Service
                        item.Price,
                        item.Quantity,
                        item.Price * item.Quantity
                    );
                }
            }
        }

        // ========================================================
        // 4. CHỨC NĂNG TÌM KIẾM & LỌC
        // ========================================================
        private void cbbLoaiDichVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachDichVuBenTrai();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachDichVuBenTrai();
        }

        // ========================================================
        // 5. THAO TÁC TRÊN BẢNG 
        // ========================================================
        private void dgvDsDichVu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDsDichVu.Columns[e.ColumnIndex].Name == "ColThem")
            {
                DataGridViewRow rowTrai = dgvDsDichVu.Rows[e.RowIndex];
                int idDV = Convert.ToInt32(rowTrai.Cells["ServiceId"].Value);
                string tenDV = rowTrai.Cells["ServiceName"].Value?.ToString();
                string donVi = rowTrai.Cells["Unit"].Value?.ToString(); // Đọc thêm Đơn vị
                decimal gia = Convert.ToDecimal(rowTrai.Cells["Price"].Value);

                bool daCo = false;
                foreach (DataGridViewRow rowPhai in dgvDsDaChon.Rows)
                {
                    if (Convert.ToInt32(rowPhai.Cells["ServiceId"].Value) == idDV)
                    {
                        int soLuongCu = Convert.ToInt32(rowPhai.Cells["Quantity"].Value);
                        rowPhai.Cells["Quantity"].Value = soLuongCu + 1;
                        rowPhai.Cells["Total"].Value = gia * (soLuongCu + 1);
                        daCo = true;
                        break;
                    }
                }

                if (!daCo)
                {
                   
                    dgvDsDaChon.Rows.Add(idDV, tenDV, donVi, gia, 1, gia * 1);
                }
            }
        }

        private void dgvDsDaChon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDsDaChon.Columns[e.ColumnIndex].Name == "ColXoa")
            {
                dgvDsDaChon.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void dgvDsDaChon_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDsDaChon.Columns[e.ColumnIndex].Name == "Quantity")
            {
                DataGridViewRow row = dgvDsDaChon.Rows[e.RowIndex];
                if (row.Cells["Quantity"].Value != null && row.Cells["Price"].Value != null)
                {
                    if (int.TryParse(row.Cells["Quantity"].Value.ToString(), out int soLuongMoi))
                    {
                        if (soLuongMoi <= 0)
                        {
                            this.BeginInvoke(new MethodInvoker(() =>
                            {
                                if (e.RowIndex < dgvDsDaChon.Rows.Count)
                                {
                                    dgvDsDaChon.Rows.RemoveAt(e.RowIndex);
                                }
                            }));
                            return;
                        }

                        decimal gia = Convert.ToDecimal(row.Cells["Price"].Value);
                        row.Cells["Total"].Value = soLuongMoi * gia;
                    }
                }
            }
        }

        // ========================================================
        // 6. LƯU VÀO DATABASE VÀ THOÁT
        // ========================================================
        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Xóa các dịch vụ cũ
                        var dichVuCu = db.BookingServices.Where(bs => bs.BookingID == _bookingId).ToList();
                        db.BookingServices.RemoveRange(dichVuCu);
                        db.SaveChanges();

                        // 2. Nạp dịch vụ mới
                        foreach (DataGridViewRow row in dgvDsDaChon.Rows)
                        {
                            BookingService bs = new BookingService();
                            bs.BookingID = _bookingId;
                            bs.ServiceId = Convert.ToInt32(row.Cells["ServiceId"].Value); 
                            bs.Quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                            bs.Price = Convert.ToDecimal(row.Cells["Price"].Value);

                            db.BookingServices.Add(bs);
                        }

                        db.SaveChanges();
                        transaction.Commit();
                        MessageBox.Show("Cập nhật dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
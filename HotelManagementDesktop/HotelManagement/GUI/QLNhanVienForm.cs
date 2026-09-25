using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Panel = System.Windows.Forms.Panel;

namespace HotelManagement.GUI
{
    public partial class QLNhanVienForm : Form
    {
        // Biến lưu trữ đường dẫn ảnh khi chọn từ máy tính
        private string duongDanAnhHienTai = "";
        HotelManagementEntities db = new HotelManagementEntities();

        // Biến dùng chung để chứa danh sách lương vừa tính
        List<BangLuong> danhSachLuongThangNay = new List<BangLuong>();

        public QLNhanVienForm()
        {
            InitializeComponent();
        }

        // ========================================================
        // HÀM BỔ TRỢ: XỬ LÝ ẢNH
        // ========================================================
        private byte[] ChuyenAnhThanhByte(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;
            return File.ReadAllBytes(filePath);
        }

       
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Hình ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Chọn ảnh đại diện cho nhân viên";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    duongDanAnhHienTai = ofd.FileName;

                    // 1. Ép PictureBox tự động thu nhỏ ảnh cho vừa vặn với khung hình
                    picNhanVien.SizeMode = PictureBoxSizeMode.StretchImage;

                    // 2. Dùng FileStream để đọc ảnh lên. 
                  
                    using (FileStream fs = new FileStream(duongDanAnhHienTai, FileMode.Open, FileAccess.Read))
                    {
                        // Thay thế ảnh cũ (nếu có) bằng ảnh mới
                        if (picNhanVien.Image != null)
                        {
                            picNhanVien.Image.Dispose();
                        }
                        picNhanVien.Image = Image.FromStream(fs);
                    }
                }
            }
        }

        // ========================================================
        // GIAO DIỆN VÀ TAB
        // ========================================================
        private void ChuyenTab(Panel pnlCanHienThi)
        {
            pnlQLHoSo.Visible = false;
            pnlTaiKhoan.Visible = false;
            pnlBangLuong.Visible = false;

            pnlCanHienThi.Visible = true;
            pnlCanHienThi.BringToFront();
        }

        private void DoiTenCot()
        {
            if (dataGridView1.Columns["MaNV"] != null)
            {
                dataGridView1.Columns["MaNV"].HeaderText = "Mã NV";
                dataGridView1.Columns["HoTen"].HeaderText = "Họ và Tên";
                dataGridView1.Columns["GioiTinh"].HeaderText = "Giới tính";
                dataGridView1.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dataGridView1.Columns["ChucVu"].HeaderText = "Chức vụ";
                dataGridView1.Columns["DiaChi"].HeaderText = "Địa chỉ";
                dataGridView1.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                if (dataGridView1.Columns["TrangThai"] != null)
                {
                    dataGridView1.Columns["TrangThai"].Visible = false;
                }
            }
        }

        private void LoadDanhSachChucVu()
        {
            using (var db = new HotelManagementEntities())
            {
                var dsChucVu = db.Roles.ToList();
                comboBoxChucVu.DataSource = dsChucVu;
                comboBoxChucVu.DisplayMember = "TenChucVu";
                comboBoxChucVu.ValueMember = "MaChucVu";
                comboBoxChucVu.SelectedIndex = -1;
            }
        }

        private void HienThiDanhSach()
        {
            string keyword = txtTimKiem.Text.Trim();
            bool hienCaNguoiNghi = chkHienNhanVienNghi.Checked;

            using (var db = new HotelManagementEntities())
            {
                var query = db.Employees.AsQueryable();

                if (hienCaNguoiNghi == false)
                {
                    query = query.Where(nv => nv.TrangThai == true);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(nv => nv.MaNV.Contains(keyword) || nv.HoTen.Contains(keyword));
                }

                var ketQua = query.Select(nv => new
                {
                    MaNV = nv.MaNV,
                    HoTen = nv.HoTen,
                    GioiTinh = nv.GioiTinh,
                    SoDienThoai = nv.SoDienThoai,
                    ChucVu = nv.Role.TenChucVu,
                    CCCD = nv.CCCD,
                    DiaChi = nv.DiaChi,
                    NgaySinh = nv.NgaySinh,
                    TrangThai = nv.TrangThai
                }).ToList();

                dataGridView1.DataSource = ketQua;
                DoiTenCot();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["TrangThai"].Value != null && (bool)row.Cells["TrangThai"].Value == false)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Strikeout);
                    }
                }
            }
        }

        private void LoadDanhSachNhanVien()
        {
            HienThiDanhSach();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            HienThiDanhSach();
        }

        private void chkHienNhanVienNghi_CheckedChanged(object sender, EventArgs e)
        {
            txtTimKiem_TextChanged(null, null);
        }

        private void DinhDangLuoi(DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // ========================================================
            
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 45, 70);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;

            dgv.RowTemplate.Height = 35;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F);

            // Chữ dữ liệu màu đen rõ nét, dễ nhìn
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.RowsDefaultCellStyle.ForeColor = Color.Black;

            // Chỉ dùng màu xanh dương sáng làm điểm nhấn KHI CLICK CHỌN DÒNG
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // Kẻ sọc ngựa vằn nhạt thanh lịch cho các hàng phía dưới
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 250);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // Màu Xanh Đen chuẩn hệ thống cho Thanh Tiêu Đề
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 45, 70);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;

           
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 45, 70);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        }
        private void QLNhanVienForm_Load(object sender, EventArgs e)
        {
            quảnLýHồSơToolStripMenuItem.PerformClick();

        
            DinhDangLuoi(dataGridView1);
            DinhDangLuoi(dataGridViewTaiKhoan);
            DinhDangLuoi(dgvBangLuong);

            chckTrangThai.Checked = true;
            LoadDanhSachChucVu();
            LoadDanhSachNhanVien();
        }

        private void quảnLýHồSơToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChuyenTab(pnlQLHoSo);
        }

        // ========================================================
        // XỬ LÝ CLICK DÒNG 
        // ========================================================
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string maNV = dataGridView1.Rows[e.RowIndex].Cells["MaNV"].Value.ToString();
            using (var db = new HotelManagementEntities())
            {
                var nhanVien = db.Employees.FirstOrDefault(nv => nv.MaNV == maNV);

                if (nhanVien != null)
                {
                    txtMaNV.Text = nhanVien.MaNV;
                    txtHoTen.Text = nhanVien.HoTen;
                    txtSdt.Text = nhanVien.SoDienThoai;
                    txtDiaChi.Text = nhanVien.DiaChi;
                    txtCCCD.Text = nhanVien.CCCD;

                    dateTimePicker.Value = nhanVien.NgaySinh.Date;
                    comboBoxChucVu.SelectedValue = nhanVien.MaChucVu;

                    chckTrangThai.Checked = nhanVien.TrangThai ?? true;

                    if (nhanVien.GioiTinh == "Nam")
                    {
                        radNam.Checked = true;
                    }
                    else if (nhanVien.GioiTinh == "Nữ")
                    {
                        radNu.Checked = true;
                    }
                    else
                    {
                        radNam.Checked = false;
                        radNu.Checked = false;
                    }

                    // --- LOAD ẢNH NHÂN VIÊN ---
                    duongDanAnhHienTai = ""; 
                    if (nhanVien.HinhAnh != null)
                    {
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(nhanVien.HinhAnh))
                            {
                                picNhanVien.Image = Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            picNhanVien.Image = null;
                        }
                    }
                    else
                    {
                        picNhanVien.Image = null;
                    }
                }
            }
        }

        // ========================================================
        // CHỨC NĂNG THÊM MỚI NHÂN VIÊN (CÓ LƯU ẢNH)
        // ========================================================
        private void btThem_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();

            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Chưa nhập mã nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var employees = new HotelManagementEntities())
            {
                bool daTonTai = employees.Employees.Any(x => x.MaNV == maNV);
                if (daTonTai)
                {
                    MessageBox.Show("Mã nhân viên này đã tồn tại. Vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNV.Focus();
                    return;
                }

                Employee em = new Employee();
                em.MaNV = maNV;
                em.HoTen = txtHoTen.Text.Trim();
                em.CCCD = txtCCCD.Text.Trim();
                em.SoDienThoai = txtSdt.Text.Trim();
                em.DiaChi = txtDiaChi.Text.Trim();
                em.MaChucVu = (int)comboBoxChucVu.SelectedValue;
                em.NgaySinh = dateTimePicker.Value;
                em.NgayVaoLam = DateTime.Now;
                em.TrangThai = true;
                em.GioiTinh = radNam.Checked ? "Nam" : "Nữ";

                // --- XỬ LÝ ẢNH ---
                if (!string.IsNullOrEmpty(duongDanAnhHienTai))
                {
                    em.HinhAnh = ChuyenAnhThanhByte(duongDanAnhHienTai);
                }

                try
                {
                    employees.Employees.Add(em);
                    employees.SaveChanges();

                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTimKiem_TextChanged(null, null);
                    ClearForm();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string chiTietLoi = "";
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            chiTietLoi += $"- Cột [{validationError.PropertyName}]: {validationError.ErrorMessage}\n";
                        }
                    }
                    MessageBox.Show("Lỗi ràng buộc dữ liệu:\n\n" + chiTietLoi, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    string loiThucSu = ex.Message;
                    if (ex.InnerException != null)
                    {
                        loiThucSu += "\n\nNguyên nhân gốc: " + ex.InnerException.Message;
                        if (ex.InnerException.InnerException != null)
                        {
                            loiThucSu += "\n\nChi tiết SQL: " + ex.InnerException.InnerException.Message;
                        }
                    }
                    MessageBox.Show("Lỗi hệ thống:\n" + loiThucSu, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ========================================================
        // CHỨC NĂNG SỬA NHÂN VIÊN
        // ========================================================
        private void btSua_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();

            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên từ danh sách để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new HotelManagementEntities())
            {
                var em = db.Employees.FirstOrDefault(x => x.MaNV == maNV);

                if (em == null)
                {
                    MessageBox.Show("Không tìm thấy nhân viên này trong CSDL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                em.HoTen = txtHoTen.Text.Trim();
                em.CCCD = txtCCCD.Text.Trim();
                em.SoDienThoai = txtSdt.Text.Trim();
                em.DiaChi = txtDiaChi.Text.Trim();
                em.NgaySinh = dateTimePicker.Value;
                em.MaChucVu = (int)comboBoxChucVu.SelectedValue;
                em.TrangThai = chckTrangThai.Checked;
                em.GioiTinh = radNam.Checked ? "Nam" : "Nữ";

                // --- XỬ LÝ CẬP NHẬT ẢNH ---
                if (!string.IsNullOrEmpty(duongDanAnhHienTai))
                {
                    em.HinhAnh = ChuyenAnhThanhByte(duongDanAnhHienTai);
                }

                db.SaveChanges();

                MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiem_TextChanged(null, null);
                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtTimKiem.Text = "";
            txtMaNV.Text = "";
            txtHoTen.Text = "";
            txtSdt.Text = "";
            txtDiaChi.Text = "";
            txtCCCD.Text = "";
            dateTimePicker.Value = DateTime.Now;
            comboBoxChucVu.SelectedIndex = -1;
            radNam.Checked = true;
            radNu.Checked = false;
            chckTrangThai.Checked = true;

            // Dọn dẹp cả ảnh
            duongDanAnhHienTai = "";
            if (picNhanVien != null) picNhanVien.Image = null;
        }

        private void btLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        //======================TÀI KHOẢN VÀ PHÂN QUYỀN=========================
        private string maNvDangChon = "";

        private void LoadNhanVienChuaCoTaiKhoan()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var nguoiCoTK = db.EmployeeAccounts.Select(a => a.MaNV).ToList();

                var dsNhanVien = db.Employees
            .Where(nv => nv.TrangThai == true)
            .Where(nv => nv.MaChucVu == 1 || nv.MaChucVu == 2)
            .Where(nv => !nguoiCoTK.Contains(nv.MaNV))
            .Select(nv => new
            {
                MaNV = nv.MaNV,
                ThongTin = nv.MaNV + " - " + nv.HoTen
            }).ToList();
                comboBoxNhanVien.DataSource = null;

                comboBoxNhanVien.DataSource = dsNhanVien;
                comboBoxNhanVien.DisplayMember = "ThongTin";
                comboBoxNhanVien.ValueMember = "MaNV";
                comboBoxNhanVien.SelectedIndex = -1;
            }
        }

        private void LamMoiForm()
        {
            maNvDangChon = "";
            txtUserName.Text = "";
            txtPass.Text = "";

            comboBoxQuyenHan.SelectedIndex = -1;
            comboBoxNhanVien.SelectedIndex = -1;

            txtUserName.Enabled = true;
            comboBoxNhanVien.Enabled = true;
            LoadNhanVienChuaCoTaiKhoan();
            LoadDanhSachNhanVien();
        }

        private void LoadDSTaiKhoan()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var dsTaiKhoan = db.EmployeeAccounts.Select(acc => new
                {
                    MaNV = acc.MaNV,
                    HoTen = acc.Employee.HoTen,
                    Username = acc.Username,
                    QuyenHan = acc.QuyenHan
                }).ToList();

                dataGridViewTaiKhoan.DataSource = dsTaiKhoan;

                if (dataGridViewTaiKhoan.Columns["MaNV"] != null)
                {
                    dataGridViewTaiKhoan.Columns["MaNV"].HeaderText = "Mã NV";
                    dataGridViewTaiKhoan.Columns["HoTen"].HeaderText = "Họ và Tên";
                    dataGridViewTaiKhoan.Columns["Username"].HeaderText = "Tên đăng nhập";
                    dataGridViewTaiKhoan.Columns["QuyenHan"].HeaderText = "Quyền hạn";
                }
            }
        }

        private void KhoiTaoTabTaiKhoan()
        {
            LoadNhanVienChuaCoTaiKhoan();
            LoadDSTaiKhoan();

            comboBoxQuyenHan.Items.Clear();
            comboBoxQuyenHan.Items.Add("Quản lý");
            comboBoxQuyenHan.Items.Add("Lễ tân");
        }

        private void comboBoxNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxNhanVien.SelectedIndex == -1 || comboBoxNhanVien.SelectedValue == null) return;
            string maNV = comboBoxNhanVien.SelectedValue.ToString();

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var nv = db.Employees.FirstOrDefault(n => n.MaNV == maNV);
            }
        }

        private void tàiKhoảnPhânQuyềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KhoiTaoTabTaiKhoan();
            ChuyenTab(pnlTaiKhoan);
        }

        private void btTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            if (comboBoxNhanVien.SelectedValue == null || txtUserName.Text.Trim() == "" || comboBoxQuyenHan.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                string userMoi = txtUserName.Text.Trim();

                if (db.EmployeeAccounts.Any(a => a.Username == userMoi))
                {
                    MessageBox.Show("Tên đăng nhập này đã có người sử dụng! Vui lòng chọn tên khác.", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                EmployeeAccount acc = new EmployeeAccount();
                acc.MaNV = comboBoxNhanVien.SelectedValue.ToString();
                acc.Username = userMoi;
                acc.Password = "123456";
                acc.QuyenHan = comboBoxQuyenHan.SelectedItem.ToString();

                db.EmployeeAccounts.Add(acc);
                db.SaveChanges();

                MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadNhanVienChuaCoTaiKhoan();
                LoadDSTaiKhoan();
                LamMoiForm();
            }
        }

        private void btCapNhat_Click(object sender, EventArgs e)
        {
            if (maNvDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một tài khoản trên lưới để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var acc = db.EmployeeAccounts.FirstOrDefault(a => a.MaNV == maNvDangChon);
                if (acc != null)
                {
                    acc.QuyenHan = comboBoxQuyenHan.SelectedItem.ToString();
                    db.SaveChanges();

                    MessageBox.Show("Cập nhật quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDSTaiKhoan();
                    LamMoiForm();
                }
            }
        }

        private void dataGridViewTK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewTaiKhoan.Rows[e.RowIndex];

                maNvDangChon = row.Cells["MaNV"].Value.ToString();
                string hoTen = row.Cells["HoTen"].Value != null ? row.Cells["HoTen"].Value.ToString() : "Chưa có tên";

                txtUserName.Text = row.Cells["Username"].Value.ToString();
                txtPass.Text = "******";
                comboBoxQuyenHan.SelectedItem = row.Cells["QuyenHan"].Value.ToString();

                var dsTam = new List<object> {
                new { MaNV = maNvDangChon, ThongTin = maNvDangChon + " - " + hoTen }
                };

                comboBoxNhanVien.DataSource = dsTam;
                comboBoxNhanVien.DisplayMember = "ThongTin";
                comboBoxNhanVien.ValueMember = "MaNV";
                comboBoxNhanVien.SelectedValue = maNvDangChon;

                txtUserName.Enabled = false;
                comboBoxNhanVien.Enabled = false;
            }
        }

        private void btDatLaiMk_Click(object sender, EventArgs e)
        {
            if (maNvDangChon == "") return;
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đặt lại mật khẩu thành '123456' cho tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var acc = db.EmployeeAccounts.FirstOrDefault(a => a.MaNV == maNvDangChon);
                    if (acc != null)
                    {
                        acc.Password = "123456";
                        db.SaveChanges();
                        MessageBox.Show($"Đã Reset mật khẩu của tài khoản '{acc.Username}' về: 123456", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LamMoiForm();
                    }
                }
            }
        }

        private void btThuHoi_Click(object sender, EventArgs e)
        {
            if (maNvDangChon == "") return;

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thu hồi (xóa) tài khoản này? Nhân viên sẽ không thể đăng nhập được nữa.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var acc = db.EmployeeAccounts.FirstOrDefault(a => a.MaNV == maNvDangChon);
                    if (acc != null)
                    {
                        db.EmployeeAccounts.Remove(acc);
                        db.SaveChanges();

                        MessageBox.Show("Đã thu hồi tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadNhanVienChuaCoTaiKhoan();
                        LoadDSTaiKhoan();
                        LamMoiForm();
                    }
                }
            }
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            LamMoiForm();
        }

        //=================BẢNG LƯƠNG==================
        private double TongGioLam(string maNV, int thang, int nam)
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var dsChamCong = db.ChamCongs.Where(cc =>
                cc.MaNV == maNV &&
                cc.GioRa != null &&
                cc.NgayCC.Month == thang &&
                cc.NgayCC.Year == nam).ToList();

                double tongGioCoHeSo = 0;
                foreach (var cc in dsChamCong)
                {
                    TimeSpan thoiGianLam = cc.GioRa.Value - cc.GioVao.Value;
                    double giolam = thoiGianLam.TotalHours;

                    double heSo = 1.0;
                    if (cc.MaCa != null)
                    {
                        var ca = db.CaLamViecs.Find(cc.MaCa);
                        if (ca != null) heSo = (double)ca.HeSoLuong;
                    }

                    tongGioCoHeSo += (giolam * heSo);
                }
                return Math.Round(tongGioCoHeSo, 2);
            }
        }

        private void btLuong_Click(object sender, EventArgs e)
        {
            danhSachLuongThangNay.Clear();

            int thang = dtpThangNam.Value.Month;
            int nam = dtpThangNam.Value.Year;

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var tatCaNV = db.Employees.Include("Role").Where(nv => nv.TrangThai == true).ToList();

                foreach (var nv in tatCaNV)
                {
                    double tongGio = TongGioLam(nv.MaNV, thang, nam);

                    if (tongGio >= 0)
                    {
                        decimal luongCuaChucVuNay = 0;
                        if (nv.Role != null && nv.Role.LuongMoiGio != null)
                        {
                            luongCuaChucVuNay = (decimal)nv.Role.LuongMoiGio;
                        }

                        BangLuong luongNV = new BangLuong();
                        luongNV.MaNV = nv.MaNV;
                        luongNV.TongGioLam = tongGio;
                        luongNV.LuongTheoGio = luongCuaChucVuNay;
                        luongNV.TongTien = (decimal)tongGio * luongCuaChucVuNay;

                        danhSachLuongThangNay.Add(luongNV);
                    }
                }

                dgvBangLuong.DataSource = null;
                dgvBangLuong.DataSource = danhSachLuongThangNay.Select(bl => new
                {
                    MaNV = bl.MaNV,
                    HoTen = tatCaNV.FirstOrDefault(nv => nv.MaNV == bl.MaNV)?.HoTen,
                    TongGioLam = bl.TongGioLam,
                    LuongTheoGio = bl.LuongTheoGio,
                    TongTien = bl.TongTien
                }).ToList();

                dgvBangLuong.Columns["MaNV"].HeaderText = "Mã NV";
                dgvBangLuong.Columns["HoTen"].HeaderText = "Họ và Tên";
                dgvBangLuong.Columns["TongGioLam"].HeaderText = "Tổng Giờ";
                dgvBangLuong.Columns["LuongTheoGio"].HeaderText = "Lương/Giờ";
                dgvBangLuong.Columns["TongTien"].HeaderText = "Thực Lĩnh";

                btLuu.Enabled = true;

                MessageBox.Show($"Đã tự động tính xong lương cho {danhSachLuongThangNay.Count} nhân viên!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            if (danhSachLuongThangNay.Count == 0) return;

            string thangNamChuoi = dtpThangNam.Value.ToString("MM/yyyy");
            string thangNamCode = thangNamChuoi.Replace("/", "");

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                foreach (var item in danhSachLuongThangNay)
                {
                    bool daTonTai = db.BangLuongs.Any(b => b.MaNV == item.MaNV && b.ThangNam == thangNamChuoi);

                    if (!daTonTai)
                    {
                        string maPhieuMoi = $"PL-{thangNamCode}-{item.MaNV}";

                        BangLuong bl = new BangLuong();
                        bl.MaPhieu = maPhieuMoi;
                        bl.MaNV = item.MaNV;
                        bl.ThangNam = thangNamChuoi;
                        bl.TongGioLam = item.TongGioLam;
                        bl.LuongTheoGio = item.LuongTheoGio;
                        bl.TienThuong = 0;
                        bl.TienPhat = 0;
                        bl.TongTien = item.TongTien;
                        bl.NgayLapPhieu = DateTime.Now;

                        db.BangLuongs.Add(bl);
                    }
                }

                db.SaveChanges();

                MessageBox.Show($"Đã chốt lương đợt {thangNamChuoi} thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btLuu.Enabled = false;
            }
        }

        private void xuatDataGridViewRaPDF(DataGridView dgv, string tenFileHeader)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF Files|*.pdf";
            sfd.FileName = tenFileHeader + ".pdf";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);

                    iTextSharp.text.Font fontTieuDe = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontCot = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontChu = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);

                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 20f, 10f);
                    iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, new FileStream(sfd.FileName, FileMode.Create));

                    pdfDoc.Open();

                    iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("BẢNG TỔNG HỢP LƯƠNG NHÂN VIÊN\n\n", fontTieuDe);
                    title.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    pdfDoc.Add(title);

                    int soCotVisible = 0;
                    foreach (DataGridViewColumn col in dgv.Columns) if (col.Visible) soCotVisible++;

                    iTextSharp.text.pdf.PdfPTable pdfTable = new iTextSharp.text.pdf.PdfPTable(soCotVisible);
                    pdfTable.WidthPercentage = 100;

                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible)
                        {
                            iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(column.HeaderText, fontCot));
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            cell.Padding = 5;
                            pdfTable.AddCell(cell);
                        }
                    }

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (dgv.Columns[cell.ColumnIndex].Visible)
                            {
                                string giaTri = cell.Value != null ? cell.Value.ToString() : "";
                                iTextSharp.text.pdf.PdfPCell pCell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(giaTri, fontChu));
                                pCell.Padding = 5;
                                pdfTable.AddCell(pCell);
                            }
                        }
                    }

                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();

                    MessageBox.Show("Đã xuất file PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btXuatPDF_Click(object sender, EventArgs e)
        {
            if (dgvBangLuong.Rows.Count > 0)
            {
                string tenFile = "BangLuong_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                xuatDataGridViewRaPDF(dgvBangLuong, tenFile);
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất PDF!", "Thông báo");
            }
        }

        private void bảngLươngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChuyenTab(pnlBangLuong);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class ServiceForm : Form
    {
        private string GetConnectionString()
        {
            string efString = ConfigurationManager.ConnectionStrings["HotelManagementEntities"].ConnectionString;
            int start = efString.IndexOf("provider connection string=\"") + "provider connection string=\"".Length;
            int end = efString.LastIndexOf("\"");
            return efString.Substring(start, end - start).Replace("&quot;", "\"");
        }

        public ServiceForm()
        {
            InitializeComponent();
            // Kết nối sự kiện thủ công
            this.Load += ServiceForm_Load;
            dgvDanhSach.CellClick += dgvDanhSach_CellClick;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnTim.Click += btnTim_Click;
        }

        private void ServiceForm_Load(object sender, EventArgs e)
        {
            DinhDangLuoi(dgvDanhSach);
            LoadComboLoaiDV();
            LoadDataGridView();
        }

        // ========================================================
        // GIAO DIỆN ĐỒNG BỘ
        // ========================================================
        private void DinhDangLuoi(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 45, 70);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 45, 70);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

            dgv.RowTemplate.Height = 35;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 250);
        }

        private void LoadDataGridView()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    string searchName = txtTimKiem.Text.Trim();
                    string selectedCategory = cboLoaiDV.SelectedValue?.ToString();

                    string sql = "SELECT ServiceId, ServiceName, Price, Unit, Category FROM Service WHERE 1=1";
                    if (!string.IsNullOrEmpty(searchName)) sql += " AND LOWER(ServiceName) LIKE @timKiem";
                    if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "Tất cả") sql += " AND Category = @Category";
                    sql += " ORDER BY ServiceId";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(searchName)) cmd.Parameters.AddWithValue("@timKiem", "%" + searchName.ToLower() + "%");
                    if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "Tất cả") cmd.Parameters.AddWithValue("@Category", selectedCategory);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDanhSach.DataSource = dt;

                    if (dgvDanhSach.Columns["ServiceId"] != null) dgvDanhSach.Columns["ServiceId"].HeaderText = "Mã";
                    if (dgvDanhSach.Columns["ServiceName"] != null) dgvDanhSach.Columns["ServiceName"].HeaderText = "Tên Dịch Vụ";
                    if (dgvDanhSach.Columns["Price"] != null) { dgvDanhSach.Columns["Price"].HeaderText = "Đơn Giá (VNĐ)"; dgvDanhSach.Columns["Price"].DefaultCellStyle.Format = "N0"; }
                    if (dgvDanhSach.Columns["Unit"] != null) dgvDanhSach.Columns["Unit"].HeaderText = "Đơn Vị";
                    if (dgvDanhSach.Columns["Category"] != null) dgvDanhSach.Columns["Category"].HeaderText = "Loại Hình";
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải: " + ex.Message); }
        }

        private void LoadComboLoaiDV()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                DataTable dt = new DataTable();
                new SqlDataAdapter("SELECT DISTINCT Category FROM Service WHERE Category IS NOT NULL AND Category != ''", conn).Fill(dt);
                DataRow row = dt.NewRow(); row["Category"] = "Tất cả"; dt.Rows.InsertAt(row, 0);
                cboLoaiDV.DataSource = dt;
                cboLoaiDV.DisplayMember = "Category";
                cboLoaiDV.ValueMember = "Category";
            }
        }

        // ========================================================
        // XỬ LÝ ĐỔ DỮ LIỆU TỪ LƯỚI RA TEXTBOX
        // ========================================================
        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];

                txtMaDV.Text = row.Cells["ServiceId"].Value.ToString();
                txtTenDV.Text = row.Cells["ServiceName"].Value.ToString();
                txtDonGia.Text = row.Cells["Price"].Value.ToString();
                txtDonViTinh.Text = row.Cells["Unit"].Value?.ToString();

                string category = row.Cells["Category"].Value?.ToString();
                cboLoaiDV.SelectedValue = string.IsNullOrEmpty(category) ? "Tất cả" : category;
            }
        }

        // ========================================================
        // THÊM / SỬA / XÓA
        // ========================================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            ExecuteQuery("INSERT INTO Service (ServiceName, Price, Unit, Category) VALUES (@Name, @Price, @Unit, @Cat)", "Thêm thành công!");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            ExecuteQuery("UPDATE Service SET ServiceName=@Name, Price=@Price, Unit=@Unit, Category=@Cat WHERE ServiceId=@Id", "Cập nhật thành công!");
        }

        private void ExecuteQuery(string sql, string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", txtTenDV.Text.Trim());
                    cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtDonGia.Text));
                    cmd.Parameters.AddWithValue("@Unit", txtDonViTinh.Text.Trim());
                    cmd.Parameters.AddWithValue("@Cat", cboLoaiDV.Text == "Tất cả" ? "" : cboLoaiDV.Text);
                    if (sql.Contains("UPDATE") || sql.Contains("DELETE")) cmd.Parameters.AddWithValue("@Id", txtMaDV.Text);
                    cmd.ExecuteNonQuery();
                    LoadDataGridView();
                    ClearForm();
                    MessageBox.Show(message);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMaDV.Text) && MessageBox.Show("Xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ExecuteQuery("DELETE FROM Service WHERE ServiceId=@Id", "Đã xóa!");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) { ClearForm(); LoadDataGridView(); }
        private void btnTim_Click(object sender, EventArgs e) { LoadDataGridView(); }
        private void ClearForm() { txtMaDV.Clear(); txtTenDV.Clear(); txtDonGia.Clear(); txtDonViTinh.Clear(); }
    }
}
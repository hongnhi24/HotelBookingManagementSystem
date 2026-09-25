using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class CustomerForm : Form
    {
        string connectionString =
        "Server=.;Database=HotelManagement;Trusted_Connection=True;";
        public CustomerForm()
        {
            InitializeComponent();
        }

        //hiển thị danh sách khách hàng từ bảng customer
        void LoadKhachHang()
        {
            dataKH.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT MaKH,TenKH,GioiTinh,CCCD,SDT,DiaChi,QuocTich FROM Customer";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataKH.Rows.Add(
                        reader["MaKH"].ToString(),
                        reader["TenKH"].ToString(),
                        reader["CCCD"].ToString(),
                        reader["SDT"].ToString(),
                        reader["DiaChi"].ToString(),
                        reader["GioiTinh"].ToString(),
                        reader["QuocTich"].ToString(),
                         "✏",
                        "🗑"
                    );
                }
            }
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            panelThongTin.Visible = false;
            panelSuaKH.Visible = false;

            // nhập số cho sdt cccd
            txtSDT.KeyPress += ChiNhapSo;
            txtCccd.KeyPress += ChiNhapSo;
            txtSDTSua.KeyPress += ChiNhapSo;
            txtCccdSua.KeyPress += ChiNhapSo;
            //icon sửa và xóa
            dataKH.Columns["colSua"].DefaultCellStyle.ForeColor = Color.Goldenrod;
            dataKH.Columns["colXoa"].DefaultCellStyle.ForeColor = Color.Red;

            dataKH.DefaultCellStyle.Font = new Font("Segoe UI Emoji", 11);
            //nút tìm
            txtTim.Text = "🔍 Nhập tên khách hàng cần tìm";
            txtTim.ForeColor = Color.Gray;
            txtTim.TextChanged += txtTim_TextChanged;

            LoadKhachHang();

           
            dataKH.Columns["colSua"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataKH.Columns["colXoa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //Canh giữa header
            dataKH.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
           
            SetPlaceholder(txtTenKH, "Họ và tên khách hàng");
            SetPlaceholder(txtCccd, "Mã căn cước công dân");
            SetPlaceholder(txtSDT, "Số điện thoại");
            SetPlaceholder(txtDiaChi, "Địa chỉ");
            SetPlaceholder(txtQuocTich, "Quốc tịch");

            // Giới tính
            cbGioiTinh.Items.Add("Nam");
            cbGioiTinh.Items.Add("Nữ");
            cbGioiTinh.Text = "Giới tính";

            txtCccd.MaxLength = 12;
            txtSDT.MaxLength = 10;
        }

       

        void SetPlaceholder(TextBox txt, string text)
        {
            txt.Text = text;
            txt.ForeColor = Color.Gray;

            txt.Enter += (s, e) =>
            {
                if (txt.ForeColor == Color.Gray)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.Leave += (s, e) =>
            {
                if (txt.Text == "")
                {
                    txt.Text = text;
                    txt.ForeColor = Color.Gray;
                }
            };
        }

        void RemovePlaceholder(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (txt.ForeColor == Color.Gray)
            {
                txt.Text = "";
                txt.ForeColor = Color.Black;
            }
        }

        // ===== NÚT THÊM =====

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtTenKH.ForeColor == Color.Gray ||
                txtCccd.ForeColor == Color.Gray)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string getID = "SELECT TOP 1 MaKH FROM Customer ORDER BY MaKH DESC";

                SqlCommand getCmd = new SqlCommand(getID, conn);
                object result = getCmd.ExecuteScalar();

                string newMaKH = "KH001";

                if (result != null)
                {
                    string lastID = result.ToString(); 
                    int num = int.Parse(lastID.Substring(2)) + 1;
                    newMaKH = "KH" + num.ToString("D3");
                }
                // kiểm tra CCCD trùng
                string check = "SELECT COUNT(*) FROM Customer WHERE CCCD=@cccd";

                SqlCommand checkCmd = new SqlCommand(check, conn);
                checkCmd.Parameters.AddWithValue("@cccd", txtCccd.Text);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("CCCD đã tồn tại!");
                    return;
                }

                // thêm khách hàng
                string query = @"INSERT INTO Customer
                                (MaKH,TenKH,GioiTinh,CCCD,SDT,DiaChi,QuocTich)
                                VALUES
                                (@MaKH,@TenKH,@GioiTinh,@CCCD,@SDT,@DiaChi,@QuocTich)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaKH", newMaKH);
                cmd.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("@CCCD", txtCccd.Text);
                cmd.Parameters.AddWithValue("@SDT", txtSDT.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@QuocTich", txtQuocTich.Text);

                cmd.ExecuteNonQuery();
                LoadKhachHang();



            }
           
            txtTenKH.Clear();
            txtCccd.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtQuocTich.Clear();

            panelThongTin.Visible = false;
            MessageBox.Show("Thêm khách hàng thành công");

        }
        //chỉ cho nhập số vào cccd và sdt
        private void ChiNhapSo(object sender, KeyPressEventArgs e)
        {
            // chỉ cho nhập số và phím backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        // ===== NÚT HỦY =====

        private void btnHuy_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Hủy thành công");
            panelThongTin.Visible = false;
        }

        private void txtTenKH_Enter(object sender, EventArgs e)
        {

        }

        private void txtTenKH_Leave(object sender, EventArgs e)
        {

        }

        private void btThem_Click(object sender, EventArgs e)
        {
            panelSuaKH.Visible = false;
            panelThongTin.Visible = true;

        }

        private void txtTim_Enter(object sender, EventArgs e)
        {
            if (txtTim.Text == "🔍 Nhập tên khách hàng cần tìm")
            {
                txtTim.Text = "";
                txtTim.ForeColor = Color.Black;
            }
        }

        private void txtTim_Leave(object sender, EventArgs e)
        {
            if (txtTim.Text == "")
            {
                txtTim.Text = "🔍 Nhập tên khách hàng cần tìm";
                txtTim.ForeColor = Color.Gray;
            }
        }
        void TimKhachHang()
        {
            // Nếu ô tìm kiếm trống hoặc đang hiện placeholder thì load lại toàn bộ
            if (string.IsNullOrWhiteSpace(txtTim.Text) || txtTim.ForeColor == Color.Gray)
            {
                LoadKhachHang();
                return;
            }

            dataKH.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                
                string query = @"SELECT MaKH, TenKH, GioiTinh, CCCD, SDT, DiaChi, QuocTich
                         FROM Customer
                         WHERE TenKH LIKE @tukhoa OR MaKH LIKE @tukhoa";

                SqlCommand cmd = new SqlCommand(query, conn);
                // Dùng chung 1 tham số @tukhoa cho cả 2 điều kiện
                cmd.Parameters.AddWithValue("@tukhoa", "%" + txtTim.Text.Trim() + "%");

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataKH.Rows.Add(
                        reader["MaKH"].ToString(),
                        reader["TenKH"].ToString(),
                        reader["CCCD"].ToString(), // Lưu ý thứ tự cột khớp với DataGridView của bạn
                        reader["SDT"].ToString(),
                        reader["DiaChi"].ToString(),
                        reader["GioiTinh"].ToString(),
                        reader["QuocTich"].ToString(),
                        "✏",
                        "🗑"
                    );
                }
            }
        }
      
        private void dataKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            // chỉ cho phép xử lý khi bấm vào cột sửa hoặc xóa
            if (dataKH.Columns[e.ColumnIndex].Name != "colSua" &&
                dataKH.Columns[e.ColumnIndex].Name != "colXoa")
                return;

            // ===== NÚT XÓA =====
            if (dataKH.Columns[e.ColumnIndex].Name == "colXoa")
            {
                DialogResult r = MessageBox.Show(
                    "Bạn có chắc muốn xóa khách hàng?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo);

                if (r == DialogResult.Yes)
                {
                    string cccd = dataKH.Rows[e.RowIndex].Cells[1].Value.ToString();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "DELETE FROM Customer WHERE CCCD=@cccd";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@cccd", cccd);

                        cmd.ExecuteNonQuery();
                    }

                    LoadKhachHang();
                }
            }

            // ===== NÚT SỬA =====
            if (dataKH.Columns[e.ColumnIndex].Name == "colSua")
            {
                panelThongTin.Visible = false;
                panelSuaKH.Parent = this;
                panelSuaKH.Visible = true;
                panelSuaKH.BringToFront();

                txtTenSua.Text = dataKH.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtCccdSua.Text = dataKH.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtSDTSua.Text = dataKH.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtDiaChiSua.Text = dataKH.Rows[e.RowIndex].Cells[4].Value.ToString();
                cbGioiTinhSua.Text = dataKH.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtQuocTichSua.Text = dataKH.Rows[e.RowIndex].Cells[6].Value.ToString();
                //LoadKhachHang();
            }
        }
        private void btCapnhap_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"UPDATE Customer
                         SET TenKH=@ten,
                             GioiTinh=@gioitinh,
                             SDT=@sdt,
                             DiaChi=@diachi,
                             QuocTich=@quoctich
                         WHERE CCCD=@cccd";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ten", txtTenSua.Text);
                cmd.Parameters.AddWithValue("@gioitinh", cbGioiTinhSua.Text);
                cmd.Parameters.AddWithValue("@sdt", txtSDTSua.Text);
                cmd.Parameters.AddWithValue("@diachi", txtDiaChiSua.Text);
                cmd.Parameters.AddWithValue("@quoctich", txtQuocTichSua.Text);
                cmd.Parameters.AddWithValue("@cccd", txtCccdSua.Text);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Cập nhật thành công");

            panelSuaKH.Visible = false;
            LoadKhachHang();
        }

        private void btHuySua_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Hủy thành công");
            panelSuaKH.Visible = false;
        }

       

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            if (txtTim.ForeColor == Color.Gray) return;

            TimKhachHang();

        }

      
    }
}
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;
using HotelManagement.Models;

namespace HotelManagement.GUI
{
    public partial class RoomForm : Form
    {
        public RoomForm()
        {
            InitializeComponent();
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {
            DinhDangLuoi(dataGridView1);
            LoadRoomTypes();
            SearchRoom();
        }

        private void DinhDangLuoi(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 45, 70);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 45, 70);

            dgv.RowTemplate.Height = 35;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 250);
        }

        private void LoadRoomTypes()
        {
            using (var db = new HotelManagementEntities())
            {
                comboBoxLoaiPhong.DataSource = db.RoomTypes.ToList();
                comboBoxLoaiPhong.DisplayMember = "TypeName";
                comboBoxLoaiPhong.ValueMember = "RoomTypeID";
            }
        }

        private void SearchRoom()
        {
            string keyword = txtTimKiem.Text.ToLower();
            using (var db = new HotelManagementEntities())
            {
                var query = db.Rooms.Include(r => r.RoomType).AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(r => r.RoomName.ToLower().Contains(keyword) ||
                                           r.RoomType.TypeName.ToLower().Contains(keyword));
                }

                dataGridView1.DataSource = query.Select(r => new
                {
                    r.RoomID,
                    r.RoomName,
                    r.RoomTypeID,
                    r.RoomType.TypeName,
                    r.Status,
                    r.Floor
                }).ToList();

                FormatColumns();
            }
        }

        private void FormatColumns()
        {
            if (dataGridView1.Columns["RoomID"] != null) dataGridView1.Columns["RoomID"].HeaderText = "Mã Phòng";
            if (dataGridView1.Columns["RoomName"] != null) dataGridView1.Columns["RoomName"].HeaderText = "Tên Phòng";
            if (dataGridView1.Columns["TypeName"] != null) dataGridView1.Columns["TypeName"].HeaderText = "Loại Phòng";
            if (dataGridView1.Columns["Status"] != null) dataGridView1.Columns["Status"].HeaderText = "Trạng Thái";
            if (dataGridView1.Columns["Floor"] != null) dataGridView1.Columns["Floor"].HeaderText = "Tầng";
            if (dataGridView1.Columns["RoomTypeID"] != null) dataGridView1.Columns["RoomTypeID"].Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            txtMaPhong.Text = row.Cells["RoomName"].Value?.ToString();
            comboBoxLoaiPhong.SelectedValue = row.Cells["RoomTypeID"].Value;
            comboBoxTrangThai.Text = row.Cells["Status"].Value?.ToString();
            numericUpDownTang.Value = Convert.ToDecimal(row.Cells["Floor"].Value);
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text)) { MessageBox.Show("Vui lòng nhập tên phòng!"); return; }

            using (var db = new HotelManagementEntities())
            {
                db.Rooms.Add(new Room
                {
                    RoomName = txtMaPhong.Text,
                    RoomTypeID = (int)comboBoxLoaiPhong.SelectedValue,
                    Status = "Trống",
                    Floor = (int)numericUpDownTang.Value
                });
                db.SaveChanges();
                SearchRoom();
                ClearInput();
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            string maPhong = txtMaPhong.Text;
            using (var db = new HotelManagementEntities())
            {
                var phong = db.Rooms.FirstOrDefault(p => p.RoomName == maPhong);
                if (phong != null)
                {
                    phong.RoomTypeID = (int)comboBoxLoaiPhong.SelectedValue;
                    phong.Floor = (int)numericUpDownTang.Value;
                    phong.Status = comboBoxTrangThai.Text;
                    db.SaveChanges();
                    SearchRoom();
                    MessageBox.Show("Cập nhật thành công!");
                }
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            string maPhong = txtMaPhong.Text;
            if (MessageBox.Show("Bạn chắc chắn muốn xóa phòng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var db = new HotelManagementEntities())
                {
                    var phong = db.Rooms.FirstOrDefault(p => p.RoomName == maPhong);
                    if (phong != null)
                    {
                        db.Rooms.Remove(phong);
                        db.SaveChanges();
                        SearchRoom();
                        ClearInput();
                    }
                }
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e) => SearchRoom();
        private void ClearInput() { txtMaPhong.Clear(); comboBoxLoaiPhong.SelectedIndex = -1; comboBoxTrangThai.SelectedIndex = -1; numericUpDownTang.Value = 0; }
    }
}

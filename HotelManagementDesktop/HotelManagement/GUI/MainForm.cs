using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class MainForm : Form
    {
        private string quyenHanHienTai = "";
        private string usernameHienTai = "";
        bool menuOpen = false;
        int index = 0;
        Button currentButton = null;

        Image[] images =
        {
            Properties.Resources.quanlyks,
            Properties.Resources.hoboi
        };

        public MainForm(string quyenHanHienTai, string usernameHienTai)
        {
            InitializeComponent();

          
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            this.quyenHanHienTai = quyenHanHienTai;
            this.usernameHienTai = usernameHienTai;
        }

        public static void EnableDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        // ========================================================
        // 1. ĐỊNH DẠNG NÚT BẤM VÀ VẼ VIỀN KÉP
        // ========================================================
        private void StyleButton(Button btn)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

           
            btn.BackColor = Color.FromArgb(5, 20, 35);
            btn.ForeColor = Color.Gold;
            btn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            btn.Height = 62;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            btn.Cursor = Cursors.Hand;

            btn.Paint += Btn_Paint;

           
            btn.MouseEnter += (s, e) =>
            {
                if (currentButton != btn)
                    btn.BackColor = Color.FromArgb(20, 45, 65);
            };

           
            btn.MouseLeave += (s, e) =>
            {
                if (currentButton != btn)
                    btn.BackColor = Color.FromArgb(5, 20, 35);
            };
        }

        private void Btn_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;

        
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen penOuter = new Pen(Color.Black, 1))
            using (Pen penInner = new Pen(Color.Black, 1))
            {
             
                e.Graphics.DrawRectangle(penOuter, 0, 0, btn.Width - 1, btn.Height - 1);

              
                e.Graphics.DrawRectangle(penInner, 2, 2, btn.Width - 5, btn.Height - 5);
            }
        }

        private void SetActiveButton(Button btn)
        {
            if (btn == null) return;

         
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(5, 20, 35);
            }

            
            currentButton = btn;

           
            currentButton.BackColor = Color.FromArgb(35, 80, 130);
        }

        
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Bật chống giật lag (DoubleBuffering) cho toàn bộ khung chứa
            EnableDoubleBuffered(panel1);
            EnableDoubleBuffered(panelContent);
            EnableDoubleBuffered(panelTop);

            if (quyenHanHienTai == "Lễ tân")
            {
                if (btQLNhanVien != null) btQLNhanVien.Visible = false;
                if (btReport != null) btReport.Visible = false;

                if (btDichVu != null) btDichVu.Visible = false;
            }
            this.BackColor = Color.FromArgb(10, 25, 40);

            // ĐỒNG BỘ CẢ 2 THANH CÙNG 1 MÀU
            panelTop.BackColor = Color.FromArgb(5, 20, 35); // Thanh trên
            panel1.BackColor = Color.FromArgb(5, 20, 35);   

            panelContent.BackColor = Color.FromArgb(220, 230, 240);
            this.Font = new Font("Segoe UI", 10F);

            // Style nút bấm
            StyleButton(btRoom);
            StyleButton(btBooking);
            StyleButton(btQLDatPhong);
            StyleButton(btCustomer);
            StyleButton(btDichVu);
            StyleButton(btInvoice);
            StyleButton(btReport);
            StyleButton(btQLNhanVien);

            panel1.Width = 200;
            panel1.Padding = new Padding(0);
            panelContent.Padding = new Padding(0);
            panel1.AutoScroll = true;
            panel1.Visible = false;

            Label title = new Label();
            title.Text = "          HOTEL MANAGEMENT";
            title.ForeColor = Color.Gold;
            title.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(60, 15);
            panelTop.Controls.Add(title);

            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.Height = 70;
                    btn.Dock = DockStyle.Top;
                    btn.Margin = new Padding(0);
                    btn.Padding = new Padding(10, 0, 0, 0);
                }
            }

            panelTop.Dock = DockStyle.Top;
            panel1.Dock = DockStyle.Left;
            panelContent.Dock = DockStyle.Fill;

            pictureSlide.Image = images[0];
            pictureSlide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureSlide.Dock = DockStyle.Fill;
            pictureSlide.Visible = true;

            panelContent.Visible = false;
            panelTop.BringToFront();
            panel1.BringToFront();

            slideTimer.Interval = 2000;
            slideTimer.Start();

            TaoPanelProfile();

            LoadCaLamViec();
        }

        // ========================================================
        // 3. XỬ LÝ CHUYỂN SLIDe
        // ========================================================
        private void slideTimer_Tick(object sender, EventArgs e)
        {
            index++;
            if (index >= images.Length) index = 0;
            pictureSlide.Image = images[index];
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            
            if (!menuOpen)
            {
                panel1.Width = 200;
                panel1.Visible = true;
                pictureSlide.Visible = false;
                menuOpen = true;
            }
            else
            {
                panel1.Visible = false;
                panelContent.Controls.Clear();
                panelContent.Visible = false;
                pictureSlide.Visible = true;
                menuOpen = false;
            }
        }

        // ========================================================
        // 4. MỞ FORM CON
        // ========================================================
        private void OpenChildForm(Form childForm)
        {
            panelContent.Visible = true;
            pictureSlide.Visible = false;
            panelContent.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.Margin = new Padding(0);
            childForm.Padding = new Padding(0);

            panelContent.Controls.Add(childForm);
            panelContent.BringToFront();
            childForm.AutoScroll = true;
            childForm.Show();
        }

        // ========================================================
        // 5. CÁC NÚT MỞ TỪNG CHỨC NĂNG
        // ========================================================
        private void btRoom_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            OpenChildForm(new RoomForm());
        }

        private void btBooking_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            OpenChildForm(new Booking(this.usernameHienTai));
        }

        private void btQLDatPhong_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            OpenChildForm(new FrmDatPhong(this.usernameHienTai));
        }

        private void btCustomer_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            OpenChildForm(new CustomerForm());
        }

        private void btDichVu_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            OpenChildForm(new ServiceForm());
        }

        private void btInvoice_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            OpenChildForm(new InvoiceForm(this.usernameHienTai));
        }

        private void btReport_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            OpenChildForm(new ReportForm());
        }

        private void btQLNhanVien_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            OpenChildForm(new QLNhanVienForm());
        }

        // ========================================================
        // 6. CHẤM CÔNG (CHECK-IN / CHECK-OUT)
        // ========================================================
        private string LayMaNhanVienHienTai()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var acc = db.EmployeeAccounts.FirstOrDefault(a => a.Username == this.usernameHienTai);
                return acc != null ? acc.MaNV : "";
            }
        }

        private void LoadCaLamViec()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var dsCa = db.CaLamViecs.Select(c => new
                {
                    MaCa = c.MaCa,
                    TenCa = c.TenCa
                }).ToList();

                cbbCaLamViec.DataSource = dsCa;
                cbbCaLamViec.DisplayMember = "TenCa";
                cbbCaLamViec.ValueMember = "MaCa";
                cbbCaLamViec.SelectedIndex = -1;
            }
        }

        private void btCheckin_Click(object sender, EventArgs e)
        {
            string maNV = LayMaNhanVienHienTai();
            if (maNV == "") return;

            if (cbbCaLamViec.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Ca làm việc trước khi Check-in!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                DateTime homNay = DateTime.Now.Date;
                bool daVao = db.ChamCongs.Any(cc => cc.MaNV == maNV && cc.NgayCC == homNay);
                if (daVao)
                {
                    MessageBox.Show("Bạn đã Check-in hôm nay rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ChamCong ccMoi = new ChamCong();
                int maCaDuocChon = (int)cbbCaLamViec.SelectedValue;
                ccMoi.MaNV = maNV;
                ccMoi.NgayCC = homNay;
                ccMoi.GioVao = DateTime.Now;
                ccMoi.TrangThai = "Đang làm việc";
                ccMoi.MaCa = maCaDuocChon;

                db.ChamCongs.Add(ccMoi);
                db.SaveChanges();

                MessageBox.Show($"Check-in thành công lúc {DateTime.Now:HH:mm}!", "Báo danh");
            }
        }

        private void btCheckout_Click(object sender, EventArgs e)
        {
            string maNV = LayMaNhanVienHienTai();
            if (maNV == "") return;

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                DateTime homNay = DateTime.Now.Date;
                var caHienTai = db.ChamCongs.FirstOrDefault(cc => cc.MaNV == maNV && cc.NgayCC == homNay && cc.GioRa == null);

                if (caHienTai != null)
                {
                    caHienTai.GioRa = DateTime.Now;
                    caHienTai.TrangThai = "Hoàn thành";
                    db.SaveChanges();
                    MessageBox.Show($"Check-out thành công lúc {DateTime.Now:HH:mm}! Nghỉ ngơi thôi.", "Báo danh");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu Check-in của bạn hoặc bạn đã Check-out rồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void TaoPanelProfile()
        {
            // 1. Tạo Panel chứa 
            Panel pnlProfile = new Panel();
            pnlProfile.Height = 170;
            pnlProfile.Dock = DockStyle.Top;
        
            pnlProfile.BackColor = Color.FromArgb(15, 45, 70);

            // 2. Nút Đăng xuất 
            LinkLabel lnkLogout = new LinkLabel();
            lnkLogout.Text = "Đăng xuất";
            lnkLogout.LinkColor = Color.LightGray;
            lnkLogout.ActiveLinkColor = Color.White;
            lnkLogout.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lnkLogout.Location = new Point(10, 10);
            lnkLogout.AutoSize = true;
            lnkLogout.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkLogout.Click += (s, e) =>
            {
                DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    this.Hide();
                    LoginForm frm = new LoginForm(); 
                    frm.ShowDialog();
                
                }
            };
            pnlProfile.Controls.Add(lnkLogout);

            // 3. Nút Thu gọn Menu "<<<" 
            Label lblCollapse = new Label();
            lblCollapse.Text = "<<<";
            lblCollapse.ForeColor = Color.LightGray;
            lblCollapse.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCollapse.Location = new Point(160, 8);
            lblCollapse.AutoSize = true;
            lblCollapse.Cursor = Cursors.Hand;
            lblCollapse.Click += btnMenu_Click;
            pnlProfile.Controls.Add(lblCollapse);

            // 4. Hình Avatar bo tròn 
            PictureBox picAvatar = new PictureBox();
            picAvatar.Size = new Size(80, 80);
            picAvatar.Location = new Point(60, 40);
            picAvatar.BackColor = Color.White; 
            picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;

            // Cắt hình tròn
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, 80, 80);
            picAvatar.Region = new Region(path);

            // VẼ VIỀN TRẮNG BAO QUANH AVATAR 
            picAvatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.White, 4)) // Viền màu trắng, độ dày 4px
                {
                  
                    e.Graphics.DrawEllipse(pen, 2, 2, picAvatar.Width - 4, picAvatar.Height - 4);
                }
            };

           
            pnlProfile.Controls.Add(picAvatar);

          
            Label lblName = new Label();
            lblName.AutoSize = false;
            lblName.Width = 200;// THÊM DÒNG NÀY: Cấp đủ chiều cao để chữ không bị kích hiển thị
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            lblName.Location = new Point(0, 130);
            lblName.ForeColor = Color.White;
            lblName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

       
            pnlProfile.Controls.Add(lblName);
            lblName.BringToFront(); 

         
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                // Tìm tài khoản
                var acc = db.EmployeeAccounts.FirstOrDefault(a => a.Username == this.usernameHienTai);

                if (acc != null)
                {
                    // Lấy mã nhân viên từ tài khoản để tìm trực tiếp trong bảng Employee
                    var nv = db.Employees.FirstOrDefault(e => e.MaNV == acc.MaNV);

                    if (nv != null)
                    {
                        // 1. Gán Tên
                        lblName.Text = nv.HoTen;

                        // 2. Gán Hình
                        if (nv.HinhAnh != null)
                        {
                            try
                            {
                                using (MemoryStream ms = new MemoryStream(nv.HinhAnh))
                                {
                                    picAvatar.Image = Image.FromStream(ms);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi hiển thị ảnh Avatar: " + ex.Message, "Lỗi");
                            }
                        }
                    }
                    else
                    {
                        lblName.Text = "Lỗi: Không tìm thấy NV";
                    }
                }
                else
                {
                    // Bật thông báo này lên để xem Username truyền từ form Login qua có bị rỗng không
                    MessageBox.Show("Tên đăng nhập truyền vào Form Main đang bị sai hoặc rỗng: [" + this.usernameHienTai + "]", "Cảnh báo Debug");
                    lblName.Text = "Administrator";
                }
            }

            // 6. Đường viền mỏng phân cách Menu
            Panel line = new Panel();
            line.Height = 1;
            line.Dock = DockStyle.Bottom;
            line.BackColor = Color.FromArgb(100, 200, 200, 200);
            pnlProfile.Controls.Add(line);

            // 7. Thêm vào Sidebar
            panel1.Controls.Add(pnlProfile);
            pnlProfile.SendToBack();
        }
    }
}
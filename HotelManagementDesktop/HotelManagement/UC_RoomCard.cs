using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class UC_RoomCard : UserControl
    {
        // ========================================================
        // 1. CÁC THUỘC TÍNH DỮ LIỆU
        // ========================================================
        public string MaPhong { get; set; }
        public string TrangThai { get; set; }
        public string TenKhachHang { get; set; }
        public string ThoiGian { get; set; }

        public DateTime? ThoiGianNhanPhong { get; set; }
        public DateTime? ThoiGianTraPhong { get; set; }
        public string LoaiHinhThue { get; set; }

        private Timer timerDemGio;

        // ========================================================
        // 2. KHAI BÁO CÁC CONTROL GIAO DIỆN (Đã đổi tên thêm chữ "ui" để chống trùng lặp)
        // ========================================================
        private Panel uiPnlTop;
        private Panel uiPnlBottom;
        private Label uiLblMaPhong;
        private Label uiLblTrangThai;
        private Label uiLblMainIcon;
        private Label uiLblTenKhach;
        private Label uiLblTimeIcon;
        private Label uiLblThoiGianText;
        private Label uiLblCleanIcon;
        private Label uiLblCleanStatus;

        public UC_RoomCard()
        {
            InitializeComponent();

            SetupUI();

            this.Controls.Clear();

            this.Controls.Add(uiPnlBottom);
            this.Controls.Add(uiPnlTop);

            DangKySuKienClick(this);

            timerDemGio = new Timer();
            timerDemGio.Interval = 60000;
            timerDemGio.Tick += Timer1_Tick;

            this.Resize += UC_RoomCard_Resize;
        }

        // ========================================================
        // TỰ ĐỘNG VẼ GIAO DIỆN HIỆN ĐẠI (ĐÃ ĐIỀU CHỈNH THEO YÊU CẦU)
        // ========================================================
        private void SetupUI()
        {
            this.Size = new Size(250, 130);
            this.Margin = new Padding(10);
            this.BackColor = Color.Transparent;

            // --- PANEL TRÊN ---
            uiPnlTop = new Panel();
            uiPnlTop.Dock = DockStyle.Fill;

            uiLblMaPhong = new Label { Location = new Point(10, 10), AutoSize = true, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            uiLblTrangThai = new Label { Location = new Point(150, 12), AutoSize = true, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Regular) };

            uiLblMainIcon = new Label { Location = new Point(10, 45), Size = new Size(40, 40), ForeColor = Color.White, Font = new Font("Segoe UI Symbol", 20, FontStyle.Regular), TextAlign = ContentAlignment.MiddleCenter };
            uiLblTenKhach = new Label { Location = new Point(55, 52), Size = new Size(185, 30), ForeColor = Color.White, Font = new Font("Segoe UI", 13, FontStyle.Bold), AutoEllipsis = true };

            uiPnlTop.Controls.Add(uiLblMaPhong);
            uiPnlTop.Controls.Add(uiLblTrangThai);
            uiPnlTop.Controls.Add(uiLblMainIcon);
            uiPnlTop.Controls.Add(uiLblTenKhach);

            // --- PANEL DƯỚI ---
            uiPnlBottom = new Panel();
            uiPnlBottom.Dock = DockStyle.Bottom;

            // [SỬA TẠI ĐÂY] Đã thu hẹp chiều cao phần màu xám xuống 28 (trước là 35)
            uiPnlBottom.Height = 25;

            // [SỬA TẠI ĐÂY] Đã đổi màu nền xám nhạt hơn (FromArgb(220, 220, 220))
            uiPnlBottom.BackColor = Color.FromArgb(220, 220, 220);

            // [SỬA TẠI ĐÂY] Đã điều chỉnh vị trí Y của các nhãn để căn giữa trong panel mới hẹp hơn
            uiLblTimeIcon = new Label { Location = new Point(10, 6), AutoSize = true, ForeColor = Color.DimGray, Font = new Font("Segoe UI Symbol", 9), Text = "⏱" };
            uiLblThoiGianText = new Label { Location = new Point(28, 6), AutoSize = true, ForeColor = Color.DimGray, Font = new Font("Segoe UI", 9) };

            uiLblCleanIcon = new Label { Location = new Point(140, 6), AutoSize = true, ForeColor = Color.DimGray, Font = new Font("Segoe UI Symbol", 9), Text = "✔" };
            uiLblCleanStatus = new Label { Location = new Point(158, 6), AutoSize = true, ForeColor = Color.DimGray, Font = new Font("Segoe UI", 9), Text = "Đã dọn dẹp" };

            uiPnlBottom.Controls.Add(uiLblTimeIcon);
            uiPnlBottom.Controls.Add(uiLblThoiGianText);
            uiPnlBottom.Controls.Add(uiLblCleanIcon);
            uiPnlBottom.Controls.Add(uiLblCleanStatus);
        }

        // ========================================================
        // HÀM XỬ LÝ CẮT BO GÓC MƯỢT MÀ
        // ========================================================
        // 1. Sửa hàm Resize để chữ Trạng thái luôn bám lề phải bất kể lúc nào
        private void UC_RoomCard_Resize(object sender, EventArgs e)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = 20;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);

            // THÊM DÒNG NÀY: ÉP chữ trạng thái luôn nằm bên phải khi thẻ thay đổi kích thước
            if (uiLblTrangThai != null)
            {
                uiLblTrangThai.Left = this.Width - uiLblTrangThai.Width - 8; // Số 8 là khoảng cách lề
            }
        }

        

        // ========================================================
        // 3. LOGIC HOẠT ĐỘNG
        // ========================================================
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (this.TrangThai == "Đang thuê" && ThoiGianNhanPhong.HasValue)
            {
                TimeSpan thoiGianO = DateTime.Now - ThoiGianNhanPhong.Value;
                string daO = $"{(int)thoiGianO.TotalHours}h {thoiGianO.Minutes}p";

                if (ThoiGianTraPhong.HasValue)
                {
                    uiLblThoiGianText.Text = $"{daO} | Out {ThoiGianTraPhong.Value.ToString("HH:mm")}";
                }
                else
                {
                    uiLblThoiGianText.Text = daO;
                }
            }
        }

        private void DangKySuKienClick(Control ctrl)
        {
            if (ctrl != this)
            {
                ctrl.Click += (sender, e) => this.OnClick(e);
            }

            foreach (Control child in ctrl.Controls)
            {
                DangKySuKienClick(child);
            }
        }

        public void CapNhatGiaoDien()
        {
            // Gán dữ liệu cơ bản
            uiLblMaPhong.Text = this.MaPhong;
            uiLblTrangThai.Text = this.TrangThai;

            // Xử lý Text Tên khách và Icon lớn
            if (this.TrangThai == "Đang thuê" || this.TrangThai == "Đã đặt")
            {
                uiLblTenKhach.Text = this.TenKhachHang.ToUpper();
                uiLblMainIcon.Text = "👤";
            }
            else if (this.TrangThai == "Đang dọn")
            {
                uiLblTenKhach.Text = "ĐANG DỌN";
                uiLblMainIcon.Text = "🧹";
            }
            else
            {
                uiLblTenKhach.Text = "PHÒNG TRỐNG";
                uiLblMainIcon.Text = "✔";
            }

            // Xử lý Đồng hồ đếm
            if (this.TrangThai == "Đang thuê")
            {
                if (ThoiGianNhanPhong.HasValue)
                {
                    TimeSpan thoiGianO = DateTime.Now - ThoiGianNhanPhong.Value;
                    string daO = $"{(int)thoiGianO.TotalHours}h {thoiGianO.Minutes}p";

                    if (ThoiGianTraPhong.HasValue)
                    {
                        uiLblThoiGianText.Text = $"{daO} | Out {ThoiGianTraPhong.Value.ToString("HH:mm")}";
                    }
                    else
                    {
                        uiLblThoiGianText.Text = daO;
                    }
                    timerDemGio.Start();
                }
                else
                {
                    uiLblThoiGianText.Text = "Lỗi: Thiếu giờ Check-in";
                    timerDemGio.Stop();
                }
            }
            else
            {
                uiLblThoiGianText.Text = this.ThoiGian;
                timerDemGio.Stop();
            }

            // Phối màu nền chuẩn xịn theo Trạng thái
            if (this.TrangThai == "Trống")
            {
                uiPnlTop.BackColor = Color.FromArgb(82, 190, 128); // Xanh lá
                uiLblCleanIcon.Text = "✔";
                uiLblCleanStatus.Text = "Đã dọn dẹp";
                uiLblCleanStatus.ForeColor = Color.DimGray;
                uiLblCleanIcon.ForeColor = Color.DimGray;
            }
            else if (this.TrangThai == "Đang thuê")
            {
                uiPnlTop.BackColor = Color.FromArgb(231, 76, 60); // Đỏ 
                uiLblCleanIcon.Text = "✕";
                uiLblCleanStatus.Text = "Chưa dọn";
                uiLblCleanStatus.ForeColor = Color.Red;
                uiLblCleanIcon.ForeColor = Color.Red;
            }
            else if (this.TrangThai == "Đã đặt")
            {
                uiPnlTop.BackColor = Color.FromArgb(105, 105, 105); // Xám
                uiLblCleanIcon.Text = "✔";
                uiLblCleanStatus.Text = "Đã dọn dẹp";
                uiLblCleanStatus.ForeColor = Color.DimGray;
                uiLblCleanIcon.ForeColor = Color.DimGray;
            }
            else if (this.TrangThai == "Đang dọn")
            {
                uiPnlTop.BackColor = Color.Orange; // Vàng cam
                uiLblCleanIcon.Text = "🧹";
                uiLblCleanStatus.Text = "Đang dọn...";
                uiLblCleanStatus.ForeColor = Color.Orange;
                uiLblCleanIcon.ForeColor = Color.Orange;
            }

            // Neo lại vị trí chữ Trạng thái
            // Giảm số 15 xuống 5 để chữ xích sát về bên phải hơn
            uiLblTrangThai.Left = this.Width - uiLblTrangThai.Width - 2;
            this.Invalidate();
        }
    }
}
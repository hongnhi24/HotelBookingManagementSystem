namespace HotelManagement.GUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btQLNhanVien = new System.Windows.Forms.Button();
            this.btDichVu = new System.Windows.Forms.Button();
            this.btReport = new System.Windows.Forms.Button();
            this.btRoom = new System.Windows.Forms.Button();
            this.btCustomer = new System.Windows.Forms.Button();
            this.btInvoice = new System.Windows.Forms.Button();
            this.btBooking = new System.Windows.Forms.Button();
            this.btQLDatPhong = new System.Windows.Forms.Button();
            this.pictureSlide = new System.Windows.Forms.PictureBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbCaLamViec = new System.Windows.Forms.ComboBox();
            this.btCheckout = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btCheckin = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.slideTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSlide)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panel1.AutoScrollMargin = new System.Drawing.Size(220, 0);
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panel1.Controls.Add(this.btQLNhanVien);
            this.panel1.Controls.Add(this.btDichVu);
            this.panel1.Controls.Add(this.btReport);
            this.panel1.Controls.Add(this.btRoom);
            this.panel1.Controls.Add(this.btCustomer);
            this.panel1.Controls.Add(this.btInvoice);
            this.panel1.Controls.Add(this.btBooking);
            this.panel1.Controls.Add(this.btQLDatPhong);
            this.panel1.Location = new System.Drawing.Point(0, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(286, 632);
            this.panel1.TabIndex = 0;
            // 
            // btQLNhanVien
            // 
            this.btQLNhanVien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btQLNhanVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btQLNhanVien.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btQLNhanVien.ForeColor = System.Drawing.Color.White;
            this.btQLNhanVien.Location = new System.Drawing.Point(-6, 540);
            this.btQLNhanVien.Name = "btQLNhanVien";
            this.btQLNhanVien.Size = new System.Drawing.Size(292, 89);
            this.btQLNhanVien.TabIndex = 6;
            this.btQLNhanVien.Text = "QL Nhân Viên";
            this.btQLNhanVien.UseVisualStyleBackColor = false;
            this.btQLNhanVien.Click += new System.EventHandler(this.btQLNhanVien_Click);
            // 
            // btDichVu
            // 
            this.btDichVu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btDichVu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDichVu.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btDichVu.ForeColor = System.Drawing.Color.White;
            this.btDichVu.Location = new System.Drawing.Point(-6, 276);
            this.btDichVu.Name = "btDichVu";
            this.btDichVu.Size = new System.Drawing.Size(292, 95);
            this.btDichVu.TabIndex = 5;
            this.btDichVu.Text = "QL Dịch Vụ";
            this.btDichVu.UseVisualStyleBackColor = false;
            this.btDichVu.Click += new System.EventHandler(this.btDichVu_Click);
            // 
            // btReport
            // 
            this.btReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btReport.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btReport.ForeColor = System.Drawing.Color.White;
            this.btReport.Location = new System.Drawing.Point(-6, 451);
            this.btReport.Name = "btReport";
            this.btReport.Size = new System.Drawing.Size(292, 89);
            this.btReport.TabIndex = 4;
            this.btReport.Text = "Báo Cáo";
            this.btReport.UseVisualStyleBackColor = false;
            this.btReport.Click += new System.EventHandler(this.btReport_Click);
            // 
            // btRoom
            // 
            this.btRoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRoom.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRoom.ForeColor = System.Drawing.Color.White;
            this.btRoom.Location = new System.Drawing.Point(-6, -7);
            this.btRoom.Name = "btRoom";
            this.btRoom.Size = new System.Drawing.Size(292, 110);
            this.btRoom.TabIndex = 0;
            this.btRoom.Text = "Phòng";
            this.btRoom.UseVisualStyleBackColor = false;
            this.btRoom.Click += new System.EventHandler(this.btRoom_Click);
            // 
            // btCustomer
            // 
            this.btCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCustomer.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCustomer.ForeColor = System.Drawing.Color.White;
            this.btCustomer.Location = new System.Drawing.Point(-6, 188);
            this.btCustomer.Name = "btCustomer";
            this.btCustomer.Size = new System.Drawing.Size(292, 95);
            this.btCustomer.TabIndex = 1;
            this.btCustomer.Text = "Khách Hàng";
            this.btCustomer.UseVisualStyleBackColor = false;
            this.btCustomer.Click += new System.EventHandler(this.btCustomer_Click);
            // 
            // btInvoice
            // 
            this.btInvoice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btInvoice.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btInvoice.ForeColor = System.Drawing.Color.White;
            this.btInvoice.Location = new System.Drawing.Point(0, 366);
            this.btInvoice.Name = "btInvoice";
            this.btInvoice.Size = new System.Drawing.Size(286, 95);
            this.btInvoice.TabIndex = 3;
            this.btInvoice.Text = "Hóa Đơn";
            this.btInvoice.UseVisualStyleBackColor = false;
            this.btInvoice.Click += new System.EventHandler(this.btInvoice_Click);
            // 
            // btBooking
            // 
            this.btBooking.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBooking.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBooking.ForeColor = System.Drawing.Color.White;
            this.btBooking.Location = new System.Drawing.Point(-6, 98);
            this.btBooking.Name = "btBooking";
            this.btBooking.Size = new System.Drawing.Size(292, 95);
            this.btBooking.TabIndex = 2;
            this.btBooking.Text = "Đặt Phòng";
            this.btBooking.UseVisualStyleBackColor = false;
            this.btBooking.Click += new System.EventHandler(this.btBooking_Click);
            // 
            // btQLDatPhong
            // 
            this.btQLDatPhong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btQLDatPhong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btQLDatPhong.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btQLDatPhong.ForeColor = System.Drawing.Color.White;
            this.btQLDatPhong.Location = new System.Drawing.Point(0, 0);
            this.btQLDatPhong.Name = "btQLDatPhong";
            this.btQLDatPhong.Size = new System.Drawing.Size(292, 95);
            this.btQLDatPhong.TabIndex = 7;
            this.btQLDatPhong.Text = "QL Đặt Phòng";
            this.btQLDatPhong.UseVisualStyleBackColor = false;
            this.btQLDatPhong.Click += new System.EventHandler(this.btQLDatPhong_Click);
            // 
            // pictureSlide
            // 
            this.pictureSlide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureSlide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureSlide.Location = new System.Drawing.Point(0, 0);
            this.pictureSlide.Name = "pictureSlide";
            this.pictureSlide.Size = new System.Drawing.Size(1318, 630);
            this.pictureSlide.TabIndex = 1;
            this.pictureSlide.TabStop = false;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.cbbCaLamViec);
            this.panelTop.Controls.Add(this.btCheckout);
            this.panelTop.Controls.Add(this.btnMenu);
            this.panelTop.Controls.Add(this.btCheckin);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1318, 41);
            this.panelTop.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(806, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "Chọn ca làm:";
            // 
            // cbbCaLamViec
            // 
            this.cbbCaLamViec.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbCaLamViec.FormattingEnabled = true;
            this.cbbCaLamViec.Location = new System.Drawing.Point(920, 5);
            this.cbbCaLamViec.Name = "cbbCaLamViec";
            this.cbbCaLamViec.Size = new System.Drawing.Size(96, 33);
            this.cbbCaLamViec.TabIndex = 1;
            // 
            // btCheckout
            // 
            this.btCheckout.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCheckout.Location = new System.Drawing.Point(1156, 0);
            this.btCheckout.Name = "btCheckout";
            this.btCheckout.Size = new System.Drawing.Size(113, 37);
            this.btCheckout.TabIndex = 0;
            this.btCheckout.Text = "Check out";
            this.btCheckout.UseVisualStyleBackColor = true;
            this.btCheckout.Click += new System.EventHandler(this.btCheckout_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMenu.Font = new System.Drawing.Font("Franklin Gothic Medium", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.Location = new System.Drawing.Point(0, 0);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(82, 41);
            this.btnMenu.TabIndex = 0;
            this.btnMenu.Text = "≡";
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btCheckin
            // 
            this.btCheckin.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCheckin.Location = new System.Drawing.Point(1039, 0);
            this.btCheckin.Name = "btCheckin";
            this.btCheckin.Size = new System.Drawing.Size(111, 36);
            this.btCheckin.TabIndex = 0;
            this.btCheckin.Text = "Check in";
            this.btCheckin.UseVisualStyleBackColor = true;
            this.btCheckin.Click += new System.EventHandler(this.btCheckin_Click);
            // 
            // panelContent
            // 
            this.panelContent.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 41);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1318, 589);
            this.panelContent.TabIndex = 1;
            // 
            // slideTimer
            // 
            this.slideTimer.Interval = 3000;
            this.slideTimer.Tick += new System.EventHandler(this.slideTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(1318, 630);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureSlide);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureSlide)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btRoom;
        private System.Windows.Forms.Button btReport;
        private System.Windows.Forms.Button btInvoice;
        private System.Windows.Forms.Button btBooking;
        private System.Windows.Forms.Button btCustomer;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.PictureBox pictureSlide;
        private System.Windows.Forms.Timer slideTimer;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btDichVu;
        private System.Windows.Forms.Button btQLNhanVien;
        private System.Windows.Forms.Button btCheckin;
        private System.Windows.Forms.Button btCheckout;
        private System.Windows.Forms.ComboBox cbbCaLamViec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btQLDatPhong;
    }
}
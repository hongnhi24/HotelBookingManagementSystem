namespace HotelManagement.GUI
{
    partial class FrmThemDichVu
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
            this.btLuu = new System.Windows.Forms.Button();
            this.btHuy = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvDsDichVu = new System.Windows.Forms.DataGridView();
            this.dgvDsDaChon = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbLoaiDichVu = new System.Windows.Forms.ComboBox();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDsDichVu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDsDaChon)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btLuu
            // 
            this.btLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btLuu.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btLuu.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLuu.Location = new System.Drawing.Point(964, 555);
            this.btLuu.Name = "btLuu";
            this.btLuu.Size = new System.Drawing.Size(156, 54);
            this.btLuu.TabIndex = 1;
            this.btLuu.Text = "Lưu";
            this.btLuu.UseVisualStyleBackColor = false;
            this.btLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btHuy
            // 
            this.btHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btHuy.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btHuy.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btHuy.Location = new System.Drawing.Point(1160, 555);
            this.btHuy.Name = "btHuy";
            this.btHuy.Size = new System.Drawing.Size(160, 54);
            this.btHuy.TabIndex = 1;
            this.btHuy.Text = "Thoát";
            this.btHuy.UseVisualStyleBackColor = false;
            this.btHuy.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.35531F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.64469F));
            this.tableLayoutPanel1.Controls.Add(this.dgvDsDichVu, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvDsDaChon, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(27, 123);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1365, 302);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // dgvDsDichVu
            // 
            this.dgvDsDichVu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDsDichVu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDsDichVu.Location = new System.Drawing.Point(3, 3);
            this.dgvDsDichVu.Name = "dgvDsDichVu";
            this.dgvDsDichVu.RowHeadersWidth = 51;
            this.dgvDsDichVu.RowTemplate.Height = 24;
            this.dgvDsDichVu.Size = new System.Drawing.Size(695, 296);
            this.dgvDsDichVu.TabIndex = 0;
            this.dgvDsDichVu.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDsDichVu_CellContentClick);
            // 
            // dgvDsDaChon
            // 
            this.dgvDsDaChon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDsDaChon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDsDaChon.Location = new System.Drawing.Point(704, 3);
            this.dgvDsDaChon.Name = "dgvDsDaChon";
            this.dgvDsDaChon.RowHeadersWidth = 51;
            this.dgvDsDaChon.RowTemplate.Height = 24;
            this.dgvDsDaChon.Size = new System.Drawing.Size(658, 296);
            this.dgvDsDaChon.TabIndex = 0;
            this.dgvDsDaChon.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDsDaChon_CellContentClick);
            this.dgvDsDaChon.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDsDaChon_CellValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(671, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 29);
            this.label1.TabIndex = 11;
            this.label1.Text = "THÊM DỊCH VỤ";
            // 
            // cbbLoaiDichVu
            // 
            this.cbbLoaiDichVu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbLoaiDichVu.FormattingEnabled = true;
            this.cbbLoaiDichVu.Location = new System.Drawing.Point(30, 47);
            this.cbbLoaiDichVu.Name = "cbbLoaiDichVu";
            this.cbbLoaiDichVu.Size = new System.Drawing.Size(175, 28);
            this.cbbLoaiDichVu.TabIndex = 12;
            this.cbbLoaiDichVu.Tag = "";
            this.cbbLoaiDichVu.Text = "Chọn loại dịch vụ";
            this.cbbLoaiDichVu.SelectedIndexChanged += new System.EventHandler(this.cbbLoaiDichVu_SelectedIndexChanged);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimKiem.Location = new System.Drawing.Point(232, 47);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(194, 28);
            this.txtTimKiem.TabIndex = 13;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtTimKiem);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.cbbLoaiDichVu);
            this.panel1.Location = new System.Drawing.Point(41, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1424, 462);
            this.panel1.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(728, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 25);
            this.label3.TabIndex = 14;
            this.label3.Text = "Danh Sách Đã Chọn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(25, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 25);
            this.label2.TabIndex = 14;
            this.label2.Text = "Danh Sách Dịch Vụ";
            // 
            // FrmThemDichVu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1477, 637);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btHuy);
            this.Controls.Add(this.btLuu);
            this.Controls.Add(this.panel1);
            this.Name = "FrmThemDichVu";
            this.Text = "FrmThemDichVu";
            this.Load += new System.EventHandler(this.FrmThemDichVu_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDsDichVu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDsDaChon)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btLuu;
        private System.Windows.Forms.Button btHuy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvDsDichVu;
        private System.Windows.Forms.DataGridView dgvDsDaChon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbLoaiDichVu;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
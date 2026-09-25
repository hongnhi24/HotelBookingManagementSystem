namespace HotelManagement.GUI
{
    partial class Booking
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
            this.flpSoDoPhong = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpSoDoPhong
            // 
            this.flpSoDoPhong.AutoScroll = true;
            this.flpSoDoPhong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSoDoPhong.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSoDoPhong.Location = new System.Drawing.Point(0, 0);
            this.flpSoDoPhong.Name = "flpSoDoPhong";
            this.flpSoDoPhong.Size = new System.Drawing.Size(1057, 864);
            this.flpSoDoPhong.TabIndex = 0;
            // 
            // Booking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 864);
            this.Controls.Add(this.flpSoDoPhong);
            this.Name = "Booking";
            this.Text = "Booking";
            this.Load += new System.EventHandler(this.Booking_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpSoDoPhong;
    }
}
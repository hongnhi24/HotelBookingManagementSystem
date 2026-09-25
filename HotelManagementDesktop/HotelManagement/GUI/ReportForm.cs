using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HotelManagement.GUI
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            // 1. Nạp dữ liệu cho 2 ComboBox (Tháng / Năm)
            for (int i = 1; i <= 12; i++) cbbThang.Items.Add(i);
            for (int i = 2020; i <= DateTime.Now.Year + 1; i++) cbbNam.Items.Add(i);

            cbbThang.SelectedItem = DateTime.Now.Month;
            cbbNam.SelectedItem = DateTime.Now.Year;

            // Đăng ký sự kiện đổi tháng/năm là load lại
            cbbThang.SelectedIndexChanged += (s, ev) => LoadBaoCaoThongKe();
            cbbNam.SelectedIndexChanged += (s, ev) => LoadBaoCaoThongKe();

            LoadBaoCaoThongKe();
           
        }

        private void ReportForm_Shown(object sender, EventArgs e)
        {
           
            this.ActiveControl = null;

            // Đặt lại ComboBox về trạng thái không được chọn nếu cần
            cbbThang.SelectionLength = 0;
            cbbNam.SelectionLength = 0;
        }

        private void LoadBaoCaoThongKe()
        {
            if (cbbThang.SelectedItem == null || cbbNam.SelectedItem == null) return;

            int thang = (int)cbbThang.SelectedItem;
            int nam = (int)cbbNam.SelectedItem;

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                // ========================================================
                // 1. TÍNH TOÁN DỮ LIỆU CHO 3 THẺ TỔNG QUAN
                // ========================================================
                var dsHoaDonThang = db.Invoices
                                      .Where(i => i.PaymentDate.Value.Month == thang
                                               && i.PaymentDate.Value.Year == nam)
                                      .ToList();

                decimal tongDoanhThu = 0;
                decimal tongTienDichVu = 0;

                foreach (var hd in dsHoaDonThang)
                {
                    tongDoanhThu += Convert.ToDecimal(hd.TotalAmount);
                    var tienDV = db.BookingServices
                                   .Where(bs => bs.BookingID == hd.BookingID)
                                   .Sum(bs => (decimal?)(bs.Quantity * bs.Price)) ?? 0;
                    tongTienDichVu += tienDV;
                }

                decimal tongTienPhong = tongDoanhThu - tongTienDichVu;
                int soPhongDat = db.Bookings.Count(b => b.CheckInDate.Value.Month == thang && b.CheckInDate.Value.Year == nam);

                lblDoanhThuPhong.Text = tongTienPhong.ToString("N0") + " VNĐ";
                lblDoanhThuDichVu.Text = tongTienDichVu.ToString("N0") + " VNĐ";
                lblSoLuongPhong.Text = soPhongDat.ToString() + " Phòng";

                // ========================================================
                // 2. VẼ BIỂU ĐỒ TRÒN 
                // ========================================================
                chartTron.Series.Clear();

                if (chartTron.Legends.Count > 0)
                {
                    chartTron.Legends[0].Docking = Docking.Bottom;
                    chartTron.Legends[0].Alignment = StringAlignment.Center;
                }

                Series seriesTron = chartTron.Series.Add("DoanhThu");
                seriesTron.ChartType = SeriesChartType.Pie;

               
                seriesTron.Label = "#PERCENT{P2}";
                seriesTron.LegendText = "#VALX (#PERCENT{P2})";

                if (tongDoanhThu > 0)
                {
                    int ptPhong = seriesTron.Points.AddXY("Doanh thu phòng", tongTienPhong);
                    seriesTron.Points[ptPhong].Color = Color.FromArgb(253, 184, 19); // Vàng

                    int ptDV = seriesTron.Points.AddXY("Doanh thu dịch vụ", tongTienDichVu);
                    seriesTron.Points[ptDV].Color = Color.FromArgb(33, 150, 243); // Xanh dương
                }

                // ========================================================
                // 3. VẼ BIỂU ĐỒ VÙNG CONG MỜ 
                // ========================================================
                chartVung.Series.Clear();

                
                chartVung.ChartAreas[0].AxisX.Interval = 1;
                chartVung.ChartAreas[0].AxisX.Minimum = 1;
                chartVung.ChartAreas[0].AxisX.Maximum = 12;
                chartVung.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
                chartVung.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

                // Hàm hỗ trợ tạo đường cong có đổ bóng
                Series TaoDuongBieuDo(string ten, Color mauSac)
                {
                    Series s = chartVung.Series.Add(ten);

                    // CHÍNH LÀ DÒNG NÀY BIẾN NÓ THÀNH BIỂU ĐỒ NHƯ TRONG HÌNH BẠN MUỐN
                    s.ChartType = SeriesChartType.SplineArea;

                    s.BorderWidth = 2;
                    s.BorderColor = mauSac;
                    s.Color = Color.FromArgb(40, mauSac); // Đổ nền màu mờ 40%

                    // Tạo cục chấm tròn ở các mốc tháng
                    s.MarkerStyle = MarkerStyle.Circle;
                    s.MarkerSize = 6;
                    s.MarkerColor = Color.White;
                    s.MarkerBorderColor = mauSac;
                    s.MarkerBorderWidth = 2;
                    return s;
                }

              
                Series sTong = TaoDuongBieuDo("Tổng doanh thu", Color.FromArgb(253, 184, 19)); // Vàng
                Series sPhong = TaoDuongBieuDo("Doanh thu phòng", Color.FromArgb(33, 150, 243)); // Xanh
                Series sDV = TaoDuongBieuDo("Doanh thu DV", Color.FromArgb(231, 76, 60)); // Đỏ

                // Quét dữ liệu 12 tháng
                for (int m = 1; m <= 12; m++)
                {
                    var hdTrongThang = db.Invoices.Where(i => i.PaymentDate.Value.Month == m && i.PaymentDate.Value.Year == nam).ToList();
                    decimal tTong = 0;
                    decimal tDV = 0;

                    foreach (var hd in hdTrongThang)
                    {
                        tTong += Convert.ToDecimal(hd.TotalAmount);
                        tDV += db.BookingServices.Where(bs => bs.BookingID == hd.BookingID).Sum(bs => (decimal?)(bs.Quantity * bs.Price)) ?? 0;
                    }
                    decimal tPhong = tTong - tDV;

                    sTong.Points.AddXY(m, tTong);
                    sPhong.Points.AddXY(m, tPhong);
                    sDV.Points.AddXY(m, tDV);
                }
            }
        }

        private void btnInBaoCao_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn tháng/năm chưa
            if (cbbThang.SelectedItem == null || cbbNam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Tháng và Năm để in báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int thang = (int)cbbThang.SelectedItem;
            int nam = (int)cbbNam.SelectedItem;

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                // 1. Lọc hóa đơn theo tháng/năm
                var dsHoaDon = db.Invoices
                                 .Where(i => i.PaymentDate.HasValue
                                          && i.PaymentDate.Value.Month == thang
                                          && i.PaymentDate.Value.Year == nam)
                                 .ToList();

                
                if (dsHoaDon.Count == 0)
                {
                    MessageBox.Show($"Không có dữ liệu doanh thu nào trong Tháng {thang}/{nam} để in báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

              
                List<HoaDonReportDTO> listData = new List<HoaDonReportDTO>();
                foreach (var hd in dsHoaDon)
                {
                    HoaDonReportDTO dto = new HoaDonReportDTO();
                    dto.MaHoaDon = hd.InvoiceID;
                    dto.NgayLap = hd.PaymentDate.HasValue ? hd.PaymentDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";

               
                    string tenNV = "Không xác định";
                    if (!string.IsNullOrEmpty(hd.MaNV))
                    {
                        var nv = db.Employees.FirstOrDefault(x => x.MaNV == hd.MaNV);
                        if (nv != null) tenNV = nv.HoTen; 
                    }
                    dto.TenNhanVien = tenNV;

                    dto.MaCTPhieuThue = hd.BookingID ?? 0;
                    dto.TongTien = Convert.ToDecimal(hd.TotalAmount);

                    listData.Add(dto);
                }

                // 3. Mở Form In và đẩy dữ liệu vào
                FrmInBaoCao frmIn = new FrmInBaoCao();

            
                Microsoft.Reporting.WinForms.ReportDataSource rds = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetHoaDon", listData);

                frmIn.reportViewer1.LocalReport.DataSources.Clear();
                frmIn.reportViewer1.LocalReport.DataSources.Add(rds);

             
                frmIn.reportViewer1.RefreshReport();
                frmIn.ShowDialog();
            }
        }
    }
}
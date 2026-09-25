using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement
{
    internal class HoaDonReportDTO
    {
        public int MaHoaDon { get; set; }
        public string NgayLap { get; set; }
        public string TenNhanVien { get; set; }
        public int MaCTPhieuThue { get; set; }
        public decimal TongTien { get; set; }
    }
}

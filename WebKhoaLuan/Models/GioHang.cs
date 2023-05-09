using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKhoaLuan.Models
{

    public class GioHang
    {
        QL_LAMBANHEntities db = new QL_LAMBANHEntities();
        public int imaSP { set; get; }
        public string sTenSP { set; get; }
        public string sHinhAnh { set; get; }
        public double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public GioHang(int maSP)
        {
            imaSP = maSP;
            SANPHAM sanpham = db.SANPHAMs.Single(s => s.MASP == imaSP);
            sTenSP = sanpham.TENSP;
            sHinhAnh = sanpham.HINH;
            dDonGia = double.Parse((sanpham.GIAGOC - (sanpham.GIAGOC * sanpham.MUCKM) / 100).ToString());
            iSoLuong = 1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKhoaLuan.Models;

namespace WebKhoaLuan.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        QL_LAMBANHEntities db = new QL_LAMBANHEntities();
        public ActionResult Index()
        {
            return View();
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ms, string strURL)
        {
            if (Session["kh"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                List<GioHang> lstGioHang = LayGioHang();
                GioHang SanPham = lstGioHang.Find(sp => sp.imaSP == ms);
                if (lstGioHang.SingleOrDefault(s => s.imaSP == ms) == null)
                {

                    SanPham = new GioHang(ms);
                    lstGioHang.Add(SanPham);
                    return Redirect(strURL);
                }
                else
                {
                    ViewBag.TB = "Đã có sản phẩm trong giỏ!";
                    SanPham.iSoLuong++;
                    return Redirect(strURL);
                }
            }
        }
        private int tongSoLuong()
        {
            int sl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                sl += lstGioHang.Sum(sp => sp.iSoLuong);
            }
            return sl;
        }
        private double tongThanhTien()
        {
            double ttt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(sp => sp.dThanhTien);
            }
            return ttt;
        }


        public ActionResult GioHang()
        {
            if (Session["kh"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            if (Session["GioHang"] == null)
            {
                ViewBag.cart = "Chưa có sản phẩm nào ở đây hết chơn :(!";
                return View();
            }
            else
            {
                List<GioHang> lstGioHang = LayGioHang();
                ViewBag.tongSoLuong = tongSoLuong();
                ViewBag.TongThanhTien = tongThanhTien();

                return View(lstGioHang);
            }
        }
        [HttpPost]
        public ActionResult EditQuantity(int MASP, FormCollection c)
        {
            if (Session["kh"] == null)
            {
                return RedirectToAction("Signin", "KhachHang");
            }
            else
            {
                TAIKHOAN kh = Session["kh"] as TAIKHOAN;
                List<GioHang> lstGioHang = LayGioHang();
                GioHang sp = lstGioHang.Single(s => s.imaSP == MASP);
                if (sp != null)
                {
                    sp.iSoLuong = int.Parse(c["quantity"].ToString());
                }
                return RedirectToAction("GioHang", "GioHang");

            }
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.tongSoLuong = tongSoLuong();
            ViewBag.TongThanhTien = tongThanhTien();


            return PartialView();
        }

        public ActionResult XoaGioHang(int MASP)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(s => s.imaSP == MASP);
            if (sp != null)
            {
                lstGioHang.RemoveAll(s => s.imaSP == MASP);
                return RedirectToAction("GioHang", "GioHang");
            }
            else
            {
                return RedirectToAction("SanPhamPartial", "SanPham");

            }
            //return RedirectToAction("GioHang", "GioHang");
        }
        public ActionResult XoaGioHang_All()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("SanPhamPartial", "SanPham");
        }
        public double CheckDC()
        {
            TAIKHOAN kh = Session["kh"] as TAIKHOAN;

            string s = kh.DIACHI;
            string[] arr = s.Split(',');

            string[] dsDCNT = { "1", "2", "3", "4", "5", "6", "7", "8", "10", "11", "Bình Thạnh", "Gò Vấp", "Phú Nhuận", "Tân Bình", "Tân Phú" };
            string[] dsDCNT1 = { "9", "12", "Thủ Đức", "Bình Tân", "Nhà Bè", "Bình Chánh", "Hóc Môn", "Củ Chi" };
            int i = 0;
            float ship = 0;
            for (i = 0; i < dsDCNT.Length; i++)
            {
                if (arr[arr.Length - 2].Contains(dsDCNT[i]) == true)
                {
                    return ship = 0 ;
                    
                }
            }
            if (i == dsDCNT.Length)
            {
                int j = 0;
                for (j = 0; j < dsDCNT1.Length; j++)
                {
                    if (arr[arr.Length - 2].Contains(dsDCNT1[j]) == true)
                    {
                        return  ship = 15000;
                        
                    }
                }
                if (j == dsDCNT1.Length)
                {
                    return  ship = 30000;
                }
            }
            return ship;
        }
        public ActionResult DatHang()
        {
            if (Session["kh"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<GioHang> lstGioHang = LayGioHang();
                ViewBag.TongSoLuong = tongSoLuong();
                ViewBag.TongTien = tongThanhTien();
                ViewBag.TongThanhTien = tongThanhTien() + CheckDC();
                 
                return View(lstGioHang);
            }
        }

        public int maxhd()
        {
            List<DONHANG> lst = db.DONHANGs.ToList();
            int count = lst.Count;
            return count;
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DONHANG ddh = new DONHANG();
            TAIKHOAN kh = (TAIKHOAN)Session["kh"];
            List<GioHang> gh = LayGioHang();

            TimeSpan timeSpan = new System.TimeSpan(7, 0, 0, 0);
            DateTime ngayDat = DateTime.Now;
            DateTime ngayGiao = ngayDat.Add(timeSpan);
            var check = f["checkpttt"];
            float tienship = float.Parse(f["txtTienship"]);

            ddh.MADH = "HDBH"+ maxhd().ToString();// nvarchar50
            ddh.USERNAME = kh.USERNAME;
            ddh.NGAYDAT = ngayDat;
            ddh.NGAYGIAO = ngayGiao;
            ddh.DIACHI = f["txtDC"];//nvarcharmax
            ddh.SDTGIAO = f["txtSDT"];//varchar11
            ddh.PHISHIP = tienship;
            ddh.TONGTIEN = tongThanhTien() + tienship;//float
            ddh.MAGIAOHANG = 1; //int


            if (check == "NH")
            {
                ddh.TINHTRANGTHANHTOAN = false; //bool
                ddh.MAPTTT = 2;//int
            }
            if (check == "PP")
            {
                ddh.TINHTRANGTHANHTOAN = true; //bool
                ddh.MAPTTT = 1;//int
            }
            db.DONHANGs.Add(ddh);
            db.SaveChanges();

            foreach (var item in gh)
            {
                CHITIETDH ctdh = new CHITIETDH();
                ctdh.MADH = ddh.MADH;
                ctdh.MASP = item.imaSP;
                ctdh.USERNAME = ddh.USERNAME;
                ctdh.SOLUONG = item.iSoLuong;
                ctdh.DONGIA = item.dDonGia;
                db.CHITIETDHs.Add(ctdh);
            }
            db.SaveChanges();
            Session["HoaDon"] = ddh;
            Session["ThanhToan"] = db.PTTHANHTOANs.Single(x => x.MAPTTT == ddh.MAPTTT).HINHTHUCTT;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }


        public ActionResult XacNhanDonHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult XacNhanDonHang(FormCollection f)
        {
            Session["HoaDon"] = null;
            Session["ThanhToan"] = null;
            Session["GioHang"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
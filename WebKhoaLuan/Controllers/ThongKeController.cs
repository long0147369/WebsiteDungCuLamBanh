using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKhoaLuan.Models;

namespace WebKhoaLuan.Controllers
{
    public class ThongKeController : Controller
    {
        QL_LAMBANHEntities db = new QL_LAMBANHEntities();
        // GET: ThongKe

        //get number of week
        public int getWeek(DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public ActionResult Index()
        {
            //get member
            List<TAIKHOAN> listkh = db.TAIKHOANs.Where(kh => kh.TRANGTHAI != null && kh.PHANQUYEN == 1).ToList();
            ViewBag.kh = listkh;

            //get so item da ban
            int slItem = 0;
            if (db.CHITIETDHs.ToList().Count() > 0)
            {
                slItem = db.CHITIETDHs.Sum(c => c.SOLUONG.HasValue ? c.SOLUONG.Value : 0);
            }

            ViewBag.slItem = slItem;

            //get sl trong tuan
            List<DONHANG> lhd = db.DONHANGs.ToList();
            List<DONHANG> lhdWeek = new List<DONHANG>();
            List<CHITIETDH> lct = new List<CHITIETDH>();
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString(); //Lấy số lượng người truy cập
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString(); //Lấy số lượng người truy cập
            foreach (DONHANG h in lhd)
            {
                if (getWeek(DateTime.Now) == getWeek((DateTime)h.NGAYDAT))
                {
                    lhdWeek.Add(h);
                }
            }
            foreach (DONHANG h in lhdWeek)
            {
                List<CHITIETDH> ct = db.CHITIETDHs.Where(c => c.MADH == h.MADH).ToList();
                foreach (CHITIETDH cc in ct)
                {
                    lct.Add(cc);
                }
            }

            ViewBag.itemWeek = lct.Count();

            //get Tong thu nhap $$
            List<CHITIETDH> lcthd = db.CHITIETDHs.OrderBy(c => c.MADH).ToList();
            double tong = 0;
            foreach (CHITIETDH c in lcthd)
            {
                tong += ((c.SOLUONG.HasValue ? c.SOLUONG.Value : 0) * (c.DONGIA.HasValue ? c.DONGIA.Value : 0));
            }
            ViewBag.tongtien = tong;




            ViewBag.TongDoanhThu = ThongKeTongDoanhThu(); //thong ke doanh thu
            ViewBag.TongDDH = ThongKeDonHang(); //thong ke don hang
            ViewBag.TongTV = ThongKeThanhVien(); //thong ke thanh vien
            return View(db.CHITIETDHs.ToList());
        }

        public decimal ThongKeTongDoanhThu()
        {

            //all doanh thu
            decimal TongDoanhThu = decimal.Parse(db.CHITIETDHs.Sum(n => n.SOLUONG * n.DONGIA).Value.ToString());
            return TongDoanhThu;
        }

        public decimal ThongKeTongDoanhThuThang(int Thang, int Nam)
        {
            //theo thang, nam tuong ung
            var lstDDH = db.DONHANGs.Where(n => n.NGAYDAT.Value.Month == Thang && n.NGAYDAT.Value.Year == Nam);
            decimal TongTien = 0;
            // duyệt chi tiết của DDH và lấy tổng tiền của tất ca các sản phẩm thuộc đơn hàng đó
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.CHITIETDHs.Sum(n => n.SOLUONG * n.DONGIA).Value.ToString());
            }
            return TongTien;
        }


        public double ThongKeDonHang()
        {
            //Đếm đơn hàng đã đặt
            double slDDH = db.DONHANGs.Count();
            return slDDH;
        }

        public double ThongKeThanhVien()
        {
            //Đếm thành viên đã đặt
            double slDDH = db.TAIKHOANs.Count();
            ViewBag.tktv = slDDH;
            return slDDH;
        }

        public ActionResult GetData()
        {
            var query = db.CHITIETDHs.Include("SANPHAM")
                .GroupBy(p => p.SANPHAM.TENSP)
                .Select(g => new { name = g.Key, count = g.Sum(w => w.SOLUONG) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData1()
        {
            var query = db.CHITIETDHs.Include("DONHANG")
                .GroupBy(p => p.DONHANG.MADH)
                .Select(g => new { name = g.Key, count = g.Sum(w => w.DONGIA) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
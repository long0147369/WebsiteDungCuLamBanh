using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebKhoaLuan.Models;

namespace WebKhoaLuan.Controllers
{
    public class DatHangController : Controller
    {
        private QL_LAMBANHEntities db = new QL_LAMBANHEntities();

        // GET: DatHang

        public ActionResult DatHang()
        {
            List<NHACC> nhacclst = db.NHACCs.ToList();
            ViewBag.nhacclst = new SelectList(nhacclst, "MANCC", "TENNCC");
            ViewBag.lstCTPDH = db.CTPHIEUDATHANGs;
            ViewBag.ListSanPham = db.SANPHAMs;
            return View();
        }


        public ActionResult GetDistrict(int MANCC)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SANPHAM> splst = db.SANPHAMs.Where(x => x.MANCC == MANCC).ToList();
            return Json(splst, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DatHang(PHIEUDATHANG model, IEnumerable<CTPHIEUDATHANG> lstModel)
        {


            ViewBag.MaNCC = db.NHACCs;
            ViewBag.ListSanPham = db.SANPHAMs;

            db.PHIEUDATHANGs.Add(model);
            db.CTPHIEUDATHANGs.AddRange(lstModel);
            db.SaveChanges();
            return Redirect("PhieuDatHang");
        }

        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            var lstSP = db.SANPHAMs.Where(n => n.SOLUONG <= 5);
            return View(lstSP);
        }
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NHACCs.OrderBy(n => n.TENNCC), "MANCC", "TENNCC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);

        }
        [HttpPost]
        public ActionResult NhapHangDon(PHIEUDATHANG model, CTPHIEUDATHANG ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NHACCs.OrderBy(n => n.TENNCC), "MANCC", "TENNCC", model.MANCC);
            model.NGAYDAT = DateTime.Now;
            db.PHIEUDATHANGs.Add(model);
            db.SaveChanges();
            ctpn.MAPHIEUDH = model.MAPHIEUDH;
            SANPHAM sp = db.SANPHAMs.Single(n => n.MASP == ctpn.MASP);
            sp.SOLUONG += ctpn.SOLUONGDAT;
            db.CTPHIEUDATHANGs.Add(ctpn);
            db.SaveChanges();
            return View(sp);

        }
        //public ActionResult PhieuDatHang()
        //{
        //    var pHIEUDATHANGs = db.PHIEUDATHANGs.Include(p => p.CTPHIEUDATHANGs).Include(p => p.TAIKHOAN);
        //    return View(pHIEUDATHANGs.ToList());

        //}
        public ActionResult PhieuDatHang(int? page)
        {

            var list = db.PHIEUDATHANGs.OrderByDescending(s => s.MAPHIEUDH).ToList().ToPagedList(page ?? 1, 10);
            return View(list);
        }
        public ActionResult CTPhieuDatHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUDATHANG model = db.PHIEUDATHANGs.SingleOrDefault(n => n.MAPHIEUDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var lstCTPDH = db.CTPHIEUDATHANGs.Where(n => n.MAPHIEUDH == id);
            ViewBag.LstCTPDH = lstCTPDH;
            return View(model);
        }




        //public ActionResult PhieuNhapHang()
        //{

        //    ViewBag.MaNCC = db.NHACCs;
        //    ViewBag.ListSanPham = db.SANPHAMs;
        //    var pHIEUNHAPHANGs = db.PHIEUNHAPHANGs.Include(p => p.NHACC).Include(p => p.PHIEUDATHANG).Include(p => p.TAIKHOAN);
        //    return View(pHIEUNHAPHANGs.ToList());
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

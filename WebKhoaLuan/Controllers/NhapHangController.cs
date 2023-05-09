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
    public class NhapHangController : Controller
    {
        private QL_LAMBANHEntities db = new QL_LAMBANHEntities();

        // GET: NhapHang
        //public ActionResult Index()
        //{

        //    var pHIEUNHAPHANGs = db.PHIEUNHAPHANGs.Include(p => p.NHACC).Include(p => p.PHIEUDATHANG).Include(p => p.TAIKHOAN);
        //    return View(pHIEUNHAPHANGs.ToList());
        //}
        public ActionResult Index(int? page)
        {

            var list = db.PHIEUNHAPHANGs.OrderByDescending(s => s.MAPHIEUNH).ToList().ToPagedList(page ?? 1, 10);
            return View(list);
        }

        // GET: NhapHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUNHAPHANG pHIEUNHAPHANG = db.PHIEUNHAPHANGs.Find(id);
            if (pHIEUNHAPHANG == null)
            {
                return HttpNotFound();
            }
            return View(pHIEUNHAPHANG);
        }

        // GET: NhapHang/Create
        public ActionResult Create()
        {
            ViewBag.MANCC = new SelectList(db.NHACCs, "MANCC", "TENNCC");
            ViewBag.MAPHIEUDH = new SelectList(db.PHIEUDATHANGs, "MAPHIEUDH", "USERNAME");
            ViewBag.USERNAME = new SelectList(db.TAIKHOANs, "USERNAME", "HOTEN");
            return View();
        }

        // POST: NhapHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAPHIEUNH,MAPHIEUDH,USERNAME,MANCC,NGAYNHAP,TONGTIEN")] PHIEUNHAPHANG pHIEUNHAPHANG)
        {
            if (ModelState.IsValid)
            {
                db.PHIEUNHAPHANGs.Add(pHIEUNHAPHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MANCC = new SelectList(db.NHACCs, "MANCC", "TENNCC", pHIEUNHAPHANG.MANCC);
            ViewBag.MAPHIEUDH = new SelectList(db.PHIEUDATHANGs, "MAPHIEUDH", "USERNAME", pHIEUNHAPHANG.MAPHIEUDH);
            ViewBag.USERNAME = new SelectList(db.TAIKHOANs, "USERNAME", "HOTEN", pHIEUNHAPHANG.USERNAME);
            return View(pHIEUNHAPHANG);
        }



        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTPHIEUNHAPHANG cTPHIEUNHAPHANG = db.CTPHIEUNHAPHANGs.Find(id);
            if (cTPHIEUNHAPHANG == null)
            {
                return HttpNotFound();
            }
            return View(cTPHIEUNHAPHANG);
        }

        // POST: CTPHIEUNHAPHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MACTPNH,MAPHIEUNH,MASP,USERNAME,SOLUONGNHAP,DONVITINH,SLTUNGDV,GIANHAP,THANHTIEN")] CTPHIEUNHAPHANG cTPHIEUNHAPHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTPHIEUNHAPHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cTPHIEUNHAPHANG);
        }






        //// POST: NhapHang/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PHIEUNHAPHANG pHIEUNHAPHANG = db.PHIEUNHAPHANGs.Find(id);
        //    db.PHIEUNHAPHANGs.Remove(pHIEUNHAPHANG);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete1(int id)
        {
            try
            {
                if (db.PHIEUNHAPHANGs.SingleOrDefault(x => x.MAPHIEUNH == id) != null)
                {
                    PHIEUNHAPHANG g = db.PHIEUNHAPHANGs.SingleOrDefault(x => x.MAPHIEUNH == id);
                    db.PHIEUNHAPHANGs.Remove(g);
                    db.SaveChanges();
                    return RedirectToAction("XemPNH", "NhapHang");
                }
            }
            catch
            {
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            try
            {
                if (db.CTPHIEUNHAPHANGs.SingleOrDefault(x => x.MACTPNH == id) != null)
                {
                    CTPHIEUNHAPHANG g = db.CTPHIEUNHAPHANGs.SingleOrDefault(x => x.MACTPNH == id);
                    db.CTPHIEUNHAPHANGs.Remove(g);
                    db.SaveChanges();
                    return RedirectToAction("Index", "NhapHang");
                }
            }
            catch
            {
            }
            return View();
        }

        [HttpGet]
        public ActionResult XemPNH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUNHAPHANG model = db.PHIEUNHAPHANGs.SingleOrDefault(n => n.MAPHIEUNH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var lstCTPNN = db.CTPHIEUNHAPHANGs.Where(n => n.MAPHIEUNH == id);
            ViewBag.lstCTPNN = lstCTPNN;
            return View(model);
        }

        [HttpPost]
        public ActionResult NhapHang(PHIEUNHAPHANG model, IEnumerable<CTPHIEUNHAPHANG> lstModel)
        {
            ViewBag.MaNCC = db.NHACCs;
            ViewBag.ListSanPham = db.SANPHAMs;
            //db.PHIEUNHAPHANGs.Add(model);
            //db.SaveChanges();
            SANPHAM sp;
            foreach (var item in lstModel)
            {
                sp = db.SANPHAMs.Single(n => n.MASP == item.MASP);
                sp.SOLUONG += item.SOLUONGNHAP;
                item.MAPHIEUNH = model.MAPHIEUNH;
            }
            //db.CTPHIEUNHAPHANGs.AddRange(lstModel);
            db.SaveChanges();
            return View();
        }

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

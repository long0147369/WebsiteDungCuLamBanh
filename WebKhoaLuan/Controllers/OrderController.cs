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
    public class OrderController : Controller
    {
        private QL_LAMBANHEntities db = new QL_LAMBANHEntities();

        // GET: Order


        public ActionResult Index(int? page)
        {
            var list = db.DONHANGs.OrderByDescending(s => s.MADH).ToList().ToPagedList(page ?? 1, 10);
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(int? page, FormCollection f)
        {
            var ten = f["timkiem"];
            var ListDH = db.DONHANGs.Where(x => x.MADH.Contains(ten)).ToList();
            if (page == null) page = 1;
            var links = (from l in db.DONHANGs.Where(x => x.MADH.Contains(ten))
                         select l).OrderByDescending(s => s.MADH);
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(links.ToPagedList(pageNumber, pageSize));

        }



        public ActionResult ChuaXacNhan()
        {
            var lst = db.DONHANGs.Where(n => n.MAGIAOHANG == 1).OrderBy(n => n.MAGIAOHANG);
            return View(lst);
        }
        public ActionResult DangGiao()
        {
            var lst = db.DONHANGs.Where(n => n.MAGIAOHANG == 2).OrderBy(n => n.MAGIAOHANG);
            return View(lst);
        }
        public ActionResult DaGiao()
        {
            var lst = db.DONHANGs.Where(n => n.MAGIAOHANG == 3).OrderBy(n => n.MAGIAOHANG);
            return View(lst);
        }

        public ActionResult DaHuy()
        {
            var lst = db.DONHANGs.Where(n => n.MAGIAOHANG == 4).OrderBy(n => n.MAGIAOHANG);
            return View(lst);
        }

        // GET: Order/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG dONHANG = db.DONHANGs.Find(id);
            if (dONHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONHANG);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.MAGIAOHANG = new SelectList(db.GIAOHANGs, "MAGIAOHANG", "TRANGTHAI");
            ViewBag.MAPTTT = new SelectList(db.PTTHANHTOANs, "MAPTTT", "HINHTHUCTT");
            ViewBag.USERNAME = new SelectList(db.TAIKHOANs, "USERNAME", "HOTEN");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MADH,USERNAME,DIACHI,NGAYDAT,NGAYGIAO,MAGIAOHANG,SDTGIAO,TONGTIEN,TINHTRANGTHANHTOAN,MAPTTT,PHISHIP")] DONHANG dONHANG)
        {
            if (ModelState.IsValid)
            {
                db.DONHANGs.Add(dONHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MAGIAOHANG = new SelectList(db.GIAOHANGs, "MAGIAOHANG", "TRANGTHAI", dONHANG.MAGIAOHANG);
            ViewBag.MAPTTT = new SelectList(db.PTTHANHTOANs, "MAPTTT", "HINHTHUCTT", dONHANG.MAPTTT);
            ViewBag.USERNAME = new SelectList(db.TAIKHOANs, "USERNAME", "HOTEN", dONHANG.USERNAME);
            return View(dONHANG);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG dONHANG = db.DONHANGs.Find(id);
            if (dONHANG == null)
            {
                return HttpNotFound();
            }
            dONHANG.DHCollection = new List<GIAOHANG>
            {
                new GIAOHANG(){MAGIAOHANG=1, TRANGTHAI = "Chưa xác nhận" },
                new GIAOHANG(){MAGIAOHANG=2, TRANGTHAI = "Đang giao" },
                new GIAOHANG(){MAGIAOHANG=4, TRANGTHAI = "Đã hủy" },
            };


            ViewBag.MAGIAOHANG = new SelectList(db.GIAOHANGs, "MAGIAOHANG", "TRANGTHAI", dONHANG.MAGIAOHANG);
            ViewBag.MAPTTT = new SelectList(db.PTTHANHTOANs, "MAPTTT", "HINHTHUCTT", dONHANG.MAPTTT);
            ViewBag.USERNAME = new SelectList(db.TAIKHOANs, "USERNAME", "HOTEN", dONHANG.USERNAME);
            return View(dONHANG);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MADH,USERNAME,DIACHI,NGAYDAT,NGAYGIAO,MAGIAOHANG,SDTGIAO,TONGTIEN,TINHTRANGTHANHTOAN,MAPTTT,PHISHIP")] DONHANG dONHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dONHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAGIAOHANG = new SelectList(db.GIAOHANGs, "MAGIAOHANG", "TRANGTHAI", dONHANG.MAGIAOHANG);
            ViewBag.MAPTTT = new SelectList(db.PTTHANHTOANs, "MAPTTT", "HINHTHUCTT", dONHANG.MAPTTT);
            ViewBag.USERNAME = new SelectList(db.TAIKHOANs, "USERNAME", "HOTEN", dONHANG.USERNAME);
            return View(dONHANG);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG dONHANG = db.DONHANGs.Find(id);
            if (dONHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONHANG);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DONHANG dONHANG = db.DONHANGs.Find(id);
            db.DONHANGs.Remove(dONHANG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DuyetDonHang(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG model = db.DONHANGs.SingleOrDefault(n => n.MADH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.CHITIETDHs.Where(n => n.MADH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpGet]
        public ActionResult XemDH(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG model = db.DONHANGs.SingleOrDefault(n => n.MADH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.CHITIETDHs.Where(n => n.MADH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        //public ActionResult PrintAll()
        //{
        //    var q = new ActionAsPdf("Index");
        //    return q;
        //}


        public ActionResult Status(string id)
        {
            DONHANG dONHANG = db.DONHANGs.Find(id);
            int status = (dONHANG.MAGIAOHANG == 1) ? 2 : 1;
            dONHANG.MAGIAOHANG = status;

            db.Entry(dONHANG).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DangGiao");
        }

        public ActionResult Status1(string id)
        {
            DONHANG dONHANG = db.DONHANGs.Find(id);
            int status = (dONHANG.MAGIAOHANG == 1) ? 4 : 1;
            dONHANG.MAGIAOHANG = status;

            db.Entry(dONHANG).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DaHuy");
        }

        public ActionResult Status2(string id)
        {
            DONHANG dONHANG = db.DONHANGs.Find(id);
            int status = (dONHANG.MAGIAOHANG == 2) ? 3 : 2;
            bool status1 = (dONHANG.TINHTRANGTHANHTOAN == false) ? true : true;
            dONHANG.MAGIAOHANG = status;
            dONHANG.TINHTRANGTHANHTOAN = status1;
            db.Entry(dONHANG).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DaGiao");
        }

      


        [HttpPost]
        public ActionResult ChuaXacNhan(int? page, FormCollection f)
        {
            var ten = f["timkiem"];
            var ListDH = db.DONHANGs.Where(x => x.MADH.Contains(ten)).ToList();
            if (page == null) page = 1;
            var links = (from l in db.DONHANGs.Where(x => x.MADH.Contains(ten))
                         select l).OrderByDescending(s => s.MADH);

            int pageSize = 8;
            int pageNumber = (page ?? 1);



            return View(links.ToPagedList(pageNumber, pageSize));

        }

        [HttpPost]
        public ActionResult DangGiao(int? page, FormCollection f)
        {
            var ten = f["timkiem"];
            var ListDH = db.DONHANGs.Where(x => x.MADH.Contains(ten)).ToList();
            if (page == null) page = 1;
            var links = (from l in db.DONHANGs.Where(x => x.MADH.Contains(ten))
                         select l).OrderByDescending(s => s.MADH);
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(links.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult DaGiao(int? page, FormCollection f)
        {
            var ten = f["timkiem"];
            var ListDH = db.DONHANGs.Where(x => x.MADH.Contains(ten)).ToList();
            if (page == null) page = 1;
            var links = (from l in db.DONHANGs.Where(x => x.MADH.Contains(ten))
                         select l).OrderByDescending(s => s.MADH);
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(links.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult DaHuy(int? page, FormCollection f)
        {
            var ten = f["timkiem"];
            var ListDH = db.DONHANGs.Where(x => x.MADH.Contains(ten)).ToList();
            if (page == null) page = 1;
            var links = (from l in db.DONHANGs.Where(x => x.MADH.Contains(ten))
                         select l).OrderByDescending(s => s.MADH);

            int pageSize = 8;
            int pageNumber = (page ?? 1);



            return View(links.ToPagedList(pageNumber, pageSize));

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

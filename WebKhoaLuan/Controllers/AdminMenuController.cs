using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebKhoaLuan.Models;


namespace WebKhoaLuan.Controllers
{
    public class AdminMenuController : Controller
    {
        // GET: AdminMenu

        QL_LAMBANHEntities db = new QL_LAMBANHEntities();
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
        public ActionResult Dashboard()
        {
            //get member
            List<TAIKHOAN> listkh = db.TAIKHOANs.Where(kh => kh.TRANGTHAI != null && kh.PHANQUYEN == 1).ToList();
            ViewBag.kh = listkh;

            //get so item da ban
            int slItem = 0;
            if (db.CHITIETDHs.ToList().Count() < 10)
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

            return View(db.CHITIETDHs.ToList());
        }


        //Get SP
        public ActionResult Index(int? page)
        {

            var list = db.SANPHAMs.OrderByDescending(s => s.MASP).ToList().ToPagedList(page ?? 1, 10);
            return View(list);
        }

        [HttpPost]
        public ActionResult InDex(int? page, FormCollection f)
        {
            var ten = f["timkiem"];
            var ListDH = db.SANPHAMs.Where(x => x.TENSP.Contains(ten)).ToList();
            if (page == null) page = 1;
            var links = (from l in db.SANPHAMs.Where(x => x.TENSP.Contains(ten))
                         select l).OrderByDescending(s => s.TENSP);

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(links.ToPagedList(pageNumber, pageSize));
        }


        //Detail
        public ActionResult Details(int? masp)
        {
            if (masp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(masp);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // GET: SANPHAMs/Create
        public ActionResult Create()
        {
            
            ViewBag.MUCKM = new SelectList(db.KHUYENMAIs, "MUCKM", "NOIDUNGKM");
            ViewBag.MALOAI = new SelectList(db.LOAISPs, "MALOAI", "TENLOAI");
            ViewBag.MANCC = new SelectList(db.NHACCs, "MANCC", "TENNCC");
            return View();

        }


        [HttpPost]
        public ActionResult TaoMoi(HttpPostedFileBase HinhAnh)
        {

            return View();
        }
        // POST: SANPHAMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MASP,TENSP,MOTA,TRONGLUONG,THANHPHAN,XUATXU,CACHDUNG,GIAGOC,HINH,PHANLOAI,MALOAI,MANCC,HANSUDUNG,SOLUONG,MUCKM")] SANPHAM sANPHAM, HttpPostedFileBase fileupload)
        {


            if (ModelState.IsValid)
            {
                if (fileupload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(fileupload.FileName);
                    string extension = Path.GetExtension(fileupload.FileName);
                    filename = filename + extension;
                    fileupload.SaveAs(Server.MapPath("~/Images/img_sp/" + filename.ToString()));
                    sANPHAM.HINH = filename;
                }
                else
                {
                    sANPHAM.HINH = sANPHAM.HINH;
                }
                db.SANPHAMs.Add(sANPHAM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            ViewBag.MUCKM = new SelectList(db.KHUYENMAIs, "MUCKM", "NOIDUNGKM", sANPHAM.MUCKM);
            ViewBag.MALOAI = new SelectList(db.LOAISPs, "MALOAI", "TENLOAI", sANPHAM.MALOAI);
            ViewBag.MANCC = new SelectList(db.NHACCs, "MANCC", "TENNCC", sANPHAM.MANCC);
            return View(sANPHAM);

        }


        // GET: SANPHAMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.MUCKM = new SelectList(db.KHUYENMAIs, "MUCKM", "NOIDUNGKM", sANPHAM.MUCKM);
            ViewBag.MALOAI = new SelectList(db.LOAISPs, "MALOAI", "TENLOAI", sANPHAM.MALOAI);
            ViewBag.MANCC = new SelectList(db.NHACCs, "MANCC", "TENNCC", sANPHAM.MANCC);
            return View(sANPHAM);
        }

        // POST: SANPHAMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MASP,TENSP,MOTA,TRONGLUONG,THANHPHAN,XUATXU,CACHDUNG,GIAGOC,HINH,PHANLOAI,MALOAI,MANCC,HANSUDUNG,SOLUONG,MUCKM")] SANPHAM sANPHAM, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                if (fileupload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(fileupload.FileName);
                    string extension = Path.GetExtension(fileupload.FileName);
                    filename = filename + extension;
                    fileupload.SaveAs(Server.MapPath("~/Images/img_sp/" + filename.ToString()));
                    sANPHAM.HINH = filename;
                }
                //if (fileupload != null)
                //{
                //    string filename = Path.GetFileNameWithoutExtension(fileupload.FileName);
                //    string extension = Path.GetExtension(fileupload.FileName);
                //    filename = filename + extension;
                //    sp.HINHMINHHOA = filename;
                //    fileupload.SaveAs(Server.MapPath("~/Content/img/products/" + filename.ToString()));
                //}
                else
                {
                    sANPHAM.HINH = sANPHAM.HINH;
                }
                db.Entry(sANPHAM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.MUCKM = new SelectList(db.KHUYENMAIs, "MUCKM", "NOIDUNGKM", sANPHAM.MUCKM);
            ViewBag.MALOAI = new SelectList(db.LOAISPs, "MALOAI", "TENLOAI", sANPHAM.MALOAI);
            ViewBag.MANCC = new SelectList(db.NHACCs, "MANCC", "TENNCC", sANPHAM.MANCC);
            return View(sANPHAM);
        }


        public ActionResult Delete(int masp)
        {
            try
            {
                if (db.SANPHAMs.SingleOrDefault(x => x.MASP == masp) != null)
                {
                    SANPHAM g = db.SANPHAMs.SingleOrDefault(x => x.MASP == masp);
                    db.SANPHAMs.Remove(g);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminMenu");
                }
            }
            catch
            {
            }
            return View();
        }

        public ActionResult Report()
        {

            return View();
        }
    }
}
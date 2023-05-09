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
    public class UserController : Controller
    {
        private QL_LAMBANHEntities db = new QL_LAMBANHEntities();


        public ActionResult Index(int? page)
        {
            var list = db.TAIKHOANs.OrderByDescending(s => s.USERNAME).ToList().ToPagedList(page ?? 1, 10);
            return View(list);
        }

        [HttpPost]
        public ActionResult InDex(int? page, FormCollection f)
        {
            var ten = f["timkiem"];
            var ListDH = db.TAIKHOANs.Where(x => x.USERNAME.Contains(ten)).ToList();
            if (page == null) page = 1;
            var links = (from l in db.TAIKHOANs.Where(x => x.USERNAME.Contains(ten))
                         select l).OrderByDescending(s => s.USERNAME);

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(links.ToPagedList(pageNumber, pageSize));
        }


        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }




        public ActionResult LockorUnlock(string id)
        {
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            bool status = (tAIKHOAN.TRANGTHAI == true) ? false : true;
            tAIKHOAN.TRANGTHAI = status;

            db.Entry(tAIKHOAN).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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

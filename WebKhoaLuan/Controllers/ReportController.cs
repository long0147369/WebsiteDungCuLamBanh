using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKhoaLuan.Models;

namespace WebKhoaLuan.Controllers
{
    public class ReportController : Controller
    {
        QL_LAMBANHEntities db = new QL_LAMBANHEntities();
        // GET: Report
        public ActionResult Index()
        {
            return View();
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
    }
}
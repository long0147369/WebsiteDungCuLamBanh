using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using WebKhoaLuan.Models;

namespace WebKhoaLuan.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        QL_LAMBANHEntities db = new QL_LAMBANHEntities();
        public ActionResult DanhMucPartial()
        {
            var listDanhMuc = db.LOAISPs.OrderBy(kh => kh.MALOAI).ToList();
            return View(listDanhMuc);
        }
        public ActionResult SPTheoDM(int ms, int ? page)
        {
            var listSP = db.SANPHAMs.Where(s => s.MALOAI == ms).OrderByDescending(s => s.MUCKM).ToList();
            LOAISP cc = db.LOAISPs.Single(t => t.MALOAI == ms);
            ViewBag.TG = cc.TENLOAI;
            if (page == null) page = 1;


            var links = db.SANPHAMs.Where(s => s.MALOAI == ms).OrderByDescending(s => s.MUCKM).ToList();
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            if (listSP.Count == 0)
            {
                ViewBag.SanPham = "Không có Sản phẩm nào!";
            }
            return View(links.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult NCCPartial()
        {
            var ListNCC = db.NHACCs.OrderBy(cc => cc.MANCC).ToList();
            ViewBag.listNcc = ListNCC;
            return View(ListNCC);
        }

        public ViewResult SPTheoNCC(int mancc, int? page)
        {
            var listsp = db.SANPHAMs.Where(s => s.MANCC == mancc).ToList();
            
            NHACC cc = db.NHACCs.Single(t => t.MANCC == mancc);
            ViewBag.TG = cc.TENNCC;
            if (page == null) page = 1;


            var links = db.SANPHAMs.Where(s => s.MANCC == mancc).OrderByDescending(s => s.MUCKM).ToList();
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            if (listsp.Count == 0)
            {
                ViewBag.SanPham = "Không có Sản phẩm nào!";
            }
            return View(links.ToPagedList(pageNumber, pageSize));






        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using WebKhoaLuan.Models;
namespace WebKhoaLuan.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QL_LAMBANHEntities db = new QL_LAMBANHEntities();
        public ActionResult SanPhamPartial(int? page)
        {
            var ListSP = db.SANPHAMs.OrderByDescending(s => s.MUCKM).ToList();
            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) { page = 1; }
            
                // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
                // theo LinkID mới có thể phân trang.
                var links = (from l in db.SANPHAMs
                             select l).OrderByDescending(x => x.MUCKM);
                // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
                int pageSize = 12;
                // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
                // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
                int pageNumber = (page ?? 1);
                // 5. Trả về các Link được phân trang theo kích thước và số trang.
                return View(links.ToPagedList(pageNumber, pageSize));
            


        }
        [HttpPost]
        public ActionResult SanPhamPartial(int? page, FormCollection f)
        {
            var ten = f["timkiem"];
            var ListSach = db.SANPHAMs.Where(x => x.TENSP.Contains(ten)).ToList();
            if (page == null) page = 1;
            var links = (from l in db.SANPHAMs.Where(x => x.TENSP.Contains(ten))
                         select l).OrderByDescending(x => x.MUCKM);
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(links.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult XemChiTiet(int ms)
        {
            SANPHAM SP = db.SANPHAMs.Single(s => s.MASP == ms);
            var a = SP.MALOAI;
            List<SANPHAM> listSPNL = db.SANPHAMs.Where(sp => sp.MALOAI == a).Take(6).ToList();
            ViewBag.spDexuat = listSPNL;
            if (SP == null)
            {
                return HttpNotFound();
            }
            return View(SP);
           
        }

        public ActionResult SanPhamGiaRe()
        {
            var listSPGR = db.SANPHAMs.OrderBy(sp => sp.GIAGOC).Take(6).ToList();

            if (listSPGR.Count == 0)
            {
                ViewBag.SanPham = "Không có Sản phẩm nào!";
            }
            return View(listSPGR);

        }
        public ActionResult SanPhamNguyenLieu()
        {
            var listSPNL = db.SANPHAMs.Where(sp => sp.PHANLOAI == "NL").Take(6).ToList();

            if (listSPNL.Count == 0)
            {
                ViewBag.SanPham = "Không có Sản phẩm nào!";
            }
            return View(listSPNL);

        }
        
    }
}
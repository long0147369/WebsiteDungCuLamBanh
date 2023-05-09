using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebKhoaLuan.Models;

namespace WebKhoaLuan.Controllers
{
    public class HomeController : Controller
    {
        QL_LAMBANHEntities db = new QL_LAMBANHEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["kh"] != null)
            {
                TAIKHOAN kh = Session["kh"] as TAIKHOAN;
                TAIKHOAN k = db.TAIKHOANs.FirstOrDefault(kk => kk.USERNAME == kh.USERNAME);
                if (k.PHANQUYEN == 3)
                {
                    return RedirectToAction("Dashboard", "AdminMenu");
                }
                return RedirectToAction("Index", "KhachHang");
            }
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection c)
        {

            var id = c["txtidUser"];
            var pass = c["txtPass"];
            if (Session["kh"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            TAIKHOAN acc = db.TAIKHOANs.FirstOrDefault(tk => tk.USERNAME == id && tk.MATKHAU == pass);
            if (acc != null)
            {
                if (acc.TRANGTHAI == true)
                {
                    if (acc.PHANQUYEN == 3)
                    {
                        Session["kh"] = acc;
                        return RedirectToAction("Dashboard", "AdminMenu");
                    }
                    else
                    {
                        Session["kh"] = acc;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    Session["tb"] = "Tài khoản Chưa được xác nhận !";
                    return RedirectToAction("DangNhap", "Home");
                }
            }
            else
            {
                Session["tb"] = "Tài khoản hoặc mật khẩu không đúng !";
                return RedirectToAction("DangNhap", "Home");
            }
        }
        public ActionResult KhachHangNow()
        {
            TAIKHOAN acc = Session["kh"] as TAIKHOAN;

            return PartialView();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(TAIKHOAN tk, FormCollection f)
        {
            var tendn = f["UserName"];
            var hoten = f["txtName"];
            var matkhau = f["Password"];
            var rematkhau = f["ConfirmPassword"];
            var dienthoai = f["PhoneNumber"];
            var email = f["Email"];
            var diachi = f["txtDiachi"];

            bool check = db.TAIKHOANs.Where(k => k.USERNAME == tendn).Count() > 0;
            bool checkmail = db.TAIKHOANs.Where(k => k.EMAIL == email).Count() > 0;
            if (check)
            {
                ViewBag.hasUser = "Tên đăng nhập" + tendn + "đã tồn tại.";
                return View();
            }
            if (checkmail)
            {
                ViewBag.hasMail = "Email " + email + " đã được đăng ký.";
                return View();
            }
            else
            {
                tk.HOTEN = hoten;
                tk.USERNAME = tendn;
                tk.MATKHAU = matkhau;
                tk.SDT = dienthoai;
                tk.EMAIL = email;
                tk.DIACHI = diachi;
                tk.PHANQUYEN = 1;
                tk.TRANGTHAI = false;
                tk.HINH = "default-avatar.jpg";
                db.TAIKHOANs.Add(tk);
                db.SaveChanges();

                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("dungculambanhhufi@gmail.com", "Dụng cụ làm bánh"),
                        new System.Net.Mail.MailAddress(tk.EMAIL));
                m.Subject = "Email confirmation";
                m.Body = string.Format("Thân gửi {0}<BR/>Cảm ơn bạn đã đăng ký tài khoản, " +
                    "Bạn vui lòng click vào link bên dưới để hoàn tất việc đăng ký :<br > " +
                    "<a href=\"{1}\" title=\"User Email Confirm\">{1}</a>"
                    + "<br >Bạn sẽ được chuyển về trang ĐĂNG NHẬP, vui lòng nhập đúng tài khoản đã đăng ký để truy cập mua sắm.<br > Chúc một ngày tốt lành. ",
                    tk.USERNAME, Url.Action("ConfirmEmail", "Home", new { Token = tk.USERNAME, Email = tk.EMAIL }, Request.Url.Scheme));
                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("dungculambanhhufi@gmail.com", "Hai123456@");
                smtp.EnableSsl = true;
                smtp.Send(m);

                return RedirectToAction("XacThuc", "Home", new { Email = tk.EMAIL });
            }

        }
        public ActionResult XacThuc(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }

        public ActionResult ConfirmEmail(string Token, string Email)
        {

            bool check1 = db.TAIKHOANs.Where(k => k.USERNAME == Token).Count() > 0;
            bool check2 = db.TAIKHOANs.Where(k => k.EMAIL == Email).Count() > 0;
            TAIKHOAN user = db.TAIKHOANs.Where(k => k.USERNAME == Token).Single();
            if (check1)
            {
                if (check2)
                {
                    user.TRANGTHAI = true;

                    db.SaveChanges();
                    Session["ok"] = "Tài khoản " + Token + "đã được đăng ký thành công!";
                    ViewBag.success = "OK";
                    return RedirectToAction("DangNhap", "Home");
                }
                else
                {
                    return RedirectToAction("XacThuc", "Home", new { Email = user.EMAIL });
                }
            }
            else
            {
                return RedirectToAction("XacThuc", "Home", new { Email = "" });
            }

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SanPhamPhanLoai()
        {
            var listSP = db.SANPHAMs.OrderByDescending(s => s.SOLUONG ).Take(8).ToList();
            if (listSP.Count == 0)
            {
                ViewBag.SanPham = "Không có Sản phẩm nào!";
            }
            return View(listSP);
        }
        public ActionResult ThongTinKH(string username)
        {

            if (Session["kh"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                TAIKHOAN k = Session["kh"] as TAIKHOAN;
                List<CHITIETDH> data = (from c in db.CHITIETDHs
                                        join d in db.DONHANGs
                                        on c.MADH equals d.MADH
                                        where d.USERNAME == k.USERNAME
                                        select c).ToList();
                List<DONHANG> datahd = db.DONHANGs.Where(c => c.USERNAME == k.USERNAME).ToList();
                List<PHIEUTRAHANG> datatrahang = db.PHIEUTRAHANGs.Where(h => h.USERNAME == k.USERNAME).ToList();
                ViewBag.cthd = data;
                ViewBag.lhd = datahd;
                ViewBag.pth = datatrahang;
                return View(k);
            }
        }
        public ActionResult UpdateThongTin(FormCollection c, HttpPostedFileBase fileupload)
        {
            if (Session["kh"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            TAIKHOAN kh = Session["kh"] as TAIKHOAN;
            TAIKHOAN k = db.TAIKHOANs.FirstOrDefault(kkk => kkk.USERNAME == kh.USERNAME);
            k.HOTEN = c["txtHoTen"];
            k.SDT = c["txtPhone"];
            //k.EMAIL = c["txtEmail"];
            k.DIACHI = c["txtAddress"];
            if (fileupload != null)
            {
                string filename = Path.GetFileNameWithoutExtension(fileupload.FileName);
                string extension = Path.GetExtension(fileupload.FileName);
                filename = filename + extension;
                fileupload.SaveAs(Server.MapPath("~/Images/customer/" + filename.ToString()));
                k.HINH = filename;
            }
            else
            {
                k.HINH = k.HINH;
            }    
            db.SaveChanges();
            Session["kh"] = k;
            return RedirectToAction("ThongTinKH", "Home");
        }
        public int maxhd()
        {
            List<PHIEUTRAHANG> lst = db.PHIEUTRAHANGs.ToList();
            int count = lst.Count;
            return count;
        }
        public string getName(int masp)
        {
            SANPHAM sp = db.SANPHAMs.Single(s => s.MASP == masp);
            return sp.TENSP.ToString();
        }
        [HttpGet]
        public ActionResult YCTraHang(string username)
        {
            if (Session["kh"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                TAIKHOAN k = Session["kh"] as TAIKHOAN;
                List<CHITIETDH> data = (from c in db.CHITIETDHs
                                        join d in db.DONHANGs
                                        on c.MADH equals d.MADH
                                        where d.USERNAME == k.USERNAME
                                        select c).ToList();
                List<DONHANG> datahd = db.DONHANGs.Where(c => c.USERNAME == k.USERNAME).ToList();
                ViewBag.cthd = data;
                ViewBag.lhd = datahd;
                return View(k);
            }
        }
        [HttpPost]
        public ActionResult YCTraHang(PHIEUTRAHANG pth, FormCollection f, HttpPostedFileBase fileupload)
        {
            TAIKHOAN kh = Session["kh"] as TAIKHOAN;
            if (Session["kh"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                pth.MAPHIEU = "PTH" + "-" + maxhd().ToString();
                pth.USERNAME = f["idNgtra"];
                pth.MADH = f["MaDonHang"];
                pth.MASP = int.Parse(f["MaSP"]);
                pth.TENSP = getName(int.Parse(f["MaSP"]));
                pth.NGAYTRA = DateTime.Now;
                pth.LYDO = f["txtLido"];

                if (fileupload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(fileupload.FileName);
                    string extension = Path.GetExtension(fileupload.FileName);
                    filename = filename + extension;
                    fileupload.SaveAs(Server.MapPath("~/Images/img_trahang/" + filename.ToString()));
                    pth.HINHANH = filename;
                }
                else
                {
                    pth.HINHANH = pth.HINHANH;
                }
                db.PHIEUTRAHANGs.Add(pth);
                db.SaveChanges();

                return RedirectToAction("ThongTinKH", "Home");
            }
        }
        public ActionResult HuyDon(string madh)
        {
            DateTime day = DateTime.Now;
            
            TimeSpan time = new System.TimeSpan(1, 0, 0, 0);
            DONHANG dh = db.DONHANGs.Single(s => s.MADH == madh);
            
            if (day - dh.NGAYDAT <= time)
            {
                dh.MAGIAOHANG = 4;
                Session["er"] = "Đã Hủy đơn hàng thành công";
            }
            else
            {
                Session["er"]="Đã quá thời gian hủy đơn theo quy định";
                
            }    
            db.SaveChanges();
            return RedirectToAction("ThongTinKH", "Home");
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult ChinhSach()
        {
            return View();
        }
        public ActionResult TuyenDung()
        {
            return View();
        }
        
    }
}
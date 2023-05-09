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
    public class NhaCCController : Controller
    {
        private QL_LAMBANHEntities db = new QL_LAMBANHEntities();

        // GET: NhaCC
        public ActionResult Index()
        {
            return View(db.NHACCs.ToList());
        }

        // GET: NhaCC/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NhaCC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANCC,TENNCC,SDT,DIACHI")] NHACC nHACC)
        {
            if (ModelState.IsValid)
            {
                db.NHACCs.Add(nHACC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nHACC);
        }

        // GET: NhaCC/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHACC nHACC = db.NHACCs.Find(id);
            if (nHACC == null)
            {
                return HttpNotFound();
            }
            return View(nHACC);
        }

        // POST: NhaCC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANCC,TENNCC,SDT,DIACHI")] NHACC nHACC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nHACC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nHACC);
        }

        // GET: NhaCC/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                if (db.NHACCs.SingleOrDefault(x => x.MANCC == id) != null)
                {
                    NHACC g = db.NHACCs.SingleOrDefault(x => x.MANCC == id);
                    db.NHACCs.Remove(g);
                    db.SaveChanges();
                    return RedirectToAction("Index", "NhaCC");
                }
            }
            catch
            {
            }
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

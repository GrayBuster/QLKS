using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;
using QLKS;

namespace QLKS.Controllers
{
    public class LoaiPhongsController : Controller
    {
        private QLKSEntities2 db = new QLKSEntities2();

        // GET: LoaiPhongs
        [ActionName("LoạiPhòng")]
        public ActionResult Index()
        {
            return View(db.LoaiPhong.ToList());
        }

        // GET: LoaiPhongs/Details/5
        [ActionName("ThêmLoạiPhòngTC")]
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiPhong loaiPhong = db.LoaiPhong.Find(x);
            if (loaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(loaiPhong);
        }

        // GET: LoaiPhongs/Create
        [ActionName("ThêmLoạiPhòng")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoaiPhongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("ThêmLoạiPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoai,TenLoai,Dongia")] LoaiPhong loaiPhong)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.LoaiPhong.Add(loaiPhong);
                    db.SaveChanges();
                    return RedirectToAction("ThêmLoạiPhòngTC", new { id = Encryption.encrypt(loaiPhong.MaLoai.ToString()) });
                }
            }
            catch
            {
                return View("LỗiLoạiPhòng");
            }


            return View(loaiPhong);
        }

        // GET: LoaiPhongs/Edit/5
        [ActionName("SửaLoạiPhòng")]
        public ActionResult Edit(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiPhong loaiPhong = db.LoaiPhong.Find(x);
            if (loaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(loaiPhong);
        }

        // POST: LoaiPhongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaLoạiPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoai,TenLoai,Dongia")] LoaiPhong loaiPhong)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(loaiPhong).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ThêmLoạiPhòngTC", new { id = Encryption.encrypt(loaiPhong.MaLoai.ToString()) });
                }
            }
            catch
            {
                return View("LỗiLoạiPhòng");
            }
            return View(loaiPhong);
        }

        // GET: LoaiPhongs/Delete/5
        [ActionName("XóaLoạiPhòng")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiPhong loaiPhong = db.LoaiPhong.Find(x);
            if (loaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(loaiPhong);
        }

        // POST: LoaiPhongs/Delete/5
        [HttpPost, ActionName("XóaLoạiPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            LoaiPhong loaiPhong = db.LoaiPhong.Find(x);
            try
            {
                db.LoaiPhong.Remove(loaiPhong);
                db.SaveChanges();
                return RedirectToAction("LoạiPhòng");
            }
            catch
            {
                return View("LỗiLoạiPhòng");
            }
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

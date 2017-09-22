using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS;
using QLKS.Models;

namespace QLKS.Controllers
{
    [MyAuthorize(Roles ="Admin")]
    public class QuiDinhsController : Controller
    {
        private QLKSEntities1 db = new QLKSEntities1();

        // GET: QuiDinhs
        [ActionName("QuiĐịnh")]
        public ActionResult Index()
        {
            return View(db.QuiDinhs.ToList());
        }

        // GET: QuiDinhs/Details/5
        [ActionName("ThôngTinQuiĐịnh")]
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuiDinh quiDinh = db.QuiDinhs.Find(x);
            if (quiDinh == null)
            {
                return HttpNotFound();
            }
            return View(quiDinh);
        }

        // GET: QuiDinhs/Create
        [ActionName("ThêmQuiĐịnh")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuiDinhs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("ThêmQuiĐịnh")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaQuiDinh,TenQuiDinh,MoTa,ThamSo,ThamSoTien")] QuiDinh quiDinh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.QuiDinhs.Add(quiDinh);
                    db.SaveChanges();
                    return RedirectToAction("ThôngTinQuiĐịnh", new { id = Encryption.encrypt(quiDinh.MaQuiDinh.ToString()) });
                }
            }
            catch
            {
                return View("LỗiQuiĐịnh");
            }
            return View(quiDinh);
        }

        // GET: QuiDinhs/Edit/5
        [ActionName("SửaQuiĐịnh")]
        public ActionResult Edit(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuiDinh quiDinh = db.QuiDinhs.Find(x);
            if (quiDinh == null)
            {
                return HttpNotFound();
            }
            return View(quiDinh);
        }

        // POST: QuiDinhs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaQuiĐịnh")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaQuiDinh,TenQuiDinh,MoTa,ThamSo,ThamSoTien")] QuiDinh quiDinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiDinh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ThôngTinQuiĐịnh",new { id=Encryption.encrypt(quiDinh.MaQuiDinh.ToString())});
            }
            return View(quiDinh);
        }

        // GET: QuiDinhs/Delete/5
        [ActionName("XóaQuiĐịnh")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuiDinh quiDinh = db.QuiDinhs.Find(x);
            if (quiDinh == null)
            {
                return HttpNotFound();
            }
            return View(quiDinh);
        }

        // POST: QuiDinhs/Delete/5
        [HttpPost, ActionName("XóaQuiĐịnh")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(id);
            QuiDinh quiDinh = db.QuiDinhs.Find(x);
            try
            {
                db.QuiDinhs.Remove(quiDinh);
                db.SaveChanges();
                return RedirectToAction("QuiĐịnh");
            }
            catch
            {
                return View("LỗiQuiĐịnh");
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

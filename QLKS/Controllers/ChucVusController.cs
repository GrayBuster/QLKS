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
    [MyAuthorize(Roles ="Admin,PM")]
    public class ChucVusController : Controller
    {
        private QLKSEntities2 db = new QLKSEntities2();
        // GET: ChucVus
        [ActionName("QuảnLýChứcVụ")]
        public ActionResult Index()
        {
            return View(db.ChucVu.ToList());
        }

        // GET: ChucVus/Details/5
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVu.Find(x);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // GET: ChucVus/Create
        [ActionName("ThêmChứcVụ")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChucVus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("ThêmChứcVụ")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCV,TenCV")] ChucVu chucVu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ChucVu.Add(chucVu);
                    db.SaveChanges();
                    return View("ThêmChứcVụThànhCông");
                }
            }
            catch
            {
                return View("LỗiChứcVụ");
            }
            return View(chucVu);
        }

        // GET: ChucVus/Edit/5
        [ActionName("SửaChứcVụ")]
        public ActionResult Edit(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVu.Find(x);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // POST: ChucVus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaChứcVụ")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCV,TenCV")] ChucVu chucVu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(chucVu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return View("SửaChứcVụThànhCông");
                }
            }
            catch
            {
                return View("LỗiChứcVụ");
            }
            return View(chucVu);
        }

        // GET: ChucVus/Delete/5
        [ActionName("XoáChứcVụ")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVu.Find(x);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // POST: ChucVus/Delete/5
        [HttpPost, ActionName("XoáChứcVụ")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            ChucVu chucVu = db.ChucVu.Find(x);
            try
            {
                db.ChucVu.Remove(chucVu);
                db.SaveChanges();
                return View("XoáThànhCông");
            }
            catch
            {
                return View("LỗiXoáChứcVụ");
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

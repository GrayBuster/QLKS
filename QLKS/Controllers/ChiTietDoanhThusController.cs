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
    public class ChiTietDoanhThusController : Controller
    {
        private QLKSEntities1 db = new QLKSEntities1();

        // GET: ChiTietDoanhThus
        [ActionName("TrangDoanhThu")]
        public ActionResult Index()
        {
            return View(db.ChiTietDoanhThus.ToList());
        }

        // GET: ChiTietDoanhThus/Details/5
        [ActionName("ThêmDoanhThuTC")]
        public ActionResult Details(int? id)
        {
            var decode = Encryption.decrypt(id.ToString());
            if (decode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDoanhThu chiTietDoanhThu = db.ChiTietDoanhThus.Find(decode);
            if (chiTietDoanhThu == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDoanhThu);
        }

        // GET: ChiTietDoanhThus/Create
        [ActionName("BáoCáoDoanhThu")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChiTietDoanhThus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("BáoCáoDoanhThu")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDoanhThu,LoaiPhong,Thang")] ChiTietDoanhThu chiTietDoanhThu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ChiTietDoanhThus.Add(chiTietDoanhThu);
                    db.SaveChanges();
                    return RedirectToAction("ThêmDoanhThuTC",new { id=Encryption.encrypt(chiTietDoanhThu.MaDoanhThu.ToString())});
                }
            }
            catch
            {
                return View("LỗiThêmDoanhThu");
            }
            
            return View(chiTietDoanhThu);
        }

        // GET: ChiTietDoanhThus/Edit/5
        [ActionName("SửaDoanhThu")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDoanhThu chiTietDoanhThu = db.ChiTietDoanhThus.Find(id);
            if (chiTietDoanhThu == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDoanhThu);
        }

        // POST: ChiTietDoanhThus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaDoanhThu")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDoanhThu,LoaiPhong,DoanhThu,Tile")] ChiTietDoanhThu chiTietDoanhThu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(chiTietDoanhThu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return View("SửaDoanhThuTC");
                }
            }
            catch
            {
                return View("LỗiSửaDoanhThu");
            }
            return View(chiTietDoanhThu);
        }

        // GET: ChiTietDoanhThus/Delete/5
        [ActionName("XóaDoanhThu")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDoanhThu chiTietDoanhThu = db.ChiTietDoanhThus.Find(id);
            if (chiTietDoanhThu == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDoanhThu);
        }

        // POST: ChiTietDoanhThus/Delete/5
        [HttpPost, ActionName("XóaDoanhThu")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietDoanhThu chiTietDoanhThu = db.ChiTietDoanhThus.Find(id);
            try
            {
                db.ChiTietDoanhThus.Remove(chiTietDoanhThu);
                db.SaveChanges();
                return View("XóaDoanhThuTC");
            }
            catch
            {
                return View("LỗiXóaDoanhThu");
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

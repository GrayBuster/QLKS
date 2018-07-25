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
    public class ChiTietThuePhongsController : Controller
    {
        private QLKSEntities2 db = new QLKSEntities2();

        // GET: ChiTietThuePhongs
        [ActionName("ChiTiếtThuêPhòng")]
        public ActionResult Index()
        {
            var chiTietThuePhongs = db.ChiTietThuePhong.Include(c => c.KhachHang).Include(c => c.ThuePhong);
            return View(chiTietThuePhongs.ToList());
        }

        // GET: ChiTietThuePhongs/Details/5
        [ActionName("ChiTiết")]
        public ActionResult Details(int? id,int? khachHang)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietThuePhong chiTietThuePhong = db.ChiTietThuePhong.Find(id,khachHang);
            if (chiTietThuePhong == null)
            {
                return HttpNotFound();
            }
            return View(chiTietThuePhong);
        }

        // GET: ChiTietThuePhongs/Create
        [ActionName("TạoChiTiếtThuêPhòng")]
        public ActionResult Create()
        {
            ViewBag.MaKhachHang = new SelectList(db.KhachHang, "MaKhachHang", "TenKhachHang");
            ViewBag.MaThuePhong = new SelectList(db.ThuePhong, "MaThuePhong", "MaThuePhong");
            return View();
        }

        // POST: ChiTietThuePhongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("TạoChiTiếtThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaThuePhong,MaKhachHang,SoLuongKhach")] ChiTietThuePhong chiTietThuePhong)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietThuePhong.Add(chiTietThuePhong);
                db.SaveChanges();
                return RedirectToAction("ChiTiết", new { id = chiTietThuePhong.MaThuePhong, khachHang = chiTietThuePhong.MaKhachHang });
            }

            ViewBag.MaKhachHang = new SelectList(db.KhachHang, "MaKhachHang", "TenKhachHang", chiTietThuePhong.MaKhachHang);
            ViewBag.MaThuePhong = new SelectList(db.ThuePhong, "MaThuePhong", "MaThuePhong", chiTietThuePhong.MaThuePhong);
            return View(chiTietThuePhong);
        }

        // GET: ChiTietThuePhongs/Edit/5
        [ActionName("SửaChiTiếtThuêPhòng")]
        public ActionResult Edit(int? id,int? khachHang)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietThuePhong chiTietThuePhong = db.ChiTietThuePhong.Find(id,khachHang);
            if (chiTietThuePhong == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHang, "MaKhachHang", "TenKhachHang", chiTietThuePhong.MaKhachHang);
            ViewBag.MaThuePhong = new SelectList(db.ThuePhong, "MaThuePhong", "MaThuePhong", chiTietThuePhong.MaThuePhong);
            return View(chiTietThuePhong);
        }

        // POST: ChiTietThuePhongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaChiTiếtThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaThuePhong,MaKhachHang,SoLuongKhach")] ChiTietThuePhong chiTietThuePhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietThuePhong).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ChiTiết",new { id=chiTietThuePhong.MaThuePhong,khachHang=chiTietThuePhong.MaKhachHang});
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHang, "MaKhachHang", "TenKhachHang", chiTietThuePhong.MaKhachHang);
            ViewBag.MaThuePhong = new SelectList(db.ThuePhong, "MaThuePhong", "MaThuePhong", chiTietThuePhong.MaThuePhong);
            return View(chiTietThuePhong);
        }

        // GET: ChiTietThuePhongs/Delete/5
        [ActionName("XoáChiTiếtThuêPhòng")]
        public ActionResult Delete(int? id,int? khachHang)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietThuePhong chiTietThuePhong = db.ChiTietThuePhong.Find(id);
            if (chiTietThuePhong == null)
            {
                return HttpNotFound();
            }
            return View(chiTietThuePhong);
        }

        // POST: ChiTietThuePhongs/Delete/5
        [HttpPost, ActionName("XoáChiTiếtThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,int khachHang)
        {
            ChiTietThuePhong chiTietThuePhong = db.ChiTietThuePhong.Find(id,khachHang);
            db.ChiTietThuePhong.Remove(chiTietThuePhong);
            db.SaveChanges();
            return RedirectToAction("ChiTiếtDoanhThu");
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

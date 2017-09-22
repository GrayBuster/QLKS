using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using PagedList;
using QLKS.Models;

namespace QLKS.Controllers
{
    [MyAuthorize(Roles ="Admin,TT")]
    public class PhongsController : Controller
    {
        private QLKSEntities1 db = new QLKSEntities1();
        private Phong phong = new Phong();
        // GET: Phongs
        [ActionName("TrangPhòng")]
        public ActionResult Index(string option, string searchString,int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            if (option == "MaPhong")
            {
                return View(db.Phongs.Where(x => x.MaPhong.ToString().Contains(searchString.ToUpper())).ToList().ToPagedList(pageNumber, pageSize));
            }
            else if (option == "TenPhong")
            {
                return View(db.Phongs.Where(x => x.TenPhong.ToUpper().Contains(searchString.ToUpper())).ToList().ToPagedList(pageNumber, pageSize));
            }
            else if (option == "LoaiPhong")
            {
                return View(db.Phongs.Where(x => x.LoaiPhong.TenLoai.ToUpper().Contains(searchString.ToUpper())).ToList().ToPagedList(pageNumber, pageSize));
            }
            else if (option == "DonGia")
            {
                return View(db.Phongs.Where(x => x.LoaiPhong.Dongia.ToString().Contains(searchString)).ToList().ToPagedList(pageNumber, pageSize));
            }
            else if (option == "TinhTrang")
            {
                return View(db.Phongs.ToList().Where(x => x.ChuyenKieuTinhTrang.ToUpper().Contains(searchString.ToUpper())).ToList().ToPagedList(pageNumber, pageSize));
            }
            else
                return View(db.Phongs.Include(i=>i.LoaiPhong).OrderBy(x=>x.MaPhong).ToPagedList(pageNumber, pageSize));
        }

        // GET: Phongs/Details/5
        [ActionName("TạoPhòngTC")]
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(x);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // GET: Phongs/Create
        [ActionName("TạoPhòng")]
        public ActionResult Create()
        {
            List<SelectListItem> loaiPhong = new List<SelectListItem>();
            foreach (var item in db.LoaiPhongs)
            {
                loaiPhong.Add(new SelectListItem
                {
                    Text = item.TenLoai,
                    Value = Convert.ToString(item.MaLoai)
                });
            }
            ViewBag.LoaiPhong = loaiPhong;
            return View();
        }

        // POST: Phongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("TạoPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhong,TenPhong,MaLoai,GhiChu,TinhTrang")] Phong phong)
        {
            List<SelectListItem> loaiPhong = new List<SelectListItem>();
            foreach (var item in db.LoaiPhongs)
            {
                loaiPhong.Add(new SelectListItem
                {
                    Text = item.TenLoai,
                    Value = Convert.ToString(item.MaLoai)
                });
            }
            ViewBag.LoaiPhong = loaiPhong;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Phongs.Add(phong);
                    db.SaveChanges();
                    return RedirectToAction("TạoPhòngTC",new { id=Encryption.encrypt(phong.MaPhong.ToString())});
                }
            }
            catch
            {
                return View("LỗiTạoPhòng");
            }
            return View(phong);
        }

        // GET: Phongs/Edit/5
        [ActionName("SửaPhòng")]
        public ActionResult Edit(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            List<SelectListItem> loaiPhong = new List<SelectListItem>();
            foreach (var item in db.LoaiPhongs)
            {
                loaiPhong.Add(new SelectListItem
                {
                    Text = item.TenLoai,
                    Value = Convert.ToString(item.MaLoai)
                });
            }
            ViewBag.LoaiPhong = loaiPhong;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.FirstOrDefault(p => p.MaPhong == x);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // POST: Phongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhong,TenPhong,MaLoai,GhiChu,TinhTrang")] Phong phong)
        {
            List<SelectListItem> loaiPhong = new List<SelectListItem>();
            foreach (var item in db.LoaiPhongs)
            {
                loaiPhong.Add(new SelectListItem
                {
                    Text = item.TenLoai,
                    Value = Convert.ToString(item.MaLoai)
                });
            }
            ViewBag.LoaiPhong = loaiPhong;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(phong).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return View("SửaPhòngTC");
                }
            }
            catch
            {
                //return View("LỗiSửaPhòng");
            }
            return View(phong);
        }

        // GET: Phongs/Delete/5
         [ActionName("XoáPhòng")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (decode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(x);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // POST: Phongs/Delete/5
        [HttpPost, ActionName("XoáPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            Phong phong = db.Phongs.Find(x);
            try
            {
                if (phong.TinhTrang.Value == false)
                {
                    db.Phongs.Remove(phong);
                    db.SaveChanges();
                    return View("XoáPhòngTC");
                }
                else
                    return View("DeleteConFirmed");
            }
            catch
            {
                return View("DeleteConFirmed");
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

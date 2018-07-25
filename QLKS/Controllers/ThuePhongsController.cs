using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Collections;
using QLKS.Models;

namespace QLKS.Controllers
{
    [MyAuthorize(Roles = "AM,TT,Admin")]
    public class ThuePhongsController : Controller
    {
        private QLKSEntities2 db = new QLKSEntities2();
        private Phong phong = new Phong();
        // GET: ThuePhongs
        [ActionName("TrangThuêPhòng")]
        public ActionResult Index()
        {
            var thuePhongs = db.ThuePhong.Include(t => t.Phong);
            return View(thuePhongs.ToList());
        }

        // GET: ThuePhongs/Details/5
        [ActionName("ThànhCông")]
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (decode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuePhong thuePhong = db.ThuePhong.Find(x);
            if (thuePhong == null)
            {
                return HttpNotFound();
            }
            return View(thuePhong);
        }

        // GET: ThuePhongs/Create
        [ActionName("ThuêPhòng")]
        public ActionResult Create()
        {
            List<SelectListItem> tenPhong = new List<SelectListItem>();
            foreach (var item in db.Phong)
            {
                if (item.TinhTrang == false)
                {

                    tenPhong.Add(new SelectListItem
                    {
                        Text = item.TenPhong,
                        Value = item.MaPhong.ToString()
                    });
                }
            }
            ViewBag.MaPhong = /*new SelectList(db.Phongs, "MaPhong", "MaPhong")*/tenPhong;
            //ViewBag.MaPhong = new SelectList(db.Phongs, "MaPhong", "MaPhong");
            return View();
        }

        // POST: ThuePhongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("ThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhong,NgayBatDauThue")] ThuePhong thuePhong)
        {
            try
            {
                  if (ModelState.IsValid)
                  {
                        {
                             db.ThuePhong.Add(thuePhong);
                             db.SaveChanges();
                             return RedirectToAction("ThànhCông",new { id=Encryption.encrypt(thuePhong.MaThuePhong.ToString())});
                        }
                  }
            }
            catch
            {
                return View("LỗiThuêPhòng");
            }
            List<SelectListItem> tenPhong = new List<SelectListItem>();
            foreach (var item in db.Phong)
            {
                if (item.TinhTrang == false)
                {

                    tenPhong.Add(new SelectListItem
                    {
                        Text = item.TenPhong,
                        Value = item.MaPhong.ToString()
                    });
                }
            }
            ViewBag.MaPhong = tenPhong;
            return View(thuePhong);
        }

        // GET: ThuePhongs/Edit/5
        [ActionName("SửaThuêPhòng")]
        public ActionResult Edit(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (decode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuePhong thuePhong = db.ThuePhong.Find(x);
            if (thuePhong == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> tenPhong = new List<SelectListItem>();
            foreach (var item in db.Phong)
            {
                if (item.TinhTrang == false)
                {

                    tenPhong.Add(new SelectListItem
                    {
                        Text = item.TenPhong,
                        Value = item.MaPhong.ToString()
                    });
                }
            }
            ViewBag.MaPhong = /*new SelectList(db.Phongs, "MaPhong", "MaPhong")*/tenPhong;
            return View(thuePhong);
        }

        // POST: ThuePhongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaThuePhong,MaPhong,NgayBatDauThue")] ThuePhong thuePhong)
        {
            try {
                if (ModelState.IsValid)
                {
                    db.Entry(thuePhong).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return View("SửaThànhCông");
                }
            }

            catch
            {
                return View("LỗiSửaTP");
            }
            List<SelectListItem> tenPhong = new List<SelectListItem>();
            foreach (var item in db.Phong)
            {
                if (item.TinhTrang == false)
                {

                    tenPhong.Add(new SelectListItem
                    {
                        Text = item.TenPhong,
                        Value = item.MaPhong.ToString()
                    });
                }
            }
            ViewBag.MaPhong = /*new SelectList(db.Phongs, "MaPhong", "MaPhong")*/tenPhong;
            return View(thuePhong);
        }

        // GET: ThuePhongs/Delete/5
        [ActionName("XoáThuêPhòng")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (decode == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuePhong thuePhong = db.ThuePhong.Find(x);
            if (thuePhong == null)
            {
                return HttpNotFound();
            }
            return View(thuePhong);
        }

        // POST: ThuePhongs/Delete/5
        [HttpPost, ActionName("XoáThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            try
            {
                ThuePhong thuePhong = db.ThuePhong.Find(x);
                db.ThuePhong.Remove(thuePhong);
                db.SaveChanges();
                return View("XoáThànhCôngTP");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return View("LỗiXoáThuêPhòng");
            }
        }

        [ActionName("ChiTiếtThuêPhòng")]
        public ActionResult ChiTietThuePhong()
        {
            var chiTietThuePhongs = db.ChiTietThuePhong.Include(c => c.KhachHang).Include(c => c.ThuePhong);
            return PartialView(chiTietThuePhongs.ToList());
        }

        [ActionName("TạoChiTiếtThuêPhòng")]
        public ActionResult TaoChiTietThuePhong()
        {
            List<SelectListItem> tenKh = new List<SelectListItem>();
            foreach (var item in db.KhachHang)
            {
                tenKh.Add(new SelectListItem
                {
                    Text = item.TenKhachHang,
                    Value = item.MaKhachHang.ToString()
                });
            }
            ViewBag.MaKhachHang = /*new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", chiTietThuePhong.MaKhachHang)*/tenKh;
            List<SelectListItem> maTP = new List<SelectListItem>();
            foreach (var i in db.ThuePhong)
            {
                maTP.Add(new SelectListItem
                {
                    Text = i.MaThuePhong.ToString(),
                    Value = i.MaThuePhong.ToString()
                });
            }
            ViewBag.MaThuePhong = /*new SelectList(db.ThuePhongs, "MaThuePhong", "MaThuePhong")*/maTP;
            return PartialView();
        }

        // POST: ChiTietThuePhongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("TạoChiTiếtThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult TaoChiTietThuePhong([Bind(Include = "MaThuePhong,MaKhachHang,SoLuongKhach")] ChiTietThuePhong chiTietThuePhong)
        {
            
            List<SelectListItem> tenKh = new List<SelectListItem>();
            foreach (var item in db.KhachHang)
            {
                tenKh.Add(new SelectListItem
                {
                    Text = item.TenKhachHang,
                    Value = item.MaKhachHang.ToString()
                });
            }
            ViewBag.MaKhachHang = /*new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", chiTietThuePhong.MaKhachHang)*/tenKh;
            List<SelectListItem> maTP = new List<SelectListItem>();
            foreach (var i in db.ThuePhong)
            {
                maTP.Add(new SelectListItem
                {
                    Text = i.MaThuePhong.ToString(),
                    Value = i.MaThuePhong.ToString()
                });
            }
            ViewBag.MaThuePhong = /*new SelectList(db.ThuePhongs, "MaThuePhong", "MaThuePhong")*/maTP;
            try
            {
                if (ModelState.IsValid)
                {
                    db.ChiTietThuePhong.Add(chiTietThuePhong);
                    db.SaveChanges();
                    return View("TrangThuêPhòng");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Không thể tạo chi tiết vì đã có chi tiết thuê phòng này rồi");

            }
            return PartialView(chiTietThuePhong);
        }

        // GET: ChiTietThuePhongs/Edit/5
        [ActionName("SửaChiTiếtThuêPhòng")]
        public ActionResult SuaChiTietThuePhong(string id, string khachHang)
        {
            var decode = Encryption.decrypt(id);
            var decodeKH = Encryption.decrypt(khachHang);
            int x = int.Parse(decode);
            int y = int.Parse(decodeKH);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietThuePhong chiTietThuePhong = db.ChiTietThuePhong.Find(x, y);
            if (chiTietThuePhong == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> tenKh = new List<SelectListItem>();
            foreach(var item in db.KhachHang)
            {
                tenKh.Add(new SelectListItem {
                Text=item.TenKhachHang,
                Value=item.MaKhachHang.ToString()
                });
            }
            ViewBag.MaKhachHang = /*new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", chiTietThuePhong.MaKhachHang)*/tenKh;
            List<SelectListItem> maTP = new List<SelectListItem>();
            foreach (var i in db.ThuePhong)
            {
                        maTP.Add(new SelectListItem
                        {
                            Text = i.MaThuePhong.ToString(),
                            Value = i.MaThuePhong.ToString()
                        });
            }
            ViewBag.MaThuePhong = /*new SelectList(db.ThuePhongs, "MaThuePhong", "MaThuePhong")*/maTP;
            return View(chiTietThuePhong);
        }

        // POST: ChiTietThuePhongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaChiTiếtThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult SuaChiTietThuePhong([Bind(Include = "MaThuePhong,MaKhachHang,SoLuongKhach")] ChiTietThuePhong chiTietThuePhong)
        {
       
            List<SelectListItem> tenKh = new List<SelectListItem>();
            foreach (var item in db.KhachHang)
            {
                tenKh.Add(new SelectListItem
                {
                    Text = item.TenKhachHang,
                    Value = item.MaKhachHang.ToString()
                });
            }
            ViewBag.MaKhachHang = /*new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", chiTietThuePhong.MaKhachHang)*/tenKh;
            List<SelectListItem> maTP = new List<SelectListItem>();
            foreach (var i in db.ThuePhong)
            {
                        maTP.Add(new SelectListItem
                        {
                            Text = i.MaThuePhong.ToString(),
                            Value = i.MaThuePhong.ToString()
                        });
            }
            ViewBag.MaThuePhong = /*new SelectList(db.ThuePhongs, "MaThuePhong", "MaThuePhong")*/maTP;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(chiTietThuePhong).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("TrangThuêPhòng");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Không thể sửa chi tiết vì đã có chi tiết thuê phòng này rồi");

            }
            return View(chiTietThuePhong);
        }

        // GET: ChiTietThuePhongs/Delete/5
        [ActionName("XoáChiTiếtThuêPhòng")]
        public ActionResult XoaChiTietThuePhong(string id, string khachHang)
        {
            var decode = Encryption.decrypt(id);
            var decodeKH = Encryption.decrypt(khachHang);
            int x = int.Parse(decode);
            int y = int.Parse(decodeKH);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietThuePhong chiTietThuePhong = db.ChiTietThuePhong.Find(x,y);
            if (chiTietThuePhong == null)
            {
                return HttpNotFound();
            }
            return View(chiTietThuePhong);
        }

        // POST: ChiTietThuePhongs/Delete/5
        [HttpPost, ActionName("XoáChiTiếtThuêPhòng")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaChiTietThuePhong(string id, string khachHang, ChiTietThuePhong chiTietThuePhong)
        {
            var decode = Encryption.decrypt(id);
            var decodeKH = Encryption.decrypt(khachHang);
            int x = int.Parse(decode);
            int y = int.Parse(decodeKH);
            chiTietThuePhong = db.ChiTietThuePhong.Find(x, y);
            try
            {
                db.ChiTietThuePhong.Remove(chiTietThuePhong);
                db.SaveChanges();
                return View("ThànhCông");
            }
            catch
            {
                return View("LỗiXoáThuêPhòng");
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

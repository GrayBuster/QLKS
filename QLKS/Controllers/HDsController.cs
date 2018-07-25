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
    [MyAuthorize(Roles = "AM,Admin")]
    public class HDsController : Controller
    {
        private QLKSEntities2 db = new QLKSEntities2();
        private ThuePhong thuePhong = new ThuePhong();
        NV nV = new NV();
        // GET: HDs
        [MyAuthorize(Roles = "KT")]
        [ActionName("QuảnLýHóaĐơn")]
        public ActionResult Index()
        {
            var hDs = db.HD.Include(X=>X.NV).Include(x=>x.KhachHang).ToList();
            return View(hDs);
        }

        // GET: HDs/Details/5
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HD hD = db.HD.Find(x);
            if (hD == null)
            {
                return HttpNotFound();
            }
            return View(hD);
        }

        // GET: HDs/Create
        [ActionName("ThanhToánHóaĐơn")]
        public ActionResult Create()
        {
            List<SelectListItem> tenNV = new List<SelectListItem>();
            List<SelectListItem> tenKH = new List<SelectListItem>();
            foreach (var item in db.NV)
            {
                if (item.ChucVu.TenCV == "Ke Toan" || item.ChucVu.TenCV == "Ke Toan Truong")
                {

                    tenNV.Add(new SelectListItem
                    {
                        Text = item.TenNV,
                        Value = item.MaNV.ToString()
                    });
                }
            }
            ViewBag.MaNV = tenNV;
            foreach(var item in db.KhachHang)
            {
                tenKH.Add(new SelectListItem
                {
                    Text = item.TenKhachHang,
                    Value = item.MaKhachHang.ToString()
                });
            }
            ViewBag.MaKH = /*new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang")*/tenKH;
            return View();
        }

        // POST: HDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("ThanhToánHóaĐơn")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhachHang,MaNV,TongTien")] HD hD)
        {
            List<SelectListItem> tenKH = new List<SelectListItem>();
            foreach (var item in db.KhachHang)
            {
                tenKH.Add(new SelectListItem
                {
                    Text = item.TenKhachHang,
                    Value = Convert.ToString(item.MaKhachHang)
                });
            }
            ViewBag.MaKH = /*new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang")*/tenKH;
            List<SelectListItem> tenNV = new List<SelectListItem>();
            foreach (var item in db.NV)
            {
                if (item.ChucVu.TenCV == "Ke Toan" || item.ChucVu.TenCV == "Ke Toan Truong")
                {

                    tenNV.Add(new SelectListItem
                    {
                        Text = item.TenNV,
                        Value = Convert.ToString(item.MaNV)
                    });
                }
            }
            ViewBag.MaNV = tenNV;
            if (ModelState.IsValid)
            {
                db.HD.Add(hD);
                db.SaveChanges();
                return View("ThanhToánHoáTC");
            }

            return View(hD);
        }

        // GET: HDs/Edit/5
        [ActionName("SửaHóaĐơn")]
        public ActionResult Edit(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            List<SelectListItem> tenKH = new List<SelectListItem>();
            foreach (var item in db.KhachHang)
            {
                tenKH.Add(new SelectListItem
                {
                    Text = item.TenKhachHang,
                    Value = Convert.ToString(item.MaKhachHang)
                });
            }
            ViewBag.MaKH = /*new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang")*/tenKH;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HD hD = db.HD.Find(x);
            if (hD == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> tenNV = new List<SelectListItem>();
            foreach (var item in db.NV)
            {
                if (item.ChucVu.TenCV == "Ke Toan" || item.ChucVu.TenCV == "Ke Toan Truong")
                {

                    tenNV.Add(new SelectListItem
                    {
                        Text = item.TenNV,
                        Value = Convert.ToString(item.MaNV)
                    });
                }
            }
            ViewBag.MaNV = tenNV;
            return View(hD);
        }

        // POST: HDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaHóaĐơn")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,MaKhachHang,MaNV,TongTien")] HD hD)
        {
            List<SelectListItem> tenKH = new List<SelectListItem>();
            foreach (var item in db.KhachHang)
            {
                tenKH.Add(new SelectListItem
                {
                    Text = item.TenKhachHang,
                    Value = Convert.ToString(item.MaKhachHang)
                });
            }
            ViewBag.MaKH = /*new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang")*/tenKH;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(hD).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return View("SửaThànhCôngHóaĐơn");
                }
            }
            catch
            {
                return View("LỗiKhiSửaHĐ");
            }
            ViewBag.MaTP = new SelectList(db.ThuePhong, "MaThuePhong", "MaThuePhong", thuePhong.MaThuePhong);
            List<SelectListItem> tenNV = new List<SelectListItem>();
            foreach (var item in db.NV)
            {
                if (item.ChucVu.TenCV == "Ke Toan" || item.ChucVu.TenCV == "Ke Toan Truong")
                {

                    tenNV.Add(new SelectListItem
                    {
                        Text = item.TenNV,
                        Value = Convert.ToString(item.MaNV)
                    });
                }
            }
            ViewBag.MaNV = tenNV;
            return View(hD);
        }

        // GET: HDs/Delete/5
        [ActionName("XóaHóaĐơn")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HD hD = db.HD.Find(x);
            if (hD == null)
            {
                return HttpNotFound();
            }
            return View(hD);
        }

        // POST: HDs/Delete/5
        [HttpPost, ActionName("XóaHóaĐơn")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            HD hD = db.HD.Find(x);
            try {
                db.HD.Remove(hD);
                db.SaveChanges();
                return View("XoáTCHD");
            }
            catch
            {
                return View("LỗiKhiXóaHD");
            }
            
        }
        //Chi Tiết Hóa Đơn

        [ActionName("ChiTiếtHóaĐơn")]
        public ActionResult ChiTietHoaDon()
        {
            var chiTietHoaDons = db.ChiTietHoaDon.Include(c => c.HD).Include(c => c.ThuePhong);
            return PartialView(chiTietHoaDons.ToList());
        }

        // GET: ChiTietHoaDons/Create
        [ActionName("TạoChiTiếtHóaĐơn")]
        public ActionResult TaoChiTietHoaDon()
        {
            ViewBag.MaHD = new SelectList(db.HD, "MaHD", "MaHD");
            ViewBag.MaThuePhong = new SelectList(db.ThuePhong, "MaThuePhong", "MaThuePhong");
            return PartialView();
        }

        // POST: ChiTietHoaDons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("TạoChiTiếtHóaĐơn")]
        public ActionResult TaoChiTietHoaDon([Bind(Include = "MaHD,MaThuePhong,SoNgayThue")] ChiTietHoaDon chiTietHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietHoaDon.Add(chiTietHoaDon);
                db.SaveChanges();
                return RedirectToAction("QuảnLýHóaĐơn");
            }

            ViewBag.MaHD = new SelectList(db.HD, "MaHD", "MaHD", chiTietHoaDon.MaHD);
            ViewBag.MaThuePhong = new SelectList(db.ThuePhong, "MaThuePhong", "MaThuePhong", chiTietHoaDon.MaThuePhong);
            return PartialView(chiTietHoaDon);
        }


        // GET: ChiTietHoaDons/Edit/5
        [ActionName("SửaChiTiếtHóaĐơn")]
        public ActionResult SuaChiTietHoaDon(string id, string maThuePhong)
        {
            var decode = Encryption.decrypt(id);
            var decodeTP = Encryption.decrypt(maThuePhong);
            int x = int.Parse(decode);
            int y = int.Parse(decodeTP);
            if (id == null && maThuePhong==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDon.Find(x,y);
            if (chiTietHoaDon == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> maTP = new List<SelectListItem>();
            foreach (var item in db.ThuePhong)
            {
                maTP.Add(new SelectListItem
                {
                    Text = item.MaThuePhong.ToString(),
                    Value = item.MaThuePhong.ToString()
                });
            }
            ViewBag.MaThuePhong =/* new SelectList(db.ThuePhongs, "MaThuePhong", "MaThuePhong", chiTietHoaDon.MaThuePhong)*/maTP;
            return View(chiTietHoaDon);
        }

        // POST: ChiTietHoaDons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("SửaChiTiếtHóaĐơn")]
        public ActionResult SuaChiTietHoaDon([Bind(Include = "MaHD,MaThuePhong,NgayThanhToan,SoNgayThue,ThanhTien")] ChiTietHoaDon chiTietHoaDon)
        {
            List<SelectListItem> maTP = new List<SelectListItem>();
            foreach (var item in db.ThuePhong)
            {
                maTP.Add(new SelectListItem
                {
                    Text = item.MaThuePhong.ToString(),
                    Value = item.MaThuePhong.ToString()
                });
            }
            ViewBag.MaThuePhong =/* new SelectList(db.ThuePhongs, "MaThuePhong", "MaThuePhong", chiTietHoaDon.MaThuePhong)*/maTP;
            if (ModelState.IsValid)
            {
                db.Entry(chiTietHoaDon).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuảnLýHóaĐơn");
            }
            //ViewBag.MaHD = new SelectList(db.HDs, "MaHD", "MaHD", chiTietHoaDon.MaHD);
            //ViewBag.MaThuePhong = new SelectList(db.ThuePhongs, "MaThuePhong", "MaThuePhong", chiTietHoaDon.MaThuePhong);
            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDons/Delete/5
        [ActionName("XoáChiTiếtHóaĐơn")]
        public ActionResult XoaChiTietHoaDon(string id,string maThuePhong)
        {
            var decode = Encryption.decrypt(id);
            var decodeTP = Encryption.decrypt(maThuePhong);
            int x = int.Parse(decode);
            int y = int.Parse(decodeTP);
            if (id == null && maThuePhong==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDon.Find(x,y);
            if (chiTietHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(chiTietHoaDon);
        }

        // POST: ChiTietHoaDons/Delete/5
        [HttpPost, ActionName("XoáChiTiếtHóaĐơn")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaChiTietHoaDonConfirmed(string id,string maThuePhong)
        {
            var decode = Encryption.decrypt(id);
            var decodeTP = Encryption.decrypt(maThuePhong);
            int x = int.Parse(decode);
            int y = int.Parse(decodeTP);
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDon.Find(x,y);
            try
            {
                db.ChiTietHoaDon.Remove(chiTietHoaDon);
                db.SaveChanges();
                return RedirectToAction("QuảnLýHóaĐơn");
            }
            catch
            {
                return View("LỗiXoáChiTiếtDoanhThu");
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

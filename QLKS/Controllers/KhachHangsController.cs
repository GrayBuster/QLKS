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
    [MyAuthorize(Roles = "TT,Admin")]
    public class KhachHangsController : Controller
    {
        private QLKSEntities2 db = new QLKSEntities2();

        // GET: KhachHangs
        [ActionName("QuảnLýKháchHàng")]
        [MyAuthorize(Roles = "TT")]
        public ActionResult Index(string option,string searchString)
        {
            var khachHangs = db.KhachHang.Include(k => k.LoaiKhach);
            if (option == "TenKH")
            {
                return View(khachHangs.Where(x => x.TenKhachHang.ToUpper().Contains(searchString.ToUpper())).ToList());
            }
            else if (option == "MaKH")
            {
                return View(khachHangs.Where(x => x.MaKhachHang.ToString().ToUpper().Contains(searchString.ToUpper())).ToList());
            }
            else if (option == "CMND")
            {
                return View(khachHangs.Where(x => x.CMND.ToUpper().Contains(searchString.ToUpper())).ToList());
            }
            else if (option == "DiaChi")
            {
                return View(khachHangs.Where(x => x.DiaChi.ToUpper().Contains(searchString.ToUpper())).ToList());
            }
            else
            {
                return View(khachHangs.OrderBy(x => x.MaKhachHang).ToList());
            }
        }

        // GET: KhachHangs/Details/5
        [ActionName("ThôngTinKháchHàng")]
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (decode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(x);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Create
        [ActionName("ThêmKháchHàng")]
        public ActionResult Create()
        {
            ViewBag.MaLoaiKhach = new SelectList(db.LoaiKhach, "MaLoaiKhach", "TenLoaiKhach");
            return View();
        }

        // POST: KhachHangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("ThêmKháchHàng")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhachHang,TenKhachHang,MaLoaiKhach,DiaChi,CMND")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHang.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("ThôngTinKháchHàng",new { id= Encryption.encrypt(khachHang.MaKhachHang.ToString()) });
            }

            ViewBag.MaLoaiKhach = new SelectList(db.LoaiKhach, "MaLoaiKhach", "TenLoaiKhach", khachHang.MaLoaiKhach);
            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        [ActionName("SửaKháchHàng")]
        public ActionResult Edit(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(x);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiKhach = new SelectList(db.LoaiKhach, "MaLoaiKhach", "TenLoaiKhach", khachHang.MaLoaiKhach);
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaKháchHàng")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhachHang,TenKhachHang,MaLoaiKhach,DiaChi,CMND")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ThôngTinKháchHàng",new { id= Encryption.encrypt(khachHang.MaKhachHang.ToString()) });
            }
            ViewBag.MaLoaiKhach = new SelectList(db.LoaiKhach, "MaLoaiKhach", "TenLoaiKhach", khachHang.MaLoaiKhach);
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5
        [ActionName("XóaKháchHàng")]
        [MyAuthorize(Roles = "TT,Admin")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(x);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("XóaKháchHàng")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            KhachHang khachHang = db.KhachHang.Find(x);
            db.KhachHang.Remove(khachHang);
            db.SaveChanges();
            return RedirectToAction("QuảnLýKháchHàng");
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

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
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data.Entity.Infrastructure;

namespace QLKS.Controllers
{
    [MyAuthorize(Roles = "AM,Admin")]
    public class DoanhThusController : Controller
    {
        private QLKSEntities1 db = new QLKSEntities1();

        // GET: DoanhThus
        [MyAuthorize(Roles = "KT")]
        [ActionName("QuảnLýDoanhThu")]
        public ActionResult Index()
        {
            decimal? total = db.ChiTietDoanhThus.Select(x => x.DoanhThu).Sum();
            ViewBag.Total = total;
            return View(db.DoanhThus.ToList());
        }
        public ActionResult TrangDoanhThu()
        {
            var chiTietDoanhThu = db.ChiTietDoanhThus.ToList();
            return PartialView(chiTietDoanhThu);
        }
        [HttpGet]
        public ActionResult CreateChiTietDoanhThu()
        {
            List<SelectListItem> maDoanhThu = new List<SelectListItem>();
            foreach(var item in db.DoanhThus)
            {
                maDoanhThu.Add(new SelectListItem
                {
                    Text=item.MaDoanhThu.ToString(),
                    Value=item.MaDoanhThu.ToString()
                });
            }
            ViewBag.MaDoanhThu = maDoanhThu;
            ViewBag.TenLoai = new SelectList(db.LoaiPhongs, "TenLoai", "MaLoai");
            return PartialView();
        }

        // POST: ChiTietDoanhThus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateChiTietDoanhThu([Bind(Include = "MaDoanhThu,MaLoai")] ChiTietDoanhThu chiTietDoanhThu)
        {
            List<SelectListItem> maDoanhThu = new List<SelectListItem>();
            foreach (var item in db.DoanhThus)
            {
                maDoanhThu.Add(new SelectListItem
                {
                    Text = item.MaDoanhThu.ToString(),
                    Value = item.MaDoanhThu.ToString()
                });
            }
            ViewBag.MaDoanhThu = maDoanhThu;
            ViewBag.TenLoai = new SelectList(db.LoaiPhongs, "MaLoai", "TenLoai");
            try
            {
                if (ModelState.IsValid)
                {
                    db.ChiTietDoanhThus.Add(chiTietDoanhThu);
                    db.SaveChanges();
                    return RedirectToAction("QuảnLýDoanhThu");
                }
            }
            catch
            {
                return View("LỗiThêmDoanhThu");
            }

            return View(chiTietDoanhThu);
        }
        [HttpGet]
        public ActionResult DeleteChiTietDoanhThu(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDoanhThu chiTietDoanhThu = db.ChiTietDoanhThus.Find(x);
            if (chiTietDoanhThu == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDoanhThu);
        }

        // POST: ChiTietDoanhThus/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteChiTietDoanhThu(string id, ChiTietDoanhThu chiTietDoanhThu)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            chiTietDoanhThu = db.ChiTietDoanhThus.Find(x);
            try
            {
                db.ChiTietDoanhThus.Remove(chiTietDoanhThu);
                db.SaveChanges();
                return View("XoáThànhCôngDoanhThu");
            }
            catch
            {
                return View("LỗiXóaDoanhThu");
            }

        }
        // GET: DoanhThus/Details/5
        [ActionName("BáoCáoThànhCông")]
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoanhThu doanhThu = db.DoanhThus.Find(x);
            if (doanhThu == null)
            {
                return HttpNotFound();
            }
            return View(doanhThu);
        }

        // GET: DoanhThus/Create
        [ActionName("BáoCáoTổngDoanhThu")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DoanhThus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("BáoCáoTổngDoanhThu")]
        public ActionResult Create([Bind(Include = "MaDoanhThu,Thang")] DoanhThu doanhThu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.DoanhThus.Add(doanhThu);
                    db.SaveChanges();
                    return RedirectToAction("BáoCáoThànhCông",new { id=Encryption.encrypt(doanhThu.MaDoanhThu.ToString())});
                }
            }
           catch
            {
                return View("LỗiBáoCáoDoanhThu");
            }
            return View(doanhThu);
        }

        // GET: DoanhThus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoanhThu doanhThu = db.DoanhThus.Find(id);
            if (doanhThu == null)
            {
                return HttpNotFound();
            }
            return View(doanhThu);
        }

        // POST: DoanhThus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDoanhThu,Thang,TongDoanhThu")] DoanhThu doanhThu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doanhThu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doanhThu);
        }

        // GET: DoanhThus/Delete/5
        [ActionName("XóaDoanhThu")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoanhThu doanhThu = db.DoanhThus.Find(x);
            if (doanhThu == null)
            {
                return HttpNotFound();
            }
            return View(doanhThu);
        }

        // POST: DoanhThus/Delete/5
        [HttpPost, ActionName("XóaDoanhThu")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            DoanhThu doanhThu = db.DoanhThus.Find(x);
            try
            {
                db.DoanhThus.Remove(doanhThu);
                db.SaveChanges();
                return View("XoáThànhCôngDoanhThu");
            }
            catch
            {
                return View("LỗiXóaDoanhThu");
            }
            
        }
        public ActionResult ReportDoanhThu()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/CrystalReportChiTietDoanhThu.rpt")));
            foreach(var i in db.ChiTietDoanhThus)
            {
                foreach(var item in db.LoaiPhongs)
                {
                    if(i.MaLoai==item.MaLoai)
                    {
                        rd.SetDataSource(db.ChiTietDoanhThus.Select(p => new
                        {
                            MaDoanhThu = p.MaDoanhThu,
                            LoaiPhong = p.LoaiPhong.TenLoai,
                            Thang = p.Thang.Value,
                            DoanhThu = p.DoanhThu.Value,
                            Tile = p.Tile.Value
                        }).ToList());
                    }
                }
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream =rd.ExportToStream
                (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "BaoCaoDoanhThu.pdf");
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

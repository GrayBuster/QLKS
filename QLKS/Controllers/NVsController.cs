using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS;
using System.Text;
using QLKS.Models;
using PagedList;

namespace QLKS.Controllers
{
    [MyAuthorize(Roles ="Admin,PM")]
    public class NVsController : Controller
    {
       
       
        private QLKSEntities2 db = new QLKSEntities2();
        // Get: NVs
        [EncryptedActionParameter]
        [ActionName("TrangNhânViên")]
        [MyAuthorize(Roles ="PM")]
        public ActionResult Index(string option, string searchString,int? page)
        {
            var nVs = db.NV.Include(n => n.ChucVu);
            if(searchString!=null)
            {
                page = 1;
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            
            if (option == "ChucVu")
            {
                return View(nVs.Where(x => x.ChucVu.TenCV.ToUpper().Contains(searchString.ToUpper())).ToList().ToPagedList(pageNumber, pageSize));
            }
            else if (option == "TenNV")
            {
                return View(nVs.Where(x => x.TenNV.ToUpper().Contains(searchString.ToUpper())).ToList().ToPagedList(pageNumber, pageSize));
            }
            else if (option == "SDT")
            {
                return View(nVs.Where(x => x.SDTNV.Contains(searchString)).ToList().ToPagedList(pageNumber, pageSize));
            }
            else if (option == "MaNV")
            {
                return View(nVs.Where(x => x.MaNV.ToString().ToUpper().Contains(searchString.ToUpper())).ToList().ToPagedList(pageNumber, pageSize));
            }
            else if (option == "DiaChi")
            {
                return View(nVs.Where(x => x.DCNV.ToUpper().Contains(searchString.ToUpper())).ToList().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(nVs.OrderBy(x=>x.MaNV).ToPagedList(pageNumber, pageSize));
            }
        }
        // GET: NVs/Details/5
        [ActionName("ThêmNhânViênTC")]
        public ActionResult Details(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NV nV = db.NV.Find(x);
            if (nV == null)
            {
                return HttpNotFound();
            }
            return View(nV);
        }

        // GET: NVs/Create
        [ActionName("ThêmNhânViên")]
        public ActionResult Create()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach(var item in db.ChucVu)
            {
                if(item.MaCV!=1 && item.MaCV!=2 && item.MaCV!=3)
                {
                    selectList.Add(new SelectListItem
                    {
                        Text = item.TenCV,
                        Value = item.MaCV.ToString()
                    });
                }
            }
            ViewBag.TenCV = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList;
            return View();
        }

        // POST: NVs/Create
        [HttpPost]
        [ActionName("ThêmNhânViên")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,TenNV,MaCV,SDTNV,DCNV")] NV nV)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var item in db.ChucVu)
            {
                if (item.MaCV != 1 && item.MaCV != 2 && item.MaCV != 3)
                {
                    selectList.Add(new SelectListItem
                    {
                        Text = item.TenCV,
                        Value = item.MaCV.ToString()
                    });
                }
            }
            ViewBag.TenCV = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList;
            try
            {
                if (ModelState.IsValid)
                {
                    db.NV.Add(nV);
                    db.SaveChanges();
                    return RedirectToAction("ThêmNhânViênTC", new { id = Encryption.encrypt(nV.MaNV.ToString()) });
                }
            }
            catch
            {
                return View("LỗiTạoNhânViên");
            }
            return View(nV);
        }

        // GET: NVs/Edit/5
        [MyAuthorize(Roles = "Admin,PM")]
        [EncryptedActionParameter]
        [ActionName("SửaNhânViên")]
        public ActionResult Edit(string id)
        {
            List<SelectListItem> selectList1 = new List<SelectListItem>();
            List<SelectListItem> selectList2 = new List<SelectListItem>();
            List<SelectListItem> selectList3 = new List<SelectListItem>();
            List<SelectListItem> selectList4 = new List<SelectListItem>();
            foreach (var item in db.ChucVu)
            {                
                  if(item.MaCV==1)
                  {
                    selectList1.Add(new SelectListItem
                        {
                            Text = item.TenCV,
                            Value = item.MaCV.ToString()
                        });
                  }
                    else if(item.MaCV==2 && item.MaCV!=1)
                    {
                    selectList2.Add(new SelectListItem
                        {
                            Text = item.TenCV,
                            Value = item.MaCV.ToString()
                        });

                    }
                else if (item.MaCV == 3 && item.MaCV != 1 && item.MaCV!=2)
                {
                    selectList3.Add(new SelectListItem
                    {
                        Text = item.TenCV,
                        Value = item.MaCV.ToString()
                    });

                }
                else if(item.MaCV!=1 || item.MaCV!=2 || item.MaCV!=3)
                {
                    selectList4.Add(new SelectListItem
                    {
                        Text = item.TenCV,
                        Value = item.MaCV.ToString()
                    });
                }
            }
            ViewBag.TenCV1 = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList1;
            ViewBag.TenCV2 = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList2;
            ViewBag.TenCV3 = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList3;
            ViewBag.TenCV4 = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList4;
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NV nV = db.NV.Find(x);
            if (nV == null)
            {
                return HttpNotFound();
            }
            return View(nV);
        }

        // POST: NVs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("SửaNhânViên")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,TenNV,MaCV,SDTNV,DCNV")] NV nV)
        {
            List<SelectListItem> selectList1 = new List<SelectListItem>();
            List<SelectListItem> selectList2 = new List<SelectListItem>();
            List<SelectListItem> selectList3 = new List<SelectListItem>();
            List<SelectListItem> selectList4 = new List<SelectListItem>();
            foreach (var item in db.ChucVu)
            {
                if (item.MaCV == 1)
                {
                    selectList1.Add(new SelectListItem
                    {
                        Text = item.TenCV,
                        Value = item.MaCV.ToString()
                    });
                }
                else if (item.MaCV == 2 && item.MaCV != 1)
                {
                    selectList2.Add(new SelectListItem
                    {
                        Text = item.TenCV,
                        Value = item.MaCV.ToString()
                    });

                }
                else if (item.MaCV == 3 && item.MaCV != 1 && item.MaCV != 2)
                {
                    selectList3.Add(new SelectListItem
                    {
                        Text = item.TenCV,
                        Value = item.MaCV.ToString()
                    });

                }
                else if (item.MaCV != 1 || item.MaCV != 2 || item.MaCV != 3)
                {
                    selectList4.Add(new SelectListItem
                    {
                        Text = item.TenCV,
                        Value = item.MaCV.ToString()
                    });
                }
            }
            ViewBag.TenCV1 = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList1;
            ViewBag.TenCV2 = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList2;
            ViewBag.TenCV3 = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList3;
            ViewBag.TenCV4 = /*new SelectList(db.ChucVus, "MaCV", "TenCV")*/selectList4;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(nV).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return View("SửaNVTC");
                }
            }
            catch
            {
                return View("LỗiSửaNV");
            }
            return View(nV);
        }

        // GET: NVs/Delete/5
        [EncryptedActionParameter]
        [MyAuthorize(Roles = "Admin,PM")]
        [ActionName("XoáNhânViên")]
        public ActionResult Delete(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NV nV = db.NV.Find(x);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(nV);
        }

        // POST: NVs/Delete/5
        [HttpPost, ActionName("XoáNhânViên")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var decode = Encryption.decrypt(id);
            int x = int.Parse(decode);
            NV nV = db.NV.Find(x);
            try
            {
                db.NV.Remove(nV);
                db.SaveChanges();
                return View("XoáNVTC");
            }
            catch
            {
                return View("LỗiXoáNV");
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

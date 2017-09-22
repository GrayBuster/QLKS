using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
namespace QLKS.Controllers
{
    public class HomeController : Controller
    {
        QLKSEntities1 db = new QLKSEntities1();
        Phong phong = new Phong();
        
        [HttpGet]
        [ActionName("TrangChủ")]
        public ActionResult Index()
        {
            List<SelectListItem> tenPhong = new List<SelectListItem>();
            foreach (var item in db.Phongs)
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
            return View("TrangChủ","_Layout");
        }
        public ActionResult ShowRoom(string searchString,int? page)
        {
            var listPhong = db.Phongs.ToList();
            if(searchString!=null)
            {
                page = 1;
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return PartialView(listPhong.ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        [ActionName("TrangChủ")]
        public ActionResult Index([Bind(Include = "MaPhong,NgayBatDauThue")] ThuePhong thuePhong)
        {
            List<SelectListItem> tenPhong = new List<SelectListItem>();
            foreach (var item in db.Phongs)
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
            var tinhTrang = phong.TinhTrang.HasValue;
            try
            {
                if (ModelState.IsValid)
                {
                    if (tinhTrang == false)
                    {
                        db.ThuePhongs.Add(thuePhong);
                        db.SaveChanges();
                        return View("DatPhong");
                    }
                }
            }
            catch
            {
                return View("CreateError");
            }
            return View("TrangChủ",thuePhong);
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Report()
        {
            return View("Report");
        }
        [HttpPost]
        public ActionResult Report(string meassage)
        {
            ViewBag.Message = "Thanks for your cooperation.";
            return View("_Thanks");
        }
        //[ActionName("LỗiTrang")]
        //public ActionResult Error()
        //{
        //    return View();
        //}
        public ActionResult TruyenDuLieu()
        {
            TaiKhoan taiKhoan = new TaiKhoan();
                foreach(var item in db.TaiKhoans)
                {
                    if(HttpContext.User.Identity.Name==item.Email)
                    {
                        taiKhoan.MaNV = item.MaNV;
                        taiKhoan.Email = item.Email;
                        taiKhoan.Pass = item.Pass;
                        taiKhoan.Roles = item.Roles;
                        Session["Email"] = item.Email;
                    if (taiKhoan.MaNV==item.MaNV)
                        {
                            Session["TenNV"] = item.NV.TenNV;
                        }
                    }
                }
            
            return PartialView(taiKhoan);
        }
    }
}
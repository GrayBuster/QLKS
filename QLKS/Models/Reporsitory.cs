using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKS.Models
{
    public class Reporsitory
    {
        static QLKSEntities1 db = new QLKSEntities1();
        static List<TaiKhoan> users = new List<TaiKhoan>() {


        //new TaiKhoan() { Email = "admin123@gmail.com",Roles = "Admin",Pass = "!Gavip123",MaNV = "4" },
        //new TaiKhoan() { Email = "personnel@gmail.com",Roles = "PM",Pass = "!Gavip123",MaNV = "2" },
        //new TaiKhoan() { Email = "accountant@gmail.com",Roles = "AM",Pass = "!Gavip123",MaNV = "3" }
        };

        public static int Id { get; private set; }

        public static TaiKhoan GetUserDetails(TaiKhoan user)
        {
            foreach (var item in db.TaiKhoans)
            {
                users.Add(
                    new TaiKhoan()
                    {
                        Email = item.Email,
                        Pass = item.Pass,
                        MaNV = item.MaNV,
                        Roles = item.Roles,
                    });
            }

            return users.Where(u => u.Email.ToLower() == user.Email.ToLower() &&
            u.Pass == user.Pass).FirstOrDefault();
        }
    }
}
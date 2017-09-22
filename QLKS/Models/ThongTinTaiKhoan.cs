using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKS.Models
{
    public class ThongTinTaiKhoan
    {
        [Required(ErrorMessage ="Email không được bỏ trống")]
        public string Email { get; set; }
    }
}
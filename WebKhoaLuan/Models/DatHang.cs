using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebKhoaLuan.Models
{
    public class DatHang
    {
        public int MASP { get; set; }
        public int MANCC { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]
        [Display(Name = "NGAYDAT")]
        public DateTime NGAYDAT { get; set; }
    }
}
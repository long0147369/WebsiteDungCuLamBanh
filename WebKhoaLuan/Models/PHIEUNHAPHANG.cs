//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebKhoaLuan.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PHIEUNHAPHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEUNHAPHANG()
        {
            this.CTPHIEUNHAPHANGs = new HashSet<CTPHIEUNHAPHANG>();
        }
    
        public int MAPHIEUNH { get; set; }
        public int MAPHIEUDH { get; set; }
        public Nullable<int> MASP { get; set; }
        public string USERNAME { get; set; }
        public Nullable<int> MANCC { get; set; }
        public Nullable<System.DateTime> NGAYNHAP { get; set; }
        public Nullable<double> TONGTIEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPHIEUNHAPHANG> CTPHIEUNHAPHANGs { get; set; }
        public virtual NHACC NHACC { get; set; }
        public virtual PHIEUDATHANG PHIEUDATHANG { get; set; }
        public virtual TAIKHOAN TAIKHOAN { get; set; }
    }
}

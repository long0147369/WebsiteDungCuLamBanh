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
    
    public partial class PHIEUDATHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEUDATHANG()
        {
            this.CTPHIEUDATHANGs = new HashSet<CTPHIEUDATHANG>();
            this.PHIEUNHAPHANGs = new HashSet<PHIEUNHAPHANG>();
        }
    
        public int MAPHIEUDH { get; set; }
        public Nullable<int> MANCC { get; set; }
        public string USERNAME { get; set; }
        public Nullable<System.DateTime> NGAYDAT { get; set; }
        public Nullable<System.DateTime> NGAYNHANDUKIEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPHIEUDATHANG> CTPHIEUDATHANGs { get; set; }
        public virtual NHACC NHACC { get; set; }
        public virtual TAIKHOAN TAIKHOAN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUNHAPHANG> PHIEUNHAPHANGs { get; set; }
    }
}

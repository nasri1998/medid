using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMListEdit
    {
        public long id { get; set; }
        public long userid { get; set; }
        public long custid { get; set; }
        public long iddokter { get; set; }
        public string? namapasien { get; set; }
        public long idrelasi { get; set; }
        public string? namarelasi { get; set; }
        public string? dokname { get; set; }
        public long dokid { get; set; }
        public long dokofid { get; set; }
        public long dokofscheid { get; set; }
        public long dokoftreatid { get; set; }
        public DateTime? appdate { get; set; }
        public long doktreatid { get; set; }
        public string? namafaskes { get; set; }
        public string? tindakanmedis { get; set; }
        public long createdby { get; set; }
        public DateTime createdon { get; set; }
        public long? modifiedby { get; set; }
        public DateTime? modifiedon { get; set; }
        public long? deletedby { get; set; }
        public DateTime? deletedon { get; set; }
        public bool isdelete { get; set; }
    }
}

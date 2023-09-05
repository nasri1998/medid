using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMPage
    {
        public string orderBy { get; set; }
        public string filter { get; set; }
        public int? pageNumber { get; set; }
        public int? showData { get; set; }
    }
}

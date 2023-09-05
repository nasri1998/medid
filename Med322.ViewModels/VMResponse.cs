using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? data { get; set; }

        public VMResponse()
        {
            Success = true;
            Message = string.Empty;
            data = new object();
        }
    }
}

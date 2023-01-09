using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsaEPay.NET.Models
{
    public interface IUsaEPayRequest
    {
        public string Endpoint { get; set; }
        //public Type ResponseType { get; set; }
        public Method RequestType { get; set; }
    }
}

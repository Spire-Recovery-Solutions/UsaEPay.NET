using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayGetRequest : IUsaEPayRequest
    {
        public string Endpoint { get; set; }
        public Method RequestType { get; set; } = Method.Get;
    }
}

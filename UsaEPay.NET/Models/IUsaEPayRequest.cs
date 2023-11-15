﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsaEPay.NET.Models
{
    public interface IUsaEPayRequest
    {
        [JsonIgnore]
        public string Endpoint { get; set; }
        //public Type ResponseType { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsaEPay.NET.Models
{
    public interface IUsaEPayResponse
    {   /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}

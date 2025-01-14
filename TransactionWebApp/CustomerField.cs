using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionWebApp
{
    public class CustomerField
    {
        public string ResponseCode { get; set; }
        public List<string> ResponseDetails { get; set; }
        public string ResponseMsg { get; set; }        
    }
}
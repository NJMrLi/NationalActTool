using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalActTest
{

    public class Result<T> 
    {

        public T Data { get; set; }

        public bool Status { get; set; }

        public string Message { get; set; }
        public long Code { get; set; }

    }
}

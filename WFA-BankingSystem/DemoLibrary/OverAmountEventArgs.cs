 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary
{
     public class OverAmountEventArgs :EventArgs
    {
        public decimal totalbalance {  get; set; }
       public  decimal userenteramount { get; set; }

        public string message {  get; set; }

        public OverAmountEventArgs(decimal totalbalance,decimal userenteramount,string message)
        {
            this.totalbalance = totalbalance;
            this.userenteramount = userenteramount;
            this.message = message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudgetApp
{
    public class Expense
    {
        public DateTime Date { get; set; }
        public string Item { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }

        public string Type { get; set; }
        public string Lb1_Income { get; set; }
        public string Lb1_Expense { get; set; }
        public string Lb1_balance { get; set; }
    }
}

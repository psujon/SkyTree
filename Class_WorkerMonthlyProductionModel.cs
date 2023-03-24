using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTree
{
    class Class_WorkerMonthlyProductionModel
    {
        public string company_name { get; set; }
        public string building_name { get; set; }
        public string department { get; set; }
        public string section { get; set; }
        public string block { get; set; }
        public string emp_cardno { get; set; }
        public string emp_name { get; set; }
        public string designation { get; set; }
        public string Prod_Date { get; set; }
        public string style { get; set; }
        public string process_name { get; set; }
        public int Quantity { get; set; }
        public int process_by { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
    }
}

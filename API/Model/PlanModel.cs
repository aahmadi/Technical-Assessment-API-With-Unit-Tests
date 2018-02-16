using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ali.Planning.API.Model
{
    public class PlanModel
    {
        public int EmployeeId { get; set; }
        public string ProjectName { get; set; }
        public int year { get; set; }
        public decimal Q1 { get; set; }
        public decimal Q2 { get; set; }
        public decimal Q3 { get; set; }
        public decimal Q4 { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class SupervisorViewModel
    {
        public string SupervisorName { get; set; }
        public string SupervisorEmail { get; set; }
        public List<DefaulterViewModel> Defaulters { get; set; }
    }

    public class DefaulterViewModel
    {
        public string EmployeeName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.Service.ViewModels
{
    public class Role
    {
        public string LoginName { get; set; }
    }

    public class Groups
    {
        public List<Role> results { get; set; }
    }
}

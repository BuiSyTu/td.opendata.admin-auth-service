using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.Service.ViewModels
{
    public class UserInfo
    {
        public UserPosition Info { get; set; }
        public List<Role> Roles { get; set; }
    }
}

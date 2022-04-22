using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.Service.ViewModels
{
    public class CurrentUser
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Title { get; set; }
        public Groups Groups { get; set; }
    }

    public class CurrentUser_d
    {
        public CurrentUser d { get; set; }
    }
}

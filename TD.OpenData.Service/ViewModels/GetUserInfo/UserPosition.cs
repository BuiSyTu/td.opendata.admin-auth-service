using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.Service.ViewModels
{
    public class UserPosition
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public UserProfile UserProfile { get; set; }
        public Position Position { get; set; }
        public Group Group { get; set; }
        public Group UserOffice { get; set; }
    }


    public class UserPosition_d
    {
        public UserPosition_results d { get; set; }
    }

    public class UserPosition_results
    {
        public List<UserPosition> results { get; set; }
    }
}

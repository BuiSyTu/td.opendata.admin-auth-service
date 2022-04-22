using Microsoft.SharePoint;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.Service.ViewModels
{
    public class PayLoadJWT
    {
        [JsonProperty("iat")]
        public int Iat { get; set; }

        [JsonProperty("exp")]
        public int Exp { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("hashPwd")]
        public string HashPwd { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("permissions")]
        public List<string> Permissions { get; set; }

        [JsonProperty("userPositionCode")]
        public string UserPositionCode { get; set; }

        [JsonProperty("userOffice")]
        public string UserOffice { get; set; }

        [JsonProperty("areaCode")]
        public string AreaCode { get; set; }
    }
}

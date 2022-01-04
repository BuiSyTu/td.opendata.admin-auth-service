using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TD.OpenData.Service.Services;

namespace TD.OpenData.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public partial class OpenDataService : WCFController, IOpenDataService
    {
        public Stream GetUserTokenKey(string user = null, string pass = null, string tokenDevice = null)
        {
            UserService userService = new();
            var userTokenKey = userService.GetUserTokenKey(user, pass, tokenDevice);

            HttpCookie cookie = new("token")
            {
                Value = userTokenKey,
                Expires = DateTime.Now.AddDays(1d)
            };
            var response = HttpContext.Current.Response;
            response.Cookies.Remove("token");
            response.Cookies.Add(cookie);

            return ApiOk(userTokenKey);
        }

        public string HelloWorld()
        {
            return "Hello world";
        }
    }
}

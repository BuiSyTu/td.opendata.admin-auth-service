using Microsoft.SharePoint;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TD.OpenData.Service.Services;
using TD.OpenData.Service.ViewModels;
using Unity;

namespace TD.OpenData.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public partial class OpenDataService : WCFController, IOpenDataService
    {
        private readonly IUnityContainer _unityContainer;
        private readonly UserService _userService;
        private readonly SharepointListService _sharepointListService;
        private readonly LoginService _loginService;

        public OpenDataService()
        {
            _unityContainer = new UnityContainer().EnableDiagnostic();
            var integration = new Integration();
            integration.ConfigureContainer(_unityContainer);

            _userService = _unityContainer.Resolve<UserService>();
            _sharepointListService = _unityContainer.Resolve<SharepointListService>();
            _loginService = _unityContainer.Resolve<LoginService>();
        }

        public Stream GetUserTokenKey(string user = null, string pass = null, string tokenDevice = null)
        {
            var userTokenKey = _userService.GetUserTokenKey(user, pass, tokenDevice);

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

        public Stream GetUserInfo()
        {
            using HttpClient client = new();

            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            System.Net.WebHeaderCollection headers = request.Headers;

            bool login = false;
            string token = headers["userTokenKey"];
            if (token != null)
            {
                login = _loginService.CheckLoginForm(client, token);
            }
            if (!login)
            {
                return Forbidden();
            }

            var userInfo = _sharepointListService.GetUserInfo(client, token);
            return ApiOk(userInfo);
        }

        public string HelloWorld()
        {
            return "Hello world";
        }
    }
}

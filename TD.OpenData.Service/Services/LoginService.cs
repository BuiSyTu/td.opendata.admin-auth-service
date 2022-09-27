using Microsoft.SharePoint;
using Microsoft.SharePoint.IdentityModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TD.OpenData.Service.ViewModels;

namespace TD.OpenData.Service.Services
{
    public class LoginService
    {
        private readonly JwtService _jwtService;

        public LoginService(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public bool CheckLogin(string token)
        {
            string urlRoot = SPContext.Current.Site.RootWeb.Url;
            string payload = _jwtService.ValidateJWT(token);
            PayLoadJWT payloadJWT = JsonConvert.DeserializeObject<PayLoadJWT>(payload);
            string user = payloadJWT.User;
            string pass = _jwtService.ExcuteDecryptAES(payloadJWT.HashPwd);
            return SPClaimsUtility.AuthenticateFormsUser(new Uri(urlRoot), user, pass);
        }

        public bool CheckLoginForm(HttpClient client, string user, string password)
        {
            string urlRoot = SPContext.Current.Site.RootWeb.Url;
            string urlAuthenSP = urlRoot + "/_vti_bin/Authentication.asmx";

            string errCode = AuthenticationSP(client, urlAuthenSP, user, password);
            return errCode == "NoError";
        }

        public bool CheckLoginForm(HttpClient client, string token)
        {
            string payload = _jwtService.ValidateJWT(token);
            PayLoadJWT payloadJWT = JsonConvert.DeserializeObject<PayLoadJWT>(payload);
            string user = payloadJWT.User;
            string pass = _jwtService.ExcuteDecryptAES(payloadJWT.HashPwd);

            return CheckLoginForm(client, user, pass);
        }

        public string AuthenticationSP(HttpClient client, string urlAuthenSP, string user, string pass)
        {
            // FIXME: To resolve "Could not establish trust relationship for the SSL/TLS secure channel with authority"
            // FIXME: This is not a good practice
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };

            StringContent dataReqAuth = GetContentAuthenticationSP(
                user.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", ""),
                pass);

            var reqAuth = new HttpRequestMessage(HttpMethod.Post, urlAuthenSP) { Content = dataReqAuth };
            var resAuth = client.SendAsync(reqAuth);
            string strResAuth = resAuth.Result.Content.ReadAsStringAsync().Result;
            XDocument xDocAuth = XDocument.Parse(strResAuth);
            XElement elementErrorCode = GetElement(xDocAuth, "ErrorCode");
            return elementErrorCode.Value;
        }

        private StringContent GetContentAuthenticationSP(string user, string pass)
        {
            string valuePost = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            valuePost += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
            valuePost += "<soap:Body><Login xmlns=\"http://schemas.microsoft.com/sharepoint/soap/\">";
            valuePost += "<username>" + user + "</username>";
            valuePost += "<password>" + pass + "</password>";
            valuePost += "</Login> </soap:Body> </soap:Envelope>";
            StringContent data = new(valuePost, Encoding.UTF8, "text/xml");
            return data;
        }

        private XElement GetElement(XDocument doc, string elementName)
        {
            foreach (XNode node in doc.DescendantNodes())
            {
                if (node is XElement element)
                {
                    if (element.Name.LocalName.Equals(elementName)) return element;
                }
            }
            return null;
        }
    }
}

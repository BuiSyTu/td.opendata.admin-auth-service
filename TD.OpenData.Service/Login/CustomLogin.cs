using Microsoft.SharePoint.IdentityModel;
using Microsoft.SharePoint.IdentityModel.Pages;
using System;
using System.Web;
using TD.OpenData.Service.Services;

namespace TD.OpenData.Service.Login
{
    public partial class CustomLogin : FormsSignInPage
    {
        protected override void RedirectToSuccessUrl()
        {
            try
            {
                if (SPClaimsUtility.AuthenticateFormsUser(
                    this.Page.Request.Url,
                    this.signInControl.UserName
                        .Trim()
                        .Replace("\r\n", "")
                        .Replace("\n", "")
                        .Replace("\r", ""),
                    this.signInControl.Password))
                {
                    var response = this.Response;
                    response.Cookies.Remove("token");
                    HttpCookie cookies = new("token");
                    var userService = new UserService(new JwtService());
                    cookies.Value = userService.GetUserTokenKey(this.signInControl.UserName, this.signInControl.Password);
                    cookies.Expires = DateTime.Now.AddDays(1d);
                    response.SetCookie(cookies);
                    response.Cookies.Add(cookies);
                }
            }
            catch { }
            base.RedirectToSuccessUrl();
        }
    }
}

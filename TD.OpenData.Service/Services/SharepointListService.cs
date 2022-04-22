using Microsoft.SharePoint;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.Service.ViewModels;

namespace TD.OpenData.Service.Services
{
    public class SharepointListService
    {
        private readonly UserService _userService;

        public SharepointListService(UserService userService)
        {
            _userService = userService;
        }

        public UserPosition GetUserPosition(HttpClient client, string userName)
        {
            string urlRoot = SPContext.Current.Site.RootWeb.Url;
            string url = urlRoot + $"/_api/web/lists/getbytitle('UserPositions')/items?$top=10&$select=ID,Code,UserProfile/Account,UserProfile/FullName,UserProfile/Birthday,UserProfile/Phone,UserProfile/Email,Position/Name,Group/GroupCode,Group/GroupName,UserOffice/GroupCode,UserOffice/GroupName&$expand=UserProfile/Account,UserProfile/FullName,UserProfile/Birthday,UserProfile/Phone,UserProfile/Email,Position/Name,Group/GroupCode,Group/GroupName,UserOffice/GroupCode,UserOffice/GroupName&$filter=UserProfile/Account eq '{userName}'";
            var req = new HttpRequestMessage(HttpMethod.Get, url) { };
            req.Headers.Add("Accept", "application/json;odata=verbose");
            var res = client.SendAsync(req);
            var resStr = res.Result.Content.ReadAsStringAsync().Result;

            var userPosition_d = JsonConvert.DeserializeObject<UserPosition_d>(resStr);
            var lstUserProfile = userPosition_d?.d?.results;
            return lstUserProfile?[0];
        }

        public CurrentUser GetCurrentUser(HttpClient client)
        {
            string urlRoot = SPContext.Current.Site.RootWeb.Url;
            var urlRole = urlRoot + "/sites/opdt" + $"/_api/web/currentuser/?$select=Id,LoginName,Title,groups/LoginName&$expand=groups/LoginName";
            var req = new HttpRequestMessage(HttpMethod.Get, urlRole) { };
            req.Headers.Add("Accept", "application/json;odata=verbose");
            var res = client.SendAsync(req);
            var resStr = res.Result.Content.ReadAsStringAsync().Result;

            var currentUser_d = JsonConvert.DeserializeObject<CurrentUser_d>(resStr);
            var currentUser = currentUser_d?.d;
            return currentUser;
        }
    
        public UserInfo GetUserInfo(HttpClient client, string token)
        {
            var userName = _userService.GetUserFromToken(token);

            return new UserInfo
            {
                Info = GetUserPosition(client, userName),
                Roles = GetCurrentUser(client)?.Groups?.results ?? null
            };
        }
    }
}

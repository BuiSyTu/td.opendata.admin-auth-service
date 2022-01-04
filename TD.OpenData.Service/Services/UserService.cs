using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.SharePoint;
using Microsoft.SharePoint.IdentityModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.Service.ViewModels;

namespace TD.OpenData.Service.Services
{
    public class UserService
    {
        private string secretJWT = "TanDan123!@#456789";

        public string CreateJWT(PayLoadJWT payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secretJWT);
            return token;
        }

        public string GetUserTokenKey(string user, string pass, string tokenDevice)
        {
            user = user.ToLower();

            PayLoadJWT payLoadJWT = new()
            {
                Iat = (int)DateTimeOffset.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                Exp = (int)DateTimeOffset.UtcNow.AddYears(3).Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                User = user,
                Sub = user
            };


            string urlRoot = SPContext.Current.Site.RootWeb.Url;
            bool isLoginSuccess = SPClaimsUtility.AuthenticateFormsUser(new Uri(urlRoot), user, pass);

            if (!isLoginSuccess) return null;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite oSite = new(urlRoot + "/sites/opdt"))
                {
                    SPWeb rootWeb = oSite.RootWeb;
                    SPList usersList = rootWeb.Lists["Users"];

                    var query = new SPQuery()
                    {
                        Query = "<Where><Contains><FieldRef Name='UserAccount' /><Value Type='Text'>" + user + "</Value></Contains></Where>",
                        RowLimit = 1
                    };
                    var usersItems = usersList.GetItems(query);
                    if (usersItems != null && usersItems.Count > 0)
                    {
                        payLoadJWT.UserPositionCode = usersItems[0]["UserPositionCode"].ToString();
                        payLoadJWT.Permissions = usersItems[0]["Permissions"].ToString();
                    }
                }

                using (SPSite oSite = new(urlRoot))
                {
                    SPWeb rootWeb = oSite.RootWeb;

                    SPList userProfilesList = rootWeb.Lists["UserProfiles"];
                    var userProfilesQuery = new SPQuery()
                    {
                        Query = "<Where><Contains><FieldRef Name='UserAccount' /><Value Type='Text'>" + user + "</Value></Contains></Where>",
                        RowLimit = 1
                    };
                    var userProfilesItems = userProfilesList.GetItems(userProfilesQuery);
                    if (userProfilesItems != null && userProfilesItems.Count > 0)
                    {
                        payLoadJWT.AreaCode = userProfilesItems[0]["AreaCode"]?.ToString();
                    }

                    SPList userPositionsList = rootWeb.Lists["UserPositions"];
                    var userPositionsQuery = new SPQuery()
                    {
                        Query = "<Where><Contains><FieldRef Name='UserAccount' /><Value Type='Text'>" + user + "</Value></Contains></Where>",
                        RowLimit = 1
                    };
                    var userPositionsItems = userPositionsList.GetItems(userProfilesQuery);
                    if (userPositionsItems != null && userPositionsItems.Count > 0)
                    {
                        payLoadJWT.UserOffice = userPositionsItems[0]["UserOffice"].ToString();
                    }
                }
            });

            return CreateJWT(payLoadJWT);
        }
    }
}

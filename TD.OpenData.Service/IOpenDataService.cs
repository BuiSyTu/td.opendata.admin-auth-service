using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace TD.OpenData.Service
{
    [ServiceContract]
    public partial interface IOpenDataService
    {
        [OperationContract]
        [WebGet(UriTemplate = "HelloWorld", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string HelloWorld();

        [OperationContract]
        [WebGet(UriTemplate = "UserTokenKey?user={user}&pass={pass}&tokenDevice={tokenDevice}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Stream GetUserTokenKey(string user = null, string pass = null, string tokenDevice = null);

        [OperationContract]
        [WebGet(UriTemplate = "UserInfo", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Stream GetUserInfo();
    }
}

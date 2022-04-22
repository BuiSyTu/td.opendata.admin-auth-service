using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.Service.Services;
using Unity;

namespace TD.OpenData.Service
{
    public class Integration
    {
        public void ConfigureContainer(IUnityContainer container)
        {
            container
                .RegisterType<JwtService>()
                .RegisterType<LoginService>()
                .RegisterType<SharepointListService>()
                .RegisterType<UserService>();
        }
    }
}

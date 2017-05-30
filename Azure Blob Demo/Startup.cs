using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Azure_Blob_Demo.Startup))]
namespace Azure_Blob_Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

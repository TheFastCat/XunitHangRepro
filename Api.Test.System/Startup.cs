namespace Api.Test.System
{
    using global::System.Web.Http;
    using Autofac;
    using Owin;

    public class Startup
    {
        HttpConfiguration _httpConfiguration;

        public Startup(HttpConfiguration httpConfiguration)
        { 
            _httpConfiguration = httpConfiguration;
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(_httpConfiguration);
        }
    }
}

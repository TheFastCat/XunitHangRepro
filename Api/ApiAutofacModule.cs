namespace Api
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Autofac;
    using Autofac.Core;
    using Autofac.Integration.WebApi;

    public class ApiAutofacModule : Module
    {             
        /// <summary>
        /// Register types dependent on HTTP configuration
        /// </summary>
        /// <remarks>Utilizes extension methods of the nuget package Autofac.WebApi2</remarks>
        protected void RegisterHttpConfigurationTypes(ContainerBuilder containerBuilder)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            httpConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            httpConfiguration.EnableCors();
            httpConfiguration.EnableSystemDiagnosticsTracing();
            httpConfiguration.MapHttpAttributeRoutes();          

            containerBuilder.RegisterInstance<HttpConfiguration>(httpConfiguration);
            containerBuilder.RegisterWebApiFilterProvider(httpConfiguration); // http://goo.gl/P84rhB
            containerBuilder.RegisterHttpRequestMessage(httpConfiguration); // supports usage of IHttpRequestMessageOperator instead of alternative IQueryString
            containerBuilder.RegisterApiControllers(typeof(AddressesController).Assembly);
        }
    }
}

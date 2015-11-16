namespace Api.Test.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Net;
    using global::System.Net.Http;
    using global::System.Net.Http.Headers;
    using global::System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Microsoft.Owin.Testing;
    using Moq;
    using Xbehave;

    public abstract class TestBase
    {
        public TestBase()
        {
            Request = new HttpRequestMessage();
            HttpConfiguration = new HttpConfiguration();
            ContainerBuilder = new ContainerBuilder();
            ContainerBuilder.RegisterApiControllers(typeof(AddressesController).Assembly);
        }

        protected HttpConfiguration HttpConfiguration { get; private set; }
        protected HttpResponseMessage Response { get; set; }
        protected HttpRequestMessage Request { get; private set; }
        protected ContainerBuilder ContainerBuilder { get; private set; }
        protected TestServer ApiServer { get; set; }
        protected HttpClient Client { get; set; }
    }
}

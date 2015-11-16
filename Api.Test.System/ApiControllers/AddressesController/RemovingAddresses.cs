namespace Api.Test.System.ApiControllers.AddressesController
{
    using global::System;
    using global::System.Linq;
    using global::System.Net;
    using global::System.Net.Http;
    using global::System.Net.Http.Formatting;
    using global::System.Threading.Tasks;
    using Api.ApiControllers;
    using Api.Test.System;
    using Moq;
    using Newtonsoft.Json.Linq;
    using Should;
    using Xbehave;
    using Xunit;

    public class RemovingAddresses : AddressesTestsSetup
    {
        readonly Uri _addressesUri = new Uri("http://localhost/api/addresses");

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RemovingAnAddressForACustomer(dynamic addressToDelete, JObject returnedContent)
        {
            const int CustomerId = 349075;

            "Given an address type that exists for a customer".
                f(() =>
                {
                    addressToDelete = new
                    {
                        OwnerId = CustomerId,
                        Type = "MAIL"
                    };

                    MockAddressesStore.Setup(i => i.DeleteAddress(It.IsAny<Address>())).Returns<dynamic>(address =>
                    {
                        var filteredAddresses = from n in this.FakeAddresses
                                                where address.OwnerId == n.Value<int>("OwnerId") &&
                                                      address.Type.Contains(n.Value<string>("Type"))
                                                select n;

                        return Task.FromResult<dynamic>(filteredAddresses.Count() == 1 ? 1 as object : 0 as object);
                    });
                });
            "When a DELETE address request is sent referencing the customer and type".
                f(async () =>
                {
                    Response = await Client.DeleteAsync(_addressesUri, new ObjectContent<dynamic>(addressToDelete, new JsonMediaTypeFormatter()));
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.DeleteAddress(It.IsAny<Address>())));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then a '200 Okay' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then no content should be returned".
                f(() =>
                {
                    returnedContent = Response.Content.ReadAsAsync<JObject>().Result;
                    returnedContent.ShouldBeNull();
                });
            "Then the response location header will empty".
                f(() => Response.Headers.Location.ShouldBeNull());
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RemovingANonExistantAddressForACustomer(dynamic addressToDelete, JObject returnedContent)
        {
            const int CustomerId = 349075;

            "Given an address type that does not exist for a customer".
                f(() =>
                {
                    addressToDelete = new
                    {
                        OwnerId = CustomerId,
                        Type = "MEET"
                    };

                    MockAddressesStore.Setup(i => i.DeleteAddress(It.IsAny<Address>())).Returns<dynamic>(address =>
                    {
                        var filteredAddresses = from n in this.FakeAddresses
                                                where address.OwnerId == n.Value<int>("OwnerId") &&
                                                      address.Type.Contains(n.Value<string>("Type"))
                                                select n;

                        return Task.FromResult<dynamic>(filteredAddresses.Count() == 0 ? 0 as object : filteredAddresses);
                    });
                });
            "When a DELETE request is sent referencing them".
                f(() =>
                {
                    Response = Client.DeleteAsync(_addressesUri, new ObjectContent<dynamic>(addressToDelete, new JsonMediaTypeFormatter())).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.DeleteAddress(It.IsAny<Address>())));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then a '404 Not Found' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.NotFound));
            "Then no content should be returned".
                f(() =>
                {
                    returnedContent = Response.Content.ReadAsAsync<JObject>().Result;
                    returnedContent.ShouldBeNull();
                });
            "Then the response location header will empty".
                f(() => Response.Headers.Location.ShouldBeNull());
        }
    }
}

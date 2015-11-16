namespace Api.Test.System.ApiControllers.AddressesController
{
    using global::System;
    using global::System.Linq;
    using global::System.Net;
    using global::System.Net.Http;
    using global::System.Net.Http.Formatting;
    using global::System.Threading.Tasks;
    using Api.ApiControllers;
    using Moq;
    using Newtonsoft.Json.Linq;
    using Should;
    using Xbehave;
    using Xunit;

    public class EditingAddresses : AddressesTestsSetup
    {
        readonly Uri _addressesUri = new Uri("http://localhost/api/addresses");

        [Scenario]
        [Trait("Category", "Api.System")]
        public void EditingAddressForACustomer(dynamic addressToAdd, JObject returnedAddress)
        {
            const int CustomerId = 346760;

            "Given changes to be made to an address associated with a customer".
                f(() =>
                {
                    addressToAdd = new
                    {
                        OwnerId = CustomerId,
                        Type    = "MEET",
                        Street1 = "2735 Chadwick",
                        Street2 = "some test street2",
                        Suite   = "Sweet Suite",
                        City    = "Denver",
                        State   = "CO",
                        Zip     = "80205",
                        Country = "US"
                    };
                });
            "Given the customer has an address of the referenced 'Type'".
                f(() =>
                {
                    MockAddressesStore.Setup(i => i.PatchAddress(It.IsAny<Address>())).Returns<dynamic>(address =>
                    {
                        var filteredAddresses = from n in this.FakeAddresses
                                                where address.OwnerId == n.Value<int>("OwnerId") &&
                                                      address.Type.Contains(n.Value<string>("Type"))
                                                select n;

                        return Task.FromResult(filteredAddresses.Count() == 1 ? 1 as object : 0 as object);
                    });
                });
            "When a PATCH request is made with the customer address to edit".
                f(async () =>
                {
                    Response = await Client.PatchAsync(_addressesUri, new ObjectContent<dynamic>(addressToAdd, new JsonMediaTypeFormatter()));
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.PatchAddress(It.IsAny<Address>())));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then a '204 No Content' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.NoContent));
            "Then no content should be returned".
                f(() =>
                {
                    Response.Content.ReadAsAsync<JObject>().Result.ShouldBeNull();
                });
            "Then the response location header will be set to the created address resource location".
                f(() => Response.Headers.Location.AbsoluteUri.ShouldEqual(_addressesUri + "?OwnerId=" + CustomerId + "&Type=" + addressToAdd.Type as string));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void EditingNonExistingAddressForACustomer(dynamic addressToAdd, JObject returnedAddress)
        {
            const int CustomerId = 349075;

            "Given changes to be made to an address associated with a customer".
                f(() =>
                {
                    addressToAdd = new
                    {
                        OwnerId = CustomerId,
                        Type    = "MEET",
                        Street1 = "2735 Chadwick",
                        City    = "Denver",
                        State   = "CO",
                        Zip     = "80205",
                        Country = "US"
                    };
                });
            "Given the customer does not have an address of the referenced 'Type'".
                f(() =>
                {
                    MockAddressesStore.Setup(i => i.PatchAddress(It.IsAny<Address>())).Returns<dynamic>(address =>
                    {
                        var filteredAddresses = from n in this.FakeAddresses
                                                where address.OwnerId == n.Value<int>("OwnerId") &&
                                                      address.Type.Contains(n.Value<string>("Type"))
                                                select n;

                        return Task.FromResult<dynamic>(filteredAddresses.Count() == 0 ? 0 as object : filteredAddresses);
                    });
                });
            "When a PATCH request is made to edit the address".
                f(async () =>
                {
                    Response = await Client.PatchAsync(_addressesUri, new ObjectContent<dynamic>(addressToAdd, new JsonMediaTypeFormatter()));
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.PatchAddress(It.IsAny<Address>())));
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
                    Response.Content.ReadAsAsync<JObject>().Result.ShouldBeNull();
                });
            "Then the response location header should be empty".
                f(() => Response.Headers.Location.ShouldBeNull());
        }
    }
}

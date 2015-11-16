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

    public class CreatingAddresses : AddressesTestsSetup
    {
        readonly Uri _addressesUri = new Uri("http://localhost/api/addresses");

        [Scenario]
        [Trait("Category", "Api.System")]
        public void AddANewAddressForACustomer(dynamic addressToAdd, JObject returnedAddress)
        {
            const int CustomerId = 47192;

            "Given a new address to associate with a customer".
                f(() =>
                {
                    addressToAdd = new
                    {
                        OwnerId    = CustomerId,
                        Type       = this.AddressTypes[1],
                        Street1    = "2735 Chadwick",
                        Street2    = "some test street2",
                        Suite      = "Sweet Suite",
                        City       = "Denver",
                        State      = "CO",
                        Zip        = "80205",
                        Country    = "US"
                    };

                    MockAddressesStore.Setup(i => i.PostAddress(It.IsAny<Address>())).Returns<dynamic>(address =>
                    {
                        address.OwnerId = CustomerId;
                        return Task.FromResult(address.OwnerId as object);
                    });
                });
            "When a POST request is made with the address to add".
                f(() =>
                {
                    Request.Method = HttpMethod.Post;
                    Request.RequestUri = _addressesUri;
                    Request.Content = new ObjectContent<dynamic>(addressToAdd, new JsonMediaTypeFormatter());
                    Response = Client.SendAsync(Request).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.PostAddress(It.IsAny<Address>())));
            "Then a response is received with content".
                f(async () =>
                {
                    Response.Content.ShouldNotBeNull();
                    returnedAddress = await Response.Content.ReadAsAsync<JObject>();
                    returnedAddress.ShouldNotBeNull();
                });
            "Then a '201 Created' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.Created));
            "Then the response location header will be set to the created address resource location".
                f(() => Response.Headers.Location.AbsoluteUri.ShouldEqual(_addressesUri + "?OwnerId=" + CustomerId + "&Type=" + addressToAdd.Type as string));
            "Then the returned address should reference the expected customer".
                f(() => returnedAddress.Value<int?>("OwnerId").ShouldEqual<int?>(addressToAdd.OwnerId as int?));
            "Then the returned address should reference the expected address type".
                f(() => returnedAddress.Value<string>("Type").ShouldEqual<string>(addressToAdd.Type as string));
            "Then the returned address should reference the expected street1".
                f(() => returnedAddress.Value<string>("Street1").ShouldEqual<string>(addressToAdd.Street1 as string));
            "Then the returned address should reference the expected street2".
                f(() => returnedAddress.Value<string>("Street2").ShouldEqual<string>(addressToAdd.Street2 as string));
            "Then the returned address should reference the expected suite".
                f(() => returnedAddress.Value<string>("Suite").ShouldEqual<string>(addressToAdd.Suite as string));
            "Then the returned address should reference the expected city".
                f(() => returnedAddress.Value<string>("City").ShouldEqual<string>(addressToAdd.City as string));
            "Then the returned address should reference the expected state".
                f(() => returnedAddress.Value<string>("State").ShouldEqual<string>(addressToAdd.State as string));
            "Then the returned address should reference the expected zip code".
                f(() => returnedAddress.Value<string>("Zip").ShouldEqual<string>(addressToAdd.Zip as string));
            "Then the returned address should reference the expected country".
                f(() => returnedAddress.Value<string>("Country").ShouldEqual<string>(addressToAdd.Country as string));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void AddAnExistingAddressTypeForACustomer(dynamic addressToAdd, JObject returnedAddress)
        {
            const int CustomerId = 349075;

            "Given a new address to associate with a customer".
                f(() =>
                {
                    addressToAdd = new
                    {
                        OwnerId = CustomerId,
                        Type = "MAIL",
                        Street1 = "2735 Chadwick",
                        City = "Denver",
                        State = "CO",
                        Zip = "80205",
                        Country = "US"
                    };
                });
            "Given the customer has an existing address of the new address's type".
                f(() =>
                {
                    MockAddressesStore.Setup(i => i.PostAddress(It.IsAny<Address>())).Returns<dynamic>(address =>
                    {
                        var filteredAddresses = from n in this.FakeAddresses
                                                where address.OwnerId == n.Value<int>("OwnerId") &&
                                                      address.Type.Contains(n.Value<string>("Type"))
                                                select n;

                        return Task.FromResult<dynamic>(filteredAddresses.Count() > 0 ? -1 as object : filteredAddresses);
                    });
                });
            "When a POST request is made with the address to add".
                f(async () =>
                {
                    Request.Method = HttpMethod.Post;
                    Request.RequestUri = _addressesUri;
                    Request.Content = new ObjectContent<dynamic>(addressToAdd, new JsonMediaTypeFormatter());
                    Response = await Client.SendAsync(Request);
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.PostAddress(It.IsAny<Address>())));
            "Then a response is received by the HTTP client".
                f(() => Response.ShouldNotBeNull());
            "Then the response has no content".
                f(() =>
                {
                    Response.Content.ReadAsAsync<JObject>().Result.ShouldBeNull();
                });
            "Then a '409 Conflict' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.Conflict));
            "Then the response location header will empty".
                f(() => Response.Headers.Location.ShouldBeNull());
        }
    }
}

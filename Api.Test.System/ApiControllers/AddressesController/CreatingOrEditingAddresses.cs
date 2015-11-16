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

    public class CreatingOrEditingAddresses : AddressesTestsSetup
    {
        readonly Uri _addressesUri = new Uri("http://localhost/api/addresses");

        [Scenario]
        [Trait("Category", "Api.System")]
        public void PutANewAddressTypeForACustomer(dynamic addressToAdd, JObject returnedAddress)
        {
            const int CustomerId = 47192;
            const string Street1 = "2735 Chadwick";
            const string Street2 = "some test street2";
            const string Suite   = "Sweet Suite";
            const string City    = "Denver";
            const string State   = "CO";
            const string Zip     = "80205";
            const string Country = "US";

            "Given a new address to associate with a customer with an existing address of that type".
                f(() =>
                {
                    addressToAdd = new
                    {
                        OwnerId    = CustomerId,
                        Type       = this.AddressTypes[1],
                        Street1    = Street1,
                        Street2    = Street2,
                        Suite      = Suite,
                        City       = City,
                        State      = State,
                        Zip        = Zip,
                        Country    = Country
                    };

                    MockAddressesStore.Setup(i => i.PutAddress(It.Is<Address>(address => address.OwnerId == CustomerId &&
                                                                                         address.Type    == this.AddressTypes[1] &&
                                                                                         address.Street1 == Street1 &&
                                                                                         address.Street2 == Street2 &&
                                                                                         address.Suite   == Suite &&
                                                                                         address.City    == City &&
                                                                                         address.State   == State &&
                                                                                         address.Zip     == Zip &&
                                                                                         address.Country == Country))).Returns<Address>(address =>
                    {
                        return Task.FromResult(2 as object); // 2 denotes DB insert
                    });
                });
            "When a PUT request is made with the address to add".
                f(async () =>
                {
                    Request.Method     = HttpMethod.Put;
                    Request.RequestUri = _addressesUri;
                    Request.Content    = new ObjectContent<dynamic>(addressToAdd, new JsonMediaTypeFormatter());
                    Response           = await Client.SendAsync(Request);
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.PutAddress(It.Is<Address>(address => address.OwnerId == CustomerId &&
                                                                                              address.Type    == this.AddressTypes[1] &&
                                                                                              address.Street1 == Street1 &&
                                                                                              address.Street2 == Street2 &&
                                                                                              address.Suite   == Suite &&
                                                                                              address.City    == City &&
                                                                                              address.State   == State &&
                                                                                              address.Zip     == Zip &&
                                                                                              address.Country == Country))));
            "Then a response is received with no content".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then a '201 Created' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.Created));
            "Then the response location header will be set to the created address resource location".
                f(() => Response.Headers.Location.AbsoluteUri.ShouldEqual(_addressesUri + "?OwnerId=" + CustomerId + "&Type=" + addressToAdd.Type as string));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void PutAnExistingAddressTypeForACustomer(dynamic addressToAdd, JObject returnedAddress)
        {
            const int CustomerId = 349075;
            const string Street1 = "2735 Chadwick";
            const string Street2 = "some test street2";
            const string Suite   = "Sweet Suite";
            const string City    = "Denver";
            const string State   = "CO";
            const string Zip     = "80205";
            const string Country = "US";

            "Given a new address to associate with a customer".
                f(() =>
                {
                    addressToAdd = new
                    {
                        OwnerId = CustomerId,
                        Type = this.AddressTypes[2],
                        Street1 = Street1,
                        Street2 = Street2,
                        Suite = Suite,
                        City = City,
                        State = State,
                        Zip = Zip,
                        Country = Country
                    };
                });
            "Given the customer has an existing address of the new address's type".
                f(() =>
                {
                    MockAddressesStore.Setup(i => i.PutAddress(It.Is<Address>(address => address.OwnerId == CustomerId &&
                                                                                         address.Type    == this.AddressTypes[2] &&
                                                                                         address.Street1 == Street1 &&
                                                                                         address.Street2 == Street2 &&
                                                                                         address.Suite   == Suite &&
                                                                                         address.City    == City &&
                                                                                         address.State   == State &&
                                                                                         address.Zip     == Zip &&
                                                                                         address.Country == Country))).Returns<Address>(address =>
                    {
                        var filteredAddresses = from n in this.FakeAddresses
                                                where address.OwnerId == n.Value<int>("OwnerId") &&
                                                      address.Type.Contains(n.Value<string>("Type"))
                                                select n;

                        return Task.FromResult<dynamic>(1); // 1 denotes db update
                    });
                });
            "When a PUT request is made with the address to add".
                f(async () =>
                {
                    Request.Method = HttpMethod.Put;
                    Request.RequestUri = _addressesUri;
                    Request.Content = new ObjectContent<dynamic>(addressToAdd, new JsonMediaTypeFormatter());
                    Response = await Client.SendAsync(Request);
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.PutAddress(It.Is<Address>(address => address.OwnerId == CustomerId &&
                                                                                              address.Type    == this.AddressTypes[2] &&
                                                                                              address.Street1 == Street1 &&
                                                                                              address.Street2 == Street2 &&
                                                                                              address.Suite   == Suite &&
                                                                                              address.City    == City &&
                                                                                              address.State   == State &&
                                                                                              address.Zip     == Zip &&
                                                                                              address.Country == Country))));
            "Then a response is received by the HTTP client".
                f(() => Response.ShouldNotBeNull());
            "Then the response has no content".
                f(() =>
                {
                    Response.Content.ReadAsAsync<JObject>().Result.ShouldBeNull();
                });
            "Then a '204 No Content' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.NoContent));
            "Then the response location header will be set to the created address resource location".
                f(() => Response.Headers.Location.AbsoluteUri.ShouldEqual(_addressesUri + "?OwnerId=" + CustomerId + "&Type=" + addressToAdd.Type as string));
        }
    }
}

namespace Api.Test.System.ApiControllers.AddressesController
{
    using global::System;
    using global::System.Linq;
    using global::System.Net;
    using global::System.Net.Http;
    using global::System.Threading.Tasks;
    using Api.ApiControllers;
    using Moq;
    using Newtonsoft.Json.Linq;
    using Should;
    using Xbehave;
    using Xunit;

    public class RetrievingCustomerAddresses : AddressesTestsSetup
    {
        readonly string _customerAddressesFormat = "http://localhost/api/customers/{0}/addresses";

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAllCustomerAddresses(JArray addresses)
        {
            const int CustomerId = 346760;

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.IsAny<AddressesFilter>())).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where customerId == n.Value<int>("OwnerId")
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'all addresses for a customer' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId))).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.IsAny<AddressesFilter>())));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then all addresses are returned for the owner".
                f(() => addresses.Count().ShouldEqual(3));
            "Then each address references the expected owner id".
                f(() => addresses.ToList().ForEach(address => address.Value<int>("OwnerId").ShouldEqual(CustomerId)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingACustomerAddressByType(JArray addresses)
        {
            const int CustomerId = 346760;

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Type.Contains(this.AddressTypes[1])))).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<int>("OwnerId") == customerId &&
                                                       addressFilter.Type.Contains(n.Value<string>("Type"))
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'address type for a customer' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "?Type=" + this.AddressTypes[1])).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Type.Contains(this.AddressTypes[1])))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then an address is returned".
                f(() => addresses.Count().ShouldEqual(1));
            "Then each address references the queried customer".
                f(() => addresses.ToList().ForEach(address => CustomerId.ShouldEqual(address.Value<int>("OwnerId"))));
            "Then each address references the queried type".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Type").ShouldEqual(this.AddressTypes[1])));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingACustomerAddressByTypes(JArray addresses)
        {
            const int CustomerId = 346760;

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Type.Contains(this.AddressTypes[1]) || addressFilter.Type.Contains(this.AddressTypes[2])))).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<int>("OwnerId") == customerId &&
                                                       addressFilter.Type.Contains(n.Value<string>("Type"))
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'address types for a customer' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "?Type=" + this.AddressTypes[1] + "&Type=" + this.AddressTypes[2])).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Type.Contains(this.AddressTypes[1]) || addressFilter.Type.Contains(this.AddressTypes[2])))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then an address is returned".
                f(() => addresses.Count().ShouldEqual(2));
            "Then each address references the queried customer".
                f(() => addresses.ToList().ForEach(address => CustomerId.ShouldEqual(address.Value<int>("OwnerId"))));
            "Then each address references the queried type".
                f(() => addresses.ToList().ForEach(address => this.AddressTypes.ShouldContain(address.Value<string>("Type"))));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingCustomerAddressesByStreet1(JArray addresses)
        {
            const int CustomerId = 349075;
            const string Street1 = "TUCAPAU";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Street1.Contains(Street1)))).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where customerId == n.Value<int>("OwnerId") && 
                                                       n.Value<string>("Street1").Contains(addressFilter.Street1)
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for a customer by street1' request is sent".
                 f(() =>
                 {
                     Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "/?Street1=" + Street1)).Result;
                 });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Street1.Contains(Street1)))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then addresses are returned".
                f(() => addresses.Count().ShouldEqual(1));
            "Then the address references the queried customer".
                f(() => addresses.First().Value<int>("OwnerId").ShouldEqual(CustomerId));
            "Then each address references the queried street".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Street1").ShouldContain(Street1)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingCustomerAddressesByStreet2(JArray addresses)
        {
            const int CustomerId = 42165;
            const string Street2 = "VOGELWEIHERSTR";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Street2.Contains(Street2)))).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("Street2") != null &&
                                                       n.Value<string>("Street2").Contains(addressFilter.Street2) &&
                                                       n.Value<int>("OwnerId") == CustomerId
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for a customer by street2' request is sent".
                 f(() =>
                 {
                     Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "/?Street2=" + Street2)).Result;
                 });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Street2.Contains(Street2)))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then addresses are returned".
                f(() => addresses.Count().ShouldEqual(1));
            "Then the address references the queried customer".
                f(() => addresses.First().Value<int>("OwnerId").ShouldEqual(CustomerId));
            "Then each address references the queried street2".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Street2").ShouldContain(Street2)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingCustomerAddressesBySuite(JArray addresses)
        {
            const int CustomerId = 346760;
            const string Suite = "sweet";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Suite.Contains(Suite)))).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("Suite") != null && 
                                                       n.Value<string>("Suite").ToString().ToLowerInvariant().Contains(addressFilter.Suite.ToLowerInvariant()) &&
                                                       n.Value<int>("OwnerId") == CustomerId
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for a customer by suite' request is sent".
                  f(() =>
                  {
                      Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "/?Suite=" + Suite)).Result;
                  });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Suite.Contains(Suite)))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then addresses are returned".
                f(() => addresses.Count().ShouldEqual(1));
            "Then the address references the queried customer".
                f(() => addresses.First().Value<int>("OwnerId").ShouldEqual(CustomerId));
            "Then each address references the queried suite".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Suite").ToLowerInvariant().ShouldContain(Suite.ToLowerInvariant())));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingCustomerAddressesByZipCode(JArray addresses)
        {
            const int CustomerId = 346760;
            const string ZipCode = "802";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Zip.Contains(ZipCode)))).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("ZipCode").Contains(addressFilter.Zip) &&
                                                       n.Value<int>("OwnerId") == CustomerId
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for a customer by zip code' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "/?Zip=" + ZipCode)).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Zip.Contains(ZipCode)))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then addresses are returned".
                f(() => addresses.Count().ShouldEqual(2));
            "Then the address references the queried customer".
                f(() => addresses.First().Value<int>("OwnerId").ShouldEqual(CustomerId));
            "Then each address references the queried zip code".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("ZipCode").ShouldContain(ZipCode)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingCustomerAddressesByCity(JArray addresses)
        {
            const int CustomerId = 346760;
            const string City = "Denver";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.City == City))).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("City").ToLowerInvariant().Contains(City.ToLowerInvariant()) &&
                                                       n.Value<int>("OwnerId") == CustomerId
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for a customer by city' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "/?City=" + City)).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.City == City))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then addresses are returned".
                f(() => addresses.Count().ShouldEqual(2));
            "Then the address references the queried customer".
                f(() => addresses.First().Value<int>("OwnerId").ShouldEqual(CustomerId));
            "Then each address's city references the queried city".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("City").ToLowerInvariant().ShouldContain(City.ToLowerInvariant())));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingCustomerAddressesByState(JArray addresses)
        {
            const int CustomerId = 346760;
            const string State = "CO";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.State == State))).Returns((int customer, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("State") == State &&
                                                       n.Value<int>("OwnerId") == CustomerId
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for a customer by state' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "/?State=" + State)).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.State == State))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then addresses are returned".
                f(() => addresses.Count().ShouldEqual(2));
            "Then the address references the queried customer".
                f(() => addresses.First().Value<int>("OwnerId").ShouldEqual(CustomerId));
            "Then each address references the queried state".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("State").ShouldEqual(State)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingCustomerAddressesByCountry(JArray addresses)
        {
            const int CustomerId = 42165;
            const string Country = "DE";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Country == Country))).Returns((int customerId, AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("Country") == Country &&
                                                       n.Value<int>("OwnerId") == CustomerId
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for a customer by country' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(string.Format(_customerAddressesFormat, CustomerId) + "/?Country=" + Country)).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetCustomerAddresses(It.Is<int>(customerId => customerId == CustomerId), It.Is<AddressesFilter>(addressFilter => addressFilter.Country == Country))));
            "Then a response is received by the HTTP client".
                f(() =>
                {
                    Response.Content.ShouldNotBeNull();
                });
            "Then content should be returned".
                f(() =>
                {
                    addresses = Response.Content.ReadAsAsync<JArray>().Result;
                    addresses.ShouldNotBeNull();
                });
            "Then a '200 OK' status is returned".
                f(() => Response.StatusCode.ShouldEqual(HttpStatusCode.OK));
            "Then addresses are returned".
                f(() => addresses.Count().ShouldEqual(1));
            "Then the address references the queried customer".
                f(() => addresses.First().Value<int>("OwnerId").ShouldEqual(CustomerId));
            "Then each address references the queried country".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Country").ShouldEqual(Country)));
        }
    }
}

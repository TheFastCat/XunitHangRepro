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

    public class RetrievingAddresses : AddressesTestsSetup
    {
        readonly Uri _addressesUri = new Uri("http://localhost/api/addresses");

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAllAddresses(JArray addresses)
        {
            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(filter => filter == null))).Returns<dynamic>(filter =>
                     {
                         return Task.FromResult<dynamic>(this.FakeAddresses);
                     });
                 });
            "When a GET addresses request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(_addressesUri).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.IsAny<AddressesFilter>())));
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
            "Then all addresses are returned for all customers".
                f(() => addresses.Count().ShouldEqual(this.FakeAddresses.Count()));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAllAddressesForOwnerId(JArray addresses)
        {
            const int OwnerIdTest = 346760;

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerId.Contains(OwnerIdTest)))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where addressFilter.OwnerId.Contains(n.Value<int>("OwnerId"))
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'all addresses for owner id' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(_addressesUri + "/?OwnerId=" + OwnerIdTest)).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerId.Contains(OwnerIdTest)))));
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
            "Then all addresses are returned for the owner id".
                f(() => addresses.Count().ShouldEqual(3));
            "Then each address references the expected owner id".
                f(() => addresses.ToList().ForEach(address => address.Value<int>("OwnerId").ShouldEqual(OwnerIdTest)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAllAddressesForOwnerIds(JArray addresses)
        {
            const int OwnerId1 = 346760;
            const int OwnerId2 = 42165;

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerId.Contains(OwnerId1) && addressFilter.OwnerId.Contains(OwnerId2)))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where addressFilter.OwnerId.Contains(n.Value<int>("OwnerId"))
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for multiple owner ids' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(_addressesUri + "/?OwnerId=" + OwnerId1 + "&OwnerId=" + OwnerId2)).Result;
                });
            "Then the request is received by the API Controller".
                  f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerId.Contains(OwnerId1) && addressFilter.OwnerId.Contains(OwnerId2)))));
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
            "Then the expected addresses are returned".
                f(() => addresses.Count().ShouldEqual(4));
            "Then each address references a queried owner".
                f(() => addresses.ToList().ForEach(address => new int[] { OwnerId1, OwnerId2 }.ShouldContain(address.Value<int>("OwnerId"))));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAllAddressesForOwnerType(JArray addresses)
        {
            const string OwnerTypeTest = "BUS";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerType.Contains(OwnerTypeTest)))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where addressFilter.OwnerType.Contains(n.Value<string>("OwnerType"))
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'all addresses for owner type' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(_addressesUri + "/?OwnerType=" + OwnerTypeTest)).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerType.Contains(OwnerTypeTest)))));
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
            "Then all addresses are returned for the queried owner type".
                f(() => addresses.Count().ShouldEqual(8));
            "Then each address references the expected owner type".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("OwnerType").ShouldEqual(OwnerTypeTest)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAllAddressesForOwnerTypes(JArray addresses)
        {
            string[] ownerType = { "BUS", "PER" }; 

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerType.Contains(ownerType[0]) && addressFilter.OwnerType.Contains(ownerType[1])))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where addressFilter.OwnerType.Contains(n.Value<string>("OwnerType"))
                                                 select n;
                         
                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses for multiple owner types' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(_addressesUri + "/?OwnerType=" + ownerType[0] + "&OwnerType=" + ownerType[1])).Result;
                });
            "Then the request is received by the API Controller".
                  f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerType.Contains(ownerType[0]) && addressFilter.OwnerType.Contains(ownerType[1])))));
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
            "Then the expected addresses are returned".
                f(() => addresses.Count().ShouldEqual(9));
            "Then each address references a queried owner type".
                f(() => addresses.ToList().ForEach(address => new string[] { ownerType[0], ownerType[1] }.ShouldContain(address.Value<string>("OwnerType"))));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAllAddressesForOwnerName(JArray addresses)
        {
            const string OwnerNameTest = "THE DILLION COMPANY";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerName.Contains(OwnerNameTest)))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where addressFilter.OwnerName.Contains(n.Value<string>("OwnerName"))
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'all addresses for owner name' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(_addressesUri + "/?OwnerName=" + OwnerNameTest)).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerName.Contains(OwnerNameTest)))));
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
            "Then all addresses are returned for the queried owner name".
                f(() => addresses.Count().ShouldEqual(3));
            "Then each address references the expected owner name".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("OwnerName").ShouldEqual(OwnerNameTest)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAnAddressTypeForAnOwner(JArray addresses)
        {
            const int OwnerIdTest = 346760;

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerId.Contains(OwnerIdTest) && addressFilter.Type.Contains(this.AddressTypes[2])))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where addressFilter.OwnerId.Contains(n.Value<int>("OwnerId")) &&
                                                       addressFilter.Type.Contains(n.Value<string>("Type")) 
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'address for owner id and type' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(new Uri(_addressesUri + "/?OwnerId=" + OwnerIdTest + "&Type=" + this.AddressTypes[2])).Result;
                });
            "Then the request is received by the API Controller".
                  f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.OwnerId.Contains(OwnerIdTest) && addressFilter.Type.Contains(this.AddressTypes[2])))));
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
            "Then the address references the queried owner id".
                f(() => addresses.ToList().ForEach(address => OwnerIdTest.ShouldEqual(address.Value<int>("OwnerId"))));
            "Then the address references the queried type".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Type").ShouldEqual(this.AddressTypes[2])));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesByStreet1(JArray addresses)
        {
            const string Street1 = "TUCAPAU";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Street1.Contains(Street1)))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("Street1").Contains(addressFilter.Street1)
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by street1' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(_addressesUri + "/?Street1=" + Street1).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Street1.Contains(Street1)))));
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
            "Then each address references the queried street".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Street1").ShouldContain(Street1)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesByStreet2(JArray addresses)
        {
            const string Street2 = "VOGELWEIHERSTR";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Street2.Contains(Street2)))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("Street2") != null &&
                                                       n.Value<string>("Street2").Contains(addressFilter.Street2)
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by street2' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(_addressesUri + "/?Street2=" + Street2).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Street2.Contains(Street2)))));
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
            "Then each address references the queried street2".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Street2").ShouldContain(Street2)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesByType(JArray addresses)
        {
            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Type.Contains(this.AddressTypes.First())))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where addressFilter.Type.Contains(n.Value<string>("Type"))
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by type' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(_addressesUri + "/?Type=" + this.AddressTypes.First()).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Type.Contains(this.AddressTypes.First())))));
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
                f(() => addresses.Count().ShouldEqual(5));
            "Then each address references the queried type".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Type").ShouldEqual(this.AddressTypes.First())));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesByTypes(JArray addresses)
        {
            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Type.Contains(this.AddressTypes[0]) && addressFilter.Type.Contains(this.AddressTypes[1])))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where addressFilter.Type.Contains(n.Value<string>("Type"))
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by multiple types' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(_addressesUri + "/?Type=" + this.AddressTypes[0] + "&Type=" + this.AddressTypes[1]).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Type.Contains(this.AddressTypes[1]) && addressFilter.Type.Contains(this.AddressTypes[1])))));
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
                f(() => addresses.Count().ShouldEqual(6));
            "Then each address references a queried type".
                f(() => addresses.ToList().ForEach(address => new string[] { this.AddressTypes[0], this.AddressTypes[1] }.Contains(address.Value<string>("Type"))));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesBySuite(JArray addresses)
        {
            const string Suite = "sweet";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Suite.Contains(Suite)))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("Suite") != null && n.Value<string>("Suite").ToString().ToLowerInvariant().Contains(addressFilter.Suite.ToLowerInvariant())
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by suite request' is sent".
                 f(() =>
                 {
                     Response = Client.GetAsync(_addressesUri + "/?Suite=" + Suite).Result;
                 });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Suite.Contains(Suite)))));
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
            "Then each address references the queried suite".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Suite").ToLowerInvariant().ShouldContain(Suite.ToLowerInvariant())));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesByZipCode(JArray addresses)
        {
            const string ZipCode = "802";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Zip.Contains(ZipCode)))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("ZipCode").Contains(addressFilter.Zip)
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by zip code request' is sent".
                  f(() =>
                  {
                      Response = Client.GetAsync(_addressesUri + "/?Zip=" + ZipCode).Result;
                  });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Zip.Contains(ZipCode)))));
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
                f(() => addresses.Count().ShouldEqual(3));
            "Then each address references the queried zip code".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("ZipCode").ShouldContain(ZipCode)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesByCity(JArray addresses)
        {
            const string City = "Denver";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.City == City))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("City").ToLowerInvariant().Contains(City.ToLowerInvariant())
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by city' request is sent".
                  f(() =>
                  {
                      Response = Client.GetAsync(_addressesUri + "/?City=" + City).Result;
                  });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.City == City))));
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
                f(() => addresses.Count().ShouldEqual(3));
            "Then each address's city references the queried city".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("City").ToLowerInvariant().ShouldContain(City.ToLowerInvariant())));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesByState(JArray addresses)
        {
            const string State = "CO";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.State == State))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("State") == State
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by state' request is sent".
                    f(() =>
                    {
                        Response = Client.GetAsync(_addressesUri + "/?State=" + State).Result;
                    });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.State == State))));
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
                f(() => addresses.Count().ShouldEqual(3));
            "Then each address references the queried state".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("State").ShouldEqual(State)));
        }

        [Scenario]
        [Trait("Category", "Api.System")]
        public void RetrievingAddressesByCountry(JArray addresses)
        {
            const string Country = "DE";

            "Given existing addresses".
                 f(() =>
                 {
                     MockAddressesStore.Setup(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Country == Country))).Returns((AddressesFilter addressFilter) =>
                     {
                         var filteredAddresses = from n in this.FakeAddresses
                                                 where n.Value<string>("Country") == Country
                                                 select n;

                         return Task.FromResult<dynamic>(filteredAddresses);
                     });
                 });
            "When a GET 'addresses by country' request is sent".
                f(() =>
                {
                    Response = Client.GetAsync(_addressesUri + "/?Country=" + Country).Result;
                });
            "Then the request is received by the API Controller".
                f(() => MockAddressesStore.Verify(i => i.GetAddresses(It.Is<AddressesFilter>(addressFilter => addressFilter.Country == Country))));
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
            "Then each address references the queried country".
                f(() => addresses.ToList().ForEach(address => address.Value<string>("Country").ShouldEqual(Country)));
        }
    }
}

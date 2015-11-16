namespace Api.Test.System.ApiControllers.AddressesController
{
    using global::System.Collections.Generic;
    using global::System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Api.ApiControllers;
    using Microsoft.Owin.Testing;
    using Moq;
    using Newtonsoft.Json.Linq;

    public abstract class AddressesTestsSetup : TestBase
    {
        public AddressesTestsSetup()
        {
            AddressTypes = new string[] { "PHYS", "MEET", "MAIL", "BILL" };
            MockAddressesStore = new Mock<IAddresses>();
            FakeAddresses = GetFakeAddresses();

            ContainerBuilder.RegisterInstance(MockAddressesStore.Object)
                                              .As<IAddresses>()
                                              .SingleInstance();

            HttpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(ContainerBuilder.Build());
            HttpConfiguration.MapHttpAttributeRoutes();
            HttpConfiguration.EnsureInitialized();

            ApiServer = TestServer.Create(new Startup(HttpConfiguration).Configuration);
            Client = ApiServer.HttpClient;
        }

        protected string[] AddressTypes { get; private set; } 
        protected Mock<IAddresses> MockAddressesStore { get; private set; }
        protected List<JObject> FakeAddresses { get; private set; }

        private List<JObject> GetFakeAddresses()
        {
            List<JObject> fakeAddresses = new List<JObject>();
            fakeAddresses.Add(JObject.Parse(@"{
                                            OwnerId: 346760,
                                            OwnerType: 'BUS',
                                            OwnerName: 'THE DILLION COMPANY',
                                            Type: 'PHYS',
                                            Street1: '3930 COOK STREET',
                                            Street2: '(Blue House)',
                                            Suite: 'Sweet',
                                            City: 'DENVER',
                                            ZipCode: '80205',
                                            State: 'CO',
                                            Country: 'US',
                                            Latitude: 34.00,
                                            Longitude: -134.00,
                                            TimeZone: 89
                                            }"));

            fakeAddresses.Add(JObject.Parse(@"{
                                            OwnerId: 346760,
                                            OwnerType: 'BUS',
                                            OwnerName: 'THE DILLION COMPANY',
                                            Type: 'MAIL',
                                            Street1: 'PO Box 45321',
                                            Street2: null,
                                            Suite: null,
                                            City: 'Boston',
                                            ZipCode: '02115',
                                            State: 'MA',
                                            Country: 'US',
                                            Latitude: 34.00,
                                            Longitude: -134.00,
                                            TimeZone: 89
                                            }"));

            fakeAddresses.Add(JObject.Parse(@"{
                                            OwnerId: 346760,
                                            OwnerType: 'BUS',
                                            OwnerName: 'THE DILLION COMPANY',
                                            Type: 'MEET',
                                            Street1: '46 West Colfax',
                                            Street2: null,
                                            Suite: null,
                                            City: 'DENVER',
                                            ZipCode: '80210',
                                            State: 'CO',
                                            Country: 'US',
                                            Latitude: 34.00,
                                            Longitude: -134.00,
                                            TimeZone: 89
                                            }"));

            fakeAddresses.Add(JObject.Parse(@"{
                                            OwnerId: 298648,
                                            OwnerType: 'PER',
                                            OwnerName: 'SCOTT MCCLELLAN',
                                            Type: 'PHYS',
                                            Street1: '2515 HUMBOLDT',
                                            Street2: null,
                                            Suite: null,
                                            City: 'DENVER',
                                            ZipCode: '80205',
                                            State: 'CO',
                                            Country: 'US',
                                            Latitude: 34.00,
                                            Longitude: -134.00,
                                            TimeZone: 89
                                            }"));

            fakeAddresses.Add(JObject.Parse(@"{
                                            OwnerId: 349075,
                                            OwnerType: 'BUS',
                                            OwnerName: 'T E I CONSTRUCTION SERVICES',
                                            Type: 'MAIL',
                                            Street1: '170 TUCAPAU RD',
                                            Street2: null,
                                            Suite: null,
                                            City: 'DUNCAN',
                                            ZipCode: '29334',
                                            State: 'SC',
                                            Country: 'US',
                                            Latitude: 34.00,
                                            Longitude: -134.00,
                                            TimeZone: 89
                                            }"));

            fakeAddresses.Add(JObject.Parse(@"{
                                            Owner: 276207,
                                            OwnerType: 'BUS',
                                            OwnerName: 'EAST FABRICATION',
                                            Type: 'PHYS',
                                            Street1: '4911 W 70TH TER',
                                            Street2: null,
                                            Suite: null,
                                            City: 'PRARIE VILLAGE',
                                            ZipCode: '66208',
                                            State: 'KS',
                                            Country: 'US',
                                            Latitude: 34.00,
                                            Longitude: -134.00,
                                            TimeZone: 89
                                            }"));

            fakeAddresses.Add(JObject.Parse(@"{
                                            OwnerId: 348942,
                                            OwnerType: 'BUS',
                                            OwnerName: 'INTERNATIONAL PAPER',
                                            Type: 'BILL',
                                            Street1: 'P O BOX 818',
                                            Street2: null,
                                            Suite: null,
                                            City: 'MILFORD',
                                            ZipCode: '45150',
                                            State: 'OH',
                                            Country: 'US',
                                            Latitude: 44.00,
                                            Longitude: -70.00,
                                            TimeZone: 89
                                            }"));

            fakeAddresses.Add(JObject.Parse(@"{
                                            OwnerId: 42165,
                                            OwnerType: 'BUS',
                                            OwnerName: 'BUNA ELECTRIC MOTOR SERVICE',
                                            Type: 'PHYS',
                                            Street1: 'A & D LDTSCM',
                                            Street2: 'VOGELWEIHERSTR 115',
                                            Suite: null,
                                            City: 'NUREMBERG',
                                            ZipCode: '90441',
                                            State: null,
                                            Country: 'DE',
                                            Latitude: 34.00,
                                            Longitude: -134.00,
                                            TimeZone: 89
                                            }"));

            fakeAddresses.Add(JObject.Parse(@"{
                                            OwnerId: 343733,
                                            OwnerType: 'BUS',
                                            OwnerName: 'MIDWEST MUNICIPAL SUPPLY',
                                            Type: 'PHYS',
                                            Street1: '1435 BLUFF RD',
                                            Street2: null,
                                            Suite: null,
                                            City: 'COLLINSVILLE',
                                            ZipCode: '62234',
                                            State: 'IL',
                                            Country: 'US',
                                            Latitude: 34.00,
                                            Longitude: -134.00,
                                            TimeZone: 89
                                            }")); 
            return fakeAddresses;
        }
    }
}

namespace Api.ApiControllers
{
    using System.Collections.Generic;

    public class AddressesFilter
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Suite { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public List<string> Type { get; set; } // 'MEET','MAIL','PHYS','BILL'
        public List<long?> OwnerId { get; set; }
        public List<string> OwnerType { get; set; } // 'BUS' (business), 'PER" (person)
        public string OwnerName { get; set; } // 'Description (name) of business or person
    }
}

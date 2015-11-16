namespace Api.ApiControllers
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class Address
    {
        public long? OwnerId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string Type { get; set; } // MAIL, MEET, PHYS, BILL

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string OwnerType { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string OwnerName { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string Street1 { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string Street2 { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string Suite { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string City { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string State { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string Zip { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string Country { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue("")]
        public string ChangedById { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(null)]
        public decimal? Latitude { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(null)]
        public decimal? Longitude { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(null)]
        public int? TimeZone { get; set; }
    }
}

namespace Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Api.ApiControllers;

    [RoutePrefix("api")]
    public class AddressesController : ClientApiBase, IAddresses
    {
        readonly IAddresses _addresses;

        public AddressesController(IAddresses addresses)
        {
            _addresses = addresses;
        }

        [Route("addresses", Name = "GetAddressesByFilter")]
        public async Task<dynamic> GetAddresses([FromUri]AddressesFilter addressFilter)
        {
            return await _addresses.GetAddresses(addressFilter);
        }

        [Route("employee/{employeeCode}/addresses", Name = "GetEmployeeAddressesByFilter")]
        public async Task<dynamic> GetEmployeeAddresses([FromUri]string employeeCode, [FromUri]AddressesFilter addressFilter)
        {
            return await _addresses.GetEmployeeAddresses(employeeCode, addressFilter);
        }

        [Route("customers/{id:int}/addresses", Name = "GetCustomerAddressesByFilter")]
        public async Task<dynamic> GetCustomerAddresses([FromUri]int id, [FromUri]AddressesFilter addressFilter)
        {
            return await _addresses.GetCustomerAddresses(id, addressFilter); 
        }

        [Route("addresses", Name = "CreateAddress")]
        public async Task<dynamic> PostAddress([FromBody]Address address)
        {
            address.OwnerId = await _addresses.PostAddress(address); // this scalar insert returns the ownerID for the address inserted into the DB  

            switch (address.OwnerId)
            {
                case -1: return StatusCode(HttpStatusCode.Conflict); // this will return failure response/message
                default: HttpResponseMessage successResponse = Request.CreateResponse(HttpStatusCode.Created, address);

                         // the returned ID represents a SoloRecords.solo_key...
                         IDictionary<string, object> routeArgs = new Dictionary<string, object>();
                         routeArgs.Add("OwnerId", address.OwnerId);
                         routeArgs.Add("Type", address.Type);
                         string uri = Url.Link("GetAddressesByFilter", routeArgs); // add a locationHeader header from the response content as per the HTTP 1.1 spec
                         successResponse.Headers.Location = new Uri(uri);
                         return successResponse;
            }
        }

        [Route("addresses", Name = "PatchAddress")]
        public async Task<dynamic> PatchAddress([FromBody]Address address)
        {
            long result = await _addresses.PatchAddress(address);

            switch (result)
            {
                case 0:  return StatusCode(HttpStatusCode.NotFound);
                default: HttpResponseMessage successResponse = Request.CreateResponse(HttpStatusCode.NoContent);

                         // the returned ID represents a SoloRecords.solo_key...
                         IDictionary<string, object> routeArgs = new Dictionary<string, object>();
                         routeArgs.Add("OwnerId", address.OwnerId);
                         routeArgs.Add("Type", address.Type);
                         string uri = Url.Link("GetAddressesByFilter", routeArgs); // add a locationHeader header from the response content as per the HTTP 1.1 spec
                         successResponse.Headers.Location = new Uri(uri);
                         return successResponse;                  
            }
        }

        [Route("addresses", Name = "DeleteAddress")]
        public async Task<dynamic> DeleteAddress([FromBody]Address address)
        {
            var result = await _addresses.DeleteAddress(address);
            HttpStatusCode statusCode = result > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return Request.CreateResponse(statusCode);  
        }

        [Route("addresses", Name = "PutAddress")]
        public async Task<dynamic> PutAddress([FromBody]Address address)
        {
            long result = await _addresses.PutAddress(address);

            HttpResponseMessage successResponse = new HttpResponseMessage();

            switch (result)
            {
                case 0: return StatusCode(HttpStatusCode.NotFound);
                case 1: successResponse = Request.CreateResponse(HttpStatusCode.NoContent);
                        break;
                case 2: successResponse = Request.CreateResponse(HttpStatusCode.Created, address); 
                        break;                 
            }
             
            // the returned ID represents a SoloRecords.solo_key...
            IDictionary<string, object> routeArgs = new Dictionary<string, object>();
            routeArgs.Add("OwnerId", address.OwnerId);
            routeArgs.Add("Type", address.Type);
            string uri = Url.Link("GetAddressesByFilter", routeArgs); // add a locationHeader header from the response content as per the HTTP 1.1 spec
            successResponse.Headers.Location = new Uri(uri);     

            return successResponse;
        }
    }
}

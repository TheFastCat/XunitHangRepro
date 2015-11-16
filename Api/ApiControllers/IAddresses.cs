namespace Api.ApiControllers
{
    using System.Threading.Tasks;

    public interface IAddresses
    {
        Task<dynamic> GetAddresses(AddressesFilter addressFilter);
        Task<dynamic> GetCustomerAddresses(int id, AddressesFilter addressFilter);
        Task<dynamic> GetEmployeeAddresses(string employeeCode, AddressesFilter addressFilter);
        Task<dynamic> PostAddress(Address address);
        Task<dynamic> PatchAddress(Address address);
        Task<dynamic> DeleteAddress(Address address);
        Task<dynamic> PutAddress(Address address);
    }
}

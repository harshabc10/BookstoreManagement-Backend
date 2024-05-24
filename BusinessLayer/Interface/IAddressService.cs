using ModelLayer.Entity;
using ModelLayer.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAddressService
    {
        Task<IEnumerable<Object>> GetAddresses(int userId);
        Task<Address> GetAddressById(int addressId);
        Task AddAddress(AddressRequest addressRequest, int userId);
        Task UpdateAddress(int addressId, AddressRequest addressRequest, int userId);
        Task DeleteAddress(int addressId);
    }
}

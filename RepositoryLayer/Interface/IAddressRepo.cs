using ModelLayer.Entity;
using ModelLayer.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public  interface IAddressRepo
    {
        Task<IEnumerable<Object>> GetAddresses(int userId);
        Task<Address> GetAddressById(int addressId);
        Task AddAddress(Address address);
        Task UpdateAddress(Address address);
        Task DeleteAddress(int addressId);
    }
}

using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public  interface IAddressRepo
    {
        Task<IEnumerable<Address>> GetAddresses(int userId);
        Task<Address> GetAddressById(int addressId);
        Task AddAddress(Address address);
        Task UpdateAddress(Address address);
        Task DeleteAddress(int addressId);
    }
}

using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressServiceImpl : IAddressService
    {
        private readonly IAddressRepo _addressRepository;

        public AddressServiceImpl(IAddressRepo addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<Address>> GetAddresses(int userId)
        {
            return await _addressRepository.GetAddresses(userId);
        }

        public async Task<Address> GetAddressById(int addressId)
        {
            return await _addressRepository.GetAddressById(addressId);
        }

        public async Task AddAddress(AddressRequest addressRequest, int userId)
        {
            var address = new Address
            {
                address = addressRequest.address,
                city = addressRequest.city,
                state = addressRequest.state,
                type = addressRequest.type,
                userId = userId
            };
            await _addressRepository.AddAddress(address);
        }

        public async Task UpdateAddress(int addressId, AddressRequest addressRequest, int userId)
        {
            var address = await _addressRepository.GetAddressById(addressId);
            if (address == null) throw new Exception("Address not found");

            address.address = addressRequest.address;
            address.city = addressRequest.city;
            address.state = addressRequest.state;
            address.type = addressRequest.type;
            address.userId = userId;

            await _addressRepository.UpdateAddress(address);
        }

        public async Task DeleteAddress(int addressId)
        {
            await _addressRepository.DeleteAddress(addressId);
        }
    }

}

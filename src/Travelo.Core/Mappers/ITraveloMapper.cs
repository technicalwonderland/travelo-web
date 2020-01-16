using System.Collections.Generic;
using Travelo.Core.Domain;
using Travelo.Core.Domain.DTO;

namespace Travelo.Core.Mappers
{
    public interface ITraveloMapper
    {
        CustomerDTO MapToCustomerDto(Customer customer);
        TripDTO MapToTripDto(Trip trip);
    }
}
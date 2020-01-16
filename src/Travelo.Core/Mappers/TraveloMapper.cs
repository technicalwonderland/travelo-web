using System.Collections.Generic;
using System.Linq;
using Travelo.Core.Domain;
using Travelo.Core.Domain.DTO;

namespace Travelo.Core.Mappers
{
    public class TraveloMapper : ITraveloMapper
    {
        public CustomerDTO MapToCustomerDto(Customer customer)
        {
            return new CustomerDTO()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                FullName = customer.FullName
            };
        }

        public TripDTO MapToTripDto(Trip trip)
        {
            var customersDtos = trip.CustomerTrips.Select(x => MapToCustomerDto(x.Customer));
            return new TripDTO()
            {
                Id = trip.Id,
                Customers = customersDtos,
                StartDate = trip.StartDateUTC,
                EndDate = trip.EndDateUTC,
                TripStatus = trip.TripStatus,
                Destination = trip.TripDestination,
                Description =  trip.Description,
                Name = trip.Name
            };
        }

    }
}
using System.Collections.Generic;
using System.Linq;

namespace Travelo.Core.Domain
{
    public static class Extensions
    {
        public static void CheckForOverlappingTripsOrFail(this Trip trip, Customer customer)
        {
            if (customer.CustomerTrips.Any(t => t.CustomerId == customer.Id &&
                                                t.Trip.StartDateUTC < trip.EndDateUTC &&
                                                trip.StartDateUTC < t.Trip.EndDateUTC))
            {
                throw new DomainException(DomainErrorCodes.OverlappingTripDates, null,
                    "Trip is overlapping with already booked trip.");
            }
        }

        public static void CheckForDuplicatedIdsOrFail(this IEnumerable<CustomerTrip> customerTrips, CustomerTrip customerTrip)
        {
            if (customerTrips.Any(x => x.Id == customerTrip.Id))
            {
                throw new DomainException(DomainErrorCodes.CustomerAlreadyAssignedToThisTrip, null,
                    "Customer already assigned to this trip!");
            }
        }
    }
}
using System;

namespace Travelo.Core.Domain
{
    public class CustomerTrip : Entity
    {
        public Guid CustomerId { get; protected set; }
        public Customer Customer { get; protected set; }

        public Guid TripId { get; protected set; }
        public Trip Trip { get; protected set; }

        protected CustomerTrip()
        {
        }

        public CustomerTrip(Guid id, Customer customer, Trip trip)
        {
            Id = id;
            TripId = trip.Id;
            Trip = trip;
            CustomerId = customer.Id;
            Customer = customer;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Travelo.Core.Domain
{
    public class Customer : Entity
    {
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string FullName { get; protected set; }
        public IEnumerable<CustomerTrip> CustomerTrips => _customerTrips;
        private ISet<CustomerTrip> _customerTrips;

        protected Customer()
        {
            _customerTrips = new HashSet<CustomerTrip>();
        }

        public Customer(Guid id, string firstName, string lastName)
        {
            Id = id;
            _customerTrips = new HashSet<CustomerTrip>();
            SetInitialDataOrFail(firstName, lastName);
        }

        public void EditCustomer(string firstName, string lastName)
        {
            SetInitialDataOrFail(firstName, lastName);
        }

        private void SetInitialDataOrFail(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException(DomainErrorCodes.ArgumentNullOrEmpty, null,
                    "First name or last name is empty or null.");
            }

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            FullName = $"{FirstName} {LastName}";
        }
        
        public void AddTrip(CustomerTrip customerTrip)
        {
            if (customerTrip == null)
            {
                throw new DomainException(DomainErrorCodes.ArgumentNullOrEmpty, null, "Trip cannot be null!");
            }
            if (customerTrip.Customer == null || customerTrip.Trip == null)
            {
                throw new DomainException(DomainErrorCodes.ArgumentNullOrEmpty, null, "Customer or Trip in customer trip is null");
            }
            if (_customerTrips.Any(x => x.Id == customerTrip.Id))
            {
                throw new DomainException(DomainErrorCodes.CustomerAlreadyAssignedToThisTrip, null,
                    "Customer already is on this trip");
            }
            customerTrip.Trip.CheckForOverlappingTripsOrFail(this);
            _customerTrips.Add(customerTrip);
        }
    }
}
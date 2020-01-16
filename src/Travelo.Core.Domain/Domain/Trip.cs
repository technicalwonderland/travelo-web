using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Travelo.Core.Domain.DTO;
using Travelo.Core.Domain.Providers;

namespace Travelo.Core.Domain
{
    public class Trip : Entity
    {
        public IEnumerable<CustomerTrip> CustomerTrips => _customerTrips;
        public DateTimeOffset StartDateUTC { get; protected set; }
        public DateTimeOffset EndDateUTC { get; protected set; }
        public TripStatus TripStatus { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string TripDestination { get; protected set; }

        //this shouldn't be here
        public static TimeSpan TripMaximumTimeLimit = new TimeSpan(31, 0, 0, 0);
        public static TimeSpan TripMinimumTimeLimit = new TimeSpan(0, 8, 0, 0);

        private ISet<CustomerTrip> _customerTrips;
        private IDateTimeOffsetProvider _dateTimeOffsetProvider;

        protected Trip()
        {
            _dateTimeOffsetProvider = new DateTimeOffsetProvider();
        }

        public Trip(Guid id, string name, string tripDestination, DateTimeOffset startDate, DateTimeOffset endDate,
            IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            Id = id;
            _customerTrips = new HashSet<CustomerTrip>();
            SetInitialDataOrFail(name, tripDestination, dateTimeOffsetProvider);
            SetDatesOrFail(startDate.ToUniversalTime(), endDate.ToUniversalTime());
            TripStatus = TripStatus.Active;
        }

        public void AddCustomer(Customer customer)
        {
            if (StartDateUTC < _dateTimeOffsetProvider.UtcNow)
            {
                throw new DomainException(DomainErrorCodes.TripAlreadyStarted, null,
                    "Cannot add customer to trip, trip already started!");
            }

            if (_customerTrips.Any(x =>
                x.CustomerId == customer.Id && x.TripId == Id || x.Id == customer.Id))
            {
                throw new DomainException(DomainErrorCodes.CustomerAlreadyAssignedToThisTrip, null,
                    "Customer is already assigned to this trip!");
            }

            this.CheckForOverlappingTripsOrFail(customer);

            var customerTrip = new CustomerTrip(Guid.NewGuid(), customer, this);
            _customerTrips.Add(customerTrip);
            customer.AddTrip(customerTrip);
        }

        public void Edit(string name, string destination, TripStatus tripStatus,
            DateTimeOffset startDate, DateTimeOffset endDate)
        {
            SetDataOrFail(name, destination);
            SetDatesOrFail(startDate.ToUniversalTime(), endDate.ToUniversalTime());
            TripStatus = tripStatus;
        }

        private void SetDataOrFail(string name, string destination)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(destination))
            {
                throw new DomainException(DomainErrorCodes.ArgumentNullOrEmpty, null, "Name or destination of trip is null!");
            }

            Name = name;
            TripDestination = destination;
        }

        private void SetInitialDataOrFail(string name, string tripDestination,
            IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(tripDestination) ||
                dateTimeOffsetProvider == null)
            {
                throw new DomainException(DomainErrorCodes.ArgumentNullOrEmpty, null,
                    "Trip name, destination or dateTimeOffsetProvider is null!");
            }

            _dateTimeOffsetProvider = dateTimeOffsetProvider;
            Name = name;
            TripDestination = tripDestination;
        }

        private void SetDatesOrFail(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (startDate > endDate)
            {
                throw new DomainException(DomainErrorCodes.StartDateAfterEndDate, null,
                    "Start date {0} cannot equal to or after end date {1}",
                    startDate.ToString(CultureInfo.InvariantCulture), endDate.ToString(CultureInfo.InvariantCulture));
            }

            if (endDate - startDate > TripMaximumTimeLimit)
            {
                throw new DomainException(DomainErrorCodes.TooLongTrip, null,
                    "Trip cannot be longer than: {0} days.",
                    TripMaximumTimeLimit.Days);
            }

            if (endDate - startDate < TripMinimumTimeLimit)
            {
                throw new DomainException(DomainErrorCodes.TooShortTrip, null,
                    "Trip cannot be shorter than: {0} hours .",
                    TripMinimumTimeLimit.Hours);
            }

            if ((startDate - _dateTimeOffsetProvider.UtcNow).Ticks < 0)
            {
                throw new DomainException(DomainErrorCodes.TripStartDateIsInThePast, null,
                    "Trip start date cannot be in the past {0}.", startDate);
            }

            StartDateUTC = startDate;
            EndDateUTC = endDate;
        }
    }
}
using System;
using Travelo.Core.Domain;

namespace Travelo.UnitTests.Helpers
{
    public static class DomainTestsHelper
    {
        public static Trip ValidTrip
            => ValidTripWithIdAndName(Guid.NewGuid(), "awesome");

        public static Trip ValidTripWithIdAndName(Guid id, string name)
        {
            var now = DateTimeOffset.Now;
            return new Trip(id, name, "Destination", now, now + TimeSpan.FromDays(7),
                new MockedDateOffsetProvider(now.ToUniversalTime()));
        }

        public static Customer ValidCustomer =>
            new Customer(Guid.NewGuid(), "Adam", "Adamski");

        public static Customer ValidCustomerWithId(Guid id) =>
            new Customer(id, "Adam", "Adamski");

        public static CustomerTrip ValidCustomerTrip => new CustomerTrip(Guid.NewGuid(), ValidCustomer, ValidTrip);
    }
}
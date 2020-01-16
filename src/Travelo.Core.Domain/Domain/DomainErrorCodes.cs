namespace Travelo.Core.Domain
{
    public enum DomainErrorCodes
    {
        StartDateAfterEndDate,
        TripStartDateIsInThePast,
        TooLongTrip,
        TooShortTrip,
        CustomerAlreadyAssignedToThisTrip,
        OverlappingTripDates,
        ArgumentNullOrEmpty,
        CustomerAlreadyExists,
        CustomerDoesNotExist,
        TripDoesNotExists,
        TripAlreadyExists,
        InvalidParameter,
        AddingCustomerToCancelledTrip,
        TripAlreadyStarted
    }
}
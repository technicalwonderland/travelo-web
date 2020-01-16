using System;
using System.Collections.Generic;

namespace Travelo.Core.Domain.DTO
{
    public class TripDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset StartDate { get;  set; }
        public DateTimeOffset EndDate { get; set; }
        public TripStatus TripStatus { get; set; }
        public IEnumerable<CustomerDTO> Customers { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
    }
}
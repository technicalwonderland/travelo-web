using System;
using Travelo.Core.Domain.Providers;

namespace Travelo.UnitTests.Helpers
{
    public class MockedDateOffsetProvider : IDateTimeOffsetProvider
    {
        public DateTimeOffset UtcNow { get; private set; }
        public DateTimeOffset Now { get; private set; }

        public MockedDateOffsetProvider(DateTimeOffset dateTimeOffset)
        {
            UtcNow = dateTimeOffset.ToUniversalTime();
            Now = dateTimeOffset;
        }
    }
}
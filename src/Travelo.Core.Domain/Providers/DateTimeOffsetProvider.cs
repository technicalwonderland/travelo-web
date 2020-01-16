using System;

namespace Travelo.Core.Domain.Providers
{
    public class DateTimeOffsetProvider : IDateTimeOffsetProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
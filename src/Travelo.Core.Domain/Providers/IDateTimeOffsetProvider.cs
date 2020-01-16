using System;

namespace Travelo.Core.Domain.Providers
{
    public interface IDateTimeOffsetProvider
    {
        DateTimeOffset UtcNow { get; }
        DateTimeOffset Now { get; }
    }
}
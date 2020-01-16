using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Travelo.DataStore;

namespace Travelo.UnitTests.Repositories
{
    public class BaseTestSqlRepository
    {
        protected TraveloDataContext GetMockedTraveloDataContext()
        {
            DbContextOptions<TraveloDataContext> options;
            var builder = new DbContextOptionsBuilder<TraveloDataContext>();
            builder.UseInMemoryDatabase("TripDB");
            builder.EnableSensitiveDataLogging();
            options = builder.Options;
            var sqlSettingsMock = new Mock<IOptions<SqlSettings>>();
            sqlSettingsMock.Setup(x => x.Value)
                .Returns(new SqlSettings() {DefaultConnection = Guid.NewGuid().ToString()});
            var traveloDataContext = new TraveloDataContext(options, sqlSettingsMock.Object);
            traveloDataContext.Database.EnsureDeleted();
            traveloDataContext.Database.EnsureCreated();
            return traveloDataContext;
        }
    }
}
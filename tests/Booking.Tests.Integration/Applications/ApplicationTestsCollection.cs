using BookingApp.Utils;

namespace BookingApp.Applications;

[CollectionDefinition(CollectionConstants.ApplicationTests)]
public class ApplicationTestsCollection : ICollectionFixture<ApiFactory>, IAsyncLifetime
{
    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
}
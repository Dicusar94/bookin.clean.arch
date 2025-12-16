using BookingApp.Utils;

namespace BookingApp.Applications;

[CollectionDefinition(CollectionConstants.ApplicationTests)]
public class ApplicationTestsCollection : ICollectionFixture<ApiFactory> 
{
}
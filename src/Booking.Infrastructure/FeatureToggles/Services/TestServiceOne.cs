using Microsoft.FeatureManagement;

namespace Booking.Infrastructure.FeatureToggles.Services;

[VariantServiceAlias("TestOne")]
public class TestServiceOne : ITestService
{
    public void Print()
    {
        Console.WriteLine(nameof(TestServiceOne));
    }
}
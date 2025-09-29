using Microsoft.FeatureManagement;

namespace Booking.Infrastructure.FeatureToggles.Services;

[VariantServiceAlias("TestTwo")]
public class TestServiceTwo : ITestService
{
    public void Print()
    {
        Console.WriteLine(nameof(TestServiceTwo));
    }
}
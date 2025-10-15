using Microsoft.FeatureManagement;

namespace BookingApp.FeatureToggles.Services;

[VariantServiceAlias("TestOne")]
public class TestServiceOne : ITestService
{
    public void Print()
    {
        Console.WriteLine(nameof(TestServiceOne));
    }
}
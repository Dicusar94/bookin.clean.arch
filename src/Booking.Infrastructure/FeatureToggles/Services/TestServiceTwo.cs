using Microsoft.FeatureManagement;

namespace BookingApp.FeatureToggles.Services;

[VariantServiceAlias("TestTwo")]
public class TestServiceTwo : ITestService
{
    public void Print()
    {
        Console.WriteLine(nameof(TestServiceTwo));
    }
}
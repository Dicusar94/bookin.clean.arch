using Shouldly;

namespace tests.TestCalculator;

public class AddShould
{
    [Fact]
    public void Add_two_integers()
    {
        // Arrange
        var sut = new TestCalculator();
        
        // Act 
        var result = sut.Add(-2, 3);
        
        // Assert 
        result.ShouldBe(1);
    }
    
    
    [Fact]
    public void Subtract_tow_integers()
    {
        // Arrange
        var sut = new TestCalculator();
        
        // Act 
        var result = sut.Subtract(5, -2);
        
        // Assert 
        result.ShouldBe(7);
    }
    
    [Theory]
    [InlineData(-3, 2, -6)]
    [InlineData(-5, 0, 0)]
    public void Multiply_two_integers(int a, int b, int expected)
    {
        // Arrange
        var sut = new TestCalculator();
        
        // Act 
        var result = sut.Multiply(a, b);
        
        // Assert 
        result.ShouldBe(expected);
    }
    
    
    [Theory]
    [InlineData(-6, 2, -3)]
    public void Divide_two_integers(int a, int b, int expected)
    {
        // Arrange
        var sut = new TestCalculator();
        
        // Act 
        var result = sut.Divide(a, b);
        
        // Assert
        result.ShouldBe(expected);
    }
    
    
    [Theory]
    [InlineData(-6, 0)]
    public void Divide_two_integers_throws_exception_divide_by_zero(int a, int b)
    {
        // Arrange
        var sut = new TestCalculator();
        
        // Act 
        var result = () => sut.Divide(a, b);
        
        // Assert
        Should.Throw<DivideByZeroException>(() =>
        {
            result.Invoke();
        });
    }
}

namespace Functional.Toolkit.OptionType.Tests;

using System;
using FluentAssertions;

public class OptionTests
{
    [Fact]
    public void Should_Return_OptionNone_If_Value_Is_Null()
    {
        // Act
        var option1 = Option.From((OptionTestClass)null);
        var value1 = () => option1.Value;

        var option2 = Option.From((OptionTestStruct?)null);
        Func<OptionTestStruct?> value2 = () => option2.Value;

        // Assert
        option1.HasValue.Should().BeFalse();
        option1.Should().Be(Option.None<OptionTestClass>());
        value1.Should().Throw<InvalidOperationException>();

        option2.HasValue.Should().BeFalse();
        option2.Should().Be(Option.None<OptionTestStruct>());
        value2.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Should_Throw_If_Assigned_Value_Is_Null()
    {
        // Act
        Action act1 = () => Option.Some<OptionTestClass>(null);

        // Assert
        act1.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Should_Return_OptionSome_If_Value_Is_Not_Null()
    {
        // Arrange
        OptionTestClass obj1 = new OptionTestClass();
        OptionTestStruct obj2 = new OptionTestStruct();

        // Act
        var option1 = Option.From(obj1);
        var option2 = Option.From(obj2);

        // Assert
        option1.HasValue.Should().BeTrue();
        option1.Value.Should().Be(obj1);

        option2.HasValue.Should().BeTrue();
        option2.Value.Should().Be(obj2);
    }


    [Fact]
    public void Should_Return_BaseType_If_Value_Nullable_Struct()
    {
        // Arrange
        int? nullableInt2 = 10;

        // Act
        var option1 = Option.From((int?)null);
        var option2 = Option.From(nullableInt2);

        // Assert
        option1.HasValue.Should().BeFalse();
        option1.IsNone.Should().BeTrue();
        option1.GetType().GenericTypeArguments[0].Should().Be<int>();

        option2.HasValue.Should().BeTrue();
        option2.Value.Should().Be(nullableInt2.Value);
        option2.Value.GetType().Should().Be<int>();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    [InlineData(12)]
    [InlineData(123d)]
    [InlineData("abc")]
    public void ImplicitConversion_GivenPrimitiveType_ThenExpectOptionValue<T>(T scenario)
    {
        // Act
        Option<T> option = scenario;

        // Assert
        option.HasValue.Should().BeTrue();
        option.Value.Should().Be(scenario);
    }

    [Fact]
    public void ImplicitConversion_GivenNull_ThenExpectEmptyOption()
    {
        // Act
        Option<string> option = null;

        // Assert
        option.HasValue.Should().BeFalse();
    }

    [Fact]
    public void ToString_Should_Return_Correct_Result()
    {
        // Arrange
        OptionTestClass obj1 = new OptionTestClass();

        // Act
        var option1 = Option.From(obj1);
        var option2 = Option.From((OptionTestClass)null);


        // Assert
        option1.ToString().Should().Be("Some<OptionTestClass>(OptionTestClass)");
        option2.ToString().Should().Be("None<OptionTestClass>");
    }
    
    [Fact]
    public void Equal_ShouldReturnTrueWhenIsTheSameValue()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(1);
        
        // Assert
        option1.Equals(option2).Should().BeTrue();
    }
    
    [Fact]
    public void Equal_ShouldReturnFalseWhenIsTheDifferentValue()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(2);
        
        // Assert
        option1.Equals(option2).Should().BeFalse();
    }
    
    [Fact]
    public void Equal_ShouldReturnFalseWhenOneOfResultsDoesntHaveValue()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.None<int>();
        
        // Assert
        option1.Equals(option2).Should().BeFalse();
        option2.Equals(option1).Should().BeFalse();
    }
    
    [Fact]
    public void Equal_ShouldReturnTrueWhenIsNone()
    {
        // Act
        var option1 = Option.None<int>();
        var option2 = Option.None<int>();
        
        // Assert
        option1.Equals(option2).Should().BeTrue();
    }
    
    [Fact]
    public void Equal_ShouldReturnTrueWhenIsTheSameValueUsingEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(1);
        
        // Assert
        (option1==option2).Should().BeTrue();
    }
    
    [Fact]
    public void Equal_ShouldReturnTrueWhenIsDifferentValueUsingNotEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(2);
        
        // Assert
        (option1!=option2).Should().BeTrue();
    }
    
    [Fact]
    public void Equal_ShouldReturnFalseWhenIsNullUsingNotEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        
        // Assert
        option1.Equals(null).Should().BeFalse();
    }
    
    [Fact]
    public void Equal_ShouldReturnTrueWhenUsingObjectEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        
        // Assert
        option1.Equals((object)Option.From(1)).Should().BeTrue();
    }
    
    [Fact]
    public void Equal_ShouldReturnFalseWhenUsingObjectEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        
        // Assert
        // ReSharper disable once SuspiciousTypeConversion.Global
        option1.Equals((object)2).Should().BeFalse();
    }
    
    [Fact]
    public void Equal_ShouldReturnHasCode()
    {
        // Act
        var option1 = Option.From(1);
        
        // Assert
        option1.GetHashCode().Should().BeGreaterThan(0);
    }
}
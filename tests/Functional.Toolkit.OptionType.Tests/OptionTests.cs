namespace Functional.Toolkit.OptionType.Tests;

using System;

public class OptionTests
{
    [Fact]
    public void Should_Return_OptionNone_If_Value_Is_Null()
    {
        // Act
        var option1 = Option.From((OptionTestClass)null);
        var option2 = Option.From((OptionTestStruct?)null);

        // Assert
        Assert.False(option1.HasValue);
        Assert.Equal(Option.None<OptionTestClass>(), option1);
        Assert.Throws<InvalidOperationException>(() => option1.Value);

        Assert.False(option2.HasValue);
        Assert.Equal(Option.None<OptionTestStruct>(), option2);
        Assert.Throws<InvalidOperationException>(() => option2.Value);
    }

    [Fact]
    public void Should_Throw_If_Assigned_Value_Is_Null()
    {
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => Option.Some<OptionTestClass>(null));
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
        Assert.True(option1.HasValue);
        Assert.Equal(obj1, option1.Value);

        Assert.True(option2.HasValue);
        Assert.Equal(obj2, option2.Value);
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
        Assert.False(option1.HasValue);
        Assert.True(option1.IsNone);
        Assert.IsType<Option<int>>(option1);

        Assert.True(option2.HasValue);
        Assert.Equal(nullableInt2.Value, option2.Value);
        Assert.IsType<int>(option2.Value);
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
        Assert.True(option.HasValue);
        Assert.Equal(scenario, option.Value);
    }

    [Fact]
    public void ImplicitConversion_GivenNull_ThenExpectEmptyOption()
    {
        // Act
        Option<string> option = null;

        // Assert
        Assert.False(option.HasValue);
    }

    [Fact]
    public void ToString_Should_Return_Correct_Result()
    {
        // Arrange
        var obj1 = new OptionTestClass();

        // Act
        var option1 = Option.From(obj1);
        var option2 = Option.From((OptionTestClass)null);


        // Assert
        Assert.Equal("Some<OptionTestClass>(OptionTestClass)", option1.ToString());
        Assert.Equal("None<OptionTestClass>", option2.ToString());
    }

    [Fact]
    public void Equal_ShouldReturnTrueWhenIsTheSameValue()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(1);

        // Assert
        Assert.True(option1.Equals(option2));
    }

    [Fact]
    public void Equal_ShouldReturnFalseWhenIsTheDifferentValue()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(2);

        // Assert
        Assert.False(option1.Equals(option2));
    }

    [Fact]
    public void Equal_ShouldReturnFalseWhenOneOfResultsDoesntHaveValue()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.None<int>();

        // Assert
        Assert.False(option1.Equals(option2));
        Assert.False(option2.Equals(option1));
    }

    [Fact]
    public void Equal_ShouldReturnTrueWhenIsNone()
    {
        // Act
        var option1 = Option.None<int>();
        var option2 = Option.None<int>();

        // Assert
        Assert.True(option1.Equals(option2));
    }

    [Fact]
    public void Equal_ShouldReturnTrueWhenIsTheSameValueUsingEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(1);

        // Assert
        Assert.True(option1 == option2);
    }

    [Fact]
    public void Equal_ShouldReturnTrueWhenIsDifferentValueUsingNotEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(2);

        // Assert
        Assert.True(option1 != option2);
    }

    [Fact]
    public void Equal_ShouldReturnFalseWhenIsNullUsingNotEqualOperator()
    {
        // Act
        var option1 = Option.From(1);

        // Assert
        Assert.False(option1.Equals(null));
    }

    [Fact]
    public void Equal_ShouldReturnTrueWhenUsingObjectEqualOperator()
    {
        // Act
        var option1 = Option.From(1);

        // Assert
        Assert.True(option1.Equals((object)Option.From(1)));
    }

    [Fact]
    public void Equal_ShouldReturnFalseWhenUsingObjectEqualOperator()
    {
        // Act
        var option1 = Option.From(1);

        // Assert
        // ReSharper disable once SuspiciousTypeConversion.Global
        Assert.False(option1.Equals((object)2));
    }

    [Fact]
    public void Equal_ShouldReturnHasCode()
    {
        // Act
        var option1 = Option.From(1);

        // Assert
        Assert.True(option1.GetHashCode() > 0);
    }
}
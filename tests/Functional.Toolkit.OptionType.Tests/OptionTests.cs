namespace Functional.Toolkit.OptionType.Tests;

using System;
using FluentAssertions;
using NUnit.Framework;

public class OptionTests
{
    [Test]
    public void Should_Return_OptionNone_If_Value_Is_Null()
    {
        // Act
        var option1 = Option.From((ClassForTest)null);
        var value1 = () => option1.Value;

        var option2 = Option.From((StructForTest?)null);
        Func<StructForTest?> value2 = () => option2.Value;

        // Assert
        option1.HasValue.Should().BeFalse();
        option1.Should().Be(Option.None<ClassForTest>());
        value1.Should().Throw<InvalidOperationException>();

        option2.HasValue.Should().BeFalse();
        option2.Should().Be(Option.None<StructForTest>());
        value2.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Should_Throw_If_Assigned_Value_Is_Null()
    {
        // Act
        Action act1 = () => Option.Some<ClassForTest>(null);

        // Assert
        act1.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Should_Return_OptionSome_If_Value_Is_Not_Null()
    {
        // Arrange
        ClassForTest obj1 = new ClassForTest();
        StructForTest obj2 = new StructForTest();

        // Act
        var option1 = Option.From(obj1);
        var option2 = Option.From(obj2);

        // Assert
        option1.HasValue.Should().BeTrue();
        option1.Value.Should().Be(obj1);

        option2.HasValue.Should().BeTrue();
        option2.Value.Should().Be(obj2);
    }


    [Test]
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

    [TestCase(false)]
    [TestCase(true)]
    [TestCase(12)]
    [TestCase(123d)]
    [TestCase("abc")]
    public void ImplicitConversion_GivenPrimitiveType_ThenExpectOptionValue<T>(T scenario)
    {
        // Act
        Option<T> option = scenario;

        // Assert
        option.HasValue.Should().BeTrue();
        option.Value.Should().Be(scenario);
    }

    [Test]
    public void ImplicitConversion_GivenNull_ThenExpectEmptyOption()
    {
        // Act
        Option<string> option = null;

        // Assert
        option.HasValue.Should().BeFalse();
    }

    [Test]
    public void ToString_Should_Return_Correct_Result()
    {
        // Arrange
        ClassForTest obj1 = new ClassForTest();

        // Act
        var option1 = Option.From(obj1);
        var option2 = Option.From((ClassForTest)null);


        // Assert
        option1.ToString().Should().Be("Some<ClassForTest>(ClassForTest)");
        option2.ToString().Should().Be("None<ClassForTest>");
    }
    
    [Test]
    public void Equal_ShouldReturnTrueWhenIsTheSameValue()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(1);
        
        // Assert
        option1.Equals(option2).Should().BeTrue();
    }
    
    [Test]
    public void Equal_ShouldReturnTrueWhenIsNone()
    {
        // Act
        var option1 = Option.None<int>();
        var option2 = Option.None<int>();
        
        // Assert
        option1.Equals(option2).Should().BeTrue();
    }
    
    [Test]
    public void Equal_ShouldReturnTrueWhenIsTheSameValueUsingEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(1);
        
        // Assert
        (option1==option2).Should().BeTrue();
    }
    
    [Test]
    public void Equal_ShouldReturnTrueWhenIsDifferentValueUsingNotEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        var option2 = Option.From(2);
        
        // Assert
        (option1!=option2).Should().BeTrue();
    }
    
    [Test]
    public void Equal_ShouldReturnFalseWhenIsNullUsingNotEqualOperator()
    {
        // Act
        var option1 = Option.From(1);
        
        // Assert
        option1.Equals(null).Should().BeFalse();
    }
    
    [Test]
    public void Equal_ShouldReturnHasCode()
    {
        // Act
        var option1 = Option.From(1);
        
        // Assert
        option1.GetHashCode().Should().BeGreaterThan(0);
    }
}
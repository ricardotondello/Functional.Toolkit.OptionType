using FluentAssertions;

namespace Functional.Toolkit.OptionType.Tests;

public class OptionExtensionsTests
{
    [Test]
    public void Should_ReturnValue_WhenValueExists()
    {
        var option = Option.Some(new StructForTest());
        var result = option.ValueOrNull();
        result.HasValue.Should().BeTrue();
        result.Should().NotBeNull();
    }

    [Test]
    public void Should_ReturnNull_WhenValueDoesntExists()
    {
        var option = Option.None<StructForTest>();
        var result = option.ValueOrNull();
        result.HasValue.Should().BeFalse();
        result.Should().BeNull();
    }

    [Test]
    public void Should_ExecuteActionWhenSome_WhenHasValue()
    {
        var option = Option.Some(true);
        var placeHolder = false;
        void Act(bool value) => placeHolder = value;
        var result = option.WhenSome(Act);

        placeHolder.Should().BeTrue();
        result.HasValue.Should().BeTrue();
    }

    [Test]
    public void Should_NotExecuteActionWhenSome_WhenIsNone()
    {
        var option = Option.None<bool>();
        var placeHolder = false;
        void Act(bool value) => placeHolder = value;
        var result = option.WhenSome(Act);

        placeHolder.Should().BeFalse();
        result.HasValue.Should().BeFalse();
    }

    [Test]
    public void Should_NotExecuteActionWhenNone_WhenHasValue()
    {
        var option = Option.Some(true);
        var placeHolder = false;
        var act = new Action(() => placeHolder = true);
        var result = option.WhenNone(act);

        placeHolder.Should().BeFalse();
        result.HasValue.Should().BeTrue();
    }

    [Test]
    public void Should_ExecuteActionWhenNone_WhenIsNone()
    {
        var option = Option.None<bool>();
        var placeHolder = false;
        var act = new Action(() => placeHolder = true);
        var result = option.WhenNone(act);

        placeHolder.Should().BeTrue();
        result.HasValue.Should().BeFalse();
    }

    [Test]
    public async Task Should_ExecuteFuncAsyncWhenSome_WhenHasValue()
    {
        var option = Option.Some(true);
        Task Fn(bool value) => Task.FromResult(true);
        var result = await option.WhenSome(Fn);

        result.HasValue.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Test]
    public async Task Should_NotExecuteFuncAsyncWhenSome_WhenIsNone()
    {
        var option = Option.None<bool>();
        Task Fn(bool value) => Task.FromResult(true);
        var result = await option.WhenSome(Fn);

        result.HasValue.Should().BeFalse();
    }

    [Test]
    public async Task Should_NotExecuteFuncAsyncWhenNone_WhenHasValue()
    {
        var option = Option.Some(true);
        var placeHolder = false;

        Task Fn()
        {
            placeHolder = true;
            return Task.CompletedTask;
        }

        var result = await option.WhenNone(Fn);

        result.HasValue.Should().BeTrue();
        result.Value.Should().BeTrue();
        placeHolder.Should().BeFalse();
    }

    [Test]
    public async Task Should_ExecuteFuncAsyncWhenNone_WhenIsNone()
    {
        var option = Option.None<bool>();
        var placeHolder = false;

        Task Fn()
        {
            placeHolder = true;
            return Task.CompletedTask;
        }

        var result = await option.WhenNone(Fn);

        result.HasValue.Should().BeFalse();
        placeHolder.Should().BeTrue();
    }

    [Test]
    public void Should_ExecuteActionWhenAny_WhenHasValue()
    {
        var option = Option.Some(true);
        var placeHolder = false;
        void Act(Option<bool> value) => placeHolder = true;

        var result = option.WhenAny(Act);

        result.HasValue.Should().BeTrue();
        placeHolder.Should().BeTrue();
    }

    [Test]
    public void Should_ExecuteActionWhenAny_WhenIsNone()
    {
        var option = Option.None<bool>();
        var placeHolder = false;
        void Act(Option<bool> value) => placeHolder = true;

        var result = option.WhenAny(Act);

        result.HasValue.Should().BeFalse();
        placeHolder.Should().BeTrue();
    }

    [Test]
    public async Task Should_ExecuteActionWhenSomeAsync_WhenHasValue()
    {
        var option = Task.FromResult(Option.Some(true));
        var placeHolder = false;

        Task Fn(bool value)
        {
            placeHolder = value;
            return Task.CompletedTask;
        }

        var result = await option.WhenSomeAsync(Fn);

        placeHolder.Should().BeTrue();
        result.HasValue.Should().BeTrue();
    }

    [Test]
    public async Task Should_NotExecuteActionWhenSomeAsync_WhenIsNone()
    {
        var option = Task.FromResult(Option.None<bool>());
        var placeHolder = false;

        Task Fn(bool value)
        {
            placeHolder = value;
            return Task.CompletedTask;
        }

        var result = await option.WhenSomeAsync(Fn);

        placeHolder.Should().BeFalse();
        result.HasValue.Should().BeFalse();
    }

    [Test]
    public async Task Should_NotExecuteActionWhenNoneAsync_WhenHasValue()
    {
        var option = Task.FromResult(Option.Some(true));
        var placeHolder = false;

        Task Fn()
        {
            placeHolder = true;
            return Task.CompletedTask;
        }

        var result = await option.WhenNoneAsync(Fn);

        placeHolder.Should().BeFalse();
        result.HasValue.Should().BeTrue();
    }

    [Test]
    public async Task Should_ExecuteActionWhenNoneAsync_WhenIsNone()
    {
        var option = Task.FromResult(Option.None<bool>());
        var placeHolder = false;

        Task Fn()
        {
            placeHolder = true;
            return Task.CompletedTask;
        }

        var result = await option.WhenNoneAsync(Fn);

        placeHolder.Should().BeTrue();
        result.HasValue.Should().BeFalse();
    }

    [Test]
    public async Task Should_ExecuteActionWhenAnyAsync_WhenHasValue()
    {
        var option = Task.FromResult(Option.Some(true));
        var placeHolder = false;
        void Act(Option<bool> value) => placeHolder = true;

        var result = await option.WhenAnyAsync(Act);

        result.HasValue.Should().BeTrue();
        placeHolder.Should().BeTrue();
    }

    [Test]
    public async Task Should_ExecuteActionWhenAnyAsync_WhenIsNone()
    {
        var option = Task.FromResult(Option.None<bool>());
        var placeHolder = false;
        void Act(Option<bool> value) => placeHolder = true;

        var result = await option.WhenAnyAsync(Act);

        result.HasValue.Should().BeFalse();
        placeHolder.Should().BeTrue();
    }

    [Test]
    public void OnSome_Should_Call_Function_If_Option_Has_Value()
    {
        // Arrange
        var option = Option.Some("hello");
        Option<string> Fn(string s) => Option.Some(s.ToUpper());

        // Act
        var result = option.OnSome(Fn);

        // Assert
        result.Should().Be(Option.Some("HELLO"));
    }

    [Test]
    public void OnSome_Should_Return_Option_If_Option_Is_None()
    {
        // Arrange
        var option = Option.None<string>();
        Option<string> Fn(string s) => Option.Some(s.ToUpper());

        // Act
        var result = option.OnSome(Fn);

        // Assert
        result.Should().Be(Option.None<string>());
    }

    [Test]
    public void OnNone_Should_Call_Function_If_Option_Is_None()
    {
        // Arrange
        var option = Option.None<string>();
        Option<string> Fn() => Option.Some("hello");

        // Act
        var result = option.OnNone(Fn);

        // Assert
        result.Should().Be(Option.Some("hello"));
    }

    [Test]
    public void OnNone_Should_Return_Option_If_Option_Has_Value()
    {
        // Arrange
        var option = Option.Some("hello");
        Option<string> Fn() => Option.Some("world");

        // Act
        var result = option.OnNone(Fn);

        // Assert
        result.Should().Be(Option.Some("hello"));
    }

    [Test]
    public void OnAny_Should_Call_Function()
    {
        // Arrange
        var option = Option.Some("hello");
        Option<string> Fn() => Option.Some("world");

        // Act
        var result = option.OnAny(Fn);

        // Assert
        result.Should().Be(Option.Some("world"));
    }

    [Test]
    public void OnAny_Should_Return_Option()
    {
        // Arrange
        var option = Option.Some("hello");
        Option<string> Fn() => Option.None<string>();

        // Act
        var result = option.OnAny(Fn);

        // Assert
        result.Should().Be(Option.None<string>());
    }

    [Test]
    public async Task OnSome_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Option.Some(1);

        // Act
        var result = await option.OnSome(async x => await Task.FromResult(Option.Some(x + 1)));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(2));
    }

    [Test]
    public async Task OnSome_WhenOptionHasNoValue_ShouldReturnNone()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = await option.OnSome(async x => await Task.FromResult(Option.Some(x + 1)));

        // Assert
        result.Should().BeEquivalentTo(Option.None<int>());
    }

    [Test]
    public async Task OnNone_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = await option.OnNone(async () => await Task.FromResult(Option.Some(1)));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(1));
    }

    [Test]
    public async Task OnNone_WhenOptionHasValue_ShouldReturnOption()
    {
        // Arrange
        var option = Option.Some(1);

        // Act
        var result = await option.OnNone(async () => await Task.FromResult(Option.Some(2)));

        // Assert
        result.Should().BeEquivalentTo(option);
    }

    [Test]
    public void OnAny_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = option.OnAny(opt => opt.Map(x => x + 1));

        // Assert
        result.Should().BeEquivalentTo(Option.None<int>());
    }

    [Test]
    public void OnAny_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Option.Some(1);

        // Act
        var result = option.OnAny(opt => opt.Map(x => x + 1));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(2));
    }

    [Test]
    public async Task OnSomeAsync_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnSomeAsync(async opt => await Task.FromResult(opt + 1));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(2));
    }

    [Test]
    public async Task OnSomeAsync_WhenOptionHasNoValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnSomeAsync(async opt => await Task.FromResult(opt + 1));

        // Assert
        result.Should().BeEquivalentTo(Option.None<int>());
    }

    [Test]
    public async Task OnNoneAsync_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnNoneAsync(async () => await Task.FromResult(Option.Some(1)));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(1));
    }

    [Test]
    public async Task OnNoneAsync_WhenOptionHasValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnNoneAsync(async () => await Task.FromResult(Option.Some(2)));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(1));
    }

    [Test]
    public async Task OnAnyAsync_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnAnyAsync(opt => opt.Map(x => x + 1));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(2));
    }

    [Test]
    public async Task OnAnyAsync_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnAnyAsync(async () => await Task.FromResult(Option.Some(1)));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(1));
    }

    [Test]
    public async Task MapAsync_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.MapAsync(x => x + 1);

        // Assert
        result.Should().BeEquivalentTo(Option.Some(2));
    }

    [Test]
    public async Task MapAsync_WhenOptionHasNoValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.MapAsync(x => x + 1);

        // Assert
        result.Should().BeEquivalentTo(Option.None<int>());
    }

    [Test]
    public async Task OnSomeAsyncTask_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnSomeAsync(x => Option.Some(x + 1));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(2));
    }

    [Test]
    public async Task OnSomeAsyncTask_WhenOptionHasNoValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnSomeAsync(x => Option.Some(x + 1));

        // Assert
        result.Should().BeEquivalentTo(Option.None<int>());
    }

    [Test]
    public async Task OnNoneAsyncTask_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnNoneAsync(() => Option.Some(1));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(1));
    }

    [Test]
    public async Task OnNoneAsyncTask_WhenOptionHasValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnNoneAsync(() => Option.Some(2));

        // Assert
        result.Should().BeEquivalentTo(Option.Some(1));
    }

    [Test]
    public void ValueOrDefault_Returns_Value_When_HasValue_True()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.ValueOrDefault();

        // Assert
        result.Should().Be(42);
    }

    [Test]
    public void ValueOrDefault_Returns_Default_When_HasValue_False()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = option.ValueOrDefault();

        // Assert
        result.Should().Be(default(int));
    }

    [Test]
    public void ValueOr_Returns_Value_When_HasValue_True()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.ValueOr(0);

        // Assert
        result.Should().Be(42);
    }

    [Test]
    public void ValueOr_Returns_Fallback_When_HasValue_False()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = option.ValueOr(0);

        // Assert
        result.Should().Be(0);
    }
}
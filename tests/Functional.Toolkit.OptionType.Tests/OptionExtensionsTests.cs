namespace Functional.Toolkit.OptionType.Tests;

public class OptionExtensionsTests
{
    [Fact]
    public void Should_ReturnValue_WhenValueExists()
    {
        var option = Option.Some(new OptionTestStruct());
        var result = option.ValueOrNull();
        Assert.True(result.HasValue);
        Assert.NotNull(result);
    }

    [Fact]
    public void Should_ReturnNull_WhenValueDoesntExists()
    {
        var option = Option.None<OptionTestStruct>();
        var result = option.ValueOrNull();
        Assert.False(result.HasValue);
        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Null(result);
    }

    [Fact]
    public void Should_ExecuteActionWhenSome_WhenHasValue()
    {
        var option = Option.Some(true);
        var placeHolder = false;
        void Act(bool value) => placeHolder = value;
        var result = option.WhenSome(Act);

        Assert.True(placeHolder);
        Assert.True(result.HasValue);
    }

    [Fact]
    public void Should_NotExecuteActionWhenSome_WhenIsNone()
    {
        var option = Option.None<bool>();
        var placeHolder = false;
        void Act(bool value) => placeHolder = value;
        var result = option.WhenSome(Act);

        Assert.False(placeHolder);
        Assert.False(result.HasValue);
    }

    [Fact]
    public void Should_NotExecuteActionWhenNone_WhenHasValue()
    {
        var option = Option.Some(true);
        var placeHolder = false;
        var act = new Action(() => placeHolder = true);
        var result = option.WhenNone(act);

        Assert.False(placeHolder);
        Assert.True(result.HasValue);
    }

    [Fact]
    public void Should_ExecuteActionWhenNone_WhenIsNone()
    {
        var option = Option.None<bool>();
        var placeHolder = false;
        var act = new Action(() => placeHolder = true);
        var result = option.WhenNone(act);

        Assert.True(placeHolder);
        Assert.False(result.HasValue);
    }

    [Fact]
    public async Task Should_ExecuteFuncAsyncWhenSome_WhenHasValue()
    {
        var option = Option.Some(true);
        Task Fn(bool value) => Task.FromResult(true);
        var result = await option.WhenSome(Fn);

        Assert.True(result.HasValue);
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_NotExecuteFuncAsyncWhenSome_WhenIsNone()
    {
        var option = Option.None<bool>();
        Task Fn(bool value) => Task.FromResult(true);
        var result = await option.WhenSome(Fn);

        Assert.False(result.HasValue);
    }

    [Fact]
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

        Assert.True(result.HasValue);
        Assert.True(result.Value);
        Assert.False(placeHolder);
    }

    [Fact]
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

        Assert.False(result.HasValue);
        Assert.True(placeHolder);
    }

    [Fact]
    public void Should_ExecuteActionWhenAny_WhenHasValue()
    {
        var option = Option.Some(true);
        var placeHolder = false;
        void Act(Option<bool> value) => placeHolder = true;

        var result = option.WhenAny(Act);

        Assert.True(result.HasValue);
        Assert.True(placeHolder);
    }

    [Fact]
    public void Should_ExecuteActionWhenAny_WhenIsNone()
    {
        var option = Option.None<bool>();
        var placeHolder = false;
        void Act(Option<bool> value) => placeHolder = true;

        var result = option.WhenAny(Act);

        Assert.False(result.HasValue);
        Assert.True(placeHolder);
    }

    [Fact]
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

        Assert.True(placeHolder);
        Assert.True(result.HasValue);
    }

    [Fact]
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

        Assert.False(placeHolder);
        Assert.False(result.HasValue);
    }

    [Fact]
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

        Assert.False(placeHolder);
        Assert.True(result.HasValue);
    }

    [Fact]
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

        Assert.True(placeHolder);
        Assert.False(result.HasValue);
    }

    [Fact]
    public async Task Should_ExecuteActionWhenAnyAsync_WhenHasValue()
    {
        var option = Task.FromResult(Option.Some(true));
        var placeHolder = false;
        void Act(Option<bool> value) => placeHolder = true;

        var result = await option.WhenAnyAsync(Act);

        Assert.True(result.HasValue);
        Assert.True(placeHolder);
    }

    [Fact]
    public async Task Should_ExecuteActionWhenAnyAsync_WhenIsNone()
    {
        var option = Task.FromResult(Option.None<bool>());
        var placeHolder = false;
        void Act(Option<bool> value) => placeHolder = true;

        var result = await option.WhenAnyAsync(Act);

        Assert.False(result.HasValue);
        Assert.True(placeHolder);
    }

    [Fact]
    public void OnSome_Should_Call_Function_If_Option_Has_Value()
    {
        // Arrange
        var option = Option.Some("hello");
        Option<string> Fn(string s) => Option.Some(s.ToUpper());

        // Act
        var result = option.OnSome(Fn);

        // Assert
        Assert.Equal(Option.Some("HELLO"), result);
    }

    [Fact]
    public void OnSome_Should_Return_Option_If_Option_Is_None()
    {
        // Arrange
        var option = Option.None<string>();
        Option<string> Fn(string s) => Option.Some(s.ToUpper());

        // Act
        var result = option.OnSome(Fn);

        // Assert
        Assert.Equal(Option.None<string>(), result);
    }

    [Fact]
    public void OnNone_Should_Call_Function_If_Option_Is_None()
    {
        // Arrange
        var option = Option.None<string>();
        Option<string> Fn() => Option.Some("hello");

        // Act
        var result = option.OnNone(Fn);

        // Assert
        Assert.Equal(Option.Some("hello"), result);
    }

    [Fact]
    public void OnNone_Should_Return_Option_If_Option_Has_Value()
    {
        // Arrange
        var option = Option.Some("hello");
        Option<string> Fn() => Option.Some("world");

        // Act
        var result = option.OnNone(Fn);

        // Assert
        Assert.Equal(Option.Some("hello"), result);
    }

    [Fact]
    public void OnAny_Should_Call_Function()
    {
        // Arrange
        var option = Option.Some("hello");
        Option<string> Fn() => Option.Some("world");

        // Act
        var result = option.OnAny(Fn);

        // Assert
        Assert.Equal(Option.Some("world"), result);
    }

    [Fact]
    public void OnAny_Should_Return_Option()
    {
        // Arrange
        var option = Option.Some("hello");
        Option<string> Fn() => Option.None<string>();

        // Act
        var result = option.OnAny(Fn);

        // Assert
        Assert.Equal(Option.None<string>(), result);
    }

    [Fact]
    public async Task OnSome_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Option.Some(1);

        // Act
        var result = await option.OnSome(async x => await Task.FromResult(Option.Some(x + 1)));

        // Assert
        Assert.Equivalent(Option.Some(2), result);
    }

    [Fact]
    public async Task OnSome_WhenOptionHasNoValue_ShouldReturnNone()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = await option.OnSome(async x => await Task.FromResult(Option.Some(x + 1)));

        // Assert
        Assert.Equal(Option.None<int>(), result);
    }

    [Fact]
    public async Task OnNone_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = await option.OnNone(async () => await Task.FromResult(Option.Some(1)));

        // Assert
        Assert.Equivalent(Option.Some(1), result);
    }

    [Fact]
    public async Task OnNone_WhenOptionHasValue_ShouldReturnOption()
    {
        // Arrange
        var option = Option.Some(1);

        // Act
        var result = await option.OnNone(async () => await Task.FromResult(Option.Some(2)));

        // Assert
        Assert.Equivalent(option, result);
    }

    [Fact]
    public void OnAny_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = option.OnAny(opt => opt.Map(x => x + 1));

        // Assert
        Assert.Equal(Option.None<int>(), result);
    }

    [Fact]
    public void OnAny_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Option.Some(1);

        // Act
        var result = option.OnAny(opt => opt.Map(x => x + 1));

        // Assert
        Assert.Equivalent(Option.Some(2), result);
    }

    [Fact]
    public async Task OnSomeAsync_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnSomeAsync(async opt => await Task.FromResult(opt + 1));

        // Assert
        Assert.Equivalent(Option.Some(2), result);
    }

    [Fact]
    public async Task OnSomeAsync_WhenOptionHasNoValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnSomeAsync(async opt => await Task.FromResult(opt + 1));

        // Assert
        Assert.Equal(Option.None<int>(), result);
    }

    [Fact]
    public async Task OnNoneAsync_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnNoneAsync(async () => await Task.FromResult(Option.Some(1)));

        // Assert
        Assert.Equivalent(Option.Some(1), result);
    }

    [Fact]
    public async Task OnNoneAsync_WhenOptionHasValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnNoneAsync(async () => await Task.FromResult(Option.Some(2)));

        // Assert
        Assert.Equivalent(Option.Some(1), result);
    }

    [Fact]
    public async Task OnAnyAsync_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnAnyAsync(opt => opt.Map(x => x + 1));

        // Assert
        Assert.Equivalent(Option.Some(2), result);
    }

    [Fact]
    public async Task OnAnyAsync_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnAnyAsync(async () => await Task.FromResult(Option.Some(1)));

        // Assert
        Assert.Equivalent(Option.Some(1), result);
    }

    [Fact]
    public async Task MapAsync_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.MapAsync(x => x + 1);

        // Assert
        Assert.Equivalent(Option.Some(2), result);
    }

    [Fact]
    public async Task MapAsync_WhenOptionHasNoValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.MapAsync(x => x + 1);

        // Assert
        Assert.Equal(Option.None<int>(), result);
    }

    [Fact]
    public async Task OnSomeAsyncTask_WhenOptionHasValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnSomeAsync(x => Option.Some(x + 1));

        // Assert
        Assert.Equivalent(Option.Some(2), result);
    }

    [Fact]
    public async Task OnSomeAsyncTask_WhenOptionHasNoValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnSomeAsync(x => Option.Some(x + 1));

        // Assert
        Assert.Equal(Option.None<int>(), result);
    }

    [Fact]
    public async Task OnNoneAsyncTask_WhenOptionHasNoValue_ShouldReturnResultOfFunction()
    {
        // Arrange
        var option = Task.FromResult(Option.None<int>());

        // Act
        var result = await option.OnNoneAsync(() => Option.Some(1));

        // Assert
        Assert.Equivalent(Option.Some(1), result);
    }

    [Fact]
    public async Task OnNoneAsyncTask_WhenOptionHasValue_ShouldReturnOptionNone()
    {
        // Arrange
        var option = Task.FromResult(Option.Some(1));

        // Act
        var result = await option.OnNoneAsync(() => Option.Some(2));

        // Assert
        Assert.Equivalent(Option.Some(1), result);
    }

    [Fact]
    public void ValueOrDefault_Returns_Value_When_HasValue_True()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.ValueOrDefault();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void ValueOrDefault_Returns_Default_When_HasValue_False()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = option.ValueOrDefault();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ValueOr_Returns_Value_When_HasValue_True()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.ValueOr(0);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void ValueOr_Returns_Fallback_When_HasValue_False()
    {
        // Arrange
        var option = Option.None<int>();

        // Act
        var result = option.ValueOr(0);

        // Assert
        Assert.Equal(0, result);
    }
}
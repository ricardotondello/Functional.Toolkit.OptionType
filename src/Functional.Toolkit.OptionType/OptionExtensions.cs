namespace Functional.Toolkit.OptionType;

/// <summary>
/// Provides extension methods for working with the Option type.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Returns the value of the Option if it has a value (Some), or null if it is empty (None).
    /// </summary>
    /// <typeparam name="TX">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <returns>The value of the Option or null.</returns>
    public static TX? ValueOrNull<TX>(this Option<TX> option) where TX : struct =>
        option.HasValue ? option.Value : null;
    
    /// <summary>
    /// Returns the value of the Option if it has a value (Some), or the default value of the underlying type if it is empty (None).
    /// </summary>
    /// <typeparam name="TX">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <returns>The value of the Option or the default value.</returns>
    public static TX? ValueOrDefault<TX>(this Option<TX> option) where TX : struct =>
        option.HasValue ? option.Value : default;

    /// <summary>
    /// Returns the value of the Option if it has a value (Some), or a fallback value if it is empty (None).
    /// </summary>
    /// <typeparam name="TX">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="fallback">The fallback value.</param>
    /// <returns>The value of the Option or the fallback value.</returns>
    public static TX? ValueOr<TX>(this Option<TX> option, TX fallback) where TX : struct =>
        option.HasValue ? option.Value : fallback;
    
    /// <summary>
    /// Executes the specified action if the Option has a value (Some). Returns the original Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="action">The action to be executed.</param>
    /// <returns>The original Option.</returns>
    public static Option<T> WhenSome<T>(this Option<T> option, Action<T> action)
    {
        if (option.HasValue)
        {
            action(option.Value);
        }

        return option;
    }

    /// <summary>
    /// Executes the specified action if the Option is empty (None). Returns the original Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="action">The action to be executed.</param>
    /// <returns>The original Option.</returns>
    public static Option<T> WhenNone<T>(this Option<T> option, Action action)
    {
        if (option.IsNone)
        {
            action();
        }

        return option;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the Option has a value (Some). Returns the original Option wrapped in a task.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="action">The async action to be executed.</param>
    /// <returns>A task representing the original Option.</returns>
    public static async Task<Option<T>> WhenSome<T>(this Option<T> option, Func<T, Task> action)
    {
        if (option.HasValue)
        {
            await action(option.Value);
        }

        return option;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the Option is empty (None). Returns the original Option wrapped in a task.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="action">The async action to be executed.</param>
    /// <returns>A task representing the original Option.</returns>
    public static async Task<Option<T>> WhenNone<T>(this Option<T> option, Func<Task> action)
    {
        if (option.IsNone)
        {
            await action();
        }

        return option;
    }

    /// <summary>
    /// Executes the specified action regardless of whether the Option has a value or is empty. Returns the original Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="action">The action to be executed.</param>
    /// <returns>The original Option.</returns>
    public static Option<T> WhenAny<T>(this Option<T> option, Action<Option<T>> action)
    {
        action(option);

        return option;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the awaited Option has a value (Some). Returns the original Option wrapped in a task.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="action">The async action to be executed.</param>
    /// <returns>A task representing the original Option.</returns>
    public static async Task<Option<T>> WhenSomeAsync<T>(this Task<Option<T>> optionTask, Func<T, Task> action)
    {
        var option = await optionTask;
        if (option.HasValue)
        {
            await action(option.Value);
        }

        return option;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the awaited Option is empty (None). Returns the original Option wrapped in a task.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="action">The async action to be executed.</param>
    /// <returns>A task representing the original Option.</returns>
    public static async Task<Option<T>> WhenNoneAsync<T>(this Task<Option<T>> optionTask, Func<Task> action)
    {
        var option = await optionTask;
        if (option.IsNone)
        {
            await action();
        }

        return option;
    }

    /// <summary>
    /// Asynchronously executes the specified action regardless of whether the awaited Option has a value or is empty. Returns the original Option wrapped in a task.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="action">The action to be executed.</param>
    /// <returns>A task representing the original Option.</returns>
    public static async Task<Option<T>> WhenAnyAsync<T>(this Task<Option<T>> optionTask, Action<Option<T>> action)
    {
        var option = await optionTask;
        action(option);

        return option;
    }

    /// <summary>
    /// Applies the specified function to the value of the Option if it has a value (Some), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="fn">The function to be applied to the Option's value.</param>
    /// <returns>A new Option containing the result of the function.</returns>
    public static Option<T> OnSome<T>(this Option<T> option, Func<T, Option<T>> fn) =>
        option.HasValue ? fn(option.Value) : option;

    /// <summary>
    /// Applies the specified function to the Option if it is empty (None), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="fn">The function to be applied when the Option is empty.</param>
    /// <returns>A new Option resulting from the function.</returns>
    public static Option<T> OnNone<T>(this Option<T> option, Func<Option<T>> fn) => option.IsNone ? fn() : option;

    /// <summary>
    /// Applies the specified function to the Option regardless of whether it has a value or is empty, and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="fn">The function to be applied.</param>
    /// <returns>A new Option resulting from the function.</returns>
    public static Option<T> OnAny<T>(this Option<T> option, Func<Option<T>> fn) => fn();

    /// <summary>
    /// Applies the specified asynchronous function to the value of the Option if it has a value (Some), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="fn">The asynchronous function to be applied to the Option's value.</param>
    /// <returns>A task representing the new Option containing the result of the function.</returns>
    public static Task<Option<T>> OnSome<T>(this Option<T> option, Func<T, Task<Option<T>>> fn) =>
        option.HasValue ? fn(option.Value) : Task.FromResult(option);

    /// <summary>
    /// Applies the specified asynchronous function to the Option if it is empty (None), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="fn">The asynchronous function to be applied when the Option is empty.</param>
    /// <returns>A task representing the new Option resulting from the function.</returns>
    public static Task<Option<T>> OnNone<T>(this Option<T> option, Func<Task<Option<T>>> fn) =>
        option.IsNone ? fn() : Task.FromResult(option);

    /// <summary>
    /// Applies the specified function to the Option regardless of whether it has a value or is empty, and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="fn">The function to be applied.</param>
    /// <returns>A new Option resulting from the function.</returns>
    public static Option<T> OnAny<T>(this Option<T> option, Func<Option<T>, Option<T>> fn) => fn(option);

    /// <summary>
    /// Asynchronously applies the specified function to the value of the Option if it has a value (Some), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="fn">The function to be applied to the Option's value.</param>
    /// <returns>A task representing the new Option containing the result of the function.</returns>
    public static async Task<Option<T>> OnSomeAsync<T>(this Task<Option<T>> optionTask, Func<T, Option<T>> fn)
    {
        var option = await optionTask;
        return option.HasValue ? fn(option.Value) : option;
    }

    /// <summary>
    /// Asynchronously applies the specified function to the Option if it is empty (None), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="fn">The function to be applied when the Option is empty.</param>
    /// <returns>A task representing the new Option resulting from the function.</returns>
    public static async Task<Option<T>> OnNoneAsync<T>(this Task<Option<T>> optionTask, Func<Option<T>> fn)
    {
        var option = await optionTask;
        return option.IsNone ? fn() : option;
    }

    /// <summary>
    /// Asynchronously applies the specified function to the value of the Option if it has a value (Some), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="fn">The asynchronous function to be applied to the Option's value.</param>
    /// <returns>A task representing the new Option containing the result of the function.</returns>
    public static async Task<Option<T>> OnSomeAsync<T>(this Task<Option<T>> optionTask, Func<T, Task<Option<T>>> fn)
    {
        var option = await optionTask;
        return option.HasValue ? await fn(option.Value) : option;
    }

    /// <summary>
    /// Asynchronously applies the specified function to the Option if it is empty (None), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="fn">The asynchronous function to be applied when the Option is empty.</param>
    /// <returns>A task representing the new Option resulting from the function.</returns>
    public static async Task<Option<T>> OnNoneAsync<T>(this Task<Option<T>> optionTask, Func<Task<Option<T>>> fn)
    {
        var option = await optionTask;
        return option.IsNone ? await fn() : option;
    }

    /// <summary>
    /// Asynchronously applies the specified function to the Option regardless of whether it has a value or is empty, and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="fn">The asynchronous function to be applied.</param>
    /// <returns>A task representing the new Option resulting from the function.</returns>
    public static Task<Option<T>> OnAnyAsync<T>(this Task<Option<T>> optionTask, Func<Task<Option<T>>> fn) => fn();

    /// <summary>
    /// Asynchronously applies the specified function to the Option regardless of whether it has a value or is empty, and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="T">The generic type of the Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="fn">The function to be applied.</param>
    /// <returns>A task representing the new Option resulting from the function.</returns>
    public static async Task<Option<T>> OnAnyAsync<T>(this Task<Option<T>> optionTask, Func<Option<T>, Option<T>> fn)
    {
        var option = await optionTask;
        return fn(option);
    }

    /// <summary>
    /// Applies the specified function to the value of the Option if it has a value (Some), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="TX">The generic type of the original Option.</typeparam>
    /// <typeparam name="TY">The generic type of the resulting Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="fn">The function to be applied to the Option's value.</param>
    /// <returns>A new Option containing the result of the function.</returns>
    public static Option<TY> Map<TX, TY>(this Option<TX> option, Func<TX, TY> fn) =>
        option.HasValue ? Option.From(fn(option.Value)) : Option.None<TY>();

    /// <summary>
    /// Asynchronously applies the specified function to the value of the Option if it has a value (Some), and returns the result as a new Option.
    /// </summary>
    /// <typeparam name="TX">The generic type of the original Option.</typeparam>
    /// <typeparam name="TY">The generic type of the resulting Option.</typeparam>
    /// <param name="optionTask">A task representing the Option instance.</param>
    /// <param name="fn">The function to be applied to the Option's value.</param>
    /// <returns>A task representing the new Option containing the result of the function.</returns>
    public static async Task<Option<TY>> MapAsync<TX, TY>(this Task<Option<TX>> optionTask, Func<TX, TY> fn)
    {
        var option = await optionTask;
        return option.HasValue ? Option.From(fn(option.Value)) : Option.None<TY>();
    }
}
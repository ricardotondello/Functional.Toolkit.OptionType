namespace Functional.Toolkit.OptionType;

public static class OptionExtensions
{
    /// <summary>
    /// In the case of a value-type, this method returns the value on case of Some, and a null in case of None
    /// </summary>
    /// <typeparam name="TX">generic type</typeparam>
    /// <param name="option">option-type</param>
    /// <returns>value(:TX) or null</returns>
    public static TX? ValueOrNull<TX>(this Option<TX> option) where TX : struct =>
        option.HasValue ? option.Value : null;

    #region WhenXX

    /*
     * When.. semantics: perform an action according to the contents of the Option-type and return back the SAME option-type for chaining.
     * Can be used for 'pattern-matching'.
     */

    public static Option<T> WhenSome<T>(this Option<T> option, Action<T> action)
    {
        if (option.HasValue)
        {
            action(option.Value);
        }

        return option;
    }

    public static Option<T> WhenNone<T>(this Option<T> option, Action action)
    {
        if (option.IsNone)
        {
            action();
        }

        return option;
    }

    public static async Task<Option<T>> WhenSome<T>(this Option<T> option, Func<T, Task> action)
    {
        if (option.HasValue)
        {
            await action(option.Value);
        }

        return option;
    }

    public static async Task<Option<T>> WhenNone<T>(this Option<T> option, Func<Task> action)
    {
        if (option.IsNone)
        {
            await action();
        }

        return option;
    }

    public static Option<T> WhenAny<T>(this Option<T> option, Action<Option<T>> action)
    {
        action(option);

        return option;
    }

    #endregion

    #region WhenXXAsync

    /*
     * When.. semantics: perform an action according to the contents of the Option-type and return back the SAME option-type for chaining.
     * Can be used for 'pattern-matching'.
     */

    public static async Task<Option<T>> WhenSomeAsync<T>(this Task<Option<T>> optionTask, Func<T, Task> action)
    {
        var option = await optionTask;
        if (option.HasValue)
        {
            await action(option.Value);
        }

        return option;
    }

    public static async Task<Option<T>> WhenNoneAsync<T>(this Task<Option<T>> optionTask, Func<Task> action)
    {
        var option = await optionTask;
        if (option.IsNone)
        {
            await action();
        }

        return option;
    }

    public static async Task<Option<T>> WhenAnyAsync<T>(this Task<Option<T>> optionTask, Action<Option<T>> action)
    {
        var option = await optionTask;
        action(option);

        return option;
    }

    #endregion

    #region OnXX

    /*
     * On.. semantics: perform an action according to the contents of the Option-type and return back the RESULTING option-type for chaining.
     */

    public static Option<T> OnSome<T>(this Option<T> option, Func<T, Option<T>> fn) =>
        option.HasValue ? fn(option.Value) : option;

    public static Option<T> OnNone<T>(this Option<T> option, Func<Option<T>> fn) => option.IsNone ? fn() : option;

    public static Option<T> OnAny<T>(this Option<T> option, Func<Option<T>> fn) => fn();

    public static Task<Option<T>> OnSome<T>(this Option<T> option, Func<T, Task<Option<T>>> fn) =>
        option.HasValue ? fn(option.Value) : Task.FromResult(option);

    public static Task<Option<T>> OnNone<T>(this Option<T> option, Func<Task<Option<T>>> fn) =>
        option.IsNone ? fn() : Task.FromResult(option);

    public static Option<T> OnAny<T>(this Option<T> option, Func<Option<T>, Option<T>> fn) => fn(option);

    #endregion

    #region OnXXAsync

    /*
     * On.. semantics: perform an action according to the contents of the Option-type and return back the RESULTING option-type for chaining.
     */

    public static async Task<Option<T>> OnSomeAsync<T>(this Task<Option<T>> optionTask, Func<T, Option<T>> fn)
    {
        var option = await optionTask;
        return option.HasValue ? fn(option.Value) : option;
    }

    public static async Task<Option<T>> OnNoneAsync<T>(this Task<Option<T>> optionTask, Func<Option<T>> fn)
    {
        var option = await optionTask;
        return option.IsNone ? fn() : option;
    }

    public static async Task<Option<T>> OnSomeAsync<T>(this Task<Option<T>> optionTask, Func<T, Task<Option<T>>> fn)
    {
        var option = await optionTask;
        return option.HasValue ? await fn(option.Value) : option;
    }

    public static async Task<Option<T>> OnNoneAsync<T>(this Task<Option<T>> optionTask, Func<Task<Option<T>>> fn)
    {
        var option = await optionTask;
        return option.IsNone ? await fn() : option;
    }

    public static Task<Option<T>> OnAnyAsync<T>(this Task<Option<T>> optionTask, Func<Task<Option<T>>> fn) => fn();

    public static async Task<Option<T>> OnAnyAsync<T>(this Task<Option<T>> optionTask, Func<Option<T>, Option<T>> fn)
    {
        var option = await optionTask;
        return fn(option);
    }

    #endregion
    
    /// <summary>
    /// Map operation, applies the value from the option to a function (TX => TY) and returns the result as Option TY
    /// </summary>
    /// <typeparam name="TX">generic type</typeparam>
    /// <typeparam name="TY">generic type</typeparam>
    /// <param name="option">this parameter of Option type</param>
    /// <param name="fn">TX => TY</param>
    /// <returns>Option TY</returns>
    public static Option<TY> Map<TX, TY>(this Option<TX> option, Func<TX, TY> fn)
        => option.HasValue ? Option.From(fn(option.Value)) : Option.None<TY>();

    /// <summary>
    /// Map operation, applies the value from the async option to a function (TX => TY) and returns the result as Option TY
    /// </summary>
    /// <typeparam name="TX">generic type</typeparam>
    /// <typeparam name="TY">generic type</typeparam>
    /// <param name="optionTask">this parameter of Option type</param>
    /// <param name="fn">TX => TY</param>
    /// <returns>Option TY</returns>
    public static async Task<Option<TY>> MapAsync<TX, TY>(this Task<Option<TX>> optionTask, Func<TX, TY> fn)
    {
        var option = await optionTask;
        return option.HasValue ? Option.From(fn(option.Value)) : Option.None<TY>();
    }
}
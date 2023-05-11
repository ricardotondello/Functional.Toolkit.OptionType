namespace Functional.Toolkit.OptionType;

public static class Option
{
    /// <summary>
    /// Creates an option instance checking the value entry
    /// </summary>
    /// <param name="valueOrNull"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Some or None depending the entry value</returns>
    public static Option<T> From<T>(T valueOrNull) => valueOrNull == null ? None<T>() : Some(valueOrNull);

    /// <summary>
    /// Creates an option instance checking the value entry
    /// </summary>
    /// <param name="valueOrNull"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Some or None depending the nullable entry value</returns>
    public static Option<T> From<T>(T? valueOrNull) where T : struct =>
        valueOrNull == null ? None<T>() : Some(valueOrNull.Value);

    /// <summary>
    /// Creates an option instance holding a value
    /// </summary>
    public static Option<T> Some<T>(T instance) => Option<T>.Some(instance);

    /// <summary>
    /// Creates an option instance holding no value
    /// </summary>
    public static Option<T> None<T>() => Option<T>.None();
}
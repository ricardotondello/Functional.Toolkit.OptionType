namespace Functional.Toolkit.OptionType;

public readonly struct Option<T> : IEquatable<Option<T>>
{
    private static readonly Option<T> Default = new(default, false);

    private readonly T _value;

    public T Value
    {
        get
        {
            if (!HasValue)
            {
                throw new InvalidOperationException();
            }

            return _value;
        }
    }

    public bool HasValue { get; }

    public bool IsNone => !HasValue;

    internal static Option<T> Some(T value) => new(value, true);

    internal static Option<T> None() => Default;

    public static implicit operator Option<T>(T value) => Option.From(value);

    public bool Equals(Option<T> other) =>
        !HasValue && !other.HasValue || HasValue && other.HasValue && Value.Equals(other.Value);

    public static bool operator ==(Option<T> a, Option<T> b) => a.Equals(b);

    public static bool operator !=(Option<T> a, Option<T> b) => !(a == b);

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is Option<T> option && Equals(option);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (_value.GetHashCode() * 397) ^ HasValue.GetHashCode();
        }
    }

    public override string ToString() =>
        HasValue ? $"Some<{typeof(T).Name}>({Value.ToString()})" : $"None<{typeof(T).Name}>";

    private Option(T value, bool hasValue)
    {
        if (hasValue && value == null)
        {
            throw new ArgumentNullException(nameof(value), "Option.Some cannot be assigned a null value");
        }

        _value = value;
        HasValue = hasValue;
    }
}
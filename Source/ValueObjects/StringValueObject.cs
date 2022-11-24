using System;
using System.Diagnostics;

namespace Proxoft.Extensions.ValueObjects;

[DebuggerDisplay("{this.GetType().Name}:{_value}")]
public abstract class StringValueObject<T> : ValueObject<T>
    where T: StringValueObject<T>
{
    private readonly string _value;

    protected StringValueObject(
        string? value) : this(
            value,
            _ => { })
    {
    }

    protected StringValueObject(
        string? value,
        int maxLength) : this(
            value,
            (string? v) => GuardFunctions.ThrowIfLength(v, maxLength))
    {
    }

    protected StringValueObject(
        string? value,
        Action<string?>? guard = null,
        Func<string?, string>? nullValueConversion = null
    )
    {
        guard?.Invoke(value);
        _value = nullValueConversion?.Invoke(value) ?? GuardFunctions.NullToEmptyConversion(value);
    }

    public bool IsEmpty => _value == string.Empty;

    public bool IsNullOrWhitespace => string.IsNullOrWhiteSpace(_value);

    protected sealed override int GetHashCodeCore()
    {
        return _value?.GetHashCode() ?? 0;
    }

    protected sealed override bool EqualsCore(T other)
    {
        return _value == other._value;
    }

    public override string ToString()
    {
        return _value;
    }

    public static implicit operator string(StringValueObject<T> value)
    {
        return value._value;
    }
}

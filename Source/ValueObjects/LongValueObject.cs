﻿using System;
using System.Diagnostics;

namespace Proxoft.Extensions.ValueObjects;

[DebuggerDisplay("{this.GetType().Name}:{_value}")]
public abstract class LongValueObject<T> : ValueObject<T>
    where T : LongValueObject<T>
{
    private readonly long _value;

    protected LongValueObject(
        long value) : this(value, v => GuardFunctions.ThrowIfNotInRange(v))
    {
    }

    protected LongValueObject(
        long value,
        long minValue = long.MinValue,
        long maxValue = long.MaxValue
    ) : this(value, v => GuardFunctions.ThrowIfNotInRange(v, minValue, maxValue))
    {
    }

    protected LongValueObject(long value, Action<long>? guard = null)
    {
        guard?.Invoke(value);

        _value = value;
    }

    protected sealed override bool EqualsCore(T other)
    {
        return _value == other._value;
    }

    protected sealed override int GetHashCodeCore()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public static implicit operator long(LongValueObject<T> value) => value._value;
}
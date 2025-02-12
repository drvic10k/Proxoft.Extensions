﻿using System;
using System.Diagnostics;

namespace Proxoft.Extensions.ValueObjects;

[DebuggerDisplay("{this.GetType().Name}:{_value}")]
public abstract class DoubleValueObject<T> : ValueObject<T>, IComparable<T>
    where T : DoubleValueObject<T>
{
    private readonly double _value;
    private readonly double _equalityTolerance;

    protected DoubleValueObject(
        double value,
        double equalityTolerance = 0.001)
        : this(value, equalityTolerance, v => GuardFunctions.ThrowIfNotInRange(v))
    {
    }

    protected DoubleValueObject(
        double value,
        double equalityTolerance = 0.001,
        double min = double.MinValue,
        double max = double.MaxValue)
        : this(value, equalityTolerance, v => GuardFunctions.ThrowIfNotInRange(v, min, max))
    {
    }

    protected DoubleValueObject(double value, double equalityTolerance = 0.001, Action<double>? guard = null)
    {
        guard?.Invoke(value);

        _value = value;
        _equalityTolerance = equalityTolerance;
    }

    protected override bool EqualsCore(T other)
    {
        double diff = Math.Abs( _value - other._value );
        return diff <= _equalityTolerance ||
               diff <= Math.Max(Math.Abs(_value), Math.Abs(other._value)) * _equalityTolerance;
    }

    protected override int GetHashCodeCore()
    {
        return _value.GetHashCode();
    }

    public int CompareTo(T? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        if (Math.Abs(this - other) < _equalityTolerance)
        {
            return 0;
        }

        return this < other
            ? -1
            : 1;
    }

    public static implicit operator double(DoubleValueObject<T> valueObject)
    {
        return valueObject._value;
    }
}

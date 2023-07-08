//#if UNITY_EDITOR

using System;

public class Limiter<TValue>
{
    private TValue _value;
    private readonly Func<TValue, bool> _predicative;

    public TValue Value
    {
        get => _value;
        set
        {
        	if(_predicative(value))
        		_value = value;
        }
    }

    public bool TrySetValue(TValue value)
    {
        var isValid = _predicative(value);

        if (isValid)
        {
            _value = value;
        }

        return isValid;
    }

    public bool IsValidValue(TValue value) => _predicative(value);

    public Limiter(Func<TValue, bool> predicative) : this(predicative, default(TValue))
    { }

    public Limiter(Func<TValue, bool> predicative, TValue defaultValue)
    {
        _predicative = predicative;
        _value = defaultValue;
    }

    public static implicit operator TValue(Limiter<TValue> limiter) => limiter._value;

}

//#endif
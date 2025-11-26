using System;

public class StatefulData<T> where T : IEquatable<T>
{
    public delegate void ValueChangedEventHandler(T previousValue, T newValue);
    public event ValueChangedEventHandler ValueChanged;

    private T _value;
    private T _previousValue;

    public StatefulData(T initialValue)
    {
        _value = initialValue;
        _previousValue = initialValue;
    }

    public T Value => _value;
    public T PreviousValue => _previousValue;

    /// <summary>
    /// Mutates this instance to the new value and triggers ValueChanged.
    /// </summary>
    public void SetValue(T newValue)
    {
        if (_value.Equals(newValue))
            return;

        _previousValue = _value;
        _value = newValue;

        ValueChanged?.Invoke(_previousValue, _value);
    }

    public override string ToString() => $"{_value}";

    public static implicit operator T(StatefulData<T> p) => p.Value;
}

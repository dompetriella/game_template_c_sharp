using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class StatefulData<T> where T : IEquatable<T>
{
	public delegate void ValueChangedEventHandler(T previousValue, T newValue);

	private readonly List<WeakReference<ValueChangedEventHandler>> _listeners = new();

	/// <summary>
	/// Public event that uses weak references internally, so dead listeners clean themselves up.
	/// </summary>
	public event ValueChangedEventHandler ValueChanged
	{
		add => AddListener(value);
		remove => RemoveListener(value);
	}

	private T _value;
	private T _previousValue;

	public StatefulData(T initialValue)
	{
		_value = initialValue;
		_previousValue = initialValue;
	}

	public T Value => _value;
	public T PreviousValue => _previousValue;

	private void AddListener(ValueChangedEventHandler handler)
	{
		if (handler == null)
			return;

		// Avoid duplicates
		if (_listeners.Any(wr => wr.TryGetTarget(out var target) && target == handler))
			return;

		_listeners.Add(new WeakReference<ValueChangedEventHandler>(handler));
	}

	private void RemoveListener(ValueChangedEventHandler handler)
	{
		_listeners.RemoveAll(wr => {
			return !wr.TryGetTarget(out var target) || target == handler;
		});
	}

	public void SetValue(T newValue)
	{
		if (_value.Equals(newValue))
			return;

		_previousValue = _value;
		_value = newValue;

		var deadRefs = new List<WeakReference<ValueChangedEventHandler>>();

		foreach (var weakRef in _listeners.ToList())
		{
			if (weakRef.TryGetTarget(out var handler))
			{
				try
				{
					handler.Invoke(_previousValue, _value);
				}
				catch (ObjectDisposedException)
				{
					deadRefs.Add(weakRef);
				}
				catch (NullReferenceException)
				{
					deadRefs.Add(weakRef);
				}
			}
			else
			{
				deadRefs.Add(weakRef);
			}
		}

		foreach (var dead in deadRefs)
			_listeners.Remove(dead);
	}

	public override string ToString() => $"{_value}";

	public static implicit operator T(StatefulData<T> p) => p.Value;
}

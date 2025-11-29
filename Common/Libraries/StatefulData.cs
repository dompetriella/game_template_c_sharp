using Godot;
using System;
using System.Collections.Generic;

public class StatefulData<T> where T : IEquatable<T>
{
	public delegate void ValueChangedHandler(T previousValue, T newValue);

	private readonly List<(WeakReference<Node> NodeRef, ValueChangedHandler Handler)> _listeners = new();

	private T _value;
	private T _previousValue;

	public StatefulData(T initialValue)
	{
		_value = initialValue;
		_previousValue = initialValue;
	}

	public T Value
	{
		get => _value;
		set => SetValue(value);
	}

	public T PreviousValue => _previousValue;

	/// <summary>
	/// Connects a callback to value changes. The callback automatically disconnects
	/// when the given Node leaves the scene tree.
	/// </summary>
	public void ValueChanged(Node node, ValueChangedHandler handler)
	{
		if (node == null || handler == null)
			return;

		var nodeRef = new WeakReference<Node>(node);
		_listeners.Add((nodeRef, handler));

		// Auto-cleanup when node exits the tree
		node.TreeExiting += () =>
		{
			_listeners.RemoveAll(pair =>
				!pair.NodeRef.TryGetTarget(out var n) || n == node
			);
		};
	}

	public void SetValue(T newValue)
	{
		if (_value.Equals(newValue))
			return;

		_previousValue = _value;
		_value = newValue;

		for (int i = _listeners.Count - 1; i >= 0; i--)
		{
			var (nodeRef, handler) = _listeners[i];
			if (!nodeRef.TryGetTarget(out var node) || !GodotObject.IsInstanceValid(node))
			{
				_listeners.RemoveAt(i);
				continue;
			}

			try
			{
				handler.Invoke(_previousValue, _value);
			}
			catch (Exception)
			{
				_listeners.RemoveAt(i);
			}
		}
	}

	public static implicit operator T(StatefulData<T> s) => s.Value;

	public override string ToString() => $"{_value}";
}

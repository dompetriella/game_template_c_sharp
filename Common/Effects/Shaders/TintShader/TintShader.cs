using Godot;

public class TintMaterial
{
	private readonly ShaderMaterial _material;

	private static readonly StringName TintColorParam = new("tint_color");
	private static readonly StringName TintStrengthParam = new("tint_strength");

	public TintMaterial(ShaderMaterial material)
	{
		_material = material;
	}

	public ShaderMaterial Material => _material;

	public Color TintColor
	{
		get => (Color)_material.GetShaderParameter(TintColorParam);
		set => _material.SetShaderParameter(TintColorParam, value);
	}

	public float TintStrength
	{
		get => (float)_material.GetShaderParameter(TintStrengthParam);
		set => _material.SetShaderParameter(TintStrengthParam, value);
	}
}

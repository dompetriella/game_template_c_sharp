using Godot;

public class BaseEffectsMaterial
{
    private readonly ShaderMaterial _material;

    public BaseEffectsMaterial(ShaderMaterial material)
    {
        _material = material;
    }

    public ShaderMaterial Material => _material;

    #region TintShader

    private static readonly StringName EnableTintParam = new("enable_tint");
    private static readonly StringName TintColorParam = new("tint_color");
    private static readonly StringName TintStrengthParam = new("tint_strength");

    public bool EnableTint
    {
        get => (bool)_material.GetShaderParameter(EnableTintParam);
        set => _material.SetShaderParameter(EnableTintParam, value);
    }

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
    #endregion

    #region OutlineShader

    private static readonly StringName EnableOutlineParam = new("enable_outline");
    private static readonly StringName OutlineColorParam = new("outline_color");
    private static readonly StringName OutlineThicknessParam = new("outline_thickness");
    public bool EnableOutline
    {
        get => (bool)_material.GetShaderParameter(EnableOutlineParam);
        set => _material.SetShaderParameter(EnableOutlineParam, value);
    }

    public Color OutlineColor
    {
        get => (Color)_material.GetShaderParameter(OutlineColorParam);
        set => _material.SetShaderParameter(OutlineColorParam, value);
    }

    public float OutlineThickness
    {
        get => (float)_material.GetShaderParameter(OutlineThicknessParam);
        set => _material.SetShaderParameter(OutlineThicknessParam, value);
    }
    #endregion
}

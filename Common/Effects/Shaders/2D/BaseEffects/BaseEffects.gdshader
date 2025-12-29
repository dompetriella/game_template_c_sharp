shader_type canvas_item;

// ======== TINT ======== //
uniform bool enable_tint = false;
uniform vec4 tint_color : source_color = vec4(1.0,1.0,1.0,1.0);
uniform float tint_strength : hint_range(0.0,1.0) = 0.0;

// ======== OUTLINE ======== //
uniform bool enable_outline = false;
uniform vec4 outline_color : source_color = vec4(0.0,0.0,0.0,1.0);
uniform float outline_thickness : hint_range(1.0,16.0) = 4.0;

void fragment() {
	vec4 base = texture(TEXTURE, UV);
	vec4 out_col = base;

	if (enable_outline && base.a <= 0.0) {
		bool neighbor = false;

		// Loop over screen-space offsets
		for (int x = -int(outline_thickness); x <= int(outline_thickness); x++) {
			for (int y = -int(outline_thickness); y <= int(outline_thickness); y++) {
				if (x == 0 && y == 0) continue;
				vec2 offset = vec2(float(x), float(y)) * SCREEN_PIXEL_SIZE;
				if (texture(TEXTURE, UV + offset).a > 0.0) {
					neighbor = true;
				}
			}
		}

		if (neighbor) {
			out_col = outline_color;
		}
	}

	// Apply tint
	if (enable_tint && out_col.a > 0.0) {
		out_col.rgb = mix(out_col.rgb, tint_color.rgb, tint_strength);
	}

	COLOR = out_col;
}

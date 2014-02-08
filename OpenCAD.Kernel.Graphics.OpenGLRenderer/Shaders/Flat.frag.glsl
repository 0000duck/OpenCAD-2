#version 400
uniform sampler2D texture;
in vec2 vertTexcoord;
void main() {

    vec4 diffuseColor = texture2D(texture, vertTexcoord);
    vec4 invertColor = 1.0 - diffuseColor;
    vec4 outColor = mix(diffuseColor, invertColor, 1);

    gl_FragColor = vec4(outColor.rgb, diffuseColor.a);
	//gl_FragColor = texture2D(texture, vertTexcoord);
}

#version 400
uniform sampler2D texture;
in vec2 vertTexcoord;
void main() {

    gl_FragColor = texture2D(texture, vertTexcoord);
}

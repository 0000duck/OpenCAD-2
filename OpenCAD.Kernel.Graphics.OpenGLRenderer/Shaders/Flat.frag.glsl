#version 400
uniform sampler2D texture;
in vec2 vertTexcoord;
layout( location = 0 ) out vec4 FragColor;
void main() {
    FragColor = texture2D(texture, vertTexcoord);
}

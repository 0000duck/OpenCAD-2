#version 400
precision highp float; 
layout (location = 0) in vec3 vert_position; 
layout (location = 1) in vec3 vert_normal; 
layout (location = 2) in vec4 vert_colour; 
layout (location = 3) in vec2 vert_texture;

out vec2 vertTexcoord;

void main(void) 
{ 
    vertTexcoord = vert_texture;
    gl_Position = vec4(vert_position, 1); 
}
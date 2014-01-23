#version 400
precision highp float; 

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

layout (location = 0) in vec3 vert; 
layout (location = 1) in vec4 colour; 
layout (location = 2) in float size; 

out Data
{
  vec4 color;
  float size;
} v;

void main()
{
    gl_Position = (Projection * View * Model) * vec4(vert, 1);
    v.size = size;
    v.color = colour;
}
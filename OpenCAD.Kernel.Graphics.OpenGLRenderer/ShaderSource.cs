using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer
{
    public static class ShaderSource
    {
        public static string Vertex
        {
            get
            {
                return @"#version 400
precision highp float; 

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

layout (location = 0) in vec3 vert; 
layout (location = 1) in vec4 colour; 
layout (location = 2) in float size; 

out VertexData
{
  vec4 color;
  float size;
} outData;

void main()
{
    gl_Position = (Projection * View * Model) * vec4(vert, 1);
    outData.size = size;
    outData.color = colour;
}
"; } }

        public static string Fragment
        {
            get
            {
                return @"#version 400
precision highp float; 

in FragmentData
{
    vec3 normal;
    vec4 color;
} frag;

void main()
{
    gl_FragColor = frag.color;
}
";
            }
        }

        public static string Geometry
        {
            get
            {
                return @"#version 330

layout (points) in;
layout (triangle_strip, max_vertices=24) out;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

uniform vec4 corners[8] =
{
    vec4( 0.5,  0.5,  0.5, 1.0), // front, top,    right
    vec4(-0.5,  0.5,  0.5, 1.0), // front, top,    left
    vec4( 0.5, -0.5,  0.5, 1.0), // front, bottom, right
    vec4(-0.5, -0.5,  0.5, 1.0), // front, bottom, left
    vec4( 0.5,  0.5, -0.5, 1.0), // back,  top,    right
    vec4(-0.5,  0.5, -0.5, 1.0), // back,  top,    left
    vec4( 0.5, -0.5, -0.5, 1.0), // back,  bottom, right
    vec4(-0.5, -0.5, -0.5, 1.0), // back,  bottom, left
};

uniform int faces[24] =
{
    0, 1, 2, 3, // front
    7, 6, 3, 2, // bottom
    7, 5, 6, 4, // back
    4, 0, 6, 2, // right
    1, 0, 5, 4, // top
    3, 1, 7, 5  // left
};

uniform vec3 normals[6] =
{
    vec3( 0.0,  0.0,  1.0),
    vec3( 0.0, -1.0,  0.0),
    vec3( 0.0,  0.0,  1.0),
    vec3( 1.0,  0.0,  0.0),
    vec3( 0.0,  1.0,  0.0),
    vec3(-1.0,  0.0,  0.0),
};

in VertexData
{
  vec4 color;
  float size;
} outData[];


out FragmentData 
{ 
    vec3 normal; 
    vec4 color; 
} frag; 

void main(void) 
{ 
    for (int f=0; f<6; f++) 
    { 
        for (int c=0; c<4; c++) 
        { 
            vec4 t = vec4(corners[faces[f * 4 + c]].xyz * (outData[0].size),1.0);
            gl_Position = gl_in[0].gl_Position + (Projection * View * Model * t); 
            frag.color  = outData[0].color; 
            frag.normal = normals[f]; 
            EmitVertex(); 
        } 
        EndPrimitive(); 
    } 
} ";
            }
        }


    }
}

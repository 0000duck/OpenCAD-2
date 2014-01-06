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

void main()
{
    gl_Position = (Projection * View * Model) * vec4(vert, 1);
}
"; } }

        public static string Fragment
        {
            get
            {
                return @"#version 400
precision highp float; 
void main()
{
    gl_FragColor = vec4(0.4,0.4,0.8,1.0);
}
";
            }
        }
    }
}

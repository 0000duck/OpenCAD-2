using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Modeling.FileFormats.StereoLithography
{
    public class STLFile:IFileFormat3D
    {
        public List<STLTriangle> Triangles { get; private set; }
        public STLFile(string filename)
        {
            Triangles = new List<STLTriangle>();
            using (var br = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                br.ReadBytes(80); //header
                var count = (int)br.ReadUInt32();
                for (var i = 0; i < count; i++)
                {
                    var normal = new Vect3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    var p1 = new Vect3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    var p2 = new Vect3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    var p3 = new Vect3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    br.ReadUInt16(); //attrib
                    Triangles.Add(new STLTriangle { P1 = p1, P2 = p2, P3 = p3, Normal = normal });
                }
            }
        }

        public void Save(string filename)
        {
            using (var bw = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate)))
            {
                bw.Write(Enumerable.Repeat(new byte(), 80).ToArray());
                bw.Write(Triangles.Count);
                foreach (var triangle in Triangles)
                {
                    bw.Write((float)triangle.Normal.X);
                    bw.Write((float)triangle.Normal.Y);
                    bw.Write((float)triangle.Normal.Z);

                    bw.Write((float)triangle.P1.X);
                    bw.Write((float)triangle.P1.Y);
                    bw.Write((float)triangle.P1.Z);

                    bw.Write((float)triangle.P2.X);
                    bw.Write((float)triangle.P2.Y);
                    bw.Write((float)triangle.P2.Z);

                    bw.Write((float)triangle.P3.X);
                    bw.Write((float)triangle.P3.Y);
                    bw.Write((float)triangle.P3.Z);
                    bw.Write((Int16)0);
                }
            }
        }
    }
}

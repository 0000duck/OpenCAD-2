using System;
using System.Collections.Generic;
using System.Linq;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling.Octree;
using OpenCAD.Kernel.Primatives;
using OpenCAD.Kernel.Scripting;

namespace OpenCAD.Desktop
{
    public class TestPart:IPartScript<OctreeModel>
    {
        public IEnumerable<IPartScriptOption> Options()
        {
            throw new NotImplementedException();
        }

        public OctreeModel Generate()
        {
            var s1 = new Sphere { Center = Vect3.Zero, Radius = 4 };
            var t = new OctreeNode(Vect3.Zero, 16, 5);
            var t1 = t.Intersect(node =>
            {
                if (node.AABB.Inside(s1)) return OctreeNode.NodeIntersectResult.Inside;
                if (s1.Intersects(node.AABB)) return OctreeNode.NodeIntersectResult.True;
                return OctreeNode.NodeIntersectResult.False;
            });
            return new OctreeModel(t1, "Test Octree");
        }
    }

    public static class IntersectionExtensions
    {
        public static bool Inside(this Vect3 v, Sphere s)
        {
            return (s.Center - v).LengthSquared < s.Radius * s.Radius;
        }
        public static bool Inside(this AABB b, Sphere s)
        {
            return b.Points.All(p => p.Inside(s));
        }
        public static bool Intersects(this Sphere s, AABB b)
        {
            return b.SqDistPointAABB(s.Center) <= Math.Pow(s.Radius, 2);
        }

        private static double SqDistPointAABB(this AABB b, Vect3 p)
        {
            var sqDist = 0.0;
            if (p.X < b.Min.X) sqDist += (b.Min.X - p.X) * (b.Min.X - p.X);
            if (p.X > b.Max.X) sqDist += (p.X - b.Max.X) * (p.X - b.Max.X);
            if (p.Y < b.Min.Y) sqDist += (b.Min.Y - p.Y) * (b.Min.Y - p.Y);
            if (p.Y > b.Max.Y) sqDist += (p.Y - b.Max.Y) * (p.Y - b.Max.Y);
            if (p.Z < b.Min.Z) sqDist += (b.Min.Z - p.Z) * (b.Min.Z - p.Z);
            if (p.Z > b.Max.Z) sqDist += (p.Z - b.Max.Z) * (p.Z - b.Max.Z);
            return sqDist;
        }
    }
}

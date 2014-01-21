using System;
using System.Linq;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Primatives;

namespace OpenCAD.Kernel.Intersection
{
    public static class IntersectionExtensions
    {
        public static bool Inside(this Vect3 v, Sphere s)
        {
            return (s.Center - v).LengthSquared < s.Radius * s.Radius;
        }
        public static bool Inside(this AABB b, Sphere s)
        {
            return b.Points.All(p => Inside((Vect3)p, s));
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
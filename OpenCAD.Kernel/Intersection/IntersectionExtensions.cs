using System;
using System.Linq;
using OpenCAD.Kernel.Geometry;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Primatives;

namespace OpenCAD.Kernel.Intersection
{
    public static class IntersectionExtensions
    {

        public static bool On(this Point p1, Point p2)
        {
            return p1.Position.X.NearlyEquals(p2.Position.X) && 
                   p1.Position.Y.NearlyEquals(p2.Position.Y) &&
                   p1.Position.Z.NearlyEquals(p2.Position.Z);
        }


        public static bool On(this Point p, Line l)
        {
            var a = l.Start.Position;
            var b = l.End.Position;
            var c = p.Position;

            var r = (b - a).CrossProduct(c - a).LengthSquared.NearlyEquals(0.0);


            return false;
        }





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
using System.Collections.Generic;
using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Primatives
{
    public class AABB
    {
        public Vect3 Min { get; private set; }
        public Vect3 Max { get; private set; }
        public Vect3 Center { get; private set; }
        public Vect3 Extent { get; private set; }

        public IEnumerable<Vect3> Points
        {
            get
            {
                yield return new Vect3(Min.X, Min.Y, Min.Z);
                yield return new Vect3(Min.X, Min.Y, Max.Z);
                yield return new Vect3(Min.X, Max.Y, Min.Z);
                yield return new Vect3(Min.X, Max.Y, Max.Z);
                yield return new Vect3(Max.X, Min.Y, Min.Z);
                yield return new Vect3(Max.X, Min.Y, Max.Z);
                yield return new Vect3(Max.X, Max.Y, Min.Z);
                yield return new Vect3(Max.X, Max.Y, Max.Z);
            }
        }


        public AABB(Vect3 min, Vect3 max)
        {
            Min = min;
            Max = max;
            Center = (Min + Max) * 0.5;
            Extent = Max - Min;
        }

        public AABB(Vect3 center, double size)
        {
            Center = center;
            Min = center - new Vect3(size / 2.0, size / 2.0, size / 2.0);
            Max = center + new Vect3(size / 2.0, size / 2.0, size / 2.0);
            Extent = new Vect3(size, size, size);
        }


    }
}
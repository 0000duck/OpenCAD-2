using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.Modeling.Octree;
using OpenCAD.Kernel.Primatives;

namespace OpenCAD.Kernel.Intersection
{
    public static class OctreeExtensions
    {
        public static OctreeNode Union(this OctreeNode a, OctreeNode b)
        {
            return new UnionOperation().Run(a, b);
        }

        public static OctreeNode Subtract(this OctreeNode a, OctreeNode b)
        {
            return new SubtractOperation().Run(a, b);
        }

        public static OctreeNode Intersect(this OctreeNode a, OctreeNode b)
        {
           return new IntersectOperation().Run(a, b);
        }


        public static OctreeNode Intersect(this OctreeNode a, Sphere s)
        {
            return a.Intersect(node =>
                {
                    if (node.AABB.Inside(s)) return OctreeNode.NodeIntersectResult.Inside;
                    if (s.Intersects(node.AABB)) return OctreeNode.NodeIntersectResult.True;
                    return OctreeNode.NodeIntersectResult.False;
                });
        }
    }
}

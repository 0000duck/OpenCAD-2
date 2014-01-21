using System;
using System.Collections.Generic;
using OpenCAD.Kernel;
using OpenCAD.Kernel.Intersection;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Modeling.Octree;
using OpenCAD.Kernel.Primatives;
using OpenCAD.Kernel.Scripting;

namespace OpenCAD.Desktop
{
    public class TestPart:IPartScript
    {
        public IEnumerable<IPartScriptOption> Options()
        {
            throw new NotImplementedException();
        }

        public IModel Generate()
        {
            var s1 = new Sphere { Center = Vect3.Zero, Radius = 4 };
            //var s1 = new Sphere { Center = Vect3.Zero, Radius = 4 };
            //var s2 = new Sphere { Center = new Vect3(3, 3, 3), Radius = 4 };

            //var b = new OctreeNode(Vect3.Zero, 16, 8);

            //var t1 = b.Intersect(node =>
            //{
            //    if (node.AABB.Inside(s1)) return OctreeNode.NodeIntersectResult.Inside;
            //    if (s1.Intersects(node.AABB)) return OctreeNode.NodeIntersectResult.True;
            //    return OctreeNode.NodeIntersectResult.False;
            //});
            //var t2 = b.Intersect(node =>
            //{
            //    if (node.AABB.Inside(s2)) return OctreeNode.NodeIntersectResult.Inside;
            //    if (s2.Intersects(node.AABB)) return OctreeNode.NodeIntersectResult.True;
            //    return OctreeNode.NodeIntersectResult.False;
            //});
            //return new OctreeModel(t1.Intersect(t2), "Test Octree");
            return new OctreeModel( new OctreeNode(Vect3.Zero, 16.0, 5).Intersect(s1), "Test Octree");
        }
    }
}

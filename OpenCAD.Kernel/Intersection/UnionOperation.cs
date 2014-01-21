using System.Linq;
using OpenCAD.Kernel.Modeling.Octree;

namespace OpenCAD.Kernel.Intersection
{
    public class UnionOperation : OctreeNodeOperation
    {
        /*
         *       b
         *    |E|F|P
         *   E|a|b|b
         * a F|a|a|a
         *   P|a|b|r
         */
        protected override OctreeNode EmptyEmpty(OctreeNode a, OctreeNode b)
        {
            return a;
        }

        protected override OctreeNode EmptyFilled(OctreeNode a, OctreeNode b)
        {
            return b;
        }

        protected override OctreeNode EmptyPartial(OctreeNode a, OctreeNode b)
        {
            return b;
        }

        protected override OctreeNode FilledEmpty(OctreeNode a, OctreeNode b)
        {
            return a;
        }

        protected override OctreeNode FilledFilled(OctreeNode a, OctreeNode b)
        {
            return a;
        }

        protected override OctreeNode FilledPartial(OctreeNode a, OctreeNode b)
        {
            return a;
        }

        protected override OctreeNode PartialEmpty(OctreeNode a, OctreeNode b)
        {
            return a;
        }

        protected override OctreeNode PartialFilled(OctreeNode a, OctreeNode b)
        {
            return b;
        }

        protected override OctreeNode PartialPartial(OctreeNode a, OctreeNode b)
        {
            return new OctreeNode(a.Center, a.Size, a.Children.Zip(b.Children, new UnionOperation().Run).ToArray(), a.Level, a.MaxLevel);
        }
    }
}
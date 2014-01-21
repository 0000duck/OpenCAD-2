using System.Linq;
using OpenCAD.Kernel.Modeling.Octree;

namespace OpenCAD.Kernel.Intersection
{
    public class SubtractOperation : OctreeNodeOperation
    {
        /*
         *       b
         *    |E|F|P
         *   E|E|E|E
         * a F|a|E|r
         *   P|a|E|r
         */
        protected override OctreeNode EmptyEmpty(OctreeNode a, OctreeNode b)
        {
            return new OctreeNode(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode EmptyFilled(OctreeNode a, OctreeNode b)
        {
            return new OctreeNode(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode EmptyPartial(OctreeNode a, OctreeNode b)
        {
            return new OctreeNode(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode FilledEmpty(OctreeNode a, OctreeNode b)
        {
            return a;
        }

        protected override OctreeNode FilledFilled(OctreeNode a, OctreeNode b)
        {
            return new OctreeNode(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode FilledPartial(OctreeNode a, OctreeNode b)
        {
            return new OctreeNode(a.Center, a.Size, new OctreeNode(a.Center, a.Size, a.CreateChildren(NodeState.Filled), a.Level, a.MaxLevel).Children.Zip(b.Children, new SubtractOperation().Run).ToArray(), a.Level, a.MaxLevel);
        }

        protected override OctreeNode PartialEmpty(OctreeNode a, OctreeNode b)
        {
            return a;
        }

        protected override OctreeNode PartialFilled(OctreeNode a, OctreeNode b)
        {
            return new OctreeNode(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode PartialPartial(OctreeNode a, OctreeNode b)
        {
            return new OctreeNode(a.Center, a.Size, a.Children.Zip(b.Children, new SubtractOperation().Run).ToArray(), a.Level, a.MaxLevel);
        }
    }
}
using System;
using OpenCAD.Kernel.Modeling.Octree;

namespace OpenCAD.Kernel.Intersection
{
    public abstract class OctreeNodeOperation
    {
        public OctreeNode Run(OctreeNode a, OctreeNode b)
        {
            if (a.State == NodeState.Empty && b.State == NodeState.Empty)
            {
                return EmptyEmpty(a, b);
            }
            if (a.State == NodeState.Empty && b.State == NodeState.Filled)
            {
                return EmptyFilled(a, b);
            }
            if (a.State == NodeState.Empty && b.State == NodeState.Partial)
            {
                return EmptyPartial(a, b);
            }

            if (a.State == NodeState.Filled && b.State == NodeState.Empty)
            {
                return FilledEmpty(a, b);
            }
            if (a.State == NodeState.Filled && b.State == NodeState.Filled)
            {
                return FilledFilled(a, b);
            }
            if (a.State == NodeState.Filled && b.State == NodeState.Partial)
            {
                return FilledPartial(a, b);
            }

            if (a.State == NodeState.Partial && b.State == NodeState.Empty)
            {
                return PartialEmpty(a, b);
            }
            if (a.State == NodeState.Partial && b.State == NodeState.Filled)
            {
                return PartialFilled(a, b);
            }
            if (a.State == NodeState.Partial && b.State == NodeState.Partial)
            {
                return PartialPartial(a, b);
            }
            throw new Exception();
        }

        protected abstract OctreeNode EmptyEmpty(OctreeNode a, OctreeNode b);
        protected abstract OctreeNode EmptyFilled(OctreeNode a, OctreeNode b);
        protected abstract OctreeNode EmptyPartial(OctreeNode a, OctreeNode b);

        protected abstract OctreeNode FilledEmpty(OctreeNode a, OctreeNode b);
        protected abstract OctreeNode FilledFilled(OctreeNode a, OctreeNode b);
        protected abstract OctreeNode FilledPartial(OctreeNode a, OctreeNode b);

        protected abstract OctreeNode PartialEmpty(OctreeNode a, OctreeNode b);
        protected abstract OctreeNode PartialFilled(OctreeNode a, OctreeNode b);
        protected abstract OctreeNode PartialPartial(OctreeNode a, OctreeNode b);
    }
}
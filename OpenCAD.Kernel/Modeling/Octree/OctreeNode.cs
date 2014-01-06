using System;
using System.Collections.Generic;
using System.Linq;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Primatives;

namespace OpenCAD.Kernel.Modeling.Octree
{
    public class OctreeNode
    {
        public int Level { get; private set; }
        public int MaxLevel { get; private set; }
        public Vect3 Center { get; private set; }
        public Double Size { get; private set; }
        public NodeState State { get; private set; }
        public AABB AABB { get { return new AABB(Center + new Vect3(-Size / 2.0, -Size / 2.0, -Size / 2.0), Center + new Vect3(Size / 2.0, Size / 2.0, Size / 2.0)); } }

        public IEnumerable<OctreeNode> Children { get; private set; }

        public OctreeNode(Vect3 center, Double size, NodeState state, int level, int maxLevel)
        {
            Center = center;
            Size = size;
            State = state;
            Level = level;
            MaxLevel = maxLevel;
            Children = Enumerable.Empty<OctreeNode>();
        }
        public OctreeNode(Vect3 center, Double size, int maxLevel)
            : this(center, size, NodeState.Empty, 0, maxLevel)
        {

        }
        public OctreeNode(Vect3 center, Double size, IEnumerable<OctreeNode> children, int level, int maxLevel)
            : this(center, size, NodeState.Partial, level, maxLevel)
        {
            Children = children;
        }

        public enum NodeIntersectResult : byte { False = 0, True = 1, Inside = 2 }

        public IEnumerable<OctreeNode> CreateChildren(NodeState state = NodeState.Empty)
        {
            var newSize = Size / 2.0;
            var half = Size / 4.0;
            return new[]
            {
                //top-front-right
                new OctreeNode(Center + new Vect3(+half, +half, +half),newSize, state, Level + 1, MaxLevel),
                //top-back-right
                new OctreeNode(Center + new Vect3(-half, +half, +half),newSize, state, Level + 1, MaxLevel),
                //top-back-left
                new OctreeNode(Center + new Vect3(-half, -half, +half),newSize, state, Level + 1, MaxLevel),
                //top-front-left
                new OctreeNode(Center + new Vect3(+half, -half, +half),newSize, state, Level + 1, MaxLevel),
                //bottom-front-right
                new OctreeNode(Center + new Vect3(+half, +half, -half),newSize, state, Level + 1, MaxLevel),
                //bottom-back-right
                new OctreeNode(Center + new Vect3(-half, +half, -half),newSize, state, Level + 1, MaxLevel),
                //bottom-back-left
                new OctreeNode(Center + new Vect3(-half, -half, -half),newSize, state, Level + 1, MaxLevel),
                //bottom-front-left
                new OctreeNode(Center + new Vect3(+half, -half, -half),newSize, state, Level + 1, MaxLevel)
            };
        }

        public OctreeNode Intersect(Func<OctreeNode, NodeIntersectResult> func)
        {
            if (Level > MaxLevel) return this;
            switch (func(this))
            {
                case NodeIntersectResult.False:
                    return this;
                case NodeIntersectResult.True:
                    if (Level == MaxLevel)
                    {
                        return new OctreeNode(Center, Size, NodeState.Filled, Level, MaxLevel);
                    }
                    return new OctreeNode(Center, Size, CreateChildren().Select(c => c.Intersect(func)), Level, MaxLevel);
                case NodeIntersectResult.Inside:
                    return new OctreeNode(Center, Size, NodeState.Filled, Level, MaxLevel);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IEnumerable<OctreeNode> Flatten()
        {
            yield return this;
            foreach (var child in Children)
            {
                foreach (var c in child.Flatten())
                {
                    yield return c;
                }
            }
        }


    }
}
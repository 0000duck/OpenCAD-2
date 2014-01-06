namespace OpenCAD.Kernel.Modeling.Octree
{
    public class OctreeModel:BaseModel
    {
        public OctreeNode Node { get; private set; }
        public OctreeModel(OctreeNode node, string name)
        {
            Node = node;
            Name = name;
        }
    }

    public enum NodeState : byte { Empty = 0, Filled = 1, Partial = 2 }
}

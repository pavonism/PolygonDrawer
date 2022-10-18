using Polygons.Shapes;

namespace Polygons.Constraints
{
    public abstract class EdgeRelation : IConstraint
    {
        protected readonly Edge brother;
        protected readonly Edge sister;

        private object id;
        public object Id => id;
        public virtual string Symbol => string.Empty;

        protected EdgeRelation(Edge brother, Edge sister, object id)
        {
            this.id = id;

            brother.AddConstraint(this);
            sister.AddConstraint(this);

            this.brother = brother;
            this.sister = sister;
        }

        public abstract void Check(Vertex movedVertex, Edge movedEdge);

        public void Delete(Edge request)
        {
            var toRemove = brother != request ? brother : sister;
            toRemove.RemoveConstraint(this);
        }
    }
}

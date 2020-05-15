namespace LPKO_2_2_Kocourkov
{
    public sealed class Edge
    {
        public Edge(Node node1, Node node2)
        {
            Node1 = node1;
            Node2 = node2;
        }

        public Node Node1 { get; }

        public Node Node2 { get; }

        public override bool Equals(object obj)
        {
            if (obj is Edge e)
            {
                return (e.Node1.Equals(Node1) && e.Node2.Equals(Node2)) || (e.Node2.Equals(Node1) && e.Node1.Equals(Node2));
            }
            return base.Equals(obj);
        }
    }
}

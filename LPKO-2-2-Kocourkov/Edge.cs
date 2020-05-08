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
    }
}

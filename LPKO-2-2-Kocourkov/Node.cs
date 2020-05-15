namespace LPKO_2_2_Kocourkov
{
    public sealed class Node
    {
        public Node(int number)
        {
            Number = number;
        }

        public int Number { get; }

        public override string ToString()
        {
            return Number.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Node n)
            {
                return Number == n.Number;
            }

            return base.Equals(obj);
        }
    }
}

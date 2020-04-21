using System;
using System.Collections.Generic;
using System.Text;

namespace SPD_Lab_FSP
{
    class Node
    {
        public int index;
        public List<Node> children = new List<Node>();

        public Node(int index, List<int> indicesToUse)
        {
            if (indicesToUse.Count == 1)
            {
                this.index = index;
            }
            else if (indicesToUse.Count > 1)
            {
                List<int> indicesToUseCopy = new List<int>(indicesToUse);
                this.index = index;
                if (!indicesToUseCopy.Remove(index) && index != -1) throw new Exception("to nie powinno sie stac!");
                foreach (int ix in indicesToUseCopy) children.Add(new Node(ix, indicesToUseCopy));
            }
        }

        public int CountEndNodes()
        {
            int count = 0;

            foreach (Node n in children) CountProcess(ref count);

            return count;
        }

        public void CountProcess(ref int count)
        {
            Console.WriteLine("countprocess");
            if (children.Count == 0) count++;
            else foreach (Node n in children) CountProcess(ref count);
        }
    }
}

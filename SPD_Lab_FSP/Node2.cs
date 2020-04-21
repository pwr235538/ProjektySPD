using System;
using System.Collections.Generic;
using System.Text;

namespace SPD_Lab_FSP
{
    class Node2
    {
        public int index;

        public Node2(ref Combinations combinations, int index, List<int> indicesToUse)
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
                foreach (int ix in indicesToUseCopy) new Node2(ref combinations, ix, indicesToUseCopy);
            }
        }
    }
}

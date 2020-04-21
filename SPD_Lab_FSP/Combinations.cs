using System;
using System.Collections.Generic;
using System.Text;

namespace SPD_Lab_FSP
{
    public class Combinations
    {
        int counter = 0;
        public int combLen = 1;
        List<int> currentCombination;
        List<List<int>> combinations = new List<List<int>>();

        public void Add(int combLen, int toAdd)
        {
            this.combLen = combLen;
            if(currentCombination == null || counter % combLen == 0)
            {
                currentCombination = new List<int>();
                combinations.Add(currentCombination);
            }

            currentCombination.Add(toAdd);
            counter++;
        }

        public void Add(int toAdd)
        {
            this.Add(this.combLen, toAdd);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using static SPD_Lab_WiTi.LabWiTi1;

namespace SPD_Lab_WiTi
{
    public static class PermutationGenerator
    {
        class formPermut
        {
            static int counter = 1;

            public void swapTwoNumber(ref int a, ref int b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            public void calcPermut(ref List<List<int>> combList, int[] list, int k, int m)
            {
                int i;
                if (k == m)
                {
                    //Console.Write(counter++ + ". ");
                    List<int> nextComb = new List<int>();
                    for (i = 0; i <= m; i++)
                    {
                        //Console.Write("{0}", list[i]);
                        nextComb.Add(list[i]);
                    }
                    combList.Add(nextComb);

                   // Console.Write("\n");
                }
                else
                    for (i = k; i <= m; i++)
                    {
                        swapTwoNumber(ref list[k], ref list[i]);
                        calcPermut(ref combList, list, k + 1, m);
                        swapTwoNumber(ref list[k], ref list[i]);
                    }
            }
        }

        public static List<List<int>> Generate(List<int> numbers)
        {
            formPermut test = new formPermut();
            int[] arr1 = numbers.ToArray();
            List<List<int>> combList = new List<List<int>>();
            test.calcPermut(ref combList, arr1, 0, numbers.Count - 1);
            return combList;
        }
    }

    public static class PermutationFinder
    {
        class formPermut
        {
            static int counter = 1;

            public void swapTwoNumber(ref int a, ref int b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            public void calcPermut(List<Task> tasks, ref List<int> bestCombOrder, ref double bestF, int[] list, int k, int m)
            {
                int i;
                if (k == m)
                {
                    List<int> nextComb = new List<int>();
                    for (i = 0; i <= m; i++)
                    {
                        nextComb.Add(list[i]);
                    }

                    double f = F(tasks, nextComb);
                    if (f < bestF)
                    {
                        bestCombOrder = nextComb;
                        bestF = f;
                    }
                }
                else
                    for (i = k; i <= m; i++)
                    {
                        swapTwoNumber(ref list[k], ref list[i]);
                        calcPermut(tasks, ref bestCombOrder, ref bestF, list, k + 1, m);
                        swapTwoNumber(ref list[k], ref list[i]);
                    }
            }
        }

        public static void GenerateAndFindBest(List<Task> tasks, ref List<int> bestCombOrder, ref double bestF, List<int> numbers)
        {
            formPermut fp = new formPermut();
            int[] arr1 = numbers.ToArray();
            List<List<int>> combList = new List<List<int>>();
            fp.calcPermut(tasks, ref bestCombOrder, ref bestF, arr1, 0, numbers.Count - 1);
        }
    }
}

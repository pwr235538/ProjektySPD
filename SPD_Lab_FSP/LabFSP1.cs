using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace SPD_Lab_FSP
{
    class LabFSP1
    {

        public class Operation
        {
            public int t;

            public Operation(int t)
            {
                this.t = t;
            }
        }

        public class Task
        {
            public int ID;
            public List<Operation> operations = new List<Operation>();

            public Task(int ID)
            {
                this.ID = ID;
            }

            public Task(int ID, List<int> operations)
            {
                this.ID = ID;
                this.operations = new List<Operation>();
                foreach (int opt in operations) this.operations.Add(new Operation(opt));
            }

            public override String ToString()
            {
                if (operations == null || operations.Count == 0) return "Task ID " + ID  + ": Empty task";
                String tr = "Task ID " + ID + ": ";
                foreach (Operation op in operations) tr += op.t + " ";

                return tr;
            }
        }

        static void Main(string[] args)
        {

            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0, p.IndexOf("SPD_Lab"));
            //Console.WriteLine("Starting ...");
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\fsp");
            //Console.WriteLine("Path: " + path);
            String[] files = { "data001.txt", "data002.txt", "data003.txt", "data004.txt" };//, "data005.txt", "data006.txt" };

            foreach (String filename in files)
            {
                int n = 0, m = 0; // liczba zadań, liczba maszyn


                //List<Machine> machines = new List<Machine>();
                List<Task> tasks = new List<Task>();

                //Console.WriteLine("\nBeginning file reading ...");
                String[] lines = File.ReadAllLines(Path.Combine(path, filename));
                Console.WriteLine("\n=======================================================================\nRead file " + filename + " completed.");

                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace("\t", " ");
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);

                    lines[i] = regex.Replace(lines[i], " ");
                    if (lines[i].StartsWith("")) lines[i] = lines[i].TrimStart(' ');

                    String[] tokens = lines[i].Split(' ');

                    if(i == 0)
                    {
                        n = Int32.Parse(tokens[0]);
                        m = Int32.Parse(tokens[1]);

                        for (int k = 1; k <= n; k++) tasks.Add(new Task(k));
                        //for (int k = 1; k <= m; k++) machines.Add(new Machine(k));
                        Console.WriteLine(filename + ": n=" + n + ", m=" + m);
                    }
                    else
                    {
                        if(tokens.Length > 2)
                            for (int k = 1; k < 2 * m; k += 2) tasks[i - 1].operations.Add(new Operation(Int32.Parse(tokens[k])));
                    }
                }

                //foreach (Task ttt in tasks) Console.WriteLine(ttt.ToString());
                //Console.WriteLine();

                int Cmax = GetCmax(m, n, tasks);
                Console.WriteLine("Cmax natural: " + Cmax);
                List<int> bestCombOrder = new List<int>();
                var stpw = new Stopwatch();
                stpw.Start();
                Cmax = GenerateAndFindBestCmax(m, n, tasks, ref bestCombOrder);
                stpw.Stop();
                Console.WriteLine("Cmax opt: " + Cmax);
                Console.Write("Opt order: "); foreach (int o in bestCombOrder) Console.Write(o + " "); Console.WriteLine();
                Console.WriteLine("Elapsed={0}", stpw.Elapsed);

            }
        }

        static int GetCmax(int m, int n, List<Task> tasks)
        {
            return GetCmax(m, n, tasks, GenerateIndecesList(n));
        }

        static int GetCmax(int m, int n, List<Task> tasks, List<int> order)
        {
            int[,] S = new int[m, n];
            int[,] C = new int[m, n];
            S[0, 0] = 0;
            for (int j = 1; j <= n; j++) // wyznaczenie czasow rozpoczecia i zakonczenia operacji na pierwszej maszynie
            {
                C[0, j - 1] = S[0, j - 1] + tasks[order[j - 1]].operations[0].t;
                if (j < n) S[0, j] = C[0, j - 1];
            }

            for (int i = 1; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j > 0) S[i, j] = Math.Max(S[i - 1, j], C[i, j - 1]);
                    else S[i, j] = C[i - 1, j];
                    C[i, j] = S[i, j] + tasks[order[j]].operations[i].t;
                }
            }

            //Console.WriteLine("S[{0},{1}] = {2}", m, n, S[m - 1, n - 1]);
            //Console.WriteLine("C[{0},{1}] = {2}\n", m, n, C[m - 1, n - 1]);

            return C[m - 1, n - 1];
        }



        static List<int> GenerateIndecesList(int x)
        {
            List<int> indeces = new List<int>();
            for (int i = 0; i < x; i++) indeces.Add(i);
            //Console.WriteLine("indices count: " + indeces.Count);
            return indeces;
        }


        static class FormPermut
        {
            static void SwapTwoNumber(ref int a, ref int b)
            {
                int temp = a;
                a = b;
                b = temp;
            }

            public static void CalcPermut(int m, int n, List<Task> tasks, ref List<int> bestCombOrder, ref int bestCmax, int[] list, int k)
            {
                int i;
                if (k == n - 1)
                {
                    List<int> nextComb = new List<int>();
                    for (i = 0; i <= n - 1; i++)
                    {
                        nextComb.Add(list[i]);
                    }

                    int cmax = GetCmax(m, n, tasks, nextComb);
                    if (cmax < bestCmax)
                    {
                        bestCombOrder = nextComb;
                        bestCmax = cmax;
                    }
                    //Console.WriteLine("better");
                }
                else
                    for (i = k; i <= n - 1; i++)
                    {
                        SwapTwoNumber(ref list[k], ref list[i]);
                        CalcPermut(m, n, tasks, ref bestCombOrder, ref bestCmax, list, k + 1);
                        SwapTwoNumber(ref list[k], ref list[i]);
                    }
            }
        }

        public static int GenerateAndFindBestCmax(int m, int n, List<Task> tasks, ref List<int> bestCombOrder)
        {
            int bestCmax = Int32.MaxValue;

            //ormPermut fp = new formPermut();
            int[] arr1 = GenerateIndecesList(n).ToArray();
            FormPermut.CalcPermut(m, n, tasks, ref bestCombOrder, ref bestCmax, arr1, 0);

            return bestCmax;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace SPD_Lab_WiTi
{
    class LabWiTi1
    {
        public class Task : IComparable<Task>
        {
            public int p, w, d;

            public Task(int p, int w, int d)
            {
                this.p = p;
                this.w = w;
                this.d = d;
            }

            public int CompareTo([AllowNull] Task other)
            {
                return this.d.CompareTo(other.d);

                if (this.d < other.d) return -1;
                else if (this.d == other.d) return 0;
                else return 1;
            }

            public override string ToString()
            {
                return "p=" + p + ", w=" + w + ", d=" + d;
            }

            public class MyComparer : IComparer<Task>
            {
                public int Compare([AllowNull] Task x, [AllowNull] Task y)
                {
                    double xdw = (double)x.d / (double)x.w;
                    double ydw = (double)y.d / (double)y.w;

                    return xdw.CompareTo(ydw);
                }
            }

            public class MyComparer2 : IComparer<Task>
            {
                public int Compare([AllowNull] Task x, [AllowNull] Task y)
                {
                    double xdwp = (double)x.d / (double)x.w * ((double)x.p);
                    double ydwp = (double)y.d / (double)y.w * ((double)y.p);

                    return xdwp.CompareTo(ydwp);
                }
            }
        }

        static void Main(string[] args)
        {
            //Stopwatch sw = new Stopwatch();

            //sw.Start();

            //List<List<int>> combList = PermutationGenerator.Generate(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11});
            ////foreach (List<int> comb in combList)
            ////{
            ////    foreach (int nb in comb) Console.Write(nb);
            ////    Console.WriteLine();
            ////}

            //sw.Stop();

            //Console.WriteLine("Elapsed={0}", sw.Elapsed);

            //Console.ReadLine();


            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0, p.IndexOf("SPD_Lab"));
            //Console.WriteLine("Starting ...");
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\witi");
            //Console.WriteLine("Path: " + path);
            String[] files = { "data11.txt" };//, "data11.txt", "data12.txt", "data13.txt", "data14.txt" };//, "data005.txt", "data006.txt" };

            foreach (String filename in files)
            {
                int n = 1, k;
                List<Task> tasks = new List<Task>();

                //Console.WriteLine("\nBeginning file reading ...");
                String[] lines = File.ReadAllLines(Path.Combine(path, filename));
                //Console.WriteLine("Read file " + filename + " completed.");

                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace("\t", " ");
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);

                    lines[i] = regex.Replace(lines[i], " ");
                    if (lines[i].StartsWith("")) lines[i] = lines[i].TrimStart(' ');

                    String[] tokens = lines[i].Split(' ');

                    if (i == 0)
                    {
                        n = Int32.Parse(tokens[0]);
                        k = Int32.Parse(tokens[1]);
                    }
                    else
                    {
                        if (tokens.Length > 1)
                            tasks.Add(new Task(Int32.Parse(tokens[0]), Int32.Parse(tokens[1]), Int32.Parse(tokens[2])));
                    }
                }

                foreach (Task ttt in tasks) Console.WriteLine(ttt.ToString());
                Console.WriteLine("1234, F = " + F(tasks));
                Console.WriteLine();

                //foreach (Task ttt in tasks) Console.WriteLine(ttt.ToString());
                Console.WriteLine("1234 indices, F = " + F(tasks, GenerateIndecesList(n)));
                Console.WriteLine();

                tasks.Sort();
                foreach (Task ttt in tasks) Console.WriteLine(ttt.ToString());
                Console.WriteLine("SortD, F = " + F(tasks));
                Console.WriteLine();

                tasks.Sort(new Task.MyComparer());
                foreach (Task ttt in tasks) Console.WriteLine(ttt.ToString());
                Console.WriteLine("My1, F = " + F(tasks));
                Console.WriteLine();

                tasks.Sort(new Task.MyComparer2());
                foreach (Task ttt in tasks) Console.WriteLine(ttt.ToString());
                Console.WriteLine("My2, F = " + F(tasks));
                Console.WriteLine();

                List<Task> bestOrderedTasks = FindOptimalThroughPZ(tasks);
                foreach (Task ttt in bestOrderedTasks) Console.WriteLine(ttt.ToString());
                Console.WriteLine("OptimalThroughPZ, F = " + F(bestOrderedTasks));
                Console.WriteLine();

            }
        }

        static List<int> GenerateIndecesList(int x)
        {
            List<int> indeces = new List<int>();
            for (int i = 0; i < x; i++) indeces.Add(i);
            Console.WriteLine("indices count: " + indeces.Count);
            return indeces;
        }
        
        public static double F(List<Task> tasks)
        {
            double lateSum = 0;
            int lastEnded = 0;
            foreach(Task t in tasks)
            {
                int endTime = lastEnded + t.p;
                lateSum += Math.Max(endTime - t.d, 0.0) * t.w;
                lastEnded = endTime;
            }

            return lateSum;
        }

        public static double F(List<Task> tasks, List<int> order)
        {
            double lateSum = 0;
            int lastEnded = 0;
            for(int i = 0; i < order.Count; i++)
            {
                int endTime = lastEnded + tasks[order[i]].p;
                lateSum += Math.Max(endTime - tasks[order[i]].d, 0.0) * tasks[order[i]].w;
                lastEnded = endTime;
            }

            return lateSum;
        }

        public static List<Task> FindOptimalThroughPZ(List<Task> tasks)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<List<int>> combinations = PermutationGenerator.Generate(GenerateIndecesList(tasks.Count));
            
            Console.WriteLine("Found combinations, Elapsed={0}", sw.Elapsed);

            List<int> bestCombOrder = null;
            double bestF = double.MaxValue;

            foreach (List<int> order in combinations)
            {
                double f = F(tasks, order);
                if (f < bestF)
                {
                    bestCombOrder = order;
                    bestF = f;
                }
            }

            sw.Stop();
            Console.WriteLine("Found best combination, Elapsed={0}", sw.Elapsed);

            List<Task> bestComb = new List<Task>();
            foreach (int o in bestCombOrder) bestComb.Add(tasks[o]);

            return bestComb;
        }
    }
}

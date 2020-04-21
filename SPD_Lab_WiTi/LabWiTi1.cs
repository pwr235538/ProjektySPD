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
                if (this.d < other.d) return -1;
                else if (this.d == other.d) return 0;
                else return 1;
            }

            public override string ToString()
            {
                return "p=" + p + ", w=" + w + ", d=" + d;
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
            String[] files = { "data10.txt" };//, "data11.txt", "data12.txt", "data13.txt", "data14.txt" };//, "data005.txt", "data006.txt" };

            foreach (String filename in files)
            {
                int n, k;
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
                Console.WriteLine();
                tasks.Sort();
                foreach (Task ttt in tasks) Console.WriteLine(ttt.ToString());
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
    }
}

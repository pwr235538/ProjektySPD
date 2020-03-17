using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;


namespace SPD_Lab_1
{
    public class Task : IComparable<Task>
    {
        public int r, p, q;

        public Task(int _r, int _p, int _q)
        {
            r = _r;
            p = _p;
            q = _q;
        }

        public int CompareTo(Task other)
        {
            if (this.r <= other.r) return -1;
            return 1;
        }
    }

    class Lab1
    {
        static void Main(string[] args)
        {
            String path = "C:\\Users\\WK\\Documents\\PWr\\SPD_Lab\\Pliki\\rpq";
            String[] files = { "data10.txt", "data20.txt", "data50.txt", "data100.txt", "data200.txt", "data500.txt", };

            foreach (String filename in files)
            {
                List<Task> tasks = new List<Task>();
                String[] lines = File.ReadAllLines(Path.Combine(path, filename));
                for (int i = 1; i < lines.Length; i++) 
                {
                    lines[i] = lines[i].Replace("\t", " ");
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);

                    lines[i] = regex.Replace(lines[i], " ");
                    if (lines[i].StartsWith("")) lines[i] = lines[i].TrimStart(' ');

                    String[] tokens = lines[i].Split(' '); ;
                    //Console.WriteLine(lines[i] + "from file: " + filename);
                    tasks.Add(new Task(Int32.Parse(tokens[0]), Int32.Parse(tokens[1]), Int32.Parse(tokens[2])));
                }

                /*
                if(filename == "data10.txt")
                {
                    foreach(Task t in tasks) Console.WriteLine("r: " + t.r + " p: " + t.p + " q: " + t.q);
                    tasks.Sort();
                    Console.WriteLine("soooooort");
                    foreach (Task t in tasks) Console.WriteLine("r: " + t.r + " p: " + t.p + " q: " + t.q);
                    Console.Read();
                } */

                List<int> s = new List<int>();
                List<int> c = new List<int>();
                List<int> cq = new List<int>();
                s.Add(tasks[0].r);
                c.Add(s[0] + tasks[0].p);
                cq.Add(c[0] + tasks[0].q);

                for (int i = 1; i < tasks.Count; i++)
                {
                    //Console.WriteLine("r: " + tasks[i].r + " p: " + tasks[i].p + " q: " + tasks[i].q);
                    s.Add(Math.Max(tasks[i].r, c[i - 1]));
                    c.Add(s[i] + tasks[i].p);
                    cq.Add(Math.Max(cq[i - 1], c[i] + tasks[i].q));
                }
                Console.WriteLine("Wyniki dla pliku: " + filename);
                Console.WriteLine("c ostatnie: " + cq[c.Count - 1]);


                tasks.Sort();
                s = new List<int>();
                c = new List<int>();
                cq = new List<int>();
                s.Add(tasks[0].r);
                c.Add(s[0] + tasks[0].p);
                cq.Add(c[0] + tasks[0].q);
                for (int i = 1; i < tasks.Count; i++)
                {
                    //Console.WriteLine("r: " + tasks[i].r + " p: " + tasks[i].p + " q: " + tasks[i].q);
                    s.Add(Math.Max(tasks[i].r, c[i - 1]));
                    c.Add(s[i] + tasks[i].p);
                    cq.Add(Math.Max(cq[i - 1], c[i] + tasks[i].q));
                }
                Console.WriteLine("Wyniki zadan posortowanych dla pliku: " + filename);
                Console.WriteLine("c ostatnie: " + cq[c.Count - 1] + "\n");
            }

            Console.Write("\nWpisz cokolwiek i wcisnij Enter aby zakonczyc: ");
            Console.Read();
        }
    }
}

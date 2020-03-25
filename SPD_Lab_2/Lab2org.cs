using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace SPD_Lab_2org
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

        public new string ToString()
        {
            return "Task - r: " + r + ", p: " + p + ", q: " + q;
        }

        public static void PrintTaskList(List<Task> l)
        {
            foreach (Task t in l) Console.WriteLine(t.ToString());
        }
    }

    class SPD_Lab_2
    {
        public static void MainOrg()
        {
            Stopwatch sw = new Stopwatch();


            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0,p.IndexOf("SPD_Lab"));
            //Console.WriteLine("Starting ...");
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\rpq");
            //Console.WriteLine("Path: " + path);
            String[] files = { "data10.txt", "data20.txt", "data50.txt", "data100.txt", "data200.txt", "data500.txt"};
            var wyniki = new List<int>();
            var wynikiCzas = new List<TimeSpan>();

            foreach (String filename in files)
            {
                //Console.WriteLine("\nBeginning file reading ...");
                String[] lines = File.ReadAllLines(Path.Combine(path, filename));
                //Console.WriteLine("Read file " + filename + " completed.");

                int k = 1;
                List<Task> tasksG = new List<Task>();
                List<Task> tasksN = new List<Task>();
                for (int i = 1; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace("\t", " ");
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);

                    lines[i] = regex.Replace(lines[i], " ");
                    if (lines[i].StartsWith("")) lines[i] = lines[i].TrimStart(' ');

                    String[] tokens = lines[i].Split(' '); ;
                    tasksN.Add(new Task(Int32.Parse(tokens[0]), Int32.Parse(tokens[1]), Int32.Parse(tokens[2])));
                }
                //Task.PrintTaskList(tasksN); Console.ReadLine();
                int t = GetMinR(tasksN).r;
                List<Task> tasksPi = new List<Task>();

                //int counter = 0;
                sw.Start();
                while (tasksN.Count > 0 || tasksG.Count > 0)
                {
                    Task minTask; 

                    while (tasksN.Count > 0 && (minTask = GetMinR(tasksN)).r <= t)
                    {
                        //if (counter++ == 0) Console.WriteLine("MinTask first: q " + minTask.q + " p " + minTask.p);
                        tasksG.Add(minTask);
                        tasksN.Remove(minTask);
                    }

                    if(tasksG.Count > 0)
                    {
                        Task maxTask = GetMaxQ(tasksG);
                        tasksG.Remove(maxTask);
                        tasksPi.Add(maxTask);
                        t = t + maxTask.p;
                        k++;
                    }
                    else
                    {
                        t = GetMinR(tasksN).r;
                    }
                }



                //Console.WriteLine("\nWyniki dla pliku: " + filename + " - wersja z znajdywaniem min i max");
                //Console.WriteLine("k: " + k + ", t: " + t + ", Pi count: " + tasksPi.Count);
                //Console.WriteLine("cq: " + Calculate(tasksPi));
                sw.Stop();
                wynikiCzas.Add(sw.Elapsed);
                sw.Reset();
                wyniki.Add(Calculate(tasksPi));
            }

            Console.WriteLine("Algorytm z podstawowy, wyniki + czasy: ");
            foreach (int w in wyniki) Console.Write(w + "  ");
            Console.WriteLine();
            foreach (TimeSpan t in wynikiCzas) Console.Write(t.TotalMilliseconds + "  ");
            Console.WriteLine();
            Console.WriteLine();
        }

        static Task GetMinR(List<Task> tasks)
        {
            if (tasks.Count == 0 || tasks == null) return null;
            else
            {
                Task min = tasks[0];
                foreach(Task t in tasks)
                {
                    if (t.r < min.r) min = t;
                }
                return min;
            }
        }

        static Task GetMaxQ(List<Task> tasks)
        {
            if (tasks.Count == 0 || tasks == null) return null;
            else
            {
                Task max = tasks[0];
                foreach (Task t in tasks)
                {
                    if (t.q > max.q) max = t;
                }
                return max;
            }
        }

        static int Calculate(List<Task> tasksPi)
        {
            if (tasksPi.Count == 0 || tasksPi == null) return -1;
            else
            {
                List<int> s = new List<int>();
                List<int> c = new List<int>();
                int cq = 0;
                s.Add(tasksPi[0].r);
                c.Add(s[0] + tasksPi[0].p);
                cq = c[0] + tasksPi[0].q;

                for (int i = 1; i < tasksPi.Count; i++)
                {
                    s.Add(Math.Max(tasksPi[i].r, c[i - 1]));
                    c.Add(s[i] + tasksPi[i].p);
                    cq = Math.Max(cq, c[i] + tasksPi[i].q);
                }

                return cq;
            }
        }
    }
}

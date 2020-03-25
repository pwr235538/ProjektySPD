using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace SPD_Lab_2org_podzial
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
        public static void MainOrg_podzial()
        {
            Stopwatch sw = new Stopwatch();

            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0,p.IndexOf("SPD_Lab"));
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\rpq");
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
                int t = 0;
                int l = 0;
                int Cmax = 0;
                var currentTask = new Task (-1, -1, Int32.MaxValue);

                sw.Start();
                while (tasksN.Count > 0 || tasksG.Count > 0)
                {
                    Task minTask = new Task(-1, -1, -1); //poniważ warunek w linii 95 wariuje

                    while (tasksN.Count > 0 && (minTask = GetMinR(tasksN)).r <= t)
                    {
                        tasksG.Add(minTask);
                        tasksN.Remove(minTask);

                        if(l != 0 && minTask.q > currentTask.q)
                            currentTask.p = t - minTask.r;

                        if (currentTask.p > 0 && tasksG.Contains(currentTask))
                            tasksG.Remove(currentTask);
                    }

                    if(tasksG.Count > 0)
                    {
                        Task maxTask = GetMaxQ(tasksG);
                        tasksG.Remove(maxTask);

                        if(minTask != null) currentTask = minTask;
                        t = t + minTask.p;
                        Cmax = Math.Max(Cmax, t + minTask.q);
                    }
                    else
                    {
                        t = GetMinR(tasksN).r;
                    }
                }

                //Console.WriteLine("\nWyniki dla pliku: " + filename + " - wersja z znajdywaniem min i max [ + PODZIAL]");
                //Console.WriteLine("cq: " + Cmax);
                sw.Stop();
                wynikiCzas.Add(sw.Elapsed);
                sw.Reset();
                wyniki.Add(Cmax);
            }

            Console.WriteLine("Algorytm podstawowy z podzialem zadan, wyniki + czasy: ");
            foreach (int w in wyniki) Console.Write(w + "  ");
            Console.WriteLine();
            foreach (TimeSpan t in wynikiCzas) Console.Write(t.TotalMilliseconds + "  ");
            Console.WriteLine();
            Console.WriteLine();
        }

        static Task GetMinR(List<Task> tasks)
        {
            if (tasks.Count == 0 || tasks == null) return new Task(0, 0, 0);
            else
            {
                Task min = tasks[0];
                foreach (Task t in tasks)
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

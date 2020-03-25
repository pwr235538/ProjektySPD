using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;


namespace SPD_Lab_2pq_podzial
{
    public class Task : IComparable<Task>
    {
        public int r, p, q;
        public char sortCriterium;

        public Task(int _r, int _p, int _q, char sc)
        {
            r = _r;
            p = _p;
            q = _q;
            sortCriterium = sc;
        }

        public int CompareTo(Task other)
        {
            if(sortCriterium == 'r')
            {
                if (this.r <= other.r) return -1;
                return 1;
            }
            else
            {
                if (this.q <= other.q) return -1;
                return 1;
            }
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

    public class TaskPQ : List<Task>
    {
        public new void Add(Task t)
        {
            if (this.Count == 0) base.Add(t);
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (t.CompareTo(this[i]) == -1)
                    {
                        this.Insert(i, t);
                        return;
                    }
                }
                base.Add(t);
            }
        }

        public void Add(Task t, char newSC)
        {
            t.sortCriterium = newSC;
            if (this.Count == 0) base.Add(t);
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (t.CompareTo(this[i]) == -1)
                    {
                        this.Insert(i, t);
                        return;
                    }
                }
                base.Add(t);
            }
        }
    }

    class SPD_Lab_2
    {
        public static void MainPQ_podzial()
        {
            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0,p.IndexOf("SPD_Lab"));
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\rpq");
            String[] files = { "data10.txt", "data20.txt", "data50.txt", "data100.txt", "data200.txt", "data500.txt" };
            var wyniki = new List<int>();

            foreach (String filename in files)
            {
                //Console.WriteLine("\nBeginning file reading ...");
                String[] lines = File.ReadAllLines(Path.Combine(path, filename));
                //Console.WriteLine("Read file " + filename + " completed.");

                int k = 1;
                var tasksG = new TaskPQ();
                var tasksN = new TaskPQ();
                for (int i = 1; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace("\t", " ");
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);

                    lines[i] = regex.Replace(lines[i], " ");
                    if (lines[i].StartsWith("")) lines[i] = lines[i].TrimStart(' ');

                    String[] tokens = lines[i].Split(' '); ;
                    tasksN.Add(new Task(Int32.Parse(tokens[0]), Int32.Parse(tokens[1]), Int32.Parse(tokens[2]), 'r'));
                }
                int t = 0;
                int l = 0;
                int Cmax = 0;
                var currentTask = new Task(-1, -1, Int32.MaxValue, 'r');

                while (tasksN.Count > 0 || tasksG.Count > 0)
                {
                    Task minTask = new Task(-1, -1, -1, 'r'); 

                    while (tasksN.Count > 0 && (minTask = tasksN[0]).r <= t)
                    {
                        tasksG.Add(minTask, 'q');
                        tasksN.Remove(minTask);

                        if (l != 0 && minTask.q > currentTask.q)
                            currentTask.p = t - minTask.r;

                        if (currentTask.p > 0 && tasksG.Contains(currentTask))
                            tasksG.Remove(currentTask);
                    }

                    if(tasksG.Count > 0)
                    {
                        Task maxTask = tasksG[tasksG.Count - 1];
                        tasksG.Remove(maxTask);

                        if (minTask != null) currentTask = minTask;
                        t = t + minTask.p;
                        Cmax = Math.Max(Cmax, t + minTask.q);
                    }
                    else
                    {
                        if (tasksN.Count > 0) t = tasksN[0].r;
                        else t = 0;
                    }
                }

                //Console.WriteLine("\nWyniki dla pliku: " + filename + " - wersja z kolejka priorytetowa [+ PODZIAL]");
                //Console.WriteLine("cq: " + Cmax);
                wyniki.Add(Cmax);
            }

            Console.WriteLine("Algorytm z kolejka priorytetowa i podzialem zadan, wyniki: ");
            foreach (int w in wyniki) Console.Write(w + "  ");
            Console.WriteLine();
            Console.WriteLine();
        }

        /* static int Calculate(List<Task> tasksPi)
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
        } */
    }
}

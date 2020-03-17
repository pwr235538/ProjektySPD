using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;


namespace Lab2ns
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
            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0,p.IndexOf("SPD_Lab"));
            Console.WriteLine("Starting ...");
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\rpq");
            Console.WriteLine("Path: " + path);
            String[] files = { "data10.txt", "data20.txt", "data50.txt", "data100.txt", "data200.txt", "data500.txt", };

            foreach (String filename in files)
            {
                Console.WriteLine("\nBeginning file reading ...");
                String[] lines = File.ReadAllLines(Path.Combine(path, filename));
                Console.WriteLine("Read file " + filename + " completed.");

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
                int t = GetMinR(tasksN).r;
                List<Task> tasksPi = new List<Task>();

                //Console.WriteLine("Beginning while loop ...");
                //int counter = 0;
                while (tasksN.Count > 0 || tasksG.Count > 0)
                {
                    //if (++counter % 10 == 0) Console.WriteLine("While iteration: " + counter);
                    Task minTask; 

                    while (tasksN.Count > 0 && (minTask = GetMinR(tasksN)).r <= t)
                    {
                        //Console.WriteLine("\nPRE - tasksN count: " + tasksN.Count + ", tasksG count: " + tasksG.Count);
                        tasksG.Add(minTask);
                        tasksN.Remove(minTask);
                        //Console.WriteLine("POST - tasksN count: " + tasksN.Count + ", tasksG count: " + tasksG.Count);
                        //Console.Write("\nWpisz cokolwiek i wcisnij Enter aby kontynuowac: ");
                        //Console.Read();
                    }

                    if(tasksG.Count > 0)
                    {
                        Task maxTask = GetMaxR(tasksG);
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


                
                Console.WriteLine("\nWyniki dla pliku: " + filename);
                Console.WriteLine("k: " + k + ", t: " + t + ", Pi count: " + tasksPi.Count);
                Console.WriteLine("cq: " + Calculate(tasksPi));
                //Console.Write("\nWpisz cokolwiek i wcisnij Enter aby kontynuowac: ");
                //Console.Read();

            }

            Console.Write("\nWpisz cokolwiek i wcisnij Enter aby zakonczyc: ");
            Console.Read();
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

        static Task GetMaxR(List<Task> tasks)
        {
            if (tasks.Count == 0 || tasks == null) return null;
            else
            {
                Task max = tasks[0];
                foreach (Task t in tasks)
                {
                    if (t.r > max.r) max = t;
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

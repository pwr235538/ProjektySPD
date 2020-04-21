using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SPD_Lab_FSP
{
    class LabFSP1
    {
        static int n, m; // liczba zadań, liczba maszyn

        public class Operation
        {
            public int t, ID, taskID;

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

        public class Machine
        {
            public int ID;
            public Operation currentOperation;

            public Machine(int iD)
            {
                ID = iD;
            }
        }

        static void Main(string[] args)
        {
            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0, p.IndexOf("SPD_Lab"));
            //Console.WriteLine("Starting ...");
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\fsp");
            //Console.WriteLine("Path: " + path);
            String[] files = { "data001.txt", "data002.txt", "data003.txt", "data003.txt", "data004.txt", "data005.txt", "data006.txt" };

            foreach (String filename in files)
            {
                List<Machine> machines = new List<Machine>();
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

                    if(i == 0)
                    {
                        n = Int32.Parse(tokens[0]);
                        m = Int32.Parse(tokens[1]);

                        for (int k = 1; k <= n; k++) tasks.Add(new Task(k));
                        for (int k = 1; k <= m; k++) machines.Add(new Machine(k));
                        Console.WriteLine(filename + ": n=" + n + ", m=" + m);
                    }
                    else
                    {
                        if(tokens.Length > 2)
                            for (int k = 1; k < 2 * m; k += 2) tasks[i - 1].operations.Add(new Operation(Int32.Parse(tokens[k])));
                        //tasksN.Add(new Task(Int32.Parse(tokens[0]), Int32.Parse(tokens[1]), Int32.Parse(tokens[2]), 'r'));
                    }
                }

                foreach (Task ttt in tasks) Console.WriteLine(ttt.ToString());
                Console.WriteLine();

            }
        }
    }
}

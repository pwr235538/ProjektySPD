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
            Stopwatch sw = new Stopwatch();

            sw.Start();

            List<List<int>> combList = PermutationGenerator.Generate(new List<int>() { 1, 2, 3,4,5,6,7,8,9,10 });
            foreach (List<int> comb in combList)
            {
                foreach (int nb in comb) Console.Write(nb);
                Console.WriteLine();
            }

            sw.Stop();

            Console.WriteLine("Elapsed={0}", sw.Elapsed);

            Console.ReadLine();


            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0, p.IndexOf("SPD_Lab"));
            //Console.WriteLine("Starting ...");
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\fsp");
            //Console.WriteLine("Path: " + path);
            String[] files = { "data001.txt", "data002.txt", "data003.txt", "data003.txt", "data004.txt" };//, "data005.txt", "data006.txt" };

            foreach (String filename in files)
            {
                int n, m; // liczba zadań, liczba maszyn


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
                        //Thread T = new Thread(() => PZ(n), stackSizeInBytes);
                        //T.Start();
                        PZ(n); 
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

        static void PZ(int x)
        {
            List<int> indeces = GenerateIndecesList(x);
            Node head = new Node(-1, indeces);
            Console.WriteLine("cheeeeck: " + head.index + " " + head.children[0].index);
            //Console.WriteLine("End nodes number: " + head.CountEndNodes());
        }

        static void PZ2(int x)
        {
            List<int> indeces = GenerateIndecesList(x);
            Combinations comb;

        }

        static List<List<int>> GetCombinations(int x)
        {
            List<List<int>> tr = new List<List<int>>();
            List<int> indeces = GenerateIndecesList(x);

            for(int i = 0; i < x; i++)
            {

            }

            return tr;
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

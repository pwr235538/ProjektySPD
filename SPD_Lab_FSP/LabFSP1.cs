using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SPD_Lab_FSP
{
    class LabFSP1
    {
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

                    String[] tokens = lines[i].Split(' ');
                    tasksN.Add(new Task(Int32.Parse(tokens[0]), Int32.Parse(tokens[1]), Int32.Parse(tokens[2]), 'r'));
                }

                //Task.PrintTaskList(tasksN); Console.ReadLine();
                int t = tasksN[0].r;
                List<Task> tasksPi = new List<Task>();


            }
        }
    }
}

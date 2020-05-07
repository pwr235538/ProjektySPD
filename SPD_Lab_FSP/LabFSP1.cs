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
            public int p;

            public Operation(int t)
            {
                this.p = t;
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
                foreach (Operation op in operations) tr += op.p + " ";

                return tr;
            }
        }

        static void Main(string[] args)
        {

            String p = System.Reflection.Assembly.GetEntryAssembly().Location;
            p = p.Substring(0, p.IndexOf("SPD_Lab"));
            //Console.WriteLine("Starting ...");
            String path = Path.Combine(p, "SPD_Lab\\Pliki\\fsp");
            //Console.WriteLine("Path: " + path);
            String[] files = { "data001.txt", "data002.txt", "data003.txt", "data004.txt", "data005.txt", "data006.txt" };// };

            foreach (String filename in files)
            {
                int n = 0, m = 0; // liczba zadań, liczba maszyn

                List<Task> tasks = new List<Task>();

                //Console.WriteLine("\nBeginning file reading ...");
                String[] lines = File.ReadAllLines(Path.Combine(path, filename));
                Console.WriteLine("\n=======================================================================\nRead file " + filename + " completed.");

                // wczytywanie i przetwarzanie danych - początek
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
                        Console.WriteLine(filename + ": n=" + n + ", m=" + m);
                    }
                    else
                    {
                        if(tokens.Length > 2)
                            for (int k = 1; k < 2 * m; k += 2) tasks[i - 1].operations.Add(new Operation(Int32.Parse(tokens[k])));
                    }
                } // wczytywanie i przetwarzanie danych - koniec

                var stpw = new Stopwatch();

                // Kolejnosc naturalna:
                int Cmax = GetCmax(m, n, tasks);
                Console.WriteLine("Cmax natural: " + Cmax);

                // Algorytm Johnsona:
                List<int> johnsonOrder = new List<int>();
                stpw.Start();
                Console.WriteLine("Cmax Johnson: " + GetCmax(m, n, GetInJohnsonOrder(m,n,tasks, ref johnsonOrder)));
                stpw.Stop();
                Console.Write("Johnson order: "); foreach (int o in johnsonOrder) Console.Write(o + " "); Console.WriteLine();
                Console.WriteLine("Johnson elapsed={0}", stpw.Elapsed);

                // czy odpalac wersje z przegladem zupelnym (brute force)
                if(false)
                {
                    // Przeglad zupelny - rozwiazanie optymalne:
                    List<int> bestCombOrder = new List<int>();
                    stpw.Restart();
                    Cmax = GenerateAndFindBestCmax(m, n, tasks, ref bestCombOrder);
                    stpw.Stop();
                    Console.WriteLine("Cmax opt: " + Cmax);
                    Console.Write("Opt order: "); foreach (int o in bestCombOrder) Console.Write(o + " "); Console.WriteLine();
                    Console.WriteLine("Opt elapsed={0}", stpw.Elapsed);
                }

            }
        }

        static int GetCmax(int m, int n, List<Task> tasks)
        {
            return GetCmax(m, n, tasks, GenerateIndecesList(n));
        }

        static int GetCmax(int m, int n, List<Task> tasks, List<int> order)
        {
            int[,] S = new int[m, n];
            int[,] C = new int[m, n];
            S[0, 0] = 0;
            for (int j = 1; j <= n; j++) // wyznaczenie czasow rozpoczecia i zakonczenia operacji na pierwszej maszynie
            {
                C[0, j - 1] = S[0, j - 1] + tasks[order[j - 1]].operations[0].p;
                if (j < n) S[0, j] = C[0, j - 1];
            }

            //Dalsza część algorytmu dla pozostałych maszyn
            for (int i = 1; i < m; i++) // iterowanie po maszynach/operacjach
            {
                for (int j = 0; j < n; j++) // iterowanie po zadaniach
                {
                    if (j > 0) S[i, j] = Math.Max(C[i - 1, j], C[i, j - 1]); // zakonczenie poprzedniej operacji tego samego zadania lub zakonczenie tej samej operacji poprzedniego zadania
                    else S[i, j] = C[i - 1, j]; // zakonczenie poprzedniej operacji tego samego zadania gdy jest to pierwsze zadanie (żeby nie wychodzic poza zakres tablicy)
                    C[i, j] = S[i, j] + tasks[order[j]].operations[i].p; // wyznaczenie czasu zakończenia przed dodanie czasu trwania operacji
                }
            }

            return C[m - 1, n - 1];
        }

        static List<Task> GetInJohnsonOrder(int m, int n, List<Task> tasks, ref List<int> optOrder)
        {
            List<Task> ordered = new List<Task>(new Task[n]);


            List<Task> N = new List<Task>(tasks); //kopia

            int l = 1, k = n;

            while(N.Count > 0)
            {
                int pmin = Int32.MaxValue, imin = -1, jmin = -1;

                // l6 > start
                for(int i = 0; i < m; i++)
                {
                    for(int j = 0; j < N.Count; j++)
                    {
                        //Console.WriteLine("j={0}, i={1}", j, i);
                        if(N[j].operations[i].p < pmin)
                        {
                            pmin = N[j].operations[i].p;
                            imin = i; jmin = j;
                        }
                    }
                }
                // l6 > end

                if(N[jmin].operations[0].p < N[jmin].operations[1].p) // l7
                {
                    ordered[l - 1] = N[jmin]; // l8
                    l++; //l9
                }
                else // l10
                {
                    ordered[k - 1] = N[jmin]; // l11
                    k--; // l12
                }

                N.RemoveAt(jmin); // l14
            }

            // PrintTasks(ordered);
            //Console.ReadLine();

            foreach (Task t in ordered) optOrder.Add(tasks.IndexOf(t));

            return ordered;
        }



        static List<int> GenerateIndecesList(int x)
        {
            List<int> indeces = new List<int>();
            for (int i = 0; i < x; i++) indeces.Add(i);
            //Console.WriteLine("indices count: " + indeces.Count);
            return indeces;
        }

        static void PrintTasks(List<Task> tasks)
        {
            for(int i = 0; i < tasks.Count; i++)
            {
                Console.Write("Task {0}: ", i);
                for (int j = 0; j < tasks[i].operations.Count; j++)
                    Console.Write("{0} ", tasks[i].operations[j].p);
                Console.WriteLine();
            }
        }


        static class FormPermut
        {
            static void SwapTwoNumber(ref int a, ref int b)
            {
                int temp = a;
                a = b;
                b = temp;
            }

            public static void CalcPermut(int m, int n, List<Task> tasks, ref List<int> bestCombOrder, ref int bestCmax, int[] list, int k)
            {
                int i;
                if (k == n - 1)
                {
                    List<int> nextComb = new List<int>();
                    for (i = 0; i <= n - 1; i++)
                    {
                        nextComb.Add(list[i]);
                    }

                    int cmax = GetCmax(m, n, tasks, nextComb);
                    if (cmax < bestCmax)
                    {
                        bestCombOrder = nextComb;
                        bestCmax = cmax;
                    }
                    //Console.WriteLine("better");
                }
                else
                    for (i = k; i <= n - 1; i++)
                    {
                        SwapTwoNumber(ref list[k], ref list[i]);
                        CalcPermut(m, n, tasks, ref bestCombOrder, ref bestCmax, list, k + 1);
                        SwapTwoNumber(ref list[k], ref list[i]);
                    }
            }
        }

        public static int GenerateAndFindBestCmax(int m, int n, List<Task> tasks, ref List<int> bestCombOrder)
        {
            int bestCmax = Int32.MaxValue;

            //ormPermut fp = new formPermut();
            int[] arr1 = GenerateIndecesList(n).ToArray();
            FormPermut.CalcPermut(m, n, tasks, ref bestCombOrder, ref bestCmax, arr1, 0);

            return bestCmax;
        }
    }


}

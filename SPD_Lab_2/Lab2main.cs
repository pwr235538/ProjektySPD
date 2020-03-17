using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;


namespace SPD_Lab_2
{
    public class SPD_Lab_2
    {
        static void Main(string[] args)
        {
            SPD_Lab_2org.SPD_Lab_2.MainOrg();
            SPD_Lab_2pq.SPD_Lab_2.MainPQ();

            Console.Write("\nWpisz cokolwiek i wcisnij Enter aby zakonczyc: ");
            Console.ReadLine();
        }
    }
}

using System;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using Lambda.Entities;

namespace Lambda
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Entre com o caminho do arquivo: ");
            string path = Console.ReadLine();

            Console.Write("Entre com o valor do salário: ");
            double salaryInfo = double.Parse(Console.ReadLine());

            List<Employes> list = new List<Employes>();

            using (StreamReader sr = File.OpenText(path))
            {
                while (!sr.EndOfStream)
                {
                    string[] fields = sr.ReadLine().Split(',');
                    string name = fields[0];
                    string email = fields[1];
                    double salary = double.Parse(fields[2], CultureInfo.InvariantCulture);
                    list.Add(new Employes(name, email, salary));
                }
            }
            Console.WriteLine("");
            Console.WriteLine("===================================================================");
            Console.WriteLine("");

            Console.WriteLine("E-mail de empregados com salário maior que " + salaryInfo + ":");
            var sort = list.Where(e => e.Salary > salaryInfo).OrderBy(e => e.Email).Select(e => e.Email);
            foreach(string email in sort)
            {
                Console.WriteLine(email);
            }
            Console.WriteLine("");
            Console.WriteLine("===================================================================");
            Console.WriteLine("");
            var sum = list.Where(n => n.Nome[0].ToString() == "M").Sum(n => n.Salary);
            Console.WriteLine("Soma dos salários dos empregados que começam com a letra 'M': " + sum.ToString("F2", CultureInfo.InvariantCulture));
        }
    }
}

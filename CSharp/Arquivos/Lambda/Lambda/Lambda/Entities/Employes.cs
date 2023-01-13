using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Entities
{
    class Employes
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public double Salary { get; set; }

        public Employes(string nome, string email, double salary)
        {
            Nome = nome;
            Email = email;
            Salary = salary;
        }
    }
}

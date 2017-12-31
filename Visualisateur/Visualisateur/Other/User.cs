using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualisateur.Other
{
    public class User
    {
        public User()
        {

        }

        public User(string p, string pa, string n)
        {
            Pseudo = p;
            Name = n;
            Path = pa;
        }

        public string Pseudo { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

    }
}

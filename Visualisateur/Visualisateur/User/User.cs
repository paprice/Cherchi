using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualisateur.User
{
    class User
    {
        string Pseudo;
        string Name;
        string Path;

        public User()
        {

        }

        public User(string p, string pa, string n)
        {
            Pseudo = p;
            Name = n;
            Path = pa;
        }

    }
}

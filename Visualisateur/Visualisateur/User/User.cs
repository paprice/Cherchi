using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualisateur.User
{
    class User
    {
        string pseudo;
        string name;
        string path;

        public User()
        {

        }

        public User(string p, string pa, string n)
        {
            pseudo = p;
            name = n;
            path = pa;
        }

        public string GetPseudo()
        {
            return pseudo;
        }

        public string GetName()
        {
            return name;
        }

        public string GetPath()
        {
            return path;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BirdEyes.Shared
{
    public class Developer
    {
        public string Name { get; set; }
        public List<Application> ApplicationsDeveloped { get; set; }

        public Developer(string name)
        {
            Name = name;
        }
    }
}

    


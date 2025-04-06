using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace mantis_tests
{
    public class ProjectData
    {
        public ProjectData(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
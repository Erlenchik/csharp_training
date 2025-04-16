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
    public static ProjectData GenerateProjectName()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string name = $"Project_{timestamp}";
            return new ProjectData(name);
        }

        public override bool Equals(object obj)
        {
            if (obj is ProjectData other)
            {
                return Name == other.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"Project: {Name}";
        }
    }
}
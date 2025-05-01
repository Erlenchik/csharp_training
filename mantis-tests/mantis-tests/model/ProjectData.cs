using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace mantis_tests
{
    public class ProjectData : IComparable<ProjectData>
    {
        public ProjectData(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        
        public int CompareTo(ProjectData other)
        {
            if (other == null)
                return 1;

            return Name.CompareTo(other.Name);
        }
        
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
            return Name != null ? Name.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return $"Project: {Name}, Id: {Id}, Description: {Description}";
        }
    }
}
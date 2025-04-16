using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace mantis_tests
{
    [TestFixture]
    public class ProjectCreationTests : AuthTestBase
    {
        [Test]
        public void TestProjectCreation()
        {

            List<ProjectData> oldProjects = app.Projects.GetProjectList();
            ProjectData project = ProjectData.GenerateProjectName();
            app.Projects.Create(project);

            List<ProjectData> newProjects = app.Projects.GetProjectList();

            oldProjects.Add(project);

            oldProjects.Sort((x, y) => x.Name.CompareTo(y.Name));
            newProjects.Sort((x, y) => x.Name.CompareTo(y.Name));

            Assert.AreEqual(oldProjects, newProjects);
        }
    }
}
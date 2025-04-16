using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace mantis_tests
{
    [TestFixture]
    public class ProjectRemovalTests : AuthTestBase
    {
        [Test]
        public void TestProjectRemoval()
        {
            List<ProjectData> oldProjects = app.Projects.GetProjectList();
            if (oldProjects.Count == 0)
            {
                ProjectData newProject = ProjectData.GenerateProjectName();
                app.Projects.Create(newProject);
                oldProjects = app.Projects.GetProjectList();
            }
            
            ProjectData projectToRemove = oldProjects[0];
            
            app.Projects.Remove(1);
            
            List<ProjectData> newProjects = app.Projects.GetProjectList();

            Assert.AreEqual(oldProjects.Count - 1, newProjects.Count);
        }
    }
}
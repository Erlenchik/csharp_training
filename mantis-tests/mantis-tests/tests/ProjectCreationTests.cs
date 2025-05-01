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

        [Test]
        public async Task TestProjectCreationAPI()
        {
            var project = new ProjectData(GenerateRandomString(9))
            {
                Description = GenerateRandomString(15)
            };

            var oldProjects = await app.Projects.GetProjectListAPI();

            app.API.CreateProjectsByApi(project);

            var newProjects = await app.Projects.GetProjectListAPI();
            oldProjects.Add(project);

            oldProjects.Sort();
            newProjects.Sort();

            Assert.AreEqual(oldProjects, newProjects);
        }
    }
}
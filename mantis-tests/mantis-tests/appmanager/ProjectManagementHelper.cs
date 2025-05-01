using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using mantis_tests.Mantis;

namespace mantis_tests
{
    public class ProjectManagementHelper : HelperBase
    {
        public ProjectManagementHelper(ApplicationManager manager) : base(manager) { }

        public ProjectManagementHelper Create(ProjectData project)
        {
            OpenPageProjectManagement();
            CheckNameProject(project);
            ProjectCreation();
            FillProjectForm(project);
            SubmitProjectCreation();
            return this;
        }

        private ProjectManagementHelper CheckNameProject(ProjectData project)
        {
            if (OpenNameList())
            {
                if (OpenNameList(project))
                {
                    ProjectData projectNew = new ProjectData("Новый проект");
                    Create(projectNew);
                }
            }
            return this;
        }

        private bool OpenNameList()
        {
            return IsElementPresent(By.XPath("//div[@id='form-container']/div[2]/div[2]/div/div/div[2]/div[2]/div/div/table/tbody/tr"));
        }

        private bool OpenNameList(ProjectData project)
        {
            return OpenNameList() && GetProjectName() == project.Name;
        }

        private string GetProjectName()
        {
            string text = driver.FindElement(By.XPath("//div[@id='form-container']/div[2]/div[2]/div/div/div[2]/div[2]/div/div/table/tbody/tr/td/a")).Text;
            return text;
        }

        public ProjectManagementHelper Remove(int v)
        {
            OpenPageProjectManagement();
            SelectProject(v);
            RemoveProject();
            SubmitRemoveProject();
            return this;
        }

        private ProjectManagementHelper OpenPageProjectManagement()
        {
            driver.FindElement(By.XPath("//a[@href='/mantisbt-2.26.4/manage_overview_page.php']")).Click();
            driver.FindElement(By.LinkText("Проекты")).Click();
            return this;
        }

        private ProjectManagementHelper ProjectCreation()
        {
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            return this;
        }

        private ProjectManagementHelper FillProjectForm(ProjectData project)
        {
            driver.FindElement(By.Id("project-name")).Click();
            driver.FindElement(By.Id("project-name")).Clear();
            driver.FindElement(By.Id("project-name")).SendKeys(project.Name);
            return this;
        }

        private ProjectManagementHelper SubmitProjectCreation()
        {
            driver.FindElement(By.XPath("//input[@value='Добавить проект']")).Click();
            return this;
        }

        private ProjectManagementHelper SelectProject(int index)
        {
            driver.FindElement(By.XPath("//div[@id='main-container']/div[2]/div[2]/div/div/div[2]/div[2]/div/div/table/tbody/tr[" + index + "]/td/a")).Click();
            return this;
        }

        private ProjectManagementHelper RemoveProject()
        {
            driver.FindElement(By.XPath("//form[@id='manage-proj-update-form']/div/div[3]/button[2]")).Click();
            return this;
        }

        private ProjectManagementHelper SubmitRemoveProject()
        {
            driver.FindElement(By.XPath("//input[@value='Удалить проект']")).Click();
            return this;
        }

        public List<ProjectData> GetProjectList()
        {
            List<ProjectData> projects = new List<ProjectData>();
            OpenPageProjectManagement();

            IReadOnlyCollection<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class,'table')]/tbody/tr"));

            foreach (IWebElement row in rows)
            {
                IList<IWebElement> nameElements = row.FindElements(By.XPath(".//td[1]/a")).ToList();
                if (nameElements.Count > 0)
                {
                    string name = nameElements[0].Text.Trim();
                    projects.Add(new ProjectData(name));
                }
            }
            return projects;
        }

        public bool IsProjectPresent(string projectName)
        {
            List<ProjectData> projects = GetProjectList();

            foreach (ProjectData project in projects)
            {
                if (project.Name == projectName)
                {
                    return true;
                }
            }
            return false;
        }

        public void ViewListInProject()
        {
            OpenPageProjectManagement();
            var projectsTable = driver.FindElement(
                By.XPath("//table[@class='table table-striped table-bordered table-condensed table-hover']"));
            var projectRows = projectsTable.FindElements(By.XPath(".//tbody/tr"));

            if (projectRows.Count == 0)
            {
                ProjectData createdProjectToRemove = new ProjectData("Добавляем проект");
                Create(createdProjectToRemove);
            }
        }

        public List<ProjectData> GetAllProjectsList()
        {
            List<ProjectData> projectList = new List<ProjectData>();
            ICollection<IWebElement> rows = driver.FindElements(
                By.XPath("//table[@class='table table-striped table-bordered table-condensed table-hover']/tbody/tr")
            );
            foreach (IWebElement row in rows)
            {
                try
                {
                    string id = row.FindElement(By.XPath("./td/a")).GetAttribute("href").Split('=')[1];
                    string name = row.FindElement(By.XPath("./td/a")).Text;
                    string description = row.FindElement(By.XPath("./td[5]")).Text;

                    ProjectData project = new ProjectData(name)
                    {
                        Id = id
                    };

                    projectList.Add(project);
                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine($"Ошибка при извлечении данных: {ex.Message}");
                }
            }
            return projectList;
        }

        public ProjectData TakeProject()
        {
            IWebElement firstRow = driver.FindElement(
                By.XPath("//table[@class='table table-striped table-bordered table-condensed table-hover']/tbody/tr[1]")
            );
            try
            {
                string id = firstRow.FindElement(By.XPath("./td/a")).GetAttribute("href").Split('=')[1];
                string name = firstRow.FindElement(By.XPath("./td/a")).Text;
                string description = firstRow.FindElement(By.XPath("./td[5]")).Text;

                return new ProjectData(name)
                {
                    Description = description,
                    Id = id
                };
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public int GetProjectCount()
        {
            return driver.FindElements(By.XPath("//table[contains(@class,'table')]/tbody/tr")).Count;
        }

        public async Task<List<ProjectData>> GetProjectListAPI()
        {
            var projectList = new List<ProjectData>();
            MantisConnectPortTypeClient client = new MantisConnectPortTypeClient();
            var projects = await client.mc_projects_get_user_accessibleAsync("administrator", "root");

            foreach (var project in projects)
            {
                var projectData = new ProjectData(project.name)
                {
                    Id = project.id,
                    Description = project.description
                };
                projectList.Add(projectData);
            }

            return projectList;
        }
    }
}

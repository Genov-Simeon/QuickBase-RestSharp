using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RestSharp;
using QuickBaseApi.Client;
using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Tests
{
    public class BaseTest
    {
        const string Projects = "Projects";
        const string Tasks = "Tasks";

        protected QuickBaseClient? QuickBaseClient { get; set; }
        protected RestClient? RestClient { get; set; }
        protected QuickBaseConfig? Config { get; set; }

        protected List<QuickBaseTable>? Tables { get; set; }
        protected QuickBaseTable? ProjectsTable { get; set; }
        protected List<QuickBaseFieldModel>? ProjectsFields { get; set; }
        protected QuickBaseTable? TasksTable { get; set; }
        protected List<QuickBaseFieldModel>? TasksFields { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            Config = Configuration.Config.GetSection("QuickBase").Get<QuickBaseConfig>();
            QuickBaseClient = new QuickBaseClient(Config);

            Tables = await QuickBaseClient.GetTablesAsync(Config.AppId);
            ProjectsTable = Tables.FirstOrDefault(t => t.Name.Equals(Projects, StringComparison.OrdinalIgnoreCase));
            TasksTable = Tables.FirstOrDefault(t => t.Name.Equals(Tasks, StringComparison.OrdinalIgnoreCase));
            ProjectsFields = await QuickBaseClient.GetFieldsByTableName(ProjectsTable.Id);
            TasksFields = await QuickBaseClient.GetFieldsByTableName(TasksTable.Id);
        }
    }
}

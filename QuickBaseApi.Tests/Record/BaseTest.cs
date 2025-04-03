using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RestSharp;
using QuickBaseApi.Client;
using QuickBaseApi.Client.Models;
using static QuickBaseApi.Client.Utils.FieldHelper;

namespace QuickBaseApi.Tests
{
    public class BaseTest
    {
        const string Projects = "Projects";
        const string Tasks = "Tasks";

        protected RestClient? RestClient { get; set; }
        protected QuickBaseConfig? Config { get; set; }
        protected QuickBaseClient? QuickBaseClient { get; set; }

        protected List<TableModel>? Tables { get; set; }
        protected TableModel? ProjectsTable { get; set; }
        protected TableModel? TasksTable { get; set; }
        protected List<FieldModel>? ProjectsFields { get; set; }
        protected List<FieldModel>? TasksFields { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            Config = Configuration.Config.GetSection("QuickBase").Get<QuickBaseConfig>();
            QuickBaseClient = new QuickBaseClient(Config);

            Tables = await QuickBaseClient.GetTablesAsync(Config.AppId);

            ProjectsTable = Tables.FirstOrDefault(t => t.Name.Equals(Projects, StringComparison.OrdinalIgnoreCase));
            TasksTable = Tables.FirstOrDefault(t => t.Name.Equals(Tasks, StringComparison.OrdinalIgnoreCase));

            ProjectsFields = GetWritableFields(await QuickBaseClient.GetFieldsByTableName(ProjectsTable.Id));
            TasksFields = GetWritableFields(await QuickBaseClient.GetFieldsByTableName(TasksTable.Id));
        }
    }
}

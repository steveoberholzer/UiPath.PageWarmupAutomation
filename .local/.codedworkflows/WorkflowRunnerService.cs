using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.CodedWorkflows;
using UiPath.CodedWorkflows.Interfaces;
using UiPath.Activities.Contracts;
using WebApplicationWarmup;

[assembly: WorkflowRunnerServiceAttribute(typeof(WebApplicationWarmup.WorkflowRunnerService))]
namespace WebApplicationWarmup
{
    public class WorkflowRunnerService
    {
        private readonly ICodedWorkflowServices _services;
        public WorkflowRunnerService(ICodedWorkflowServices services)
        {
            _services = services;
        }

        /// <summary>
        /// Invokes the Workflows/Config/GetAssets.xaml
        /// </summary>
        /// <param name="isolated">Indicates whether to isolate executions (run them within a different process)</param>
        public (string out_sPageConfig, bool out_bNotificationsEnabled, string out_sNotificationEmailAddresses, int out_iGeneralTimeout) GetAssets(System.Boolean isolated = false)
        {
            var result = _services.WorkflowInvocationService.RunWorkflow(@"Workflows\Config\GetAssets.xaml", new Dictionary<string, object> { }, default, isolated, default, GetAssemblyName());
            return ((string)result["out_sPageConfig"], (bool)result["out_bNotificationsEnabled"], (string)result["out_sNotificationEmailAddresses"], (int)result["out_iGeneralTimeout"]);
        }

        /// <summary>
        /// Invokes the Main.xaml
        /// </summary>
        /// <param name="isolated">Indicates whether to isolate executions (run them within a different process)</param>
        public void Main(System.Boolean isolated = false)
        {
            var result = _services.WorkflowInvocationService.RunWorkflow(@"Main.xaml", new Dictionary<string, object> { }, default, isolated, default, GetAssemblyName());
        }

        /// <summary>
        /// Invokes the Workflows/Parsing/Services/PageConfigs.cs
        /// </summary>
        /// <param name="isolated">Indicates whether to isolate executions (run them within a different process)</param>
        public void PageConfigs(System.Boolean isolated = false)
        {
            var result = _services.WorkflowInvocationService.RunWorkflow(@"Workflows\Parsing\Services\PageConfigs.cs", new Dictionary<string, object> { }, default, isolated, default, GetAssemblyName());
        }

        /// <summary>
        /// Invokes the Workflows/Automation/WarmupPage.xaml
        /// </summary>
        /// <param name="isolated">Indicates whether to isolate executions (run them within a different process)</param>
        public WebApplicationWarmup.Workflows.Models.WarmupResult WarmupPage(WebApplicationWarmup.Workflows.Parsing.Models.PageConfig in_oPageConfig, bool in_bNotificationsEnabled, string in_sNotificationEmailAddresses, int in_iGeneralTimeout, System.Boolean isolated = false)
        {
            var result = _services.WorkflowInvocationService.RunWorkflow(@"Workflows\Automation\WarmupPage.xaml", new Dictionary<string, object> { { "in_oPageConfig", in_oPageConfig }, { "in_bNotificationsEnabled", in_bNotificationsEnabled }, { "in_sNotificationEmailAddresses", in_sNotificationEmailAddresses }, { "in_iGeneralTimeout", in_iGeneralTimeout } }, default, isolated, default, GetAssemblyName());
            return (WebApplicationWarmup.Workflows.Models.WarmupResult)result["out_oResult"];
        }

        /// <summary>
        /// Invokes the Workflows/Models/WarmupResult.cs
        /// </summary>
        /// <param name="isolated">Indicates whether to isolate executions (run them within a different process)</param>
        public void WarmupResult(System.Boolean isolated = false)
        {
            var result = _services.WorkflowInvocationService.RunWorkflow(@"Workflows\Models\WarmupResult.cs", new Dictionary<string, object> { }, default, isolated, default, GetAssemblyName());
        }

        private string GetAssemblyName()
        {
            var assemblyProvider = _services.Container.Resolve<ILibraryAssemblyProvider>();
            return assemblyProvider.GetLibraryAssemblyName(GetType().Assembly);
        }
    }
}
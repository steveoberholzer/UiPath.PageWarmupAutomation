using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.CodedWorkflows;
using UiPath.CodedWorkflows.Interfaces;
using UiPath.Activities.Contracts;
using K2Warmup;

[assembly: WorkflowRunnerServiceAttribute(typeof(K2Warmup.WorkflowRunnerService))]
namespace K2Warmup
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
        /// Invokes the Workflows/Automation/WarmupPage.xaml
        /// </summary>
        /// <param name="isolated">Indicates whether to isolate executions (run them within a different process)</param>
        public void WarmupPage(string in_sUrl, bool in_bWaitForElement, string in_sElementSelector, string in_sSelectorType, int in_iTimeoutOverride, int in_iGeneralTimeout, System.Boolean isolated = false)
        {
            var result = _services.WorkflowInvocationService.RunWorkflow(@"Workflows\Automation\WarmupPage.xaml", new Dictionary<string, object> { { "in_sUrl", in_sUrl }, { "in_bWaitForElement", in_bWaitForElement }, { "in_sElementSelector", in_sElementSelector }, { "in_sSelectorType", in_sSelectorType }, { "in_iTimeoutOverride", in_iTimeoutOverride }, { "in_iGeneralTimeout", in_iGeneralTimeout } }, default, isolated, default, GetAssemblyName());
        }

        /// <summary>
        /// Invokes the Workflows/Automation/WarmupPageShell.xaml
        /// </summary>
        /// <param name="isolated">Indicates whether to isolate executions (run them within a different process)</param>
        public K2Warmup.Workflows.Models.WarmupResult WarmupPageShell(K2Warmup.Workflows.Parsing.Models.PageConfig in_oPageConfig, int in_iGeneralTimeout, System.Boolean isolated = false)
        {
            var result = _services.WorkflowInvocationService.RunWorkflow(@"Workflows\Automation\WarmupPageShell.xaml", new Dictionary<string, object> { { "in_oPageConfig", in_oPageConfig }, { "in_iGeneralTimeout", in_iGeneralTimeout } }, default, isolated, default, GetAssemblyName());
            return (K2Warmup.Workflows.Models.WarmupResult)result["out_oResult"];
        }

        private string GetAssemblyName()
        {
            var assemblyProvider = _services.Container.Resolve<ILibraryAssemblyProvider>();
            return assemblyProvider.GetLibraryAssemblyName(GetType().Assembly);
        }
    }
}
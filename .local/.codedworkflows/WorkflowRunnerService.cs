using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.CodedWorkflows;
using UiPath.Activities.Contracts;

namespace K2Warmup
{
    public class WorkflowRunnerService
    {
        private readonly Func<string, IDictionary<string, object>, TimeSpan?, bool, InvokeTargetSession, IDictionary<string, object>> _runWorkflowHandler;
        public WorkflowRunnerService(Func<string, IDictionary<string, object>, TimeSpan?, bool, InvokeTargetSession, IDictionary<string, object>> runWorkflowHandler)
        {
            _runWorkflowHandler = runWorkflowHandler;
        }

        /// <summary>
        /// Invokes the Main.xaml
        /// </summary>
        public void Main()
        {
            var result = _runWorkflowHandler(@"Main.xaml", new Dictionary<string, object> { }, default, default, default);
        }

        /// <summary>
        /// Invokes the Workflows/Config/GetAssets.xaml
        /// </summary>
        public (string out_sPageConfig, bool out_bNotificationsEnabled, string out_sNotificationEmailAddresses, int out_iGeneralTimeout) GetAssets()
        {
            var result = _runWorkflowHandler(@"Workflows\Config\GetAssets.xaml", new Dictionary<string, object> { }, default, default, default);
            return ((string)result["out_sPageConfig"], (bool)result["out_bNotificationsEnabled"], (string)result["out_sNotificationEmailAddresses"], (int)result["out_iGeneralTimeout"]);
        }

        /// <summary>
        /// Invokes the Workflows/Automation/WarmupPage.xaml
        /// </summary>
        public K2Warmup.Workflows.Models.WarmupResult WarmupPage(K2Warmup.Workflows.Parsing.Models.PageConfig in_oPageConfig, int in_iGeneralTimeout)
        {
            var result = _runWorkflowHandler(@"Workflows\Automation\WarmupPage.xaml", new Dictionary<string, object> { { "in_oPageConfig", in_oPageConfig }, { "in_iGeneralTimeout", in_iGeneralTimeout } }, default, default, default);
            return (K2Warmup.Workflows.Models.WarmupResult)result["out_oResult"];
        }
    }
}
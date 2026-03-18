using System;
using System.Collections.Generic;
using System.Data;
using UiPath.Activities.System.Jobs.Coded;
using UiPath.CodedWorkflows;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;

namespace WebApplicationWarmup
{
    public partial class CodedWorkflow : CodedWorkflowBase
    {
        private Lazy<global::WebApplicationWarmup.WorkflowRunnerService> _workflowRunnerServiceLazy;
        private Lazy<ConnectionsManager> _connectionsManagerLazy;
        public CodedWorkflow()
        {
            _ = new System.Type[]
            {
                typeof(UiPath.Core.Activities.API.ISystemService)
            };
            _workflowRunnerServiceLazy = new Lazy<global::WebApplicationWarmup.WorkflowRunnerService>(() => new global::WebApplicationWarmup.WorkflowRunnerService(this.services));
#pragma warning disable
            _connectionsManagerLazy = new Lazy<ConnectionsManager>(() => new ConnectionsManager(serviceContainer));
#pragma warning restore
        }

        protected global::WebApplicationWarmup.WorkflowRunnerService workflows => _workflowRunnerServiceLazy.Value;
        protected ConnectionsManager connections => _connectionsManagerLazy.Value;
#pragma warning disable
        protected UiPath.Core.Activities.API.ISystemService system { get => serviceContainer.Resolve<UiPath.Core.Activities.API.ISystemService>() ; }
#pragma warning restore
    }
}
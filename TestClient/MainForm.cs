using System.Diagnostics;
using HostingTest.Client;
using HostingTest.Client.Serialization;
using HostingTest.Contracts;
using HyperSoa.Client;
using HyperSoa.Client.Extensions;
using HyperSoa.Client.RemoteAdmin.Extensions;
using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.RemoteAdmin.Models;

namespace TestClient
{
    public partial class MainForm : Form
    {
        internal const string ClientAgentName = "HyperNodeTestClient";

        private const bool EnableFiddler = false;
        private const string AliceHttpEndpoint = $"http://localhost{(EnableFiddler ? ".fiddler" : "")}:8005/HyperNode/AliceLocal00";
        private const string EveHttpEndpoint1 = $"http://localhost{(EnableFiddler ? ".fiddler" : "")}:8005/HyperNode/EveLocal00";
        private const string EveHttpEndpoint2 = $"http://localhost{(EnableFiddler ? ".fiddler" : "")}:8020/HyperNode/EveLocal01";

        private const string TargetEndpoint = EveHttpEndpoint1;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Events

        private void btnLaunchRemoteAdmin_Click(object sender, EventArgs e)
        {
            new RemoteAdminForm().Show(this);
        }

        private void btnHostingTestForm_Click(object sender, EventArgs e)
        {
            new HostingTestClientForm().Show(this);
        }

        private async void btnRefreshCommandList_Click(object sender, EventArgs e)
        {
            try
            {
                cboCommandNames.DataSource = null;

                var client = new HyperNodeHttpClient(
                    TargetEndpoint
                ).AsRemoteAdminClient(
                    ClientAgentName
                );

                cboCommandNames.DataSource = (
                    await client.GetNodeStatusAsync(
                        new GetNodeStatusRequest()
                    )
                ).Commands?.Select(
                    c => c.CommandName
                ).OrderBy(
                    cn => cn
                ).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRunCommand_Click(object sender, EventArgs e)
        {
            btnRunCommand.Enabled = false;

            try
            {
                // Clear out our data source first
                ClearResponseData();

                var optionFlags = MessageProcessOptionFlags.None;
                if (chkReturnTaskTrace.Checked)
                    optionFlags |= MessageProcessOptionFlags.ReturnTaskTrace;
                if (chkRunConcurrently.Checked)
                    optionFlags |= MessageProcessOptionFlags.RunConcurrently;
                if (chkCacheProgressInfo.Checked)
                    optionFlags |= MessageProcessOptionFlags.CacheTaskProgress;

                HyperNodeMessageResponse? response;

                switch (cboCommandNames.Text)
                {
                    case "ComplexCommand":
                        response = await RunComplexCommand(optionFlags);
                        break;
                    case "LongRunningCommand":
                    case "LongRunningSingletonCommand":
                        response = await RunLongRunningCommand(optionFlags);
                        break;
                    case "Echo":
                        response = await RunEchoCommand(optionFlags);
                        break;
                    default:
                        response = await RunNoContractCommand(optionFlags);
                        await RunNoContractCommand(true, true);
                        break;
                }

                PopulateResponseSummary(lstRealTimeResponse, response);
                PopulateTaskTrace(tvwRealTimeTaskTrace, response);

                if (response?.NodeAction != HyperNodeActionType.Rejected &&
                    optionFlags.HasFlag(MessageProcessOptionFlags.CacheTaskProgress) &&
                    !string.IsNullOrWhiteSpace(response?.TaskId))
                {
                    await TrackCommandProgress(response.TaskId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRunCommand.Enabled = true;
            }
        }

        private async void btnCancelCurrentTask_Click(object sender, EventArgs e)
        {
            var targetTaskId = txtTaskId.Text;
            if (!string.IsNullOrWhiteSpace(targetTaskId))
            {
                var client = new HyperNodeHttpClient(TargetEndpoint).AsRemoteAdminClient(ClientAgentName);

                var cancelSuccess = (
                    await client.CancelTaskAsync(
                        new CancelTaskRequest
                        {
                            TaskId = targetTaskId
                        }
                    )
                ).ProcessStatusFlags.HasFlag(
                    MessageProcessStatusFlags.Success
                );

                if (cancelSuccess)
                    MessageBox.Show("Task cancelled successfully!", "Task Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"Unable to cancel task with Task ID '{targetTaskId}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Events

        #region Private Methods

        private void ClearResponseData()
        {
            // Clear out the real-time response data
            lstRealTimeResponse.DataSource = null;
            tvwRealTimeTaskTrace.Nodes.Clear();

            // Clear out the response data
            lstCachedActivityItems.DataSource = null;
            lstCachedResponseSummary.DataSource = null;
            tvwCachedTaskTrace.Nodes.Clear();
        }

        private static void PopulateResponseSummary(ListControl? lstTarget, HyperNodeMessageResponse? response)
        {
            if (lstTarget == null)
                return;

            if (response == null)
            {
                lstTarget.DataSource = null;
                return;
            }

            lstTarget.DataSource = new[]
            {
                $"Task ID: {response.TaskId}",
                $"Responding Node Name: {response.RespondingNodeName}",
                $"Total Run Time: {response.TotalRunTime}",
                $"Node Action: {response.NodeAction}",
                $"Node Action Reason: {response.NodeActionReason}",
                $"Process Status Flags: {response.ProcessStatusFlags}",
                $"Response Byte Count: {response.CommandResponseBytes?.Length.ToString() ?? "<null>"}",
                $"Task Trace Count: {response.TaskTrace?.Length.ToString() ?? "<null>"}"
            };
        }

        private static void PopulateTaskTrace(TreeView? tvwTaskTrace, HyperNodeMessageResponse? response)
        {
            if (tvwTaskTrace == null)
                return;

            if (response == null)
            {
                tvwTaskTrace.Nodes.Clear();
                return;
            }

            tvwTaskTrace.Nodes.AddRange(
                new[]
                {
                    new TreeNode(
                        response.RespondingNodeName,
                        response.TaskTrace?.Select(
                            i => new TreeNode(
                                FormatActivityItem(i),
                                string.IsNullOrWhiteSpace(i.EventDetail)
                                    ? Array.Empty<TreeNode>()
                                    : new []
                                    {
                                        new TreeNode(i.EventDetail)
                                    }
                            )
                        ).ToArray() ?? Array.Empty<TreeNode>()
                    )
                    {
                        Tag = response
                    }
                }
            );
        }

        private static string FormatActivityItem(HyperNodeActivityItem item)
        {
            double? progressPercentage = null;

            if (item.ProgressTotal > 0)
                progressPercentage = item.ProgressPart / item.ProgressTotal;

            return $"{item.EventDateTime:G} {item.Agent}{(progressPercentage.HasValue || item.Elapsed.HasValue ? $" ({item.Elapsed}{(item.Elapsed.HasValue && progressPercentage.HasValue ? " " : "")}{progressPercentage:P})" : "")} - {item.EventDescription}";
        }

        private static string[]? GetActivityStrings(IEnumerable<HyperNodeActivityItem>? activity)
        {
            return activity?.OrderBy(
                i => i.EventDateTime
            ).Select(
                FormatActivityItem
            ).ToArray();
        }

        private async Task TrackCommandProgress(string taskId)
        {
            txtTaskId.Text = taskId;

            IProgress<HyperNodeTaskProgressInfo> progress = new Progress<HyperNodeTaskProgressInfo>(
                progressInfo =>
                {
                    lstCachedActivityItems.DataSource = GetActivityStrings(progressInfo.Activity);
                }
            );

            var finalTaskProgressInfo = await Task.Run(
                async () =>
                {
                    var progressTimer = new Stopwatch();
                    progressTimer.Start();

                    var taskProgressInfo = new HyperNodeTaskProgressInfo();

                    var client = new HyperNodeHttpClient(TargetEndpoint).AsRemoteAdminClient(ClientAgentName);

                    while (!taskProgressInfo.IsComplete && progressTimer.Elapsed <= TimeSpan.FromMinutes(2))
                    {
                        try
                        {
                            var commandResponse = await client.GetCachedTaskProgressInfoAsync(
                                new GetCachedTaskProgressInfoRequest
                                {
                                    TaskId = taskId
                                }
                            ).ConfigureAwait(false);

                            taskProgressInfo = commandResponse.TaskProgressInfo ?? new HyperNodeTaskProgressInfo();
                            if (!commandResponse.TaskProgressCacheEnabled)
                            {
                                taskProgressInfo.Activity = new[]
                                {
                                    new HyperNodeActivityItem
                                    {
                                        Agent = ClientAgentName,
                                        EventDescription = $"Warning: Task progress cache is not enabled for HyperNode at \'{TargetEndpoint}\'."
                                    }
                                };

                                // Make sure we exit the loop, since we're not going to get anything useful in this case.
                                taskProgressInfo.IsComplete = true;
                            }

                            progress.Report(taskProgressInfo);

                            Task.Delay(500).Wait();
                        }
                        catch
                        {
                            break;
                        }
                    }

                    progressTimer.Stop();

                    return taskProgressInfo;
                }
            );

            lstCachedActivityItems.DataSource = GetActivityStrings(finalTaskProgressInfo.Activity);
            PopulateResponseSummary(lstCachedResponseSummary, finalTaskProgressInfo.Response);
            PopulateTaskTrace(tvwCachedTaskTrace, finalTaskProgressInfo.Response);
        }

        private async Task<ComplexCommandResponse> RunComplexCommand(bool includeTaskTrace, bool cacheTaskProgress)
        {
            return await new HostingTestClient(
                ClientAgentName,
                TargetEndpoint
            ).ComplexCommandAsync(
                new ComplexCommandRequest
                {
                    MyDateTime = new DateTime(2002, 4, 12),
                    MyInt32 = 1000,
                    MyString = "My String!",
                    MyTimeSpan = TimeSpan.FromMinutes(17)
                }.CreatedBy(
                    ClientAgentName
                ).WithTaskTrace(
                    includeTaskTrace
                ).WithProgressCaching(
                    cacheTaskProgress
                )
            ).ConfigureAwait(false);
        }

        private async Task<HyperNodeMessageResponse> RunComplexCommand(MessageProcessOptionFlags optionFlags)
        {
            // Create our message request
            var serializer = new DataContractJsonSerializer<ComplexCommandRequest, ComplexCommandResponse>();
            var commandRequestBytes = serializer.SerializeRequest(
                new ComplexCommandRequest
                {
                    MyDateTime = new DateTime(2002, 4, 12),
                    MyInt32 = 1000,
                    MyString = "My String!",
                    MyTimeSpan = TimeSpan.FromMinutes(17)
                }
            );

            return await new HyperNodeHttpClient(
                TargetEndpoint
            ).ProcessMessageAsync(
                new HyperNodeMessageRequest
                {
                    CreatedByAgentName = ClientAgentName,
                    CommandName = cboCommandNames.Text,
                    CommandRequestBytes = commandRequestBytes,
                    ProcessOptionFlags = optionFlags
                }
            ).ConfigureAwait(false);
        }

        private async Task<HyperNodeMessageResponse> RunLongRunningCommand(MessageProcessOptionFlags optionFlags)
        {
            // Create our message request
            var serializer = new ProtoContractSerializer<LongRunningCommandRequest, EmptyCommandResponse>();
            var commandRequestBytes = serializer.SerializeRequest(
                new LongRunningCommandRequest
                {
                    TotalRunTime = TimeSpan.FromHours(1),
                    MinimumSleepInterval = TimeSpan.FromSeconds(1),
                    MaximumSleepInterval = TimeSpan.FromSeconds(5)
                }
            );

            return await new HyperNodeHttpClient(
                TargetEndpoint
            ).ProcessMessageAsync(
                new HyperNodeMessageRequest
                {
                    CreatedByAgentName = ClientAgentName,
                    CommandName = cboCommandNames.Text,
                    CommandRequestBytes = commandRequestBytes,
                    ProcessOptionFlags = optionFlags
                }
            ).ConfigureAwait(false);
        }

        private async Task<string> RunLongRunningCommand(bool includeTaskTrace, bool cacheTaskProgress)
        {
            return await new HostingTestClient(
                ClientAgentName,
                TargetEndpoint
            ).RunLongRunningCommandAsync(
                new LongRunningCommandRequest
                {
                    TotalRunTime = TimeSpan.FromHours(1),
                    MinimumSleepInterval = TimeSpan.FromSeconds(1),
                    MaximumSleepInterval = TimeSpan.FromSeconds(5)
                }.WithTaskTrace(
                    includeTaskTrace
                ).WithProgressCaching(
                    cacheTaskProgress
                )
            ).ConfigureAwait(false);
        }

        private async Task<HyperNodeMessageResponse> RunEchoCommand(MessageProcessOptionFlags optionFlags)
        {
            // Create our message request
            var serializer = new ProtoContractSerializer<EchoRequest, EchoResponse>();
            var commandRequestBytes = serializer.SerializeRequest(
                new EchoRequest
                {
                    Prompt = "Marco!"
                }
            );

            return await new HyperNodeHttpClient(
                TargetEndpoint
            ).ProcessMessageAsync(
                new HyperNodeMessageRequest
                {
                    CreatedByAgentName = ClientAgentName,
                    CommandName = cboCommandNames.Text,
                    CommandRequestBytes = commandRequestBytes,
                    ProcessOptionFlags = optionFlags
                }
            ).ConfigureAwait(false);
        }

        private static async Task RunNoContractCommand(bool includeTaskTrace, bool cacheTaskProgress)
        {
            await new HostingTestClient(
                ClientAgentName,
                TargetEndpoint
            ).NoContractCommandAsync(
                new EmptyCommandRequest().CreatedBy(
                    ClientAgentName
                ).WithTaskTrace(
                    includeTaskTrace
                ).WithProgressCaching(
                    cacheTaskProgress
                )
            ).ConfigureAwait(false);
        }

        private async Task<HyperNodeMessageResponse> RunNoContractCommand(MessageProcessOptionFlags optionFlags)
        {
            return await new HyperNodeHttpClient(
                TargetEndpoint
            ).ProcessMessageAsync(
                new HyperNodeMessageRequest
                {
                    CreatedByAgentName = ClientAgentName,
                    CommandName = cboCommandNames.Text,
                    ProcessOptionFlags = optionFlags
                }
            ).ConfigureAwait(false);
        }

        #endregion Private Methods
    }
}
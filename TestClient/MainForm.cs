using System.Diagnostics;
using HostingTest.Contracts;
using HyperSoa.Client;
using HyperSoa.Client.Serialization;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.RemoteAdmin.Models;

namespace TestClient
{
    public partial class MainForm : Form
    {
        private const string ClientAgentName = "HyperNodeTestClient";
        private const string AliceHttpEndpoint = "http://localhost:8005/HyperNode/AliceLocal00";

        public MainForm()
        {
            InitializeComponent();
        }

        #region Events

        private async void btnRefreshCommandList_Click(object sender, EventArgs e)
        {
            try
            {
                cboCommandNames.DataSource = null;

                var serializer = new ProtoContractSerializer<EmptyCommandRequest, GetNodeStatusResponse>();
                var msg = new HyperNodeMessageRequest
                {
                    CreatedByAgentName = ClientAgentName,
                    CommandName = RemoteAdminCommandName.GetNodeStatus
                };

                var client = new HyperNodeClient(AliceHttpEndpoint);
                var response = await client.ProcessMessageAsync(msg);

                if (response.CommandResponseBytes?.Length > 0)
                {
                    cboCommandNames.DataSource = serializer.DeserializeResponse(
                        response.CommandResponseBytes
                    )?.Commands.Select(
                        c => c.CommandName
                    ).OrderBy(
                        cn => cn
                    ).ToList();
                }
                else
                {
                    MessageBox.Show(response.RespondingNodeName + " did not send back a command response string.");
                }
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

                var msg = new HyperNodeMessageRequest
                {
                    CreatedByAgentName = ClientAgentName,
                    CommandName = cboCommandNames.Text,
                    CommandRequestBytes = commandRequestBytes,
                    ProcessOptionFlags = (chkReturnTaskTrace.Checked
                                             ? MessageProcessOptionFlags.ReturnTaskTrace
                                             : MessageProcessOptionFlags.None) |
                                         (chkRunConcurrently.Checked
                                             ? MessageProcessOptionFlags.RunConcurrently
                                             : MessageProcessOptionFlags.None) |
                                         (chkCacheProgressInfo.Checked
                                             ? MessageProcessOptionFlags.CacheTaskProgress
                                             : MessageProcessOptionFlags.None)
                };

                var client = new HyperNodeClient(AliceHttpEndpoint);
                var response = await client.ProcessMessageAsync(msg);

                PopulateResponseSummary(lstRealTimeResponse, response);
                PopulateTaskTrace(tvwRealTimeTaskTrace, response);

                if (response.NodeAction != HyperNodeActionType.Rejected &&
                    msg.ProcessOptionFlags.HasFlag(MessageProcessOptionFlags.CacheTaskProgress) &&
                    !string.IsNullOrWhiteSpace(response.TaskId))
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
                // Create our message request
                var serializer = new ProtoContractSerializer<CancelTaskRequest, EmptyCommandResponse>();
                var msg = new HyperNodeMessageRequest
                {
                    CreatedByAgentName = ClientAgentName,
                    CommandName = RemoteAdminCommandName.CancelTask,
                    CommandRequestBytes = serializer.SerializeRequest(
                        new CancelTaskRequest
                        {
                            TaskId = targetTaskId
                        }
                    )
                };

                var cancelSuccess = (
                    await new HyperNodeClient(AliceHttpEndpoint).ProcessMessageAsync(msg)
                )?.ProcessStatusFlags.HasFlag(
                    MessageProcessStatusFlags.Success
                ) ?? false;

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
            lstActivityItems.DataSource = null;
            lstResponseSummary.DataSource = null;
            tvwTaskTrace.Nodes.Clear();
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

        private static string[] GetActivityStrings(IEnumerable<HyperNodeActivityItem> activity)
        {
            return activity.OrderBy(
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
                    lstActivityItems.DataSource = GetActivityStrings(progressInfo.Activity);
                }
            );

            var finalTaskProgressInfo = await Task.Run(
                async () =>
                {
                    var progressTimer = new Stopwatch();
                    progressTimer.Start();

                    var taskProgressInfo = new HyperNodeTaskProgressInfo();

                    var alice = new HyperNodeClient(AliceHttpEndpoint);
                    var serializer = new ProtoContractSerializer<GetCachedTaskProgressInfoRequest, GetCachedTaskProgressInfoResponse>();
                    var request = new HyperNodeMessageRequest
                    {
                        CreatedByAgentName = ClientAgentName,
                        CommandName = RemoteAdminCommandName.GetCachedTaskProgressInfo,
                        CommandRequestBytes = serializer.SerializeRequest(
                            new GetCachedTaskProgressInfoRequest
                            {
                                TaskId = taskId
                            }
                        )
                    };

                    while (!taskProgressInfo.IsComplete && progressTimer.Elapsed <= TimeSpan.FromMinutes(2))
                    {
                        var aliceResponse = await alice.ProcessMessageAsync(
                            request
                        ).ConfigureAwait(false);

                        var targetResponse = aliceResponse;
                        if (!(targetResponse.CommandResponseBytes?.Length > 0))
                            break;

                        var commandResponse = serializer.DeserializeResponse(targetResponse.CommandResponseBytes);
                        taskProgressInfo = commandResponse?.TaskProgressInfo ?? new HyperNodeTaskProgressInfo();
                        if (!(commandResponse?.TaskProgressCacheEnabled ?? false))
                        {
                            taskProgressInfo.Activity.Add(
                                new HyperNodeActivityItem
                                {
                                    Agent = ClientAgentName,
                                    EventDescription = "Warning: Task progress cache is not enabled for HyperNode \'Alice\'."
                                }
                            );

                            // Make sure we exit the loop, since we're not going to get anything useful in this case.
                            taskProgressInfo.IsComplete = true;
                        }

                        progress.Report(taskProgressInfo);

                        Task.Delay(500).Wait();
                    }

                    progressTimer.Stop();

                    return taskProgressInfo;
                }
            );

            lstActivityItems.DataSource = GetActivityStrings(finalTaskProgressInfo.Activity);
            PopulateResponseSummary(lstResponseSummary, finalTaskProgressInfo.Response);
            PopulateTaskTrace(tvwTaskTrace, finalTaskProgressInfo.Response);
        }

        #endregion Private Methods
    }
}
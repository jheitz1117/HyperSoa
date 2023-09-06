using System.Text;
using HostingTest.Client;
using HostingTest.Contracts;
using HyperSoa.Client;
using HyperSoa.Client.Extensions;
using HyperSoa.Client.RemoteAdmin.Extensions;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.RemoteAdmin.Models;

namespace TestClient
{
    public partial class HostingTestClientForm : Form
    {
        private HostingTestClient? _client;

        public HostingTestClientForm()
        {
            InitializeComponent();

            cboEndpoints.DisplayMember = "DisplayText";
            cboEndpoints.ValueMember = "Value";
            cboEndpoints.DataSource = new[]
            {
                new
                {
                    DisplayText = "AliceNode00",
                    Value = "http://localhost:8005/HyperNode/AliceLocal00"
                },
                new
                {
                    DisplayText = "EveLocalHttp",
                    Value = "http://localhost:8005/HyperNode/EveLocal00"
                },
                new
                {
                    DisplayText = "EveLocalHttps",
                    Value = "https://localhost:8020/HyperNode/EveLocal00"
                }
            };
        }

        #region Events

        private void cboEndpoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEndpointUri.Text = cboEndpoints.SelectedValue as string;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnect.Enabled = false;

                _client = new HostingTestClient(
                    MainForm.ClientAgentName,
                    txtEndpointUri.Text
                );

                // Initial attempt to get live tasks
                await RefreshNodeStatus();

                cboEndpoints.Enabled = false;
                btnDisconnect.Enabled = true;
                pnlServiceDetails.Enabled = true;
            }
            catch (Exception ex)
            {
                btnConnect.Enabled = true;

                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnDisconnect.Enabled = false;

                _client = null;

                // If successfully disconnected, reenable target selection
                pnlServiceDetails.Enabled = false;
                cboEndpoints.Enabled = true;
                btnConnect.Enabled = true;
            }
            catch (Exception ex)
            {
                btnDisconnect.Enabled = true;

                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAsyncCommandNoContracts_Click(object sender, EventArgs e)
        {
            try
            {
                string? taskId = null;
                var builder = new StringBuilder();

                if (_client != null)
                {
                    taskId = await _client.RunNoContractCommandAsync(
                        CommandMetaData.Default().WithTaskTrace(
                            chkReturnTaskTrace.Checked
                        ).WithProgressCaching(
                            chkCacheProgressInfo.Checked
                        ).WithResponseHandler(
                            context =>
                            {
                                if (context.HyperNodeResponse.TaskTrace?.Any() ?? false)
                                {
                                    foreach (var activityItem in context.HyperNodeResponse.TaskTrace)
                                    {
                                        builder.AppendLine(activityItem.EventDescription);
                                    }
                                }
                            }
                        )
                    );
                }

                MessageBox.Show(builder.Insert(0, $"Task ID: {taskId}\r\n\r\n").ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAsyncCommandRequestOnly_Click(object sender, EventArgs e)
        {
            try
            {
                string? taskId = null;
                var builder = new StringBuilder();

                if (_client != null)
                {
                    taskId = await _client.RunLongRunningCommandAsync(
                        new LongRunningCommandRequest().WithTaskTrace(
                            chkReturnTaskTrace.Checked
                        ).WithProgressCaching(
                            chkCacheProgressInfo.Checked
                        ).WithResponseHandler(
                            context =>
                            {
                                if (context.HyperNodeResponse.TaskTrace?.Any() ?? false)
                                {
                                    foreach (var activityItem in context.HyperNodeResponse.TaskTrace)
                                    {
                                        builder.AppendLine(activityItem.EventDescription);
                                    }
                                }
                            }
                        )
                    );
                }

                MessageBox.Show(builder.Insert(0, $"Task ID: {taskId}\r\n\r\n").ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSyncCommandNoContracts_Click(object sender, EventArgs e)
        {
            try
            {
                string? taskId = null;
                var builder = new StringBuilder();

                if (_client != null)
                {
                    await _client.NoContractCommandAsync(
                        CommandMetaData.Default().WithTaskTrace(
                            chkReturnTaskTrace.Checked
                        ).WithProgressCaching(
                            chkCacheProgressInfo.Checked
                        ).WithResponseHandler(
                            context =>
                            {
                                taskId = context.HyperNodeResponse.TaskId;

                                if (context.HyperNodeResponse.TaskTrace?.Any() ?? false)
                                {
                                    foreach (var activityItem in context.HyperNodeResponse.TaskTrace)
                                    {
                                        builder.AppendLine(activityItem.EventDescription);
                                    }
                                }
                            }
                        )
                    );
                }

                MessageBox.Show(builder.Insert(0, $"Task ID: {taskId}\r\n\r\n").ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSyncCommandRequestOnly_Click(object sender, EventArgs e)
        {
            try
            {
                string? taskId = null;
                var builder = new StringBuilder();

                if (_client != null)
                {
                    await _client.NoContractCommandAsync(
                        new EmptyCommandRequest().WithTaskTrace(
                            chkReturnTaskTrace.Checked
                        ).WithProgressCaching(
                            chkCacheProgressInfo.Checked
                        ).WithResponseHandler(
                            context =>
                            {
                                taskId = context.HyperNodeResponse.TaskId;

                                if (context.HyperNodeResponse.TaskTrace?.Any() ?? false)
                                {
                                    foreach (var activityItem in context.HyperNodeResponse.TaskTrace)
                                    {
                                        builder.AppendLine(activityItem.EventDescription);
                                    }
                                }
                            }
                        )
                    );
                }

                MessageBox.Show(builder.Insert(0, $"Task ID: {taskId}\r\n\r\n").ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSyncCommandResponseOnly_Click(object sender, EventArgs e)
        {

        }

        private async void btnSyncCommandRequestResponse_Click(object sender, EventArgs e)
        {
            try
            {
                string? taskId = null;
                var builder = new StringBuilder();

                if (_client != null)
                {
                    var commandResponse = await _client.ComplexCommandAsync(
                        new ComplexCommandRequest
                        {
                            MyDateTime = DateTime.Now,
                            MyInt32 = 23,
                            MyString = "hello",
                            MyTimeSpan = TimeSpan.FromMinutes(3)
                        }.WithTaskTrace(
                            chkReturnTaskTrace.Checked
                        ).WithProgressCaching(
                            chkCacheProgressInfo.Checked
                        ).WithResponseHandler(
                            context =>
                            {
                                taskId = context.HyperNodeResponse.TaskId;

                                if (context.HyperNodeResponse.TaskTrace?.Any() ?? false)
                                {
                                    foreach (var activityItem in context.HyperNodeResponse.TaskTrace)
                                    {
                                        builder.AppendLine(activityItem.EventDescription);
                                    }
                                }
                            }
                        )
                    );
                
                    MessageBox.Show(commandResponse.ResponseStringNotTheSame ?? "<null>", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                MessageBox.Show(builder.Insert(0, $"Task ID: {taskId}\r\n\r\n").ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Events

        #region Private Methods

        private async Task RefreshNodeStatus()
        {
            GetNodeStatusResponse? nodeStatus = null;

            if (_client != null)
                nodeStatus = await _client.AsRemoteAdminClient().GetNodeStatusAsync(new GetNodeStatusRequest());

            if (nodeStatus?.Commands?.Any(c => c.CommandType == HyperNodeCommandType.Custom) ?? false)
            {
                // Success
            }
        }

        #endregion Private Methods
    }
}

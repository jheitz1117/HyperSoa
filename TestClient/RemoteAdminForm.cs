using HyperSoa.Client.Extensions;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.RemoteAdminClient;
using TestClient.ViewModels;

namespace TestClient
{
    public partial class RemoteAdminForm : Form
    {
        private bool _refreshingNodeStatus;

        private RemoteAdminHyperNodeClient? _client;

        public RemoteAdminForm()
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

                _client = new RemoteAdminHyperNodeClient(
                    MainForm.ClientAgentName,
                    txtEndpointUri.Text
                );

                // Initial attempt to get live tasks
                await RefreshNodeStatus();

                cboEndpoints.Enabled = false;
                btnDisconnect.Enabled = true;
                pnlServiceDetails.Enabled = true;
                tmrRefreshNodeStatus.Enabled = true;
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

                // Other disconnect activities here
                tmrRefreshNodeStatus.Enabled = false;

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

        private async void tmrRefreshNodeStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                GetNodeStatusResponse? nodeStatus = null;

                if (_client != null)
                    nodeStatus = await _client.GetNodeStatusAsync(true);

                bsLiveTasks.DataSource = nodeStatus?.LiveTasks?.Select(
                    t => new LiveTaskStatusViewModel
                    {
                        CommandName = t.CommandName,
                        Elapsed = t.Elapsed,
                        TaskId = t.TaskId,
                        Status = t.IsCancellationRequested ? "Cancelling" : "Running"
                    }
                ).ToArray();
            }
            catch (Exception ex)
            {
                tmrRefreshNodeStatus.Enabled = false;
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void chkEnableDiagnostics_CheckedChanged(object sender, EventArgs e)
        {
            // Avoid an infinite loop
            if (!_refreshingNodeStatus)
            {
                try
                {
                    if (!(await EnableDiagnostics(chkEnableDiagnostics.Checked)))
                        MessageBox.Show($"Unable to {(chkEnableDiagnostics.Checked ? "enable" : "disable")} diagnostics.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    await RefreshNodeStatus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void chkEnableProgressCache_CheckedChanged(object sender, EventArgs e)
        {
            // Avoid an infinite loop
            if (!_refreshingNodeStatus)
            {
                try
                {
                    if (!(await EnableProgressCache(chkEnableProgressCache.Checked)))
                        MessageBox.Show($"Unable to {(chkEnableProgressCache.Checked ? "enable" : "disable")} progress cache.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    await RefreshNodeStatus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnRefreshNodeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                await RefreshNodeStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void grdCommands_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (sender is DataGridView gridView &&
                    e.RowIndex > -1 &&
                    gridView.Rows[e.RowIndex].DataBoundItem is CommandStatusViewModel command
                   )
                {
                    var currentColumn = gridView.Columns[e.ColumnIndex];
                    var checkboxColumn = currentColumn as DataGridViewCheckBoxColumn;

                    if (checkboxColumn == enabledDataGridViewCheckBoxColumn)
                    {
                        if (!(await EnableCommand(command.CommandName, !command.Enabled)))
                            MessageBox.Show($"Unable to {(command.Enabled ? "disable" : "enable")} command.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        await RefreshNodeStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void grdLiveTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (sender is DataGridView gridView &&
                    e.RowIndex > -1 &&
                    gridView.Rows[e.RowIndex].DataBoundItem is LiveTaskStatusViewModel liveTask
                   )
                {
                    var currentColumn = gridView.Columns[e.ColumnIndex];
                    var buttonColumn = currentColumn as DataGridViewButtonColumn;

                    if (buttonColumn == CancelDataGridViewButtonColumn)
                    {
                        if (!(await CancelTask(liveTask.TaskId)))
                            MessageBox.Show("Unable to cancel task.");

                        await RefreshNodeStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEcho_Click(object sender, EventArgs e)
        {
            try
            {
                var echoResponse = await _client.EchoAsync(
                    new EchoRequest
                    {
                        Prompt = "Hello!"
                    }.WithMetaData(
                        _client.ClientApplicationName,
                        chkReturnTaskTrace.Checked,
                        chkCacheProgressInfo.Checked
                    )
                ).ConfigureAwait(false);

                MessageBox.Show(echoResponse.Reply, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            _refreshingNodeStatus = true;

            GetNodeStatusResponse? nodeStatus = null;

            if (_client != null)
                nodeStatus = await _client.GetNodeStatusAsync(true);

            chkEnableDiagnostics.Checked = nodeStatus?.DiagnosticsEnabled ?? false;
            chkEnableProgressCache.Checked = nodeStatus?.TaskProgressCacheEnabled ?? false;

            bsCommands.DataSource = nodeStatus?.Commands?.Select(
                c => new CommandStatusViewModel
                {
                    CommandName = c.CommandName,
                    Enabled = c.Enabled,
                    CommandType = c.CommandType.ToString()
                }
            ).OrderBy(
                c => c.CommandType
            ).ToArray();

            bsLiveTasks.DataSource = nodeStatus?.LiveTasks?.Select(
                t => new LiveTaskStatusViewModel
                {
                    CommandName = t.CommandName,
                    Elapsed = t.Elapsed,
                    TaskId = t.TaskId,
                    Status = t.IsCancellationRequested ? "Cancelling" : "Running"
                }
            ).ToArray();

            _refreshingNodeStatus = false;
        }

        private async Task<bool> CancelTask(string? taskId)
        {
            var success = false;

            if (_client != null)
            {
                success = (
                    await _client.CancelTaskAsync(
                        new CancelTaskRequest
                        {
                            TaskId = taskId
                        }
                    )
                ).ProcessStatusFlags.HasFlag(
                    MessageProcessStatusFlags.Success
                );
            }

            return success;
        }

        private async Task<bool> EnableCommand(string? commandName, bool enable)
        {
            var success = false;

            if (_client != null)
            {
                success = (
                    await _client.EnableCommandAsync(
                        new EnableCommandModuleRequest
                        {
                            CommandName = commandName,
                            Enable = enable
                        }
                    )
                ).ProcessStatusFlags.HasFlag(
                    MessageProcessStatusFlags.Success
                );
            }

            return success;
        }

        private async Task<bool> EnableDiagnostics(bool enable)
        {
            var success = false;

            if (_client != null)
            {
                success = (
                    await _client.EnableDiagnosticsAsync(
                        new EnableDiagnosticsRequest
                        {
                            Enable = enable
                        }
                    )
                ).ProcessStatusFlags.HasFlag(
                    MessageProcessStatusFlags.Success
                );
            }

            return success;
        }

        private async Task<bool> EnableProgressCache(bool enable)
        {
            var success = false;

            if (_client != null)
            {
                success = (
                    await _client.EnableTaskProgressCacheAsync(
                        new EnableTaskProgressCacheRequest
                        {
                            Enable = enable
                        }
                    )
                ).ProcessStatusFlags.HasFlag(
                    MessageProcessStatusFlags.Success
                );
            }

            return success;
        }

        #endregion Private Methods
    }
}

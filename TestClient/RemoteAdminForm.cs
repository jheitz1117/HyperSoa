using HyperSoa.Client.Extensions;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.RemoteAdminClient;
using TestClient.ViewModels;

namespace TestClient
{
    public partial class RemoteAdminForm : Form
    {
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
                await RefreshLiveTasks();

                cboEndpoints.Enabled = false;
                btnDisconnect.Enabled = true;
                pnlServiceDetails.Enabled = true;
                tmrRefreshLiveTasks.Enabled = true;
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
                tmrRefreshLiveTasks.Enabled = false;

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

        private async void tmrRefreshLiveTasks_Tick(object sender, EventArgs e)
        {
            try
            {
                await RefreshLiveTasks();
            }
            catch (Exception ex)
            {
                tmrRefreshLiveTasks.Enabled = false;
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRefreshLiveTasks_Click(object sender, EventArgs e)
        {
            try
            {
                await RefreshLiveTasks();
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

        private async Task RefreshLiveTasks()
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

        #endregion Private Methods
    }
}

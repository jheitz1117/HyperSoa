using HyperSoa.Client.Extensions;
using HyperSoa.Contracts;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.RemoteAdminClient;

namespace TestClient
{
    public partial class RemoteAdminForm : Form
    {
        private readonly RemoteAdminHyperNodeClient _client;

        public RemoteAdminForm()
        {
            InitializeComponent();
        }

        public RemoteAdminForm(IHyperNodeService service)
            : this()
        {
            _client = new RemoteAdminHyperNodeClient(service);
        }

        #region Events

        private async void btnEcho_Click(object sender, EventArgs e)
        {
            try
            {
                var echoResponse = await RunEchoCommand(chkReturnTaskTrace.Checked, chkCacheProgressInfo.Checked);

                MessageBox.Show(echoResponse.Reply, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Events

        #region Private Methods
        
        private async Task<EchoResponse> RunEchoCommand(bool includeTaskTrace, bool cacheTaskProgress)
        {
            return await _client.WithTaskTrace(
                includeTaskTrace
            ).WithProgressCaching(
                cacheTaskProgress
            ).EchoAsync(
                new EchoRequest
                {
                    Prompt = "Hello!"
                }
            ).ConfigureAwait(false);
        }
        
        #endregion Private Methods
    }
}

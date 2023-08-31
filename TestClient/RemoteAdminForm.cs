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
    }
}

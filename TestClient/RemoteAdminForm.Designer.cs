namespace TestClient
{
    partial class RemoteAdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtEchoPrompt = new TextBox();
            lblEchoPrompt = new Label();
            btnEcho = new Button();
            chkCacheProgressInfo = new CheckBox();
            chkReturnTaskTrace = new CheckBox();
            SuspendLayout();
            // 
            // txtEchoPrompt
            // 
            txtEchoPrompt.Location = new Point(133, 102);
            txtEchoPrompt.Name = "txtEchoPrompt";
            txtEchoPrompt.Size = new Size(100, 23);
            txtEchoPrompt.TabIndex = 0;
            txtEchoPrompt.Text = "Hello!";
            // 
            // lblEchoPrompt
            // 
            lblEchoPrompt.AutoSize = true;
            lblEchoPrompt.Location = new Point(77, 105);
            lblEchoPrompt.Name = "lblEchoPrompt";
            lblEchoPrompt.Size = new Size(50, 15);
            lblEchoPrompt.TabIndex = 1;
            lblEchoPrompt.Text = "Prompt:";
            // 
            // btnEcho
            // 
            btnEcho.Location = new Point(239, 102);
            btnEcho.Name = "btnEcho";
            btnEcho.Size = new Size(75, 23);
            btnEcho.TabIndex = 2;
            btnEcho.Text = "Echo";
            btnEcho.UseVisualStyleBackColor = true;
            btnEcho.Click += btnEcho_Click;
            // 
            // chkCacheProgressInfo
            // 
            chkCacheProgressInfo.AutoSize = true;
            chkCacheProgressInfo.Location = new Point(77, 163);
            chkCacheProgressInfo.Name = "chkCacheProgressInfo";
            chkCacheProgressInfo.Size = new Size(131, 19);
            chkCacheProgressInfo.TabIndex = 9;
            chkCacheProgressInfo.Text = "Cache Progress Info";
            chkCacheProgressInfo.UseVisualStyleBackColor = true;
            // 
            // chkReturnTaskTrace
            // 
            chkReturnTaskTrace.AutoSize = true;
            chkReturnTaskTrace.Location = new Point(77, 144);
            chkReturnTaskTrace.Name = "chkReturnTaskTrace";
            chkReturnTaskTrace.Size = new Size(116, 19);
            chkReturnTaskTrace.TabIndex = 8;
            chkReturnTaskTrace.Text = "Return Task Trace";
            chkReturnTaskTrace.UseVisualStyleBackColor = true;
            // 
            // RemoteAdminForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(chkCacheProgressInfo);
            Controls.Add(chkReturnTaskTrace);
            Controls.Add(btnEcho);
            Controls.Add(lblEchoPrompt);
            Controls.Add(txtEchoPrompt);
            Name = "RemoteAdminForm";
            Text = "Remote Administration";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtEchoPrompt;
        private Label lblEchoPrompt;
        private Button btnEcho;
        private CheckBox chkCacheProgressInfo;
        private CheckBox chkReturnTaskTrace;
    }
}
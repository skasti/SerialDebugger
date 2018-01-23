namespace SerialDebugger
{
    partial class MainForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.settingsPanel = new System.Windows.Forms.GroupBox();
            this.spASelectorLabel = new System.Windows.Forms.Label();
            this.spASelector = new System.Windows.Forms.ComboBox();
            this.spARefresh = new System.Windows.Forms.Button();
            this.spBRefresh = new System.Windows.Forms.Button();
            this.spBSelector = new System.Windows.Forms.ComboBox();
            this.spBSelectorLabel = new System.Windows.Forms.Label();
            this.eolLabel = new System.Windows.Forms.Label();
            this.eolInput = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.spTextLog = new System.Windows.Forms.RichTextBox();
            this.spHexLog = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(371, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.spTextLog);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.spHexLog);
            this.splitContainer1.Size = new System.Drawing.Size(1071, 942);
            this.splitContainer1.SplitterDistance = 502;
            this.splitContainer1.TabIndex = 0;
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.startButton);
            this.settingsPanel.Controls.Add(this.eolInput);
            this.settingsPanel.Controls.Add(this.eolLabel);
            this.settingsPanel.Controls.Add(this.spBRefresh);
            this.settingsPanel.Controls.Add(this.spBSelector);
            this.settingsPanel.Controls.Add(this.spBSelectorLabel);
            this.settingsPanel.Controls.Add(this.spARefresh);
            this.settingsPanel.Controls.Add(this.spASelector);
            this.settingsPanel.Controls.Add(this.spASelectorLabel);
            this.settingsPanel.Location = new System.Drawing.Point(12, 12);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(343, 292);
            this.settingsPanel.TabIndex = 1;
            this.settingsPanel.TabStop = false;
            this.settingsPanel.Text = "Connection Settings";
            // 
            // spASelectorLabel
            // 
            this.spASelectorLabel.AutoSize = true;
            this.spASelectorLabel.Location = new System.Drawing.Point(6, 46);
            this.spASelectorLabel.Name = "spASelectorLabel";
            this.spASelectorLabel.Size = new System.Drawing.Size(101, 20);
            this.spASelectorLabel.TabIndex = 0;
            this.spASelectorLabel.Text = "Serial Port A:";
            // 
            // spASelector
            // 
            this.spASelector.FormattingEnabled = true;
            this.spASelector.Location = new System.Drawing.Point(6, 69);
            this.spASelector.Name = "spASelector";
            this.spASelector.Size = new System.Drawing.Size(290, 28);
            this.spASelector.TabIndex = 1;
            // 
            // spARefresh
            // 
            this.spARefresh.Location = new System.Drawing.Point(302, 69);
            this.spARefresh.Name = "spARefresh";
            this.spARefresh.Size = new System.Drawing.Size(35, 38);
            this.spARefresh.TabIndex = 2;
            this.spARefresh.Text = "U";
            this.spARefresh.UseVisualStyleBackColor = true;
            this.spARefresh.Click += new System.EventHandler(this.spARefresh_Click);
            // 
            // spBRefresh
            // 
            this.spBRefresh.Location = new System.Drawing.Point(302, 138);
            this.spBRefresh.Name = "spBRefresh";
            this.spBRefresh.Size = new System.Drawing.Size(35, 39);
            this.spBRefresh.TabIndex = 5;
            this.spBRefresh.Text = "U";
            this.spBRefresh.UseVisualStyleBackColor = true;
            this.spBRefresh.Click += new System.EventHandler(this.spBRefresh_Click);
            // 
            // spBSelector
            // 
            this.spBSelector.FormattingEnabled = true;
            this.spBSelector.Location = new System.Drawing.Point(6, 138);
            this.spBSelector.Name = "spBSelector";
            this.spBSelector.Size = new System.Drawing.Size(290, 28);
            this.spBSelector.TabIndex = 4;
            // 
            // spBSelectorLabel
            // 
            this.spBSelectorLabel.AutoSize = true;
            this.spBSelectorLabel.Location = new System.Drawing.Point(6, 115);
            this.spBSelectorLabel.Name = "spBSelectorLabel";
            this.spBSelectorLabel.Size = new System.Drawing.Size(101, 20);
            this.spBSelectorLabel.TabIndex = 3;
            this.spBSelectorLabel.Text = "Serial Port A:";
            // 
            // eolLabel
            // 
            this.eolLabel.AutoSize = true;
            this.eolLabel.Location = new System.Drawing.Point(6, 184);
            this.eolLabel.Name = "eolLabel";
            this.eolLabel.Size = new System.Drawing.Size(122, 20);
            this.eolLabel.TabIndex = 6;
            this.eolLabel.Text = "EOL Sequence:";
            // 
            // eolInput
            // 
            this.eolInput.Location = new System.Drawing.Point(6, 207);
            this.eolInput.Name = "eolInput";
            this.eolInput.Size = new System.Drawing.Size(331, 26);
            this.eolInput.TabIndex = 7;
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startButton.Location = new System.Drawing.Point(6, 258);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 28);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // spTextLog
            // 
            this.spTextLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spTextLog.Location = new System.Drawing.Point(0, 0);
            this.spTextLog.Name = "spTextLog";
            this.spTextLog.Size = new System.Drawing.Size(502, 942);
            this.spTextLog.TabIndex = 0;
            this.spTextLog.Text = "";
            // 
            // spHexLog
            // 
            this.spHexLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spHexLog.Location = new System.Drawing.Point(0, 0);
            this.spHexLog.Name = "spHexLog";
            this.spHexLog.Size = new System.Drawing.Size(565, 942);
            this.spHexLog.TabIndex = 0;
            this.spHexLog.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1454, 966);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Serial Debugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort spA;
        private System.IO.Ports.SerialPort spB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox settingsPanel;
        private System.Windows.Forms.Button spARefresh;
        private System.Windows.Forms.ComboBox spASelector;
        private System.Windows.Forms.Label spASelectorLabel;
        private System.Windows.Forms.Button spBRefresh;
        private System.Windows.Forms.ComboBox spBSelector;
        private System.Windows.Forms.Label spBSelectorLabel;
        private System.Windows.Forms.Label eolLabel;
        private System.Windows.Forms.TextBox eolInput;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.RichTextBox spTextLog;
        private System.Windows.Forms.RichTextBox spHexLog;
    }
}


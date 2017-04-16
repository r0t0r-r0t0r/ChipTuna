namespace ChipTuna.FmPlayer
{
    partial class VgmPlayerForm
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
            this.stopButton = new System.Windows.Forms.Button();
            this.dropVgmPanel = new System.Windows.Forms.Panel();
            this.dropVgmLabel = new System.Windows.Forms.Label();
            this.dropVgmPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // stopButton
            // 
            this.stopButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stopButton.Location = new System.Drawing.Point(0, 379);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(356, 23);
            this.stopButton.TabIndex = 7;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.OnStopButtonClick);
            // 
            // dropVgmPanel
            // 
            this.dropVgmPanel.AllowDrop = true;
            this.dropVgmPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dropVgmPanel.Controls.Add(this.dropVgmLabel);
            this.dropVgmPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dropVgmPanel.Location = new System.Drawing.Point(0, 0);
            this.dropVgmPanel.Name = "dropVgmPanel";
            this.dropVgmPanel.Size = new System.Drawing.Size(356, 379);
            this.dropVgmPanel.TabIndex = 8;
            this.dropVgmPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDropVgmPanelDragDrop);
            this.dropVgmPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDropVgmPanelDragEnter);
            // 
            // dropVgmLabel
            // 
            this.dropVgmLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dropVgmLabel.Location = new System.Drawing.Point(0, 0);
            this.dropVgmLabel.Name = "dropVgmLabel";
            this.dropVgmLabel.Size = new System.Drawing.Size(354, 377);
            this.dropVgmLabel.TabIndex = 0;
            this.dropVgmLabel.Text = "Drop VGM File Here";
            this.dropVgmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VgmPlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 402);
            this.Controls.Add(this.dropVgmPanel);
            this.Controls.Add(this.stopButton);
            this.Name = "VgmPlayerForm";
            this.Text = "Vgm Player";
            this.dropVgmPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Panel dropVgmPanel;
        private System.Windows.Forms.Label dropVgmLabel;
    }
}


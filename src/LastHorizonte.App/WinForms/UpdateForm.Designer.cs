namespace LastHorizonte
{
	partial class UpdateForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
			this.linkLabel = new System.Windows.Forms.LinkLabel();
			this.noButton = new System.Windows.Forms.Button();
			this.yesButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.latestChangesTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// linkLabel
			// 
			this.linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabel.Location = new System.Drawing.Point(215, 171);
			this.linkLabel.Name = "linkLabel";
			this.linkLabel.Size = new System.Drawing.Size(64, 23);
			this.linkLabel.TabIndex = 7;
			this.linkLabel.TabStop = true;
			this.linkLabel.Text = "Ver más...";
			// 
			// noButton
			// 
			this.noButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.noButton.Location = new System.Drawing.Point(196, 197);
			this.noButton.Name = "noButton";
			this.noButton.Size = new System.Drawing.Size(75, 23);
			this.noButton.TabIndex = 6;
			this.noButton.Text = "No";
			this.noButton.Click += new System.EventHandler(this.noButton_Click);
			// 
			// yesButton
			// 
			this.yesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.yesButton.Location = new System.Drawing.Point(110, 197);
			this.yesButton.Name = "yesButton";
			this.yesButton.Size = new System.Drawing.Size(75, 23);
			this.yesButton.TabIndex = 5;
			this.yesButton.Text = "Sí";
			this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 32);
			this.label1.TabIndex = 4;
			this.label1.Text = "Hay una nueva versión disponible.\r\n¿Te gustaría descargarla ahora?";
			// 
			// latestChangesTextBox
			// 
			this.latestChangesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.latestChangesTextBox.Location = new System.Drawing.Point(12, 65);
			this.latestChangesTextBox.Multiline = true;
			this.latestChangesTextBox.Name = "latestChangesTextBox";
			this.latestChangesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.latestChangesTextBox.Size = new System.Drawing.Size(265, 103);
			this.latestChangesTextBox.TabIndex = 9;
			this.latestChangesTextBox.TextChanged += new System.EventHandler(this.latestChangesTextBox_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(9, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(154, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Últimos cambios:";
			// 
			// UpdateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(291, 226);
			this.Controls.Add(this.latestChangesTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.linkLabel);
			this.Controls.Add(this.noButton);
			this.Controls.Add(this.yesButton);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(207, 220);
			this.Name = "UpdateForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "UpdateForm";
			this.Load += new System.EventHandler(this.UpdateForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.LinkLabel linkLabel;
		private System.Windows.Forms.Button noButton;
		private System.Windows.Forms.Button yesButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox latestChangesTextBox;
		private System.Windows.Forms.Label label2;
	}
}
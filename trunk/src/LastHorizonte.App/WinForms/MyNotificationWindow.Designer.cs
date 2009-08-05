using System;
using System.Drawing;
using System.Windows.Forms;

namespace LastHorizonte.WinForms
{
	partial class MyNotificationWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.descriptionLabel = new LinkLabel();
			this.titleLabel = new Growl.DisplayStyle.ExpandingLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.BackColor = System.Drawing.Color.Transparent;
			this.descriptionLabel.Location = new System.Drawing.Point(9, 27);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(240, 32);
			this.descriptionLabel.TabIndex = 6;
			this.descriptionLabel.Text = "[description]";
			this.descriptionLabel.UseMnemonic = false;
			// 
			// titleLabel
			// 
			this.titleLabel.BackColor = System.Drawing.Color.Transparent;
			this.titleLabel.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleLabel.Location = new System.Drawing.Point(9, 9);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(240, 18);
			this.titleLabel.TabIndex = 5;
			this.titleLabel.Text = "[title]";
			this.titleLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
			this.titleLabel.UseMnemonic = false;
			this.titleLabel.LabelHeightChanged += new Growl.DisplayStyle.ExpandingLabel.LabelHeightChangedEventHandler(this.titleLabel_LabelHeightChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(9, 9);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(64, 64);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// MyNotificationWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(260, 61);
			this.Controls.Add(this.descriptionLabel);
			this.Controls.Add(this.titleLabel);
			this.Controls.Add(this.pictureBox1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "MyNotificationWindow";
			this.Text = "PlainWindow";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private LinkLabel descriptionLabel;
		private Growl.DisplayStyle.ExpandingLabel titleLabel;
		private PictureBox pictureBox1;
	}
}
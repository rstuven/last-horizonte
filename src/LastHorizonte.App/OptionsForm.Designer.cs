namespace LastHorizonte
{
	partial class OptionsForm
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.GroupBox groupBox2;
			System.Windows.Forms.GroupBox groupBox3;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
			this.usernameTextBox = new System.Windows.Forms.TextBox();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.rememberPasswordCheckBox = new System.Windows.Forms.CheckBox();
			this.notifySystemTrayCheckBox = new System.Windows.Forms.CheckBox();
			this.notifyLastFmCheckBox = new System.Windows.Forms.CheckBox();
			this.notifyMsnMessegerCheckBox = new System.Windows.Forms.CheckBox();
			this.startActivatedCheckBox = new System.Windows.Forms.CheckBox();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.activatedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.activatedCheckBox = new System.Windows.Forms.CheckBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.startOnWindowsSessionCheckBox = new System.Windows.Forms.CheckBox();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			groupBox2 = new System.Windows.Forms.GroupBox();
			groupBox3 = new System.Windows.Forms.GroupBox();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			groupBox3.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(this.usernameTextBox);
			groupBox1.Controls.Add(this.passwordTextBox);
			groupBox1.Controls.Add(this.rememberPasswordCheckBox);
			groupBox1.Location = new System.Drawing.Point(12, 178);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(325, 101);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "Cuenta de Last.fm";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(18, 49);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(64, 13);
			label2.TabIndex = 4;
			label2.Text = "Contraseña:";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(18, 26);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(99, 13);
			label1.TabIndex = 2;
			label1.Text = "Nombre de usuario:";
			// 
			// usernameTextBox
			// 
			this.usernameTextBox.Location = new System.Drawing.Point(123, 23);
			this.usernameTextBox.Name = "usernameTextBox";
			this.usernameTextBox.Size = new System.Drawing.Size(164, 20);
			this.usernameTextBox.TabIndex = 3;
			this.usernameTextBox.TextChanged += new System.EventHandler(this.usernameTextBox_TextChanged);
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Location = new System.Drawing.Point(123, 46);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.PasswordChar = '*';
			this.passwordTextBox.Size = new System.Drawing.Size(164, 20);
			this.passwordTextBox.TabIndex = 5;
			this.passwordTextBox.TextChanged += new System.EventHandler(this.passwordTextBox_TextChanged);
			// 
			// rememberPasswordCheckBox
			// 
			this.rememberPasswordCheckBox.AutoSize = true;
			this.rememberPasswordCheckBox.Location = new System.Drawing.Point(123, 72);
			this.rememberPasswordCheckBox.Name = "rememberPasswordCheckBox";
			this.rememberPasswordCheckBox.Size = new System.Drawing.Size(139, 17);
			this.rememberPasswordCheckBox.TabIndex = 6;
			this.rememberPasswordCheckBox.Text = "Recordar mi contraseña";
			this.rememberPasswordCheckBox.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			groupBox2.Controls.Add(this.notifySystemTrayCheckBox);
			groupBox2.Controls.Add(this.notifyLastFmCheckBox);
			groupBox2.Controls.Add(this.notifyMsnMessegerCheckBox);
			groupBox2.Location = new System.Drawing.Point(12, 285);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(325, 93);
			groupBox2.TabIndex = 1;
			groupBox2.TabStop = false;
			groupBox2.Text = "Cuando empiece un nuevo tema...";
			// 
			// notifySystemTrayCheckBox
			// 
			this.notifySystemTrayCheckBox.AutoSize = true;
			this.notifySystemTrayCheckBox.Location = new System.Drawing.Point(21, 67);
			this.notifySystemTrayCheckBox.Name = "notifySystemTrayCheckBox";
			this.notifySystemTrayCheckBox.Size = new System.Drawing.Size(123, 17);
			this.notifySystemTrayCheckBox.TabIndex = 5;
			this.notifySystemTrayCheckBox.Text = "Notificar por pantalla";
			this.notifySystemTrayCheckBox.UseVisualStyleBackColor = true;
			// 
			// notifyLastFmCheckBox
			// 
			this.notifyLastFmCheckBox.AutoSize = true;
			this.notifyLastFmCheckBox.Location = new System.Drawing.Point(21, 20);
			this.notifyLastFmCheckBox.Name = "notifyLastFmCheckBox";
			this.notifyLastFmCheckBox.Size = new System.Drawing.Size(111, 17);
			this.notifyLastFmCheckBox.TabIndex = 3;
			this.notifyLastFmCheckBox.Text = "Notificar a Last.fm";
			this.notifyLastFmCheckBox.UseVisualStyleBackColor = true;
			// 
			// notifyMsnMessegerCheckBox
			// 
			this.notifyMsnMessegerCheckBox.AutoSize = true;
			this.notifyMsnMessegerCheckBox.Location = new System.Drawing.Point(21, 44);
			this.notifyMsnMessegerCheckBox.Name = "notifyMsnMessegerCheckBox";
			this.notifyMsnMessegerCheckBox.Size = new System.Drawing.Size(219, 17);
			this.notifyMsnMessegerCheckBox.TabIndex = 3;
			this.notifyMsnMessegerCheckBox.Text = "Notificar a Messenger (sólo en Windows)";
			this.notifyMsnMessegerCheckBox.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			groupBox3.Controls.Add(this.startOnWindowsSessionCheckBox);
			groupBox3.Controls.Add(this.startActivatedCheckBox);
			groupBox3.Location = new System.Drawing.Point(13, 384);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(324, 72);
			groupBox3.TabIndex = 2;
			groupBox3.TabStop = false;
			groupBox3.Text = "Inicio";
			// 
			// startActivatedCheckBox
			// 
			this.startActivatedCheckBox.AutoSize = true;
			this.startActivatedCheckBox.Location = new System.Drawing.Point(20, 19);
			this.startActivatedCheckBox.Name = "startActivatedCheckBox";
			this.startActivatedCheckBox.Size = new System.Drawing.Size(148, 17);
			this.startActivatedCheckBox.TabIndex = 0;
			this.startActivatedCheckBox.Text = "Activar scrobbling al inicio";
			this.startActivatedCheckBox.UseVisualStyleBackColor = true;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activatedToolStripMenuItem,
            this.openProfileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(179, 120);
			this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
			// 
			// activatedToolStripMenuItem
			// 
			this.activatedToolStripMenuItem.CheckOnClick = true;
			this.activatedToolStripMenuItem.Name = "activatedToolStripMenuItem";
			this.activatedToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.activatedToolStripMenuItem.Text = "Scrobbling activado";
			this.activatedToolStripMenuItem.Click += new System.EventHandler(this.activatedToolStripMenuItem_Click);
			// 
			// openProfileToolStripMenuItem
			// 
			this.openProfileToolStripMenuItem.Name = "openProfileToolStripMenuItem";
			this.openProfileToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.openProfileToolStripMenuItem.Text = "Abrir perfil en Last.fm";
			this.openProfileToolStripMenuItem.Click += new System.EventHandler(this.openProfileToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.optionsToolStripMenuItem.Text = "Opciones...";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.aboutToolStripMenuItem.Text = "Acerca de...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.exitToolStripMenuItem.Text = "Salir";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.Location = new System.Drawing.Point(180, 467);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(77, 28);
			this.acceptButton.TabIndex = 4;
			this.acceptButton.Text = "Aceptar";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(263, 467);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(74, 28);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancelar";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// activatedCheckBox
			// 
			this.activatedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.activatedCheckBox.AutoSize = true;
			this.activatedCheckBox.Location = new System.Drawing.Point(14, 471);
			this.activatedCheckBox.Name = "activatedCheckBox";
			this.activatedCheckBox.Size = new System.Drawing.Size(120, 17);
			this.activatedCheckBox.TabIndex = 3;
			this.activatedCheckBox.Text = "Scrobbling activado";
			this.activatedCheckBox.UseVisualStyleBackColor = true;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// startOnWindowsSessionCheckBox
			// 
			this.startOnWindowsSessionCheckBox.AutoSize = true;
			this.startOnWindowsSessionCheckBox.Location = new System.Drawing.Point(20, 43);
			this.startOnWindowsSessionCheckBox.Name = "startOnWindowsSessionCheckBox";
			this.startOnWindowsSessionCheckBox.Size = new System.Drawing.Size(265, 17);
			this.startOnWindowsSessionCheckBox.TabIndex = 1;
			this.startOnWindowsSessionCheckBox.Text = "Iniciar automáticamente con la sesión de Windows";
			this.startOnWindowsSessionCheckBox.UseVisualStyleBackColor = true;
			// 
			// OptionsForm
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::LastHorizonte.Properties.Resources.splash;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(349, 504);
			this.Controls.Add(groupBox1);
			this.Controls.Add(groupBox3);
			this.Controls.Add(groupBox2);
			this.Controls.Add(this.activatedCheckBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.acceptButton);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "OptionsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Opciones";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			this.contextMenuStrip.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripMenuItem activatedToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		public System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.TextBox usernameTextBox;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.CheckBox rememberPasswordCheckBox;
		private System.Windows.Forms.CheckBox notifyMsnMessegerCheckBox;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.CheckBox notifyLastFmCheckBox;
		private System.Windows.Forms.CheckBox notifySystemTrayCheckBox;
		private System.Windows.Forms.CheckBox activatedCheckBox;
		private System.Windows.Forms.CheckBox startActivatedCheckBox;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openProfileToolStripMenuItem;
		private System.Windows.Forms.CheckBox startOnWindowsSessionCheckBox;
	}
}
namespace SMLEdit
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.TilePaletteDisplay = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSMLROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.LevelDisplay = new System.Windows.Forms.FlowLayoutPanel();
            this.oscar = new System.Windows.Forms.PictureBox();
            this.TilePaletteDisplay.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.oscar)).BeginInit();
            this.SuspendLayout();
            // 
            // TilePaletteDisplay
            // 
            this.TilePaletteDisplay.AutoScroll = true;
            this.TilePaletteDisplay.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TilePaletteDisplay.Controls.Add(this.panel1);
            this.TilePaletteDisplay.Dock = System.Windows.Forms.DockStyle.Right;
            this.TilePaletteDisplay.Location = new System.Drawing.Point(941, 24);
            this.TilePaletteDisplay.Name = "TilePaletteDisplay";
            this.TilePaletteDisplay.Padding = new System.Windows.Forms.Padding(4);
            this.TilePaletteDisplay.Size = new System.Drawing.Size(312, 621);
            this.TilePaletteDisplay.TabIndex = 2;
            this.TilePaletteDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 16);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(272, 147);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 379);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Level";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(42, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(41, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.comboBox1);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(941, 29);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSMLROMToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "file";
            // 
            // openSMLROMToolStripMenuItem
            // 
            this.openSMLROMToolStripMenuItem.Name = "openSMLROMToolStripMenuItem";
            this.openSMLROMToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.openSMLROMToolStripMenuItem.Text = "open SML-ROM";
            this.openSMLROMToolStripMenuItem.Click += new System.EventHandler(this.openSMLROMToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1253, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // LevelDisplay
            // 
            this.LevelDisplay.AutoScroll = true;
            this.LevelDisplay.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.LevelDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.LevelDisplay.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.LevelDisplay.Location = new System.Drawing.Point(0, 53);
            this.LevelDisplay.Name = "LevelDisplay";
            this.LevelDisplay.Size = new System.Drawing.Size(941, 293);
            this.LevelDisplay.TabIndex = 4;
            this.LevelDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.TileMatrix_Paint);
            // 
            // oscar
            // 
            this.oscar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.oscar.Location = new System.Drawing.Point(87, 379);
            this.oscar.Name = "oscar";
            this.oscar.Size = new System.Drawing.Size(430, 217);
            this.oscar.TabIndex = 5;
            this.oscar.TabStop = false;
            this.oscar.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1253, 645);
            this.Controls.Add(this.oscar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LevelDisplay);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.TilePaletteDisplay);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TilePaletteDisplay.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.oscar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSMLROMToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.FlowLayoutPanel TilePaletteDisplay;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel LevelDisplay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox oscar;
    }
}


namespace IDDTool
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.LstItems = new System.Windows.Forms.ListBox();
            this.TexturePanel = new System.Windows.Forms.Panel();
            this.TextureImg = new System.Windows.Forms.PictureBox();
            this.LstTextures = new System.Windows.Forms.ListBox();
            this.ControlsPanel = new System.Windows.Forms.Panel();
            this.BtnImportDDS = new System.Windows.Forms.Button();
            this.BtnImportPNG = new System.Windows.Forms.Button();
            this.BtnExportDDS = new System.Windows.Forms.Button();
            this.BtnExportPNG = new System.Windows.Forms.Button();
            this.TextBoxesPanel = new System.Windows.Forms.Panel();
            this.NUDH = new System.Windows.Forms.NumericUpDown();
            this.NUDW = new System.Windows.Forms.NumericUpDown();
            this.NUDY = new System.Windows.Forms.NumericUpDown();
            this.NUDX = new System.Windows.Forms.NumericUpDown();
            this.LblH = new System.Windows.Forms.Label();
            this.LblW = new System.Windows.Forms.Label();
            this.LblY = new System.Windows.Forms.Label();
            this.LblX = new System.Windows.Forms.Label();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.MainLayout.SuspendLayout();
            this.TexturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextureImg)).BeginInit();
            this.ControlsPanel.SuspendLayout();
            this.TextBoxesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDX)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 3;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Controls.Add(this.LstItems, 1, 0);
            this.MainLayout.Controls.Add(this.TexturePanel, 2, 1);
            this.MainLayout.Controls.Add(this.LstTextures, 0, 0);
            this.MainLayout.Controls.Add(this.ControlsPanel, 2, 0);
            this.MainLayout.Controls.Add(this.TextBoxesPanel, 2, 2);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 24);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 3;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainLayout.Size = new System.Drawing.Size(624, 417);
            this.MainLayout.TabIndex = 0;
            // 
            // LstItems
            // 
            this.LstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LstItems.FormattingEnabled = true;
            this.LstItems.Location = new System.Drawing.Point(131, 3);
            this.LstItems.Name = "LstItems";
            this.MainLayout.SetRowSpan(this.LstItems, 3);
            this.LstItems.Size = new System.Drawing.Size(122, 411);
            this.LstItems.TabIndex = 2;
            this.LstItems.SelectedIndexChanged += new System.EventHandler(this.LstItems_SelectedIndexChanged);
            // 
            // TexturePanel
            // 
            this.TexturePanel.AutoScroll = true;
            this.TexturePanel.Controls.Add(this.TextureImg);
            this.TexturePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TexturePanel.Location = new System.Drawing.Point(259, 33);
            this.TexturePanel.Name = "TexturePanel";
            this.TexturePanel.Size = new System.Drawing.Size(362, 351);
            this.TexturePanel.TabIndex = 0;
            // 
            // TextureImg
            // 
            this.TextureImg.Location = new System.Drawing.Point(0, 0);
            this.TextureImg.Name = "TextureImg";
            this.TextureImg.Size = new System.Drawing.Size(64, 64);
            this.TextureImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.TextureImg.TabIndex = 0;
            this.TextureImg.TabStop = false;
            this.TextureImg.Paint += new System.Windows.Forms.PaintEventHandler(this.TextureImg_Paint);
            this.TextureImg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextureImg_MouseDown);
            this.TextureImg.MouseLeave += new System.EventHandler(this.TextureImg_MouseLeave);
            this.TextureImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TextureImg_MouseMove);
            this.TextureImg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TextureImg_MouseUp);
            // 
            // LstTextures
            // 
            this.LstTextures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LstTextures.FormattingEnabled = true;
            this.LstTextures.Location = new System.Drawing.Point(3, 3);
            this.LstTextures.Name = "LstTextures";
            this.MainLayout.SetRowSpan(this.LstTextures, 3);
            this.LstTextures.Size = new System.Drawing.Size(122, 411);
            this.LstTextures.TabIndex = 1;
            this.LstTextures.SelectedIndexChanged += new System.EventHandler(this.LstTextures_SelectedIndexChanged);
            // 
            // ControlsPanel
            // 
            this.ControlsPanel.Controls.Add(this.BtnImportDDS);
            this.ControlsPanel.Controls.Add(this.BtnImportPNG);
            this.ControlsPanel.Controls.Add(this.BtnExportDDS);
            this.ControlsPanel.Controls.Add(this.BtnExportPNG);
            this.ControlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlsPanel.Location = new System.Drawing.Point(256, 0);
            this.ControlsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ControlsPanel.Name = "ControlsPanel";
            this.ControlsPanel.Size = new System.Drawing.Size(368, 30);
            this.ControlsPanel.TabIndex = 3;
            // 
            // BtnImportDDS
            // 
            this.BtnImportDDS.Location = new System.Drawing.Point(266, 3);
            this.BtnImportDDS.Name = "BtnImportDDS";
            this.BtnImportDDS.Size = new System.Drawing.Size(80, 24);
            this.BtnImportDDS.TabIndex = 3;
            this.BtnImportDDS.Text = "&Import DDS";
            this.BtnImportDDS.UseVisualStyleBackColor = true;
            this.BtnImportDDS.Click += new System.EventHandler(this.BtnImportDDS_Click);
            // 
            // BtnImportPNG
            // 
            this.BtnImportPNG.Location = new System.Drawing.Point(180, 3);
            this.BtnImportPNG.Name = "BtnImportPNG";
            this.BtnImportPNG.Size = new System.Drawing.Size(80, 24);
            this.BtnImportPNG.TabIndex = 2;
            this.BtnImportPNG.Text = "&Import PNG";
            this.BtnImportPNG.UseVisualStyleBackColor = true;
            this.BtnImportPNG.Click += new System.EventHandler(this.BtnImportPNG_Click);
            // 
            // BtnExportDDS
            // 
            this.BtnExportDDS.Location = new System.Drawing.Point(89, 3);
            this.BtnExportDDS.Name = "BtnExportDDS";
            this.BtnExportDDS.Size = new System.Drawing.Size(80, 24);
            this.BtnExportDDS.TabIndex = 1;
            this.BtnExportDDS.Text = "&Export DDS";
            this.BtnExportDDS.UseVisualStyleBackColor = true;
            this.BtnExportDDS.Click += new System.EventHandler(this.BtnExportDDS_Click);
            // 
            // BtnExportPNG
            // 
            this.BtnExportPNG.Location = new System.Drawing.Point(3, 3);
            this.BtnExportPNG.Name = "BtnExportPNG";
            this.BtnExportPNG.Size = new System.Drawing.Size(80, 24);
            this.BtnExportPNG.TabIndex = 0;
            this.BtnExportPNG.Text = "&Export PNG";
            this.BtnExportPNG.UseVisualStyleBackColor = true;
            this.BtnExportPNG.Click += new System.EventHandler(this.BtnExportPNG_Click);
            // 
            // TextBoxesPanel
            // 
            this.TextBoxesPanel.Controls.Add(this.NUDH);
            this.TextBoxesPanel.Controls.Add(this.NUDW);
            this.TextBoxesPanel.Controls.Add(this.NUDY);
            this.TextBoxesPanel.Controls.Add(this.NUDX);
            this.TextBoxesPanel.Controls.Add(this.LblH);
            this.TextBoxesPanel.Controls.Add(this.LblW);
            this.TextBoxesPanel.Controls.Add(this.LblY);
            this.TextBoxesPanel.Controls.Add(this.LblX);
            this.TextBoxesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxesPanel.Location = new System.Drawing.Point(256, 387);
            this.TextBoxesPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TextBoxesPanel.Name = "TextBoxesPanel";
            this.TextBoxesPanel.Size = new System.Drawing.Size(368, 30);
            this.TextBoxesPanel.TabIndex = 4;
            // 
            // NUDH
            // 
            this.NUDH.Location = new System.Drawing.Point(259, 3);
            this.NUDH.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.NUDH.Name = "NUDH";
            this.NUDH.Size = new System.Drawing.Size(48, 22);
            this.NUDH.TabIndex = 11;
            this.NUDH.ValueChanged += new System.EventHandler(this.NUD_ValueChanged);
            // 
            // NUDW
            // 
            this.NUDW.Location = new System.Drawing.Point(181, 3);
            this.NUDW.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.NUDW.Name = "NUDW";
            this.NUDW.Size = new System.Drawing.Size(48, 22);
            this.NUDW.TabIndex = 10;
            this.NUDW.ValueChanged += new System.EventHandler(this.NUD_ValueChanged);
            // 
            // NUDY
            // 
            this.NUDY.Location = new System.Drawing.Point(100, 3);
            this.NUDY.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.NUDY.Minimum = new decimal(new int[] {
            16384,
            0,
            0,
            -2147483648});
            this.NUDY.Name = "NUDY";
            this.NUDY.Size = new System.Drawing.Size(48, 22);
            this.NUDY.TabIndex = 9;
            this.NUDY.ValueChanged += new System.EventHandler(this.NUD_ValueChanged);
            // 
            // NUDX
            // 
            this.NUDX.Location = new System.Drawing.Point(25, 4);
            this.NUDX.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.NUDX.Minimum = new decimal(new int[] {
            16384,
            0,
            0,
            -2147483648});
            this.NUDX.Name = "NUDX";
            this.NUDX.Size = new System.Drawing.Size(48, 22);
            this.NUDX.TabIndex = 8;
            this.NUDX.ValueChanged += new System.EventHandler(this.NUD_ValueChanged);
            // 
            // LblH
            // 
            this.LblH.AutoSize = true;
            this.LblH.Location = new System.Drawing.Point(235, 6);
            this.LblH.Name = "LblH";
            this.LblH.Size = new System.Drawing.Size(18, 13);
            this.LblH.TabIndex = 7;
            this.LblH.Text = "H:";
            // 
            // LblW
            // 
            this.LblW.AutoSize = true;
            this.LblW.Location = new System.Drawing.Point(154, 6);
            this.LblW.Name = "LblW";
            this.LblW.Size = new System.Drawing.Size(21, 13);
            this.LblW.TabIndex = 6;
            this.LblW.Text = "W:";
            // 
            // LblY
            // 
            this.LblY.AutoSize = true;
            this.LblY.Location = new System.Drawing.Point(79, 6);
            this.LblY.Name = "LblY";
            this.LblY.Size = new System.Drawing.Size(15, 13);
            this.LblY.TabIndex = 3;
            this.LblY.Text = "Y:";
            // 
            // LblX
            // 
            this.LblX.AutoSize = true;
            this.LblX.Location = new System.Drawing.Point(3, 6);
            this.LblX.Name = "LblX";
            this.LblX.Size = new System.Drawing.Size(16, 13);
            this.LblX.TabIndex = 2;
            this.LblX.Text = "X:";
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuHelp});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(624, 24);
            this.MainMenu.TabIndex = 1;
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpen,
            this.MenuSave,
            this.MenuSeparator0,
            this.MenuExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(37, 20);
            this.MenuFile.Text = "&File";
            // 
            // MenuOpen
            // 
            this.MenuOpen.Image = ((System.Drawing.Image)(resources.GetObject("MenuOpen.Image")));
            this.MenuOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MenuOpen.Size = new System.Drawing.Size(156, 26);
            this.MenuOpen.Text = "&Open";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuSave
            // 
            this.MenuSave.Image = ((System.Drawing.Image)(resources.GetObject("MenuSave.Image")));
            this.MenuSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MenuSave.Size = new System.Drawing.Size(156, 26);
            this.MenuSave.Text = "&Save";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuSeparator0
            // 
            this.MenuSeparator0.Name = "MenuSeparator0";
            this.MenuSeparator0.Size = new System.Drawing.Size(153, 6);
            // 
            // MenuExit
            // 
            this.MenuExit.Image = ((System.Drawing.Image)(resources.GetObject("MenuExit.Image")));
            this.MenuExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(156, 26);
            this.MenuExit.Text = "&Exit";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // MenuHelp
            // 
            this.MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuAbout});
            this.MenuHelp.Name = "MenuHelp";
            this.MenuHelp.Size = new System.Drawing.Size(44, 20);
            this.MenuHelp.Text = "&Help";
            // 
            // MenuAbout
            // 
            this.MenuAbout.Image = ((System.Drawing.Image)(resources.GetObject("MenuAbout.Image")));
            this.MenuAbout.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.MenuAbout.Name = "MenuAbout";
            this.MenuAbout.Size = new System.Drawing.Size(156, 26);
            this.MenuAbout.Text = "&About";
            this.MenuAbout.Click += new System.EventHandler(this.MenuAbout_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.MainLayout);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.MainMenu;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bayonetta IDD Tool";
            this.MainLayout.ResumeLayout(false);
            this.TexturePanel.ResumeLayout(false);
            this.TexturePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextureImg)).EndInit();
            this.ControlsPanel.ResumeLayout(false);
            this.TextBoxesPanel.ResumeLayout(false);
            this.TextBoxesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDX)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.Panel TexturePanel;
        private System.Windows.Forms.PictureBox TextureImg;
        private System.Windows.Forms.ListBox LstItems;
        private System.Windows.Forms.ListBox LstTextures;
        private System.Windows.Forms.Panel ControlsPanel;
        private System.Windows.Forms.Button BtnImportDDS;
        private System.Windows.Forms.Button BtnImportPNG;
        private System.Windows.Forms.Button BtnExportDDS;
        private System.Windows.Forms.Button BtnExportPNG;
        private System.Windows.Forms.Panel TextBoxesPanel;
        private System.Windows.Forms.NumericUpDown NUDH;
        private System.Windows.Forms.NumericUpDown NUDW;
        private System.Windows.Forms.NumericUpDown NUDY;
        private System.Windows.Forms.NumericUpDown NUDX;
        private System.Windows.Forms.Label LblH;
        private System.Windows.Forms.Label LblW;
        private System.Windows.Forms.Label LblY;
        private System.Windows.Forms.Label LblX;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator0;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.ToolStripMenuItem MenuHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuAbout;
    }
}


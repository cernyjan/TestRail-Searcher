namespace TestRail_Searcher
{
    partial class TestRailSearcherForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestRailSearcherForm));
            this.updateBtn = new System.Windows.Forms.Button();
            this.searchTxt = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.projectsCmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.suitesChckListBox = new System.Windows.Forms.CheckedListBox();
            this.cleanBtn = new System.Windows.Forms.Button();
            this.picLoader = new System.Windows.Forms.PictureBox();
            this.logoutBtn = new System.Windows.Forms.Button();
            this.loginLabel = new System.Windows.Forms.Label();
            this.testCasesDataGridView = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.testCasesCountLbl = new System.Windows.Forms.Label();
            this.ytChbx = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.foundTestCasesCountLbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLoader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testCasesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(353, 134);
            this.updateBtn.Margin = new System.Windows.Forms.Padding(2);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(83, 24);
            this.updateBtn.TabIndex = 0;
            this.updateBtn.Text = "Update DB";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // searchTxt
            // 
            this.searchTxt.Location = new System.Drawing.Point(11, 180);
            this.searchTxt.Margin = new System.Windows.Forms.Padding(2);
            this.searchTxt.Name = "searchTxt";
            this.searchTxt.Size = new System.Drawing.Size(338, 20);
            this.searchTxt.TabIndex = 1;
            // 
            // searchBtn
            // 
            this.searchBtn.Location = new System.Drawing.Point(353, 178);
            this.searchBtn.Margin = new System.Windows.Forms.Padding(2);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(83, 23);
            this.searchBtn.TabIndex = 2;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // projectsCmb
            // 
            this.projectsCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectsCmb.FormattingEnabled = true;
            this.projectsCmb.Location = new System.Drawing.Point(56, 5);
            this.projectsCmb.Margin = new System.Windows.Forms.Padding(2);
            this.projectsCmb.Name = "projectsCmb";
            this.projectsCmb.Size = new System.Drawing.Size(380, 21);
            this.projectsCmb.TabIndex = 3;
            this.projectsCmb.SelectionChangeCommitted += new System.EventHandler(this.projectCmb_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Projects:";
            // 
            // suitesChckListBox
            // 
            this.suitesChckListBox.FormattingEnabled = true;
            this.suitesChckListBox.Location = new System.Drawing.Point(56, 29);
            this.suitesChckListBox.Margin = new System.Windows.Forms.Padding(2);
            this.suitesChckListBox.Name = "suitesChckListBox";
            this.suitesChckListBox.Size = new System.Drawing.Size(941, 94);
            this.suitesChckListBox.TabIndex = 5;
            this.suitesChckListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.suitesChckListBox_ItemCheck);
            // 
            // cleanBtn
            // 
            this.cleanBtn.Location = new System.Drawing.Point(286, 134);
            this.cleanBtn.Margin = new System.Windows.Forms.Padding(2);
            this.cleanBtn.Name = "cleanBtn";
            this.cleanBtn.Size = new System.Drawing.Size(62, 24);
            this.cleanBtn.TabIndex = 6;
            this.cleanBtn.Text = "Clean DB";
            this.cleanBtn.UseVisualStyleBackColor = true;
            this.cleanBtn.Click += new System.EventHandler(this.cleanBtn_Click);
            // 
            // picLoader
            // 
            this.picLoader.BackColor = System.Drawing.Color.Transparent;
            this.picLoader.Image = global::TestRail_Searcher.Properties.Resources.Loadingsome;
            this.picLoader.Location = new System.Drawing.Point(861, 127);
            this.picLoader.Margin = new System.Windows.Forms.Padding(2);
            this.picLoader.Name = "picLoader";
            this.picLoader.Size = new System.Drawing.Size(136, 77);
            this.picLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLoader.TabIndex = 8;
            this.picLoader.TabStop = false;
            // 
            // logoutBtn
            // 
            this.logoutBtn.Location = new System.Drawing.Point(941, 5);
            this.logoutBtn.Margin = new System.Windows.Forms.Padding(2);
            this.logoutBtn.Name = "logoutBtn";
            this.logoutBtn.Size = new System.Drawing.Size(56, 23);
            this.logoutBtn.TabIndex = 9;
            this.logoutBtn.Text = "Logout";
            this.logoutBtn.UseVisualStyleBackColor = true;
            this.logoutBtn.Click += new System.EventHandler(this.logoutBtn_Click);
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(730, 10);
            this.loginLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(0, 13);
            this.loginLabel.TabIndex = 10;
            // 
            // testCasesDataGridView
            // 
            this.testCasesDataGridView.AllowUserToAddRows = false;
            this.testCasesDataGridView.AllowUserToDeleteRows = false;
            this.testCasesDataGridView.AllowUserToResizeRows = false;
            this.testCasesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.testCasesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testCasesDataGridView.Location = new System.Drawing.Point(11, 206);
            this.testCasesDataGridView.Margin = new System.Windows.Forms.Padding(2);
            this.testCasesDataGridView.Name = "testCasesDataGridView";
            this.testCasesDataGridView.ReadOnly = true;
            this.testCasesDataGridView.RowTemplate.Height = 24;
            this.testCasesDataGridView.Size = new System.Drawing.Size(986, 344);
            this.testCasesDataGridView.TabIndex = 11;
            this.testCasesDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.testCasesDataGridView1_CellContentClick);
            this.testCasesDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.testCasesDataGridView_CellContentDoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 139);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Test Cases in local DB:";
            // 
            // testCasesCountLbl
            // 
            this.testCasesCountLbl.AutoSize = true;
            this.testCasesCountLbl.Location = new System.Drawing.Point(129, 139);
            this.testCasesCountLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.testCasesCountLbl.Name = "testCasesCountLbl";
            this.testCasesCountLbl.Size = new System.Drawing.Size(13, 13);
            this.testCasesCountLbl.TabIndex = 14;
            this.testCasesCountLbl.Text = "0";
            // 
            // ytChbx
            // 
            this.ytChbx.AutoSize = true;
            this.ytChbx.Location = new System.Drawing.Point(457, 182);
            this.ytChbx.Margin = new System.Windows.Forms.Padding(2);
            this.ytChbx.Name = "ytChbx";
            this.ytChbx.Size = new System.Drawing.Size(152, 17);
            this.ytChbx.TabIndex = 15;
            this.ytChbx.Text = "Test Cases from YouTrack";
            this.ytChbx.UseVisualStyleBackColor = true;
            this.ytChbx.CheckedChanged += new System.EventHandler(this.ytChbx_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 152);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Found Test Cases:";
            // 
            // foundTestCasesCountLbl
            // 
            this.foundTestCasesCountLbl.AutoSize = true;
            this.foundTestCasesCountLbl.Location = new System.Drawing.Point(129, 153);
            this.foundTestCasesCountLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.foundTestCasesCountLbl.Name = "foundTestCasesCountLbl";
            this.foundTestCasesCountLbl.Size = new System.Drawing.Size(13, 13);
            this.foundTestCasesCountLbl.TabIndex = 17;
            this.foundTestCasesCountLbl.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Suites:";
            // 
            // TestRailSearcherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.foundTestCasesCountLbl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ytChbx);
            this.Controls.Add(this.testCasesCountLbl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.testCasesDataGridView);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.logoutBtn);
            this.Controls.Add(this.picLoader);
            this.Controls.Add(this.cleanBtn);
            this.Controls.Add(this.suitesChckListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.projectsCmb);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchTxt);
            this.Controls.Add(this.updateBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "TestRailSearcherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestRail - Searcher";
            this.Load += new System.EventHandler(this.TestRailSearcher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLoader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testCasesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.TextBox searchTxt;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.ComboBox projectsCmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox suitesChckListBox;
        private System.Windows.Forms.Button cleanBtn;
        private System.Windows.Forms.PictureBox picLoader;
        private System.Windows.Forms.Button logoutBtn;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.DataGridView testCasesDataGridView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label testCasesCountLbl;
        private System.Windows.Forms.CheckBox ytChbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label foundTestCasesCountLbl;
        private System.Windows.Forms.Label label2;
    }
}


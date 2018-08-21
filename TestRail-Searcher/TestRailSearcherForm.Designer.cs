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
            ((System.ComponentModel.ISupportInitialize)(this.picLoader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testCasesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(471, 165);
            this.updateBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(111, 30);
            this.updateBtn.TabIndex = 0;
            this.updateBtn.Text = "Update DB";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // searchTxt
            // 
            this.searchTxt.Location = new System.Drawing.Point(15, 222);
            this.searchTxt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchTxt.Name = "searchTxt";
            this.searchTxt.Size = new System.Drawing.Size(449, 22);
            this.searchTxt.TabIndex = 1;
            // 
            // searchBtn
            // 
            this.searchBtn.Location = new System.Drawing.Point(471, 219);
            this.searchBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(111, 28);
            this.searchBtn.TabIndex = 2;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // projectsCmb
            // 
            this.projectsCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectsCmb.FormattingEnabled = true;
            this.projectsCmb.Location = new System.Drawing.Point(75, 6);
            this.projectsCmb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.projectsCmb.Name = "projectsCmb";
            this.projectsCmb.Size = new System.Drawing.Size(391, 24);
            this.projectsCmb.TabIndex = 3;
            this.projectsCmb.SelectionChangeCommitted += new System.EventHandler(this.projectCmb_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Project:";
            // 
            // suitesChckListBox
            // 
            this.suitesChckListBox.FormattingEnabled = true;
            this.suitesChckListBox.Location = new System.Drawing.Point(15, 36);
            this.suitesChckListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.suitesChckListBox.Name = "suitesChckListBox";
            this.suitesChckListBox.Size = new System.Drawing.Size(979, 106);
            this.suitesChckListBox.TabIndex = 5;
            this.suitesChckListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.suitesChckListBox_ItemCheck);
            // 
            // cleanBtn
            // 
            this.cleanBtn.Location = new System.Drawing.Point(381, 165);
            this.cleanBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cleanBtn.Name = "cleanBtn";
            this.cleanBtn.Size = new System.Drawing.Size(83, 30);
            this.cleanBtn.TabIndex = 6;
            this.cleanBtn.Text = "Clean DB";
            this.cleanBtn.UseVisualStyleBackColor = true;
            this.cleanBtn.Click += new System.EventHandler(this.cleanBtn_Click);
            // 
            // picLoader
            // 
            this.picLoader.BackColor = System.Drawing.Color.Transparent;
            this.picLoader.Image = global::TestRail_Searcher.Properties.Resources.Loadingsome;
            this.picLoader.Location = new System.Drawing.Point(813, 155);
            this.picLoader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.picLoader.Name = "picLoader";
            this.picLoader.Size = new System.Drawing.Size(181, 95);
            this.picLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLoader.TabIndex = 8;
            this.picLoader.TabStop = false;
            // 
            // logoutBtn
            // 
            this.logoutBtn.Location = new System.Drawing.Point(919, 6);
            this.logoutBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logoutBtn.Name = "logoutBtn";
            this.logoutBtn.Size = new System.Drawing.Size(75, 28);
            this.logoutBtn.TabIndex = 9;
            this.logoutBtn.Text = "Logout";
            this.logoutBtn.UseVisualStyleBackColor = true;
            this.logoutBtn.Click += new System.EventHandler(this.logoutBtn_Click);
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(582, 9);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(0, 17);
            this.loginLabel.TabIndex = 10;
            // 
            // testCasesDataGridView
            // 
            this.testCasesDataGridView.AllowUserToAddRows = false;
            this.testCasesDataGridView.AllowUserToDeleteRows = false;
            this.testCasesDataGridView.AllowUserToResizeRows = false;
            this.testCasesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.testCasesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testCasesDataGridView.Location = new System.Drawing.Point(15, 254);
            this.testCasesDataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.testCasesDataGridView.Name = "testCasesDataGridView";
            this.testCasesDataGridView.ReadOnly = true;
            this.testCasesDataGridView.RowTemplate.Height = 24;
            this.testCasesDataGridView.Size = new System.Drawing.Size(979, 288);
            this.testCasesDataGridView.TabIndex = 11;
            this.testCasesDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.testCasesDataGridView1_CellContentClick);
            this.testCasesDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.testCasesDataGridView_CellContentDoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "Test Cases in local DB:";
            // 
            // testCasesCountLbl
            // 
            this.testCasesCountLbl.AutoSize = true;
            this.testCasesCountLbl.Location = new System.Drawing.Point(172, 171);
            this.testCasesCountLbl.Name = "testCasesCountLbl";
            this.testCasesCountLbl.Size = new System.Drawing.Size(16, 17);
            this.testCasesCountLbl.TabIndex = 14;
            this.testCasesCountLbl.Text = "0";
            // 
            // ytChbx
            // 
            this.ytChbx.AutoSize = true;
            this.ytChbx.Location = new System.Drawing.Point(641, 224);
            this.ytChbx.Name = "ytChbx";
            this.ytChbx.Size = new System.Drawing.Size(166, 21);
            this.ytChbx.TabIndex = 15;
            this.ytChbx.Text = "YouTrack Test Cases";
            this.ytChbx.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "Found Test Cases:";
            // 
            // foundTestCasesCountLbl
            // 
            this.foundTestCasesCountLbl.AutoSize = true;
            this.foundTestCasesCountLbl.Location = new System.Drawing.Point(172, 188);
            this.foundTestCasesCountLbl.Name = "foundTestCasesCountLbl";
            this.foundTestCasesCountLbl.Size = new System.Drawing.Size(16, 17);
            this.foundTestCasesCountLbl.TabIndex = 17;
            this.foundTestCasesCountLbl.Text = "0";
            // 
            // TestRailSearcherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 553);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
    }
}


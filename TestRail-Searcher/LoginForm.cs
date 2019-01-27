using System;
using System.Windows.Forms;

namespace TestRail_Searcher
{
    public partial class LoginForm : Form
    {
        static readonly string AdminCollectionName = "Administrator";
        readonly DatabaseServer _dbs = new DatabaseServer(@"Database.db", AdminCollectionName);

        public LoginForm()
        {
            InitializeComponent();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            this.Text = Program.VersionLabel + "u";

            var administrator = _dbs.GetAdmin();
            if (administrator != null)
            {
                serverTxt.Text = administrator.Server;
                loginTxt.Text = administrator.Login;
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            loginProgressBar.Visible = true;
            var trr = new TestRailReader(serverTxt.Text, loginTxt.Text, passwordTxt.Text);
            if (trr.TryLogin())
            {
                this.DialogResult = DialogResult.OK;

                var administrator = _dbs.GetAdmin();
                if (administrator != null)
                {
                    administrator.SetProperties(1, serverTxt.Text, loginTxt.Text, administrator.ProjectId);
                }
                else
                {
                    administrator = new Administrator();
                    administrator.SetProperties(1, serverTxt.Text, loginTxt.Text, -1);
                }

                try
                {
                    if (_dbs.DocumentExists(AdminCollectionName, 1))
                    {
                        _dbs.UpdateDocument(administrator);
                    }
                    else
                    {
                        _dbs.InsertDocument(administrator);
                    }
                }
                catch (Exception ex)
                {
                    Program.LogException(ex);
                }
            }
            else
            {
                loginProgressBar.Visible = false;
                MessageBox.Show(@"Cannot login, try again.");
                serverTxt.Focus();
            }
        }
    }
}

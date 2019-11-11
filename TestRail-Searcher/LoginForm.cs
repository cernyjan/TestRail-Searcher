using System;
using System.Collections.Generic;
using System.Linq;
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
            this.Text = Program.VersionLabel;

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
            bool accessGranted;

            var domain = "@neopost.com";
            if (loginTxt.Text.Substring(loginTxt.Text.Length - domain.Length, domain.Length).Equals(domain))
            {
                accessGranted = trr.TryLogin(domain);
            }
            else
            {
                accessGranted = trr.TryLogin();
            }

            if (accessGranted)
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

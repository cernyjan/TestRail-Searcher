using System;
using System.Windows.Forms;

namespace TestRail_Searcher
{
    public partial class LoginForm : Form
    {
        readonly DatabaseServer _dbs = new DatabaseServer(@"Database.db", "Administrator");

        public LoginForm()
        {
            InitializeComponent();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            var administrator = _dbs.GetAdmin();
            if (administrator != null)
            {
                serverTxt.Text = administrator.Server;
                loginTxt.Text = administrator.Login;
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
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
                    _dbs.InsertDocument(administrator);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Cannot insert duplicate key in unique index '_id'"))
                    {
                        _dbs.UpdateDocument(administrator);
                    }
                }
            }
            else
            {
                MessageBox.Show("Cannot login, try again.");
                serverTxt.Focus();
            }
        }
    }
}

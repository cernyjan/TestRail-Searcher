using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace TestRail_Searcher
{
    public partial class TestRailSearcherForm : Form
    {
        string DatabaseFilePath = @"Database.db";
        string AdminCollectionName = "Administrator";
        string TestCasesCollectionName = "TestCases";
        string _server;
        string _user;
        string _password;
        TestRailReader _trr;
        bool _youTrackTestCases = false;

        protected int ProjectId = -1;
        protected List<string> Suites = new List<string>();
        
        public TestRailSearcherForm()
        {
            InitializeComponent();
        }

        private void TestRailSearcher_Load(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() != DialogResult.OK)
            {
                this.Close();
            }
            loginForm.Close();

            this._server = loginForm.serverTxt.Text;
            this._user = loginForm.loginTxt.Text;
            this._password = loginForm.passwordTxt.Text;

            loginLabel.Text = this._user;
            searchTxt.Text = "";
            SetLoading(false);

            _trr = new TestRailReader(this._server, this._user, this._password);
            var projects = _trr.GetProjects();
            foreach (var project in projects)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = (string) project["name"];
                item.Value = project["id"];
                projectsCmb.Items.Add(item);
            }

            var dbs = new DatabaseServer(DatabaseFilePath, AdminCollectionName);
            var admin = dbs.GetAdmin();
            var index = 0;
            foreach (var projectsCmbItem in projectsCmb.Items)
            {
                if (((ComboboxItem)projectsCmbItem).Value.ToString().Equals(admin.ProjectId.ToString()))
                {
                    projectsCmb.SelectedIndex = index;
                    FillSuites();
                    break;
                }
                index++;
            }

            testCasesDataGridView.Columns.Add("Suite", "Suite");
            testCasesDataGridView.Columns.Add("Category", "Category");
            testCasesDataGridView.Columns.Add("ID", "ID");
            testCasesDataGridView.Columns.Add("Original ID", "Original ID");
            testCasesDataGridView.Columns.Add("Title", "Title");
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void FillSuites()
        {
            Suites.Clear();
            testCasesCountLbl.Text = "0";
            if (projectsCmb.SelectedItem != null)
                ProjectId = Int32.Parse(((ComboboxItem)projectsCmb.SelectedItem).Value.ToString());
            var suites = _trr.GetSuites(ProjectId);
            suitesChckListBox.Items.Clear();
            foreach (var suite in suites)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = (string)suite["name"];
                item.Value = suite["id"];
                suitesChckListBox.Items.Add(item);
            }
            var dbs = new DatabaseServer(DatabaseFilePath, AdminCollectionName);
            var admin = dbs.GetAdmin();
            admin.ProjectId = ProjectId;
            dbs.UpdateDocument(admin);
        }

        private void projectCmb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FillSuites();
        }

        private void suitesChckListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (sender != null && e != null)
            {
                // Get the checkListBox selected time and it's CheckState
                CheckedListBox checkListBox = (CheckedListBox)sender;
                string selectedItem = (checkListBox.SelectedItem as ComboboxItem)?.Value.ToString();

                // If curent value was checked, then remove from list
                if (e.CurrentValue == CheckState.Checked &&
                    Suites.Contains(selectedItem))
                {
                    Suites.Remove(selectedItem);
                }
                // else if new value is checked, then add to list
                else if (e.NewValue == CheckState.Checked &&
                         !Suites.Contains(selectedItem))
                {
                    Suites.Insert(0, selectedItem);
                }

                try
                {
                    Thread threadInput = new Thread(CountTestCases);
                    threadInput.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void SetLoading(bool displayLoader)
        {
            if (displayLoader)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    picLoader.Visible = true;
                    //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                });
                cleanBtn.Invoke((MethodInvoker)delegate {
                    cleanBtn.Enabled = false;
                    updateBtn.Enabled = false;
                    projectsCmb.Enabled = false;
                    suitesChckListBox.Enabled = false;
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    picLoader.Visible = false;
                    //this.Cursor = System.Windows.Forms.Cursors.Default;
                });
                cleanBtn.Invoke((MethodInvoker)delegate {
                    cleanBtn.Enabled = true;
                    updateBtn.Enabled = true;
                    projectsCmb.Enabled = true;
                    suitesChckListBox.Enabled = true;
                });
            }
        }

        private void UpdateDatabase()
        {
            SetLoading(true);

            foreach (string suite in Suites)
            {
                var testCases = _trr.GetTestCases(ProjectId, Int32.Parse(suite));
                foreach (var testCase in testCases)
                {
                    var originalId = testCase["custom_custom_original_id"].ToString();
                    var title = testCase["title"].ToString();
                    var notes = testCase["custom_notes"].ToString().ToLower();
                    var preconds = testCase["custom_preconds"].ToString().ToLower();
                    var comments = testCase["custom_custom_comments"].ToString().ToLower();
                    var steps = new List<string>();
                    var expecteds = new List<string>();
                    foreach (var step in testCase["custom_steps_separated"])
                    {
                        steps.Add((string)step["content"]);
                        expecteds.Add((string)step["expected"]);
                    }
                    var stepsInString = String.Join(", ", steps.ToArray()).ToLower();
                    var expectedsInString = String.Join(", ", expecteds.ToArray()).ToLower();

                    var dbs = new DatabaseServer(DatabaseFilePath, TestCasesCollectionName);

                    // Create your new Test Case instance
                    var testCaseDocument = new TestCase();
                    testCaseDocument.SetProperties(
                        (int)testCase["id"],
                        originalId,
                        title,
                        (int)testCase["section_id"],
                        (int?)testCase["milestone_id"],
                        (int)testCase["suite_id"],
                        notes,
                        preconds,
                        stepsInString,
                        expectedsInString,
                        comments,
                        (int)testCase["updated_on"]
                    );

                    try
                    {
                        dbs.InsertDocument(testCaseDocument);
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("Cannot insert duplicate key in unique index '_id'"))
                        {
                            if (dbs.IsTestCaseUpdatable(testCaseDocument.Id, testCaseDocument.UpdatedOn))
                            {
                                dbs.UpdateDocument(testCaseDocument);
                            }
                        }
                        else
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            SetLoading(false);

            try
            {
                Thread threadInput = new Thread(CountTestCases);
                threadInput.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CleanDatabase()
        {
            SetLoading(true);
            var dbs = new DatabaseServer(DatabaseFilePath, TestCasesCollectionName);
            dbs.DeleteAllDocuments(TestCasesCollectionName);
            SetLoading(false);

            try
            {
                Thread threadInput = new Thread(CountTestCases);
                threadInput.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SearchTestCases()
        {
            SetLoading(true);
            testCasesDataGridView.Invoke((MethodInvoker)delegate {
                testCasesDataGridView.Rows.Clear();
            });
            testCasesDataGridView.Invoke((MethodInvoker)delegate {
                foundTestCasesCountLbl.Text = "searching...";
            });
            var dbs = new DatabaseServer(DatabaseFilePath, TestCasesCollectionName);
            var result = dbs.GetAllTestCasesByKeyword(this.Suites, searchTxt.Text, _youTrackTestCases);
            foreach (var testCase in result)
            {
                object[] row = {
                    _trr.GetSuiteName(testCase.SuiteId),
                    _trr.GetSectionName(testCase.SectionId),
                    testCase.Id.ToString(),
                    testCase.CustomCustomOriginalId,
                    testCase.Title };
                testCasesDataGridView.Invoke((MethodInvoker)delegate {
                    testCasesDataGridView.Rows.Add(row);
                    this.testCasesDataGridView.Columns[testCasesDataGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                });
            }
            testCasesDataGridView.Invoke((MethodInvoker)delegate {
                foundTestCasesCountLbl.Text = testCasesDataGridView.Rows.Count.ToString();
            });
            SetLoading(false);
        }

        private void CountTestCases()
        {
            SetLoading(true);
            var dbs = new DatabaseServer(DatabaseFilePath, TestCasesCollectionName);
            testCasesCountLbl.Invoke((MethodInvoker)delegate {
                testCasesCountLbl.Text = dbs.GetTestCasesCount(Suites).ToString();
            });
            SetLoading(false);
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (ProjectId != -1 && Suites.Count > 0)
            {
                try
                {
                    Thread threadInput = new Thread(UpdateDatabase);
                    threadInput.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select Project and at least one Suite.");
            }
        }

        private void cleanBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Thread threadInput = new Thread(CleanDatabase);
                threadInput.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(searchTxt.Text) && !String.IsNullOrWhiteSpace(searchTxt.Text) && ProjectId != -1 && Suites.Count > 0)
            {
                try
                {
                    Thread threadInput = new Thread(SearchTestCases);
                    threadInput.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select Project, at least one Suite and put any keyword.");
            }
            
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            TestRailSearcherForm form = new TestRailSearcherForm();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void testCasesDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                object value = testCasesDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                Clipboard.SetText(value.ToString());
            }
        }

        private void testCasesDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 2)
            {
                object value = testCasesDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                //e.g. https://testrail.quadient.group/index.php?/cases/view/860801
                if (!this._server.EndsWith("/"))
                {
                    this._server += "/";
                }
                var url = this._server + "index.php?/cases/view/" + value;
                System.Diagnostics.Process.Start(url);
            }
        }

        private void ytChbx_CheckedChanged(object sender, EventArgs e)
        {
            _youTrackTestCases = !_youTrackTestCases;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestRail_Searcher
{
    public partial class TestRailSearcherForm : Form
    {
        int Threads = 8;
        string DatabaseFilePath = @"Database.db";
        string AdminCollectionName = "Administrator";
        string TestCasesCollectionName = "TestCases";
        string _server;
        string _user;
        string _password;
        TestRailReader _trr;
        bool _youTrackTestCases = false;

        protected int ProjectId = -1;
        protected List<string> SelectedSuites = new List<string>();
        protected Dictionary<int, string> Suites = new Dictionary<int, string>();
        protected Dictionary<int, List<string>> Sections = new Dictionary<int, List<string>>();

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
                if (((ComboboxItem) projectsCmbItem).Value.ToString().Equals(admin.ProjectId.ToString()))
                {
                    projectsCmb.SelectedIndex = index;
                    SetLoading(true);
                    FillSuites();
                    GetSections();
                    SetLoading(false);
                    break;
                }
                index++;
            }

            testCasesDataGridView.Columns.Add("Suite", "Suite");
            testCasesDataGridView.Columns.Add("Category", "Category");
            testCasesDataGridView.Columns.Add("ID", "ID");
            testCasesDataGridView.Columns.Add("Original ID", "Original ID");
            testCasesDataGridView.Columns.Add("Title", "Title");

            this.Text = Program.VersionLabel;
            loginForm.Close();
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
            SelectedSuites.Clear();
            Suites.Clear();
            testCasesCountLbl.Text = "0";
            if (projectsCmb.SelectedItem != null)
                ProjectId = Int32.Parse(((ComboboxItem) projectsCmb.SelectedItem).Value.ToString());
            var suites = _trr.GetSuites(ProjectId);
            suitesChckListBox.Items.Clear();
            foreach (var suite in suites)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = (string) suite["name"];
                item.Value = suite["id"];
                suitesChckListBox.Items.Add(item);
                Suites.Add((int) suite["id"], (string) suite["name"]);
            }
            var dbs = new DatabaseServer(DatabaseFilePath, AdminCollectionName);
            var admin = dbs.GetAdmin();
            admin.ProjectId = ProjectId;
            dbs.UpdateDocument(admin);
        }

        private void GetSections()
        {
            Sections.Clear();
            if (projectsCmb.SelectedItem != null)
                ProjectId = Int32.Parse(((ComboboxItem) projectsCmb.SelectedItem).Value.ToString());
            foreach (var suite in Suites)
            {
                var sections = _trr.GetSections(ProjectId, suite.Key);
                foreach (var section in sections)
                {
                    Sections.Add((int) section["id"],
                        new List<string> {(string) section["name"], (string) section["parent_id"]});
                }
            }
        }

        private void projectCmb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetLoading(true);
            FillSuites();
            GetSections();
            SetLoading(false);
        }

        private void suitesChckListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (sender != null && e != null)
            {
                // Get the checkListBox selected time and it's CheckState
                CheckedListBox checkListBox = (CheckedListBox) sender;
                string selectedItem = (checkListBox.SelectedItem as ComboboxItem)?.Value.ToString();

                // If curent value was checked, then remove from list
                if (e.CurrentValue == CheckState.Checked &&
                    SelectedSuites.Contains(selectedItem))
                {
                    SelectedSuites.Remove(selectedItem);
                }
                // else if new value is checked, then add to list
                else if (e.NewValue == CheckState.Checked &&
                         !SelectedSuites.Contains(selectedItem))
                {
                    SelectedSuites.Insert(0, selectedItem);
                }

                try
                {
                    Thread threadInput = new Thread(CountTestCases);
                    threadInput.Start();
                }
                catch (Exception ex)
                {
                    Program.LogException(ex);
                }
            }
        }

        private void SetLoading(bool displayLoader)
        {
            if (displayLoader)
            {
                this.Invoke((MethodInvoker) delegate
                {
                    picLoader.Visible = true;
                    //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                });
                cleanBtn.Invoke((MethodInvoker) delegate
                {
                    cleanBtn.Enabled = false;
                    updateBtn.Enabled = false;
                    projectsCmb.Enabled = false;
                    suitesChckListBox.Enabled = false;
                });
            }
            else
            {
                this.Invoke((MethodInvoker) delegate
                {
                    picLoader.Visible = false;
                    //this.Cursor = System.Windows.Forms.Cursors.Default;
                });
                cleanBtn.Invoke((MethodInvoker) delegate
                {
                    cleanBtn.Enabled = true;
                    updateBtn.Enabled = true;
                    projectsCmb.Enabled = true;
                    suitesChckListBox.Enabled = true;
                });
            }
        }

        private string GetSuiteName(int suiteId)
        {
            return Suites[suiteId];
        }

        private string GetSectionName(int sectionId)
        {
            var sectionName = Sections[sectionId][0];
            string parentId = Sections[sectionId][1];
            if (string.IsNullOrEmpty(parentId))
            {
                return sectionName;
            }
            else
            {
                var parentName = Sections[Int32.Parse(parentId)][0];
                return parentName + " › " + sectionName;
            }
        }

        private void UpdateDatabase()
        {
            SetLoading(true);
            var testCasesCount = 0;
            testCasesCountLbl.Invoke((MethodInvoker)delegate
            {
                testCasesCountLbl.Text = "updating...";
            });
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(SelectedSuites, new ParallelOptions {MaxDegreeOfParallelism = Threads}, suite =>
            {
                var testCases = _trr.GetTestCases(ProjectId, Int32.Parse(suite));
                Parallel.ForEach(testCases, new ParallelOptions {MaxDegreeOfParallelism = Threads}, testCase =>
                {
                    var originalId = testCase["custom_custom_original_id"].ToString();
                    var title = testCase["title"].ToString();
                    var sectionId = (int) testCase["section_id"];
                    var sectionName = GetSectionName(sectionId);
                    var suiteId = (int) testCase["suite_id"];
                    var suiteName = GetSuiteName(suiteId);
                    var notes = testCase["custom_notes"].ToString().ToLower();
                    var preconds = testCase["custom_preconds"].ToString().ToLower();
                    var jTokenComments = testCase["custom_custom_comments"];
                    var comments = "";
                    if (jTokenComments != null)
                    {
                        comments = jTokenComments.ToString().ToLower();
                    }
                    var steps = new List<string>();
                    var expecteds = new List<string>();
                    foreach (var step in testCase["custom_steps_separated"])
                    {
                        steps.Add((string) step["content"]);
                        expecteds.Add((string) step["expected"]);
                    }
                    var stepsInString = String.Join(", ", steps.ToArray()).ToLower();
                    var expectedsInString = String.Join(", ", expecteds.ToArray()).ToLower();

                    var dbs = new DatabaseServer(DatabaseFilePath, TestCasesCollectionName);

                    // Create your new Test Case instance
                    var testCaseDocument = new TestCase();
                    testCaseDocument.SetProperties(
                        (int) testCase["id"],
                        originalId,
                        title,
                        sectionId,
                        sectionName,
                        (int?) testCase["milestone_id"],
                        suiteId,
                        suiteName,
                        notes,
                        preconds,
                        stepsInString,
                        expectedsInString,
                        comments,
                        (int) testCase["updated_on"]
                    );

                    try
                    {
                        if (dbs.DocumentExists(TestCasesCollectionName, testCaseDocument.Id))
                        {
                            if (dbs.IsTestCaseUpdatable(testCaseDocument.Id, testCaseDocument.UpdatedOn))
                            {
                                dbs.UpdateDocument(testCaseDocument);
                            }
                        }
                        else
                        {
                            dbs.InsertDocument(testCaseDocument);
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.LogException(ex);
                    }
                    testCasesCountLbl.Invoke((MethodInvoker) delegate
                    {
                        testCasesCountLbl.Text = (++testCasesCount).ToString();
                    });
                });
            });

            sw.Stop();
            Console.WriteLine(@"Update took: " + sw.Elapsed.ToString("ss") + @"s");

            SetLoading(false);

            try
            {
                Thread threadInput = new Thread(CountTestCases);
                threadInput.Start();
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
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
                Program.LogException(ex);
            }
        }

        private void SearchTestCases()
        {
            SetLoading(true);
            testCasesDataGridView.Invoke((MethodInvoker) delegate
            {
                testCasesDataGridView.Rows.Clear();
            });
            testCasesDataGridView.Invoke((MethodInvoker) delegate
            {
                foundTestCasesCountLbl.Text = "searching...";
            });
            var dbs = new DatabaseServer(DatabaseFilePath, TestCasesCollectionName);
            var result = dbs.GetAllTestCasesByKeyword(this.SelectedSuites, searchTxt.Text, _youTrackTestCases);
            foreach (var testCase in result)
            {
                object[] row =
                {
                    Suites[testCase.SuiteId],
                    testCase.SectionName,
                    testCase.Id.ToString(),
                    testCase.CustomCustomOriginalId,
                    testCase.Title
                };
                testCasesDataGridView.Invoke((MethodInvoker) delegate
                {
                    testCasesDataGridView.Rows.Add(row);
                    this.testCasesDataGridView.Columns[testCasesDataGridView.ColumnCount - 1].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                });
            }
            testCasesDataGridView.Invoke((MethodInvoker) delegate
            {
                foundTestCasesCountLbl.Text = testCasesDataGridView.Rows.Count.ToString();
            });

            SetLoading(false);
        }

        private void CountTestCases()
        {
            SetLoading(true);
            var dbs = new DatabaseServer(DatabaseFilePath, TestCasesCollectionName);
            testCasesCountLbl.Invoke((MethodInvoker) delegate
            {
                testCasesCountLbl.Text = dbs.GetTestCasesCount(SelectedSuites).ToString();
            });
            SetLoading(false);
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (ProjectId != -1 && SelectedSuites.Count > 0)
            {

                try
                {
                    Thread threadInput = new Thread(UpdateDatabase);
                    threadInput.Start();
                }
                catch (Exception ex)
                {
                    Program.LogException(ex);
                }
            }
            else
            {
                MessageBox.Show(@"Select Project and at least one Suite.");
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
                Program.LogException(ex);
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(searchTxt.Text) && !String.IsNullOrWhiteSpace(searchTxt.Text) &&
                ProjectId != -1 && SelectedSuites.Count > 0)
            {
                try
                {
                    Thread threadInput = new Thread(SearchTestCases);
                    threadInput.Start();
                }
                catch (Exception ex)
                {
                    Program.LogException(ex);
                }
            }
            else
            {
                MessageBox.Show(@"Select Project, at least one Suite and put any keyword.");
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
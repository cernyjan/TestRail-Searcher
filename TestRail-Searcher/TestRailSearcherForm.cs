using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

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
        protected Dictionary<int, string> Assignees = new Dictionary<int, string>();
        JArray _caseFields;
        readonly Dictionary<int, string> _statuses = new Dictionary<int, string>();
        readonly Dictionary<int, string> _testTypes = new Dictionary<int, string>();
        readonly Dictionary<int, string> _tags = new Dictionary<int, string>();
        
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
            _caseFields = _trr.GetCaseFields();
            GetAssignees();

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
                    GetStatusesAndTestTypesAndTags();
                    SetLoading(false);
                    break;
                }
                index++;
            }

            testCasesDataGridView.Columns.Add("Suite", "Suite");
            testCasesDataGridView.Columns.Add("ID", "ID");
            testCasesDataGridView.Columns.Add("Category", "Category");
            testCasesDataGridView.Columns.Add("Title", "Title");
            testCasesDataGridView.Columns.Add("Original ID", "Original ID");
            testCasesDataGridView.Columns.Add("Test Type", "Test Type");
            testCasesDataGridView.Columns.Add("Tags", "Tags");
            testCasesDataGridView.Columns.Add("Status", "Status");
            testCasesDataGridView.Columns.Add("Assignee", "Assignee");

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

        private void GetStatusesAndTestTypesAndTags()
        {
            _statuses.Clear();
            _testTypes.Clear();
            _tags.Clear();
            foreach (JObject content in _caseFields.Children<JObject>())
            {
                try
                {
                    foreach (JProperty prop in content.Properties())
                    {
                        if (prop.Name.Equals("system_name"))
                        {
                            if (prop.Value.ToString().Equals("custom_custom_status"))
                            {
                                string statusValues = content["configs"][0]["options"]["items"].ToString();
                                List<string> statusList = statusValues.Split('\n').ToList<string>();

                                foreach (var status in statusList)
                                {
                                    string[] statusSplit = status.Split(new string[] {", "}, StringSplitOptions.None);
                                    _statuses.Add(Convert.ToInt32(statusSplit[0]), statusSplit[1]);
                                }
                            }
                            else if (prop.Value.ToString().Equals("custom_custom_test_type"))
                            {
                                var configs = content["configs"];
                                foreach (var config in configs)
                                {
                                    List<int> projects = new List<int>();
                                    var temp = config["context"]["project_ids"].ToList();
                                    foreach (var t in temp)
                                    {
                                        var a = Convert.ToInt32(t.ToString());
                                        projects.Add(a);
                                    }
                                    if (projects.Contains(ProjectId))
                                    {
                                        var testTypeValues = config["options"]["items"].ToString();
                                        List<string> testTypeList = testTypeValues.Split('\n').ToList<string>();

                                        foreach (var testType in testTypeList)
                                        {
                                            string[] testTypeSplit = testType.Split(new string[] {", "},
                                                StringSplitOptions.None);
                                            _testTypes.Add(Convert.ToInt32(testTypeSplit[0]), testTypeSplit[1]);
                                        }
                                    }
                                }
                            }
                            else if (prop.Value.ToString().Equals("custom_custom_tags"))
                            {
                                var configs = content["configs"];
                                foreach (var config in configs)
                                {
                                    List<int> projects = new List<int>();
                                    var temp = config["context"]["project_ids"].ToList();
                                    foreach (var t in temp)
                                    {
                                        var a = Convert.ToInt32(t.ToString());
                                        projects.Add(a);
                                    }
                                    if (projects.Contains(ProjectId))
                                    {
                                        var tagValues = config["options"]["items"].ToString();
                                        List<string> tagList = tagValues.Split('\n').ToList<string>();

                                        foreach (var tag in tagList)
                                        {
                                            string[] tagSplit = tag.Split(new string[] {", "}, StringSplitOptions.None);
                                            _tags.Add(Convert.ToInt32(tagSplit[0]), tagSplit[1]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.LogException(ex);
                }
            }
        }

        private void GetAssignees()
        {
            Assignees.Clear();
            var users = _trr.GetUsers();
            foreach (var user in users)
            {
                Assignees.Add((int)user["id"], (string)user["name"]);
            }
        }

        private void projectCmb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetLoading(true);
            FillSuites();
            GetSections();
            GetStatusesAndTestTypesAndTags();
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

        private string GetStatusName(int statusId)
        {
            if (statusId > -1)
            {
                return _statuses[statusId];
            }
            return "";
        }

        private string GetTestTypeName(List<int> testTypeIds)
        {
            if (testTypeIds.Count > 0)
            {

                StringBuilder testTypes = new StringBuilder();
                foreach (var id in testTypeIds)
                {
                    testTypes.Append(_testTypes[id]);
                    testTypes.Append(", ");
                }
                var testType = testTypes.ToString();
                return testType.Substring(0, testType.Length - 2);
            }
            return "";
        }

        private string GetTagName(List<int> tagIds)
        {
            if (tagIds.Count > 0)
            {
                StringBuilder tags = new StringBuilder();
                foreach (var id in tagIds)
                {
                    tags.Append(_tags[id]);
                    tags.Append(", ");
                }
                var tag = tags.ToString();
                return tag.Substring(0, tag.Length - 2);
            }
            return "";
        }

        private string GetAssigneeName(int assigneeId)
        {
            if (assigneeId > -1)
            {
                return Assignees[assigneeId];
            }
            return "";
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
                var exceptions = new ConcurrentQueue<Exception>();
                Parallel.ForEach(testCases, new ParallelOptions {MaxDegreeOfParallelism = Threads}, testCase =>
                {
                    try
                    {
                        var originalId = testCase["custom_custom_original_id"].ToString();
                        var title = testCase["title"].ToString();
                        var sectionId = (int) testCase["section_id"];
                        var sectionName = GetSectionName(sectionId);
                        var suiteId = (int) testCase["suite_id"];
                        var suiteName = GetSuiteName(suiteId);
                        var jTokencustomCustomStatusId = testCase["custom_custom_status"];
                        var customCustomStatusId = -1;
                        if (jTokencustomCustomStatusId != null)
                            if (jTokencustomCustomStatusId.Type != JTokenType.Null)
                                customCustomStatusId = (int)jTokencustomCustomStatusId;
                        var customCustomStatusName = GetStatusName(customCustomStatusId);
                        var customCustomTestTypeIds = testCase["custom_custom_test_type"].ToObject<List<int>>();
                        var customCustomTestTypeName = GetTestTypeName(customCustomTestTypeIds);
                        var jTokencustomCustomTagsIds = testCase["custom_custom_tags"];
                        List<int> customCustomTagsIds = new List<int>();
                        if (jTokencustomCustomTagsIds != null)
                            if (jTokencustomCustomTagsIds.Type != JTokenType.Null)
                                customCustomTagsIds = jTokencustomCustomTagsIds.ToObject<List<int>>();
                        var customCustomTagsName = GetTagName(customCustomTagsIds);
                        var jTokenAssigneeId = testCase["custom_assignee"];
                        var customAssigneeId = -1;
                        if (jTokenAssigneeId != null)
                            if (jTokenAssigneeId.Type != JTokenType.Null)
                                customAssigneeId = (int)jTokenAssigneeId;
                        var customAssigneeName = GetAssigneeName(customAssigneeId);
                        var notes = testCase["custom_notes"].ToString().ToLower();
                        var preconds = testCase["custom_preconds"].ToString().ToLower();
                        var jTokenComments = testCase["custom_custom_comments"];
                        var comments = "";
                        if (jTokenComments != null)
                        {
                            comments = jTokenComments.ToString().ToLower();
                        }
                        var steps = new List<string>();
                        List<string> expecteds = new List<string>();
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
                            customCustomStatusId,
                            customCustomStatusName,
                            customCustomTestTypeIds,
                            customCustomTestTypeName,
                            customCustomTagsIds,
                            customCustomTagsName,
                            customAssigneeId,
                            customAssigneeName,
                            notes,
                            preconds,
                            stepsInString,
                            expectedsInString,
                            comments,
                            (int) testCase["updated_on"]
                        );
                        
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

                        testCasesCountLbl.Invoke((MethodInvoker)delegate
                        {
                            testCasesCountLbl.Text = (++testCasesCount).ToString();
                        });
                    }
                    catch (Exception ex)
                    {
                        exceptions.Enqueue(ex);
                    }
                });

                if (exceptions.Count > 0)
                {
                    try
                    {
                        throw new AggregateException(exceptions);
                    }
                    catch (AggregateException exs)
                    {
                        foreach (var ex in exs.InnerExceptions)
                        {
                            Program.LogException(ex);
                        }
                        MessageBox.Show(@"Cannot update DB properly, see log file.");
                    }
                }

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
            try
            {
                var result = dbs.GetAllTestCasesByKeyword(this.SelectedSuites, searchTxt.Text, _youTrackTestCases);
                foreach (var testCase in result)
                {
                    object[] row =
                    {
                        Suites[testCase.SuiteId],
                        testCase.Id.ToString(),
                        testCase.SectionName,
                        testCase.Title,
                        testCase.CustomCustomOriginalId,
                        testCase.CustomCustomTestTypeName,
                        testCase.CustomCustomTagsName,
                        testCase.CustomCustomStatusName,
                        testCase.CustomAssigneeName
                    };
                    testCasesDataGridView.Invoke((MethodInvoker) delegate
                    {
                        testCasesDataGridView.Rows.Add(row);
                        this.testCasesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    });
                }
                testCasesDataGridView.Invoke((MethodInvoker) delegate
                {
                    foundTestCasesCountLbl.Text = testCasesDataGridView.Rows.Count.ToString();
                });
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show(@"Cannot search test cases in DB, see log file.");
            }

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
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
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
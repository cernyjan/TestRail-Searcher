using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiteDB;

namespace TestRail_Searcher
{
    class DatabaseServer
    {
        readonly LiteCollection<Administrator> _administratorCollection;
        readonly LiteCollection<TestCase> _testCaseCollection;
        readonly LiteDatabase _database;

        public DatabaseServer(string filePath, string collectionName)
        {
            this._database = new LiteDatabase(filePath);
            switch (collectionName)
            {
                case "Administrator":
                    this._administratorCollection = _database.GetCollection<Administrator>(collectionName);
                    this._administratorCollection.EnsureIndex(x => x.Id, true);
                    this._administratorCollection.EnsureIndex(x => x.Login, true);
                    break;
                case "TestCases":
                    this._testCaseCollection = _database.GetCollection<TestCase>(collectionName);
                    this._testCaseCollection.EnsureIndex(x => x.Id, true);
                    this._testCaseCollection.EnsureIndex(x => x.CustomCustomOriginalId, "LOWER($.CustomCustomOriginalId)");
                    this._testCaseCollection.EnsureIndex(x => x.Title, "LOWER($.Title)");
                    this._testCaseCollection.EnsureIndex(x => x.SectionName, "LOWER($.SectionName)");
                    this._testCaseCollection.EnsureIndex(x => x.SuiteName, "LOWER($.SuiteName)");
                    break;
                default:
                    throw new NotImplementedException
                        ("wrong collection name");
            }
        }

        public void DeleteAllDocuments(string collectionName)
        {
            _database.DropCollection(collectionName);
        }

        public void InsertDocument(object document)
        {
            switch (document.GetType().Name)
            {
                case "Administrator":
                    this._administratorCollection.Insert((Administrator)document);
                    break;
                case "TestCase":
                    this._testCaseCollection.Insert((TestCase)document);
                    break;
                default:
                    throw new NotImplementedException
                        ("wrong document type");
            }
        }

        public void UpdateDocument(object document)
        {
            switch (document.GetType().Name)
            {
                case "Administrator":
                    this._administratorCollection.Update((Administrator)document);
                    break;
                case "TestCase":
                    this._testCaseCollection.Update((TestCase)document);
                    break;
                default:
                    throw new NotImplementedException
                        ("wrong document type");
            }
        }

        public bool DocumentExists(string collectionName, int id)
        {
            switch (collectionName)
            {
                case "Administrator":
                    var admin = this._administratorCollection.FindById(id);
                    return admin != null;
                case "TestCases":
                    var testCase = this._testCaseCollection.FindById(id);
                    return testCase != null;
                default:
                    throw new NotImplementedException
                        ("wrong collection name");
            }
        }

        public bool IsTestCaseUpdatable(int id, int updatedOn)
        {
            var result = this._testCaseCollection.FindById(id);
            return result.UpdatedOn < updatedOn;
        }

        public IEnumerable<TestCase> GetAllTestCasesByKeyword(List<string> suites, string keyword, bool yt = false)
        {
            if (yt)
            {
                List<string> keywords = keyword.Split(',').ToList().ConvertAll(d => d.ToLower().Trim());
                var results = this._testCaseCollection.Find(testCase =>
                    (suites.IndexOf(testCase.SuiteId.ToString()) > -1) && keywords.Any(k => testCase.CustomCustomOriginalId.Contains(k)));
                return results;
            }
            if (keyword.Length > 2)
            {
                if (keyword[0].Equals('"') && keyword[keyword.Length - 1].Equals('"'))
                {
                    keyword = keyword.Trim('"').ToLower();
                    var results = this._testCaseCollection.Find(x => 
                        (suites.IndexOf(x.SuiteId.ToString()) > -1) && 
                        (x.Id.ToString().Contains(keyword)
                        || x.CustomCustomOriginalId.Contains(keyword)
                        || x.Title.Contains(keyword)
                        || x.SuiteName.Contains(keyword)
                        || x.SectionName.Contains(keyword)
                        || x.CustomNotes.Contains(keyword)
                        || x.CustomPreconds.Contains(keyword)
                        || x.CustomSteps.Contains(keyword)
                        || x.CustomExpecteds.Contains(keyword)
                        || x.CustomCustomComments.Contains(keyword)
                    ));
                    return results;
                }
                else
                {
                    List<string> keywords = keyword.Split(' ').ToList().ConvertAll(d => d.ToLower().Trim());
                    var results = this._testCaseCollection.Find(testCase => 
                        (suites.IndexOf(testCase.SuiteId.ToString()) > -1) && 
                        (IdContainsKeyword(keywords, testCase)
                        || keywords.Any(k => testCase.CustomCustomOriginalId.Contains(k))
                        || keywords.Any(k => testCase.Title.Contains(k))
                        || keywords.Any(k => testCase.SuiteName.Contains(k))
                        || keywords.Any(k => testCase.SectionName.Contains(k))
                        || keywords.Any(k => testCase.CustomNotes.Contains(k))
                        || keywords.Any(k => testCase.CustomPreconds.Contains(k))
                        || keywords.Any(k => testCase.CustomSteps.Contains(k))
                        || keywords.Any(k => testCase.CustomExpecteds.Contains(k))
                        || keywords.Any(k => testCase.CustomCustomComments.Contains(k))
                    ));
                    return results;
                }
            }
            MessageBox.Show(@"Put at least 3 characters (including quotation marks for exact search).");
            return new List<TestCase>();
        }

        static bool IdContainsKeyword(List<string> keywords, TestCase testCase)
        {
            var tcid = testCase.Id.ToString();
            return keywords.Any(k => tcid.Contains(k));
        }

        public int GetTestCasesCount(List<string> suites)
        {
            var result = this._testCaseCollection.Find(x => suites.IndexOf(x.SuiteId.ToString()) > -1).Count();
            return result;
        }

        public Administrator GetAdmin()
        {
            var result = this._administratorCollection.FindOne(x => x.Login != "");
            return result;
        }

        public IEnumerable<Administrator> GetAllAdmin()
        {
            var result = this._administratorCollection.Find(x => x.Login != "");
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace TestRail_Searcher
{
    class DatabaseServer
    {
        readonly LiteCollection<Administrator> _administratorcollection;
        readonly LiteCollection<TestCase> _testCasecollection;
        readonly LiteDatabase _database;

        public DatabaseServer(string filePath, string collectionName)
        {
            this._database = new LiteDatabase(filePath);
            switch (collectionName)
            {
                case "Administrator":
                    this._administratorcollection = _database.GetCollection<Administrator>(collectionName);
                    this._administratorcollection.EnsureIndex(x => x.Id, true);
                    this._administratorcollection.EnsureIndex(x => x.Login, true);
                    break;
                case "TestCases":
                    this._testCasecollection = _database.GetCollection<TestCase>(collectionName);
                    this._testCasecollection.EnsureIndex(x => x.Id, true);
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
                    this._administratorcollection.Insert((Administrator)document);
                    break;
                case "TestCase":
                    this._testCasecollection.Insert((TestCase)document);
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
                    this._administratorcollection.Update((Administrator)document);
                    break;
                case "TestCase":
                    this._testCasecollection.Update((TestCase)document);
                    break;
                default:
                    throw new NotImplementedException
                        ("wrong document type");
            }
        }

        public bool IsTestCaseUpdatable(int id, int updatedOn)
        {
            var result = this._testCasecollection.FindById(id);
            return result.UpdatedOn < updatedOn;
        }

        public IEnumerable<TestCase> GetAllTestCasesByKeyword(List<string> suites, string keyword)
        {
            keyword = keyword.ToLower();
            var results =  this._testCasecollection.Find(x => (suites.IndexOf(x.SuiteId.ToString()) > -1) && (x.Id.ToString().Contains(keyword) 
            || x.CustomCustomOriginalId.Contains(keyword)
            || x.Title.Contains(keyword)
            || x.CustomNotes.Contains(keyword)
            || x.CustomPreconds.Contains(keyword)
            || x.CustomSteps.Contains(keyword)
            || x.CustomExpecteds.Contains(keyword)
            || x.CustomCustomComments.Contains(keyword)
            ));
            return results;
        }

        public int GetTestCasesCount(List<string> suites)
        {
            var result = this._testCasecollection.Find(x => suites.IndexOf(x.SuiteId.ToString()) > -1).Count();
            return result;
        }

        public Administrator GetAdmin()
        {
            var result = this._administratorcollection.FindOne(x => x.Login != "");
            return result;
        }

        public IEnumerable<Administrator> GetAllAdmin()
        {
            var result = this._administratorcollection.Find(x => x.Login != "");
            return result;
        }
    }
}

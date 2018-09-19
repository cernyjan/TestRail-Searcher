using System.Collections.Generic;

namespace TestRail_Searcher
{
    class TestCase
    {
        public int Id { get; set; }
        public string CustomCustomOriginalId { get; set; }
        public string Title { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int MilestoneId { get; set; }
        public int SuiteId { get; set; }
        public string SuiteName { get; set; }
        public int CustomCustomStatusId { get; set; }
        public string CustomCustomStatusName { get; set; }
        public List<int> CustomCustomTestTypeIds { get; set; }
        public string CustomCustomTestTypeName { get; set; }
        public List<int> CustomCustomTagsIds { get; set; }
        public string CustomCustomTagsName { get; set; }
        public int CustomAssigneeId { get; set; }
        public string CustomAssigneeName { get; set; }
        public string CustomNotes { get; set; }
        public string CustomPreconds { get; set; }
        public string CustomSteps { get; set; }
        public string CustomExpecteds { get; set; }
        public string CustomCustomComments { get; set; }
        public int UpdatedOn { get; set; }

        public void SetProperties(
            int id, 
            string customCustomOriginalId, 
            string title, 
            int sectionId, 
            string sectionName, 
            int? milestoneId, 
            int suiteId, 
            string suiteName, 
            int customCustomStatusId,
            string customCustomStatusName,
            List<int> customCustomTestTypeIds,
            string customCustomTestTypeName,
            List<int> customCustomTagsIds,
            string customCustomTagsName,
            int customAssigneeId,
            string customAssigneeName,
            string customNotes, 
            string customPreconds, 
            string customSteps, 
            string customExpecteds, 
            string customCustomComments, 
            int updatedOn)
        {
            Id = id;
            CustomCustomOriginalId = customCustomOriginalId;
            Title = string.IsNullOrEmpty(title) ? "" : title;
            SectionId = sectionId;
            SectionName = sectionName;
            if (milestoneId == null)
            {
                MilestoneId = -1;
            }
            else
            {
                MilestoneId = (int) milestoneId;
            }
            SuiteId = suiteId;
            SuiteName = suiteName;
            CustomCustomStatusId = customCustomStatusId;
            CustomCustomStatusName = customCustomStatusName;
            CustomCustomTestTypeIds = customCustomTestTypeIds;
            CustomCustomTestTypeName = customCustomTestTypeName;
            CustomCustomTagsIds = customCustomTagsIds;
            CustomCustomTagsName = customCustomTagsName;
            CustomAssigneeId = customAssigneeId;
            CustomAssigneeName = customAssigneeName;
            CustomNotes = string.IsNullOrEmpty(customNotes) ? "" : customNotes;
            CustomPreconds = string.IsNullOrEmpty(customPreconds) ? "" : customPreconds;
            CustomSteps = string.IsNullOrEmpty(customSteps) ? "" : customSteps;
            CustomExpecteds = string.IsNullOrEmpty(customExpecteds) ? "" : customExpecteds;
            CustomCustomComments = string.IsNullOrEmpty(customCustomComments) ? "" : customCustomComments;
            UpdatedOn = updatedOn;
        }
    }
}

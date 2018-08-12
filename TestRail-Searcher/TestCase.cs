namespace TestRail_Searcher
{
    class TestCase
    {
        public int Id { get; set; }
        public string CustomCustomOriginalId { get; set; }
        public string Title { get; set; }
        public int SectionId { get; set; }
        public int MilestoneId { get; set; }
        public int SuiteId { get; set; }
        public string CustomNotes { get; set; }
        public string CustomPreconds { get; set; }
        public string CustomSteps { get; set; }
        public string CustomExpecteds { get; set; }
        public string CustomCustomComments { get; set; }
        public int UpdatedOn { get; set; }

        public void SetProperties(int id, string customCustomOriginalId, string title, int sectionId, int? milestoneId, int suiteId, string customNotes, string customPreconds, string customSteps, string customExpecteds, string customCustomComments, int updatedOn)
        {
            Id = id;
            CustomCustomOriginalId = customCustomOriginalId;
            Title = string.IsNullOrEmpty(title) ? "" : title;
            SectionId = sectionId;
            if (milestoneId == null)
            {
                MilestoneId = -1;
            }
            else
            {
                MilestoneId = (int) milestoneId;
            }
            SuiteId = suiteId;
            CustomNotes = string.IsNullOrEmpty(customNotes) ? "" : customNotes;
            CustomPreconds = string.IsNullOrEmpty(customPreconds) ? "" : customPreconds;
            CustomSteps = string.IsNullOrEmpty(customSteps) ? "" : customSteps;
            CustomExpecteds = string.IsNullOrEmpty(customExpecteds) ? "" : customExpecteds;
            CustomCustomComments = string.IsNullOrEmpty(customCustomComments) ? "" : customCustomComments;
            UpdatedOn = updatedOn;
        }
    }
}

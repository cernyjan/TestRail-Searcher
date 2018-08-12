namespace TestRail_Searcher
{
    class Administrator
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Login { get; set; }
        public int ProjectId { get; set; }

        public void SetProperties(int id, string server, string login, int projectId)
        {
            Id = id;
            Server = server;
            Login = login;
            ProjectId = projectId;
        }
    }
}

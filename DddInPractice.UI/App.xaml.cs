namespace DddInPractice.Logic.UI
{
    public partial class App
    {
        public App()
        {
            Initializer.Init(@"Server=(localdb)\MSSQLLocalDB;Database=DddInPractice;Trusted_Connection=true");
        }
    }
}

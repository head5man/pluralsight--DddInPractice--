using DddInPractice.Logic;
using DddInPractice.Logic.Management;
using FluentAssertions;
using Xunit;

namespace DddInPractice.Tests
{
    public class TemporaryTests
    {
        [Fact(Skip = "Temporary testing")]
        public void Test()
        {
            SessionFactory.Init(@"Server=(localdb)\MSSQLLocalDB;Database=DddInPractice;Trusted_Connection=true");
            //SessionFactory.Init(@"Server=.;Database=DddInPractice;Trusted_Connection=true");

            HeadOfficeInstance.Init();

            var head = HeadOfficeInstance.Instance;
            head.Id.Should().Be(1L);
        }
    }
}

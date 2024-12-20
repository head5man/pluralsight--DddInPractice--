using Xunit;
using FluentAssertions;
using DddInPractice.Logic.SnackMachine;

namespace DddInPractice.Logic.Integration.Tests
{
    public class SnackIntegration
    {
        [Fact]
        public void Snack_constants_equal_the_database_values()
        {
            SessionFactory.Init(@"Server=(localdb)\MSSQLLocalDB;Database=DddInPractice;Trusted_Connection=true");

            var repository = new SnackRepository();
            var snack1 = repository.GetById(1);
            var snack2 = repository.GetById(2);
            var snack3 = repository.GetById(3);

            snack1.Should().Be(Snack.Chocolate);
            snack1.Name.Should().Be(Snack.Chocolate.Name);
            snack2.Should().Be(Snack.Soda);
            snack2.Name.Should().Be(Snack.Soda.Name);
            snack3.Should().Be(Snack.Gum);
            snack3.Name.Should().Be(Snack.Gum.Name);
        }

        [Fact]
        public void None_should_not_exist_in_the_database()
        {
            SessionFactory.Init(@"Server=(localdb)\MSSQLLocalDB;Database=DddInPractice;Trusted_Connection=true");

            var repository = new SnackRepository();
            repository.GetById(0).Should().BeNull();
        }
    }
}
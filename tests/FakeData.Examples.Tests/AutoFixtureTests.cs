using AutoFixture;

namespace FakeData.Examples.Tests
{
    public class AutoFixtureTests
    {
        [Fact]
        public void Person()
        {
            // Create a Fixture instance for generating auto-mocked data
            var fixture = new Fixture();

            // Generate a person with auto-mocked data
            var person = fixture.Build<Person>()
                .With(x => x.Email, "test@email.com")
                .Create();

            Console.WriteLine(person);
        }
    }
}
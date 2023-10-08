using AutoFixture;
using System.Text.Json;
using Xunit.Abstractions;

namespace FakeData.Examples.Tests
{
    public class AutoFixtureTests
    {
        private readonly ITestOutputHelper outputHelper;

        public AutoFixtureTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }


        [Fact]
        public void Person()
        {
            // Create a Fixture instance for generating auto-mocked data
            var fixture = new Fixture();

            // Generate a person with auto-mocked data
            var person = fixture.Build<Person>()
                .With(x => x.Email, "test@email.com")
                .Create();

            outputHelper.WriteLine(JsonSerializer.Serialize(person));
        }
    }
}
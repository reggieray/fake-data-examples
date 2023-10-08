using Bogus;
using System.Runtime.Serialization;
using System.Text.Json;
using Xunit.Abstractions;

namespace FakeData.Examples.Tests
{
    public class BogusTests
    {
        private readonly ITestOutputHelper outputHelper;

        public BogusTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void Person()
        {
            // Create a Faker instance for generating fake data
            var faker = new FakeDataGenerator();

            // Generate a fake person
            var fakePerson = faker.Person.Generate();

            outputHelper.WriteLine(JsonSerializer.Serialize(fakePerson));
        }
    }

    public class FakeDataGenerator
    {
        public Faker<Person> Person;
        public Faker<Address> Address;

        public FakeDataGenerator() 
        {
            Address = new Faker<Address>()
                .WithRecord()
                .StrictMode(true)
                .RuleFor(o => o.Line1, f => f.Address.StreetAddress())
                .RuleFor(o => o.Line2, f => f.Address.SecondaryAddress())
                .RuleFor(o => o.Town, f => f.Address.City())
                .RuleFor(o => o.PostCode, f => f.Address.ZipCode())
                .RuleFor(o => o.Country, f => f.Address.Country());

            Person = new Faker<Person>()
                .WithRecord()
                .StrictMode(true)
                .RuleFor(o => o.FirstName, f => f.Person.FirstName)
                .RuleFor(o => o.LastName, f => f.Person.LastName)
                .RuleFor(o => o.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(o => o.Address, Address.Generate());
        }
    }

    public static class ExtensionsForBogus
    {
        public static Faker<T> WithRecord<T>(this Faker<T> faker) where T : class
        {
            faker.CustomInstantiator(_ => FormatterServices.GetUninitializedObject(typeof(T)) as T);
            return faker;
        }
    }
}
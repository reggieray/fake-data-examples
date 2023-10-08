# Introduction

A small comparison of two popular fake or mock data nuget libraries. 

# Statistics

| Comparison 8/10/2023      | Bogus                                         | AutoFixture                                          |
| ------------------------- | --------------------------------------------- | ---------------------------------------------------- |
| Source Link               | [Github](https://github.com/bchavez/Bogus)    | [Github](https://github.com/AutoFixture/AutoFixture) |
| NuGet Link                | [Nuget](https://www.nuget.org/packages/Bogus) | [Nuget](https://www.nuget.org/packages/AutoFixture)  |
| NuGet Downloads (Total)   | 55.8M                                         | 116.6M                                               |
| NuGet Downloads (Per Day) | 18.3K                                         | 25.1K                                                |
| Github Stars              | 7.7k                                          | 3.1k                                                 |
| Github Watchers           | 125                                           | 94                                                   |
| Github Forks              | 450                                           | 339                                                  |

# Example Usage

The classes used to populate fake data with are the following records. 

```csharp
public record Address(string Line1, string Line2, string Town, string PostCode, string Country);

public record Person(string FirstName, string LastName, string Email, Address Address);
```

## Bogus 

Test code setup.

```csharp
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
```

Example output

```json
{
  "FirstName": "Carole",
  "LastName": "Fay",
  "Email": "Carole.Fay@hotmail.com",
  "Address": {
    "Line1": "678 Geovany Forest",
    "Line2": "Suite 413",
    "Town": "Jeremychester",
    "PostCode": "20819",
    "Country": "Venezuela"
  }
}
```

## AutoFixture

Test code setup

```csharp
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
```
Example output
```json
{
  "FirstName": "FirstNamef1b2b680-0a79-4015-96bf-8b36a39ec79d",
  "LastName": "LastName2af92601-cc8e-4bf2-b458-21abf995fb0a",
  "Email": "test@email.com",
  "Address": {
    "Line1": "Line16a0323fa-4db8-4e70-adc3-8091e5408618",
    "Line2": "Line2527e5086-140c-4201-b361-2ac2952acf4d",
    "Town": "Towndfd93b2f-1728-47e8-86a0-d0bb55a1b1ae",
    "PostCode": "PostCodeadf7d835-9249-42f0-b82a-0b80002084f5",
    "Country": "Countrye942137a-8641-4f1c-a362-b2761903fb3e"
  }
}
```
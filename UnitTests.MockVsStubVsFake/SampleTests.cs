using Moq;

namespace UnitTests.MockVsStubVsFake;

public class SampleTests
{
    private const string JohnSmith = "John Smith";
    private const int JohnSmithId = 999;

    [Fact]
    public void MockSample()
    {
        // Arrange
        var personRepoMock = new Mock<IPersonRepository>();
        personRepoMock.Setup(m => m.Add(It.Is<string>(s => s == JohnSmith))).Verifiable();
        personRepoMock.Setup(m => m.GetId(It.Is<string>(s => s == JohnSmith))).Returns(JohnSmithId).Verifiable();
        var personService = new PersonService(personRepoMock.Object);

        // Act
        var personId = personService.AddPerson(JohnSmith);

        // Assert
        personRepoMock.Verify();
        Assert.Equal(JohnSmithId, personId);
    }

    [Fact]
    public void StubSample()
    {
        // Arrange
        var personService = new PersonService(new StubPersonRepository());

        // Act
        var personId = personService.AddPerson(JohnSmith);

        // Assert
        Assert.Equal(1, personId);
    }

    [Fact]
    public void FakeSample()
    {
        // Arrange
        var fakePersonRepo = new FakePersonRepository();
        var personService = new PersonService(fakePersonRepo);

        // Act
        var personId = personService.AddPerson(JohnSmith);

        // Assert
        Assert.Equal(0, personId);
        Assert.Contains(JohnSmith, fakePersonRepo.People);
    }
}

public class FakePersonRepository : IPersonRepository
{
    public List<string> People = new();

    public void Add(string name)
    {
        if (People.IndexOf(name) == -1)
        {
            People.Add(name);
        }
    }

    public int GetId(string name) => People.FindIndex(person => person == name);
}


public class StubPersonRepository : IPersonRepository
{
    public void Add(string name)
    {
        // nothing  
    }

    public int GetId(string name) => 1;
}
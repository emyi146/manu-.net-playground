namespace UnitTests.MockVsStubVsFake;

public interface IPersonRepository
{
    void Add(string name);
    int GetId(string name);
}

namespace UnitTests.MockVsStubVsFake;

public class PersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public int AddPerson(string name)
    {
        _personRepository.Add(name);
        return _personRepository.GetId(name);
    }
}

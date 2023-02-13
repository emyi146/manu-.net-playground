using MessageProcessingMediatR.Customers.Api.Contracts.Data;
using MessageProcessingMediatR.Customers.Api.Domain;

namespace MessageProcessingMediatR.Customers.Api.Mapping;

public static class DtoToDomainMapper
{
    public static Customer ToCustomer(this CustomerDto customerDto)
    {
        return new Customer
        {
            Id = customerDto.Id,
            Email = customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth
        };
    }
}

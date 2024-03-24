// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
namespace ImmutableAnalyzer.Sample;

// If you don't see warnings, build the Analyzers Project.
public enum UserRole
{
    Admin,
    Moderator,
    Customer
}

[Immutable]
public class UserDto
{
    public int Id { get; set; }

    public string Name { get; init; } = string.Empty;

    public List<int> FriendsIds { get; init; } = new();

    public UserRole Role { get; set; } = UserRole.Customer;

    public OrganizationDto Organization { get; init; } = new();
}

[Immutable]
public class OrganizationDto
{
    public string Name { get; set; } = string.Empty;

    public IList<string> PhoneNumbers { get; set; } = new List<string>();

    public IDictionary<int, string> Departments { get; set; } = new Dictionary<int, string>();
}
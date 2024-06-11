// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System;
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
    /// <summary>
    /// IM0002 - immutable class property can't have a public set accessor.
    /// </summary>
    public int Id { get; set; }

    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// IM0001 - immutable class property can't have a mutable type.
    /// </summary>
    public List<int> FriendsIds { get; init; } = new();

    /// <summary>
    /// immutable class property can be an enum.
    /// </summary>
    public UserRole Role { get; init; } = UserRole.Customer;

    /// <summary>
    /// immutable class property can be an array of immutable type.
    /// </summary>
    public OrganizationDto[] Organization { get; init; } = Array.Empty<OrganizationDto>();

    /// <summary>
    /// IM0001 - immutable class property can't be an array of mutable type.
    /// </summary>
    public PhoneNumber[] PhoneNumbers { get; init; } = Array.Empty<PhoneNumber>();
}

[Immutable]
public struct OrganizationDto
{
    public OrganizationDto() { }

    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// IM0001 - immutable class property can't have a mutable type.
    /// IM0002 - immutable class property can't have a public set accessor.
    /// </summary>
    public IList<string> PhoneNumbers { get; set; } = new List<string>();

    /// <summary>
    /// IM0001 - immutable class property can't have a mutable type.
    /// IM0002 - immutable class property can't have a public set accessor.
    /// </summary>
    public IDictionary<int, string> Departments { get; set; } = new Dictionary<int, string>();

    public IReadOnlyList<int> BuildingIds { get; init; } = new List<int>();
}

// ReSharper disable once ClassNeverInstantiated.Global
public record PhoneNumber
{
    public string CountryCode { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;
}

/// <summary>
/// IM0003 - immutable record can't have a mutable parameter.
/// </summary>
[Immutable]
// ReSharper disable once NotAccessedPositionalProperty.Global
public record PetInfo(List<string> Toys);

[Immutable]
public interface IPaginationSpecification
{
    int Page { get; }

    int PerPage { get; }
}

[Immutable]
public class UserSpecification : IPaginationSpecification
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int Page { get; init; }
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int PerPage { get; init; }

    /// <summary>
    /// IM0001 - immutable class property can't have a mutable type.
    /// </summary>
    public IEnumerable<string> Organization { get; init; } = Array.Empty<string>();
}
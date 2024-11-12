using Refit;
using System.Text.Json.Serialization;
using System.Text.Json;
using Tamir.SharpSsh.jsch;
using Planet.Core.Shared.Response;
using Planet.Core.Shared.Request;

namespace SonbsTest;

public interface IGaravotApi
{
    // no auth
    [Get("/api/v1/delegates/get_all_frontend")]
    Task<ApiResponse<DelegateFeDto>> GetDelegatesAsync(CancellationToken cancellation = default);

    // no auth
    [Get("/api/v1/delegategroups/get_all_frontend")]
    Task<ApiResponse<SearchResponse<DelegateGroupFeDto>>> GetDelegateGroupsAsync(CancellationToken cancellation = default);

    // auth
    [Get("/api/v1/delegategroupdelegates/search")]
    Task<ApiResponse<SearchResponse<GovernmentBodyDelegateDto>>> GetDelegateGroupDelegatesAsync(DelegateGroupDelegateSearchRequest request, CancellationToken cancellation = default);
}

public static class GaravotApiFactory
{
    public static IGaravotApi Create(HttpClient http)
        => RestService.For<IGaravotApi>(http, _refitSettings);

    private static readonly RefitSettings _refitSettings = new RefitSettings
    {
        ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { (JsonConverter)new JsonStringEnumConverter() }
        })
    };
}

public sealed class DelegateFeDto
{
    public ICollection<int> DelegateGroupIds { get; set; } = new List<int>();
    public long DelegateId { get; set; }
    //public string ExternalId { get; set; }
    public string FirstName { get; set; }
    //public ICollection<int> GovernmentBodyIds { get; set; } = new List<int>();
    public string Image { get; set; }
    public string LastName { get; set; }
    public ICollection<int> LegislatureIds { get; set; } = new List<int>();
}

public sealed class GovernmentBodyDelegateDto : AbpAudited
{
    public DelegateDto Delegate { get; set; }
    public int DelegateId { get; set; }
    public DateTimeOffset? End { get; set; }
    public string EndReason { get; set; }
    public string ExternalId { get; set; }
    public GovernmentBodyDto GovernmentBody { get; set; }
    public long GovernmentBodyDelegateId { get; set; }
    public int GovernmentBodyId { get; set; }
    public bool IsSynchronized { get; set; }
    public string Role { get; set; }
    public DateTimeOffset? Start { get; set; }
}
public class AbpAudited
{
    public UserInfo? Creator { get; set; }
    public UserInfo? LastModifier { get; set; }
    public UserInfo? Deleter { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTime? DeletionTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? CreatorId { get; set; }
}
public sealed class DelegateDto : AbpAudited
{
    public string DelegateCardNumber { get; set; }
    public bool DelegateCanBeDeleted { get; set; }
    public string DelegateExternalId { get; set; }
    public bool DelegateEnabled { get; set; }
    public string DelegateKey { get; set; }
    public DelegateType DelegateType { get; set; }
    public bool DelegateIsSynchronized { get; set; }
    public string Address { get; set; }
    public bool Adult { get; set; } = true;
    public string Biography { get; set; }
    public DateTime? Birthday { get; set; }
    public bool CanBeDeleted { get; set; }
    public string City { get; set; }
    public string Company { get; set; }
    public string Country { get; set; }
    public DateTimeOffset? Deathday { get; set; }
    public string ExternalId { get; set; }
    public string FirstName { get; set; }
    public Gender Gender { get; set; }
    public string Homepage { get; set; }
    public string Image { get; set; }
    public bool IsSynchronized { get; set; }
    public string Key { get; set; }
    public Language Language { get; set; }
    public string LastName { get; set; }
    public string Occupation { get; set; }
    public long PersonId { get; set; }
    public PersonState PersonState { get; set; }
    public string Phone { get; set; }
    public string PlaceOfBirth { get; set; }
    public string ProvinceOfBirth { get; set; }
    public UserInfo User { get; set; }
}
public enum Gender : short
{
    Unknown = 0,
    Female = 1,
    Male = 2
}
public enum Language { it, fr, en, es, de, ja, zh, ar }

public enum PersonState : short
{
    Disabled = 0,
    Active = 1,
    Awaiting = 2,
    Cancelled = 3,
    Expired = 4,
    Suspended = 5,
}
public enum DelegateType : short
{
    Undefined = 0,
    Councilor = 1,
    President = 2,
    VicePresident = 3,
    Assessor = 4,
    Secretary = 5,
    UnderSecretary = 6,
    RegionPresident = 7,
    Senator = 8,
    Deputy = 9,
}
public sealed class GovernmentBodyDto : AbpAudited
{
    public string Acronym { get; set; }
    public bool CanBeDeleted { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public DateTimeOffset? End { get; set; }
    public string EndReason { get; set; }
    public string ExternalId { get; set; }
    public int GovernmentBodyId { get; set; }
    public GovernmentBodyType? GovernmentBodyType { get; set; }
    public string Image { get; set; }
    public bool IsSynchronized { get; set; }
    public LegislatureDto Legislature { get; set; }
    public int LegislatureId { get; set; }
    public string Link { get; set; }
    public string Name { get; set; }
    public DateTimeOffset? Start { get; set; }
}
public enum GovernmentBodyType : short
{
    Undefined = 0,
    RegionalCouncilGroup = 1,
    RegionalCouncilBureau = 2,
    RegionalCommittee = 3,
    ElectionsCommittee = 5,
    SpecialCommission = 6,
    CommissionOfInquiry = 7,
    RulesCommittee = 8,
    JointCommitteeOnControlAndEvaluation = 9,
    StandingCommission = 10,
    ConferenceOfPresidents = 11,
    SubCommission = 12,
    RegionalCouncil = 20,
    MunicipalCouncilGroup = 21,
    MunicipalCommittee = 22,
    MunicipalCouncil = 23,
}
public class LegislatureDto : AbpAudited
{
    public bool CanBeDeleted { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public DateTimeOffset? End { get; set; }
    public string EndReason { get; set; }
    public string ExternalId { get; set; }
    public bool IsPublic { get; set; }
    public bool IsSynchronized { get; set; }
    public int LegislatureId { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public string RomanNumeral { get; set; }
    public DateTimeOffset? Start { get; set; }
}
public sealed class DelegateGroupFeDto
{
    public string Acronym { get; set; }
    public int DelegateGroupId { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int LegislatureId { get; set; }
    public string Name { get; set; }
}
public sealed class DelegateGroupDelegateSearchRequest
{
    public DelegateGroupDelegateSearchFilters Filters { get; set; }
    public bool IncludeAggregations { get; set; }
    public IEnumerable<SortCriteria<DelegateGroupDelegateSortField>> SortBy { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Skip { get; set; }
}
public sealed class DelegateGroupDelegateSearchFilters
{
    public int? DelegateGroupId { get; set; }
    public long? DelegateId { get; set; }
    public string ExternalId { get; set; }
    public DateTimeOffset? From { get; set; }
    public int? GovernmentBodyId { get; set; }
    public IEnumerable<GovernmentBodyType> GovernmentBodyType { get; set; }
    public bool? IsSynchronized { get; set; }
    public DateTimeOffset? To { get; set; }
    public Language Language { get; set; }
    public string? Query { get; set; }
}
public enum DelegateGroupDelegateSortField : short
{
    DelegateGroupName,
    DelegateLastName,
    Start,
}

namespace RebTelAssignment.Domain.Models.BaseModels;

public class SimpleAuditEntity : IBaseEntity<long>, ICreatedEntity, IModifiedEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}
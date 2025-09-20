namespace RebTelAssignment.Domain.Models.BaseModels;

public abstract class BaseEntity : IBaseEntity<long>
{
    public long Id { get; set; }
}
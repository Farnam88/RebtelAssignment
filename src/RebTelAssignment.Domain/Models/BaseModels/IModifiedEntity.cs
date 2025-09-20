namespace RebTelAssignment.Domain.Models.BaseModels;

/// <summary>
/// Base Update Entity Interface for entities that requires UpdateDateTime.
/// </summary>
public interface IModifiedEntity:IEntityFlag
{
    /// <summary>
    /// The date time of which the entity was updated
    /// </summary>
    DateTime? LastModifiedAt { get; set; }
}
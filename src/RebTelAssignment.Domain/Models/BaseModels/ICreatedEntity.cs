namespace RebTelAssignment.Domain.Models.BaseModels;

/// <summary>
/// Base Creation Entity Interface for entities that requires CreateDateTime.
/// </summary>
public interface ICreatedEntity:IEntityFlag
{
    /// <summary>
    /// The date time of which the entity was created
    /// </summary>
    DateTime CreatedAt { get; set; }
}
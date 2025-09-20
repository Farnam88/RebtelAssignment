namespace RebTelAssignment.Domain.Models.BaseModels;

/// <summary>
/// Base Entity Interface with Identifier of type TKey
/// </summary>
/// <typeparam name="TKey">Type of the Id</typeparam>
public interface IBaseEntity<TKey>:IEntityFlag
{
    /// <summary>
    /// Identifier
    /// </summary>
    public TKey Id { get; set; }
}
namespace RebTelAssignment.Domain.Models.BaseModels;

/// <summary>
/// A Concurrency Interface fo entities that requires Concurrency handling
/// </summary>
public interface IConcurrentEntity:IEntityFlag
{
    /// <summary>
    /// RowVersion for concurrency
    /// </summary>
    byte[] RowVersion { get; set; }
}
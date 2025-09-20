using RebTelAssignment.Domain.Models.BaseModels;
using RebTelAssignment.Domain.Models.Enums;

namespace RebTelAssignment.Domain.Models;

public class LoanSetting : SimpleAuditEntity
{
    public LoanDurationUnitType LoanDurationUnitType { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }
}
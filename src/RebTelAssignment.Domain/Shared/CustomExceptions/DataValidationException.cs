using RebTelAssignment.Domain.Shared.CustomExceptions.Shared;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebTelAssignment.Domain.Shared.CustomExceptions;

public sealed class DataValidationException(
    string displayMessage = ExceptionDisplayErrorMessages.DataValidationException,
    string exceptionMessage = ExceptionErrorMessages.DataValidationException,
    IReadOnlyList<ErrorDetail> errorDetails = null!,
    IReadOnlyList<MetaData> metaData = null!,
    Exception innerException = null!)
    : BaseException(displayMessage, exceptionMessage, errorDetails, metaData, innerException)
{
    protected override long ErrorCode => ExceptionErrorCodes.DataValidationException;
}
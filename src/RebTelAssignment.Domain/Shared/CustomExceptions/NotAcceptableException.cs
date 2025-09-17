using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebTelAssignment.Domain.Shared.CustomExceptions;

public sealed class NotAcceptableException(
    string displayMessage = ExceptionDisplayErrorMessages.NotAcceptableException,
    string exceptionMessage = ExceptionErrorMessages.NotAcceptableException,
    IReadOnlyList<ErrorDetail> errorDetails = null!,
    IReadOnlyList<MetaData> metaData = null!,
    Exception innerException = null!)
    : BaseException(displayMessage, exceptionMessage, errorDetails, metaData, innerException)
{
    protected override long ErrorCode => ExceptionErrorCodes.NotAcceptableException;
}
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebTelAssignment.Domain.Shared.CustomExceptions;

public sealed class ExternalServiceException(
    string displayMessage = ExceptionDisplayErrorMessages.ExternalServiceException,
    string exceptionMessage = ExceptionErrorMessages.ExternalServiceException,
    IReadOnlyList<ErrorDetail> errorDetails = null!,
    IReadOnlyList<MetaData> metaData = null!,
    Exception innerException = null!)
    : BaseException(displayMessage, exceptionMessage, errorDetails, metaData, innerException)
{
    protected override long ErrorCode => ExceptionErrorCodes.ExternalServiceException;
}
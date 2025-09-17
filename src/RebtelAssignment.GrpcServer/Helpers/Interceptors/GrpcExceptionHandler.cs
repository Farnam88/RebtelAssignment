using Grpc.Core;
using RebTelAssignment.Domain.Shared.CustomExceptions;

namespace RebtelAssignment.GrpcServer.Helpers.Interceptors;

public static class GrpcExceptionHandler
{
    /// <summary>
    /// Error code key text
    /// </summary>
    private const string ErrorCode = "error_code";

    /// <summary>
    /// Error message key text
    /// </summary>
    private const string ErrorMessage = "error_message";


    public static void Handle<T>(this ServerCallContext context, Exception exception, ILogger<T> logger)
    {
        var res = exception switch
        {
            BusinessLogicException logicException => HandleBusinessException(logicException, logger),
            NullException nullException => HandleNullException(nullException, logger),
            NotFoundException foundException =>
                HandleNotFoundException(foundException, logger),
            DataValidationException validationException => HandleValidationException(validationException,
                logger),
            RpcException rpcException => HandleRpcException(rpcException, logger),
            _ => HandleDefault(exception, logger)
        };
        context.Status = res.Status;
        foreach (var trailer in res.Trailers)
        {
            context.ResponseTrailers.Add(trailer.Key, trailer.Value);
        }
    }

    private static RpcException HandleBusinessException<T>(BusinessLogicException exception,
        ILogger<T> logger)
    {
        Log(logger, exception, LogLevel.Warning, exception.Message);

        return new RpcException(new Status(StatusCode.InvalidArgument, exception.DisplayMessage),
            CreateTrailers(exception), exception.Message);
    }

    private static RpcException HandleNullException<T>(NullException exception,
        ILogger<T> logger)
    {
        Log(logger, exception, LogLevel.Error, exception.Message);

        return new RpcException(new Status(StatusCode.Internal, exception.DisplayMessage),
            CreateTrailers(exception), exception.DisplayMessage);
    }

    private static RpcException HandleNotFoundException<T>(NotFoundException exception,
        ILogger<T> logger)
    {
        Log(logger, exception, LogLevel.Information, exception.Message);

        return new RpcException(new Status(StatusCode.NotFound, exception.DisplayMessage),
            CreateTrailers(exception), exception.Message);
    }

    private static RpcException HandleValidationException<T>(DataValidationException exception,
        ILogger<T> logger)
    {
        Log(logger, exception, LogLevel.Information, exception.Message);

        return new RpcException(new Status(StatusCode.InvalidArgument, exception.DisplayMessage),
            CreateTrailers(exception), exception.Message);
    }

    private static RpcException HandleRpcException<T>(RpcException exception,
        ILogger<T> logger)
    {
        Log(logger, exception, LogLevel.Error, $"gRPC exception: {exception.Message}");

        var responseMetadata = CreateTrailers(exception);

        return new RpcException(exception.Status, responseMetadata, "gRPC error");
    }

    private static RpcException HandleDefault<T>(Exception exception, ILogger<T> logger)
    {
        Log(logger, exception, LogLevel.Error, exception.Message);
        var customException = new InternalServiceException(exception.Message, innerException: exception);
        return new RpcException(new Status(StatusCode.Internal, customException.DisplayMessage),
            CreateTrailers(customException
            ), customException.Message);
    }

    private static Metadata CreateTrailers(BaseException exception)
    {
        var metadata = new Metadata
        {
            { ErrorCode, exception.Code.ToString() },
            { ErrorMessage, exception.DisplayMessage }
        };
        if (exception.ErrorDetails.Count > 0)
        {
            foreach (var detail in exception.ErrorDetails.Where(w => w.Expose).ToList())
            {
                metadata.Add(detail.Title, detail.Value);
            }
        }

        return metadata;
    }

    private static Metadata CreateTrailers(RpcException exception)
    {
        var responseMetadata = new Metadata()
        {
            { ErrorCode, ExceptionErrorCodes.UnhandledException.ToString() }
        };
        foreach (var trailer in exception.Trailers)
        {
            responseMetadata.Add(trailer.Key, trailer.ValueBytes);
        }

        return responseMetadata;
    }


    private static void Log<T>(ILogger<T> logger, Exception ex, LogLevel level, string message)
    {
        switch (level)
        {
            case LogLevel.Warning:
                logger.LogWarning(ex, message);
                return;
            case LogLevel.Information:
                logger.LogInformation(ex, message);
                return;
            default:
                logger.LogError(ex, message);
                return;
        }
    }
}
namespace RebtelAssignment.GrpcServer.Services;

public class LoaningService : LoaningServices.LoaningService.LoaningServiceBase
{
    private readonly ILogger<LoaningService> _logger;
    
    public LoaningService(ILogger<LoaningService> logger)
    {
        _logger = logger;
    }
}
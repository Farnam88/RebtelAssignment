using Grpc.Core;
using InitialProto;

namespace RebtelAssignment.GrpcServer.Services;

public class TestService : Test.TestBase
{
    private readonly ILogger<TestService> _logger;
    
    public TestService(ILogger<TestService> logger)
    {
        _logger = logger;
    }

    public override async Task<TestResponse> Test(TestRequest request, ServerCallContext context)
    {
        return await Task.FromResult(new TestResponse
        {
            Message = "Test 1"
        });
    }
}
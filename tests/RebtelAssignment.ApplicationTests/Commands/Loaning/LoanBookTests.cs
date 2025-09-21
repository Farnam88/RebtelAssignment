using AutoFixture;
using MediatR;
using Moq;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Shared.CustomExceptions;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.ApplicationTests.CommandTests;

public class LoanBookTests
{
    private readonly Fixture _fixture;
    private readonly CancellationToken _ct;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly LoanBookCommandHandler _sut;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;


    public LoanBookTests()
    {
        _fixture = new();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _ct = new CancellationToken();
        _mediatorMock = new();
        _unitOfWorkMock = new();
        _sut = new(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task LoaningBook_Should_Return_NotFound_WhenBatchDoesNotExist_Result()
    {
        //Arrange
        ResultModel<LoanBookResponseDto> expectedResult=ResultModel<LoanBookResponseDto>.Fail(new NotFoundException("Batch not found",errorDetails:new List<ErrorDetail>
        {
            new ("Batch",$"Book Batch with Id 1000 not found")
        }));

        var loanBookRequest = _fixture.Create<LoanBookCommand>();
        loanBookRequest.BatchIds.Add(1000);
        var batchRepoMock=new Mock<IRepository<Batch>>();

        var getBatchesSpec = new GetBatchesSpec(loanBookRequest.BatchIds);

        batchRepoMock.Setup(s => s.ToListAsync(getBatchesSpec, _ct))
            .ReturnsAsync(Enumerable.Empty<BatchDto>())
            .Verifiable();
        
        _unitOfWorkMock.Setup(s => s.Repository<Batch>())
            .Returns(batchRepoMock.Object);

        //Act

        var result = _sut.Handle(loanBookRequest,_ct);

        //Assert
        
        result.ShouldBeEquivalentTo(expectedResult);
        batchRepoMock.Verify(v=>v.ToListAsync(getBatchesSpec, _ct), Times.Once);
        _unitOfWorkMock.Verify(v=>v.Repository<Batch>());
        
    }
}
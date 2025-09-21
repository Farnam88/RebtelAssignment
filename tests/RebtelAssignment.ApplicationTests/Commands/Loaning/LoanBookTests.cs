using Ardalis.Specification;
using AutoFixture;
using MediatR;
using Moq;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Loaning.Commands;
using RebtelAssignment.Application.Core.Loaning.RepositorySpecifications;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Shared.CustomExceptions;
using RebTelAssignment.Domain.Shared.DataWrapper;
using Shouldly;

namespace RebtelAssignment.ApplicationTests.Commands.Loaning;

public class LoanBookTests
{
    private readonly Fixture _fixture;
    private readonly CancellationToken _ct;
    private readonly Mock<IMediator> _mediatorMock;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;


    public LoanBookTests()
    {
        _fixture = new();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        // Autofixture v4 doesn't have support for DateOnly/TimeOnly types
        _fixture.Customize<DateOnly>(composer => composer.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));
        _ct = CancellationToken.None;
        _mediatorMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task LoaningBook_Should_Return_NotFound_WhenBatchDoesNotExist_Result()
    {
        //Arrange
        ResultModel<LoanDto> expectedResult = ResultModel<LoanDto>.Fail(new NotFoundException("Batch not found"));

        var loanBookRequest = _fixture.Create<LoanBookCommand>();

        var batchRepoMock = new Mock<IRepository<Batch>>();

        batchRepoMock.Setup(s => s.ToListAsync<BatchDto>(It.IsAny<BaseSpec<Batch,BatchDto>>(), _ct))
            .ReturnsAsync(new List<BatchDto>())
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Batch>())
            .Returns(batchRepoMock.Object);

        LoanBookCommandHandler sut = new(_unitOfWorkMock.Object);

        //Act

        var result = await sut.Handle(loanBookRequest, _ct);

        //Assert

        result.ShouldBeEquivalentTo(expectedResult);
        batchRepoMock.Verify(v => v.ToListAsync(It.IsAny<BaseSpec<Batch,BatchDto>>(), _ct), Times.Once);
        _unitOfWorkMock.Verify(v => v.Repository<Batch>());
    }
}
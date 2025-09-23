using AutoFixture;
using MapsterMapper;
using Moq;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Loaning.Commands;
using RebtelAssignment.Application.Core.Services;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;
using RebTelAssignment.Domain.Shared.CustomExceptions;
using RebTelAssignment.Domain.Shared.DataWrapper;
using RebTelAssignment.Domain.Shared.Events;
using RebTelAssignment.Domain.Shared.Events.EventMessages;
using RebTelAssignment.Domain.Shared.Extensions;
using Shouldly;

namespace RebtelAssignment.ApplicationTests.Commands.Loaning;

public class LoanBookTests
{
    private readonly Fixture _fixture;
    private readonly CancellationToken _ct;
    private readonly LoanBookCommandHandler _sut;
    private readonly Mock<ILoanDueDateCalculatorService> _dueDateCalculatorMock;
    private readonly Mock<IEventPublisher<LoanCreatedMessage>> _eventPublisher;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public LoanBookTests()
    {
        _fixture = new();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        // Autofixture v4 doesn't have support for DateOnly/TimeOnly types
        _fixture.Customize<DateOnly>(composer => composer.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));
        _ct = CancellationToken.None;
        _unitOfWorkMock = new();
        _dueDateCalculatorMock = new();
        _eventPublisher = new();
        _sut = new LoanBookCommandHandler(_unitOfWorkMock.Object, _dueDateCalculatorMock.Object,
            _eventPublisher.Object);
    }

    [Fact]
    public async Task LoaningBook_Should_Return_NotFound_WhenBatchDoesNotExist()
    {
        //Arrange
        ResultModel<Success> expectedResult = ResultModel<Success>.Fail(new NotFoundException("Batch not found"));

        var loanBookRequest = _fixture.Create<LoanBookCommand>();

        var batchRepoMock = new Mock<IRepository<Batch>>();

        batchRepoMock.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct))
            .ReturnsAsync(new List<Batch>())
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Batch>())
            .Returns(batchRepoMock.Object);

        //Act

        var result = await _sut.Handle(loanBookRequest, _ct);

        //Assert

        result.ShouldBeEquivalentTo(expectedResult);
        batchRepoMock.Verify(v => v.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct), Times.Once);
        _unitOfWorkMock.Verify(v => v.Repository<Batch>());
    }

    [Fact]
    public async Task LoaningBook_Should_Return_BusinessLogicError_WhenBatchHasNoAvailableQuantity()
    {
        //Arrange
        ResultModel<Success> expectedResult =
            ResultModel<Success>.Fail(new BusinessLogicException("Batch has no available quantity"));

        var loanBookRequest = new LoanBookCommand([1], 1);

        var batchRepoMock = new Mock<IRepository<Batch>>();

        var noAvailabilityBatch = _fixture.Create<Batch>();
        noAvailabilityBatch.Quantity = 10;
        noAvailabilityBatch.QuantityLoaned = 3;
        noAvailabilityBatch.QuantityDamaged = 3;
        noAvailabilityBatch.QuantityMissing = 4;

        batchRepoMock.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct))
            .ReturnsAsync(new List<Batch>
            {
                noAvailabilityBatch
            })
            .Verifiable();


        _unitOfWorkMock.Setup(s => s.Repository<Batch>())
            .Returns(batchRepoMock.Object)
            .Verifiable();

        //Act

        var result = await _sut.Handle(loanBookRequest, _ct);

        //Assert

        result.ShouldBeEquivalentTo(expectedResult);
        batchRepoMock.Verify(v => v.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct), Times.Once);

        _unitOfWorkMock.Verify(v => v.Repository<Batch>());
    }

    [Fact]
    public async Task LoaningBook_Should_Return_NotFoundError_WhenMemberIsNotFound()
    {
        //Arrange
        ResultModel<Success> expectedResult =
            ResultModel<Success>.Fail(new NotFoundException("Member not found"));

        var loanBookRequest = new LoanBookCommand([1], 1);

        var batchRepoMock = new Mock<IRepository<Batch>>();

        var batch = _fixture.Create<Batch>();
        batch.Quantity = 10;
        batch.QuantityLoaned = 1;
        batch.QuantityDamaged = 1;
        batch.QuantityMissing = 1;

        batchRepoMock.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct))
            .ReturnsAsync(new List<Batch>
            {
                batch
            })
            .Verifiable();

        var memberRepoMock = new Mock<IRepository<Member>>();
        memberRepoMock.Setup(s =>
                s.FirstOrDefaultAsync(It.IsAny<BaseSpec<Member, MemberDto>>(), _ct))
            .ReturnsAsync((MemberDto?)null)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Batch>())
            .Returns(batchRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Member>())
            .Returns(memberRepoMock.Object)
            .Verifiable();

        //Act

        var result = await _sut.Handle(loanBookRequest, _ct);

        //Assert

        result.ShouldBeEquivalentTo(expectedResult);
        batchRepoMock.Verify(v => v.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct), Times.Once);
        memberRepoMock.Verify(
            v => v.FirstOrDefaultAsync(It.IsAny<BaseSpec<Member, MemberDto>>(), _ct),
            Times.Once);
        _unitOfWorkMock.Verify(v => v.Repository<Batch>());
        _unitOfWorkMock.Verify(v => v.Repository<Member>());
    }

    [Fact]
    public async Task LoaningBook_Should_Return_InternalServiceError_WhenLoanSettingIsNotFound()
    {
        //Arrange
        ResultModel<Success> expectedResult =
            ResultModel<Success>.Fail(new InternalServiceException("LoanSetting is not set"));

        var loanBookRequest = new LoanBookCommand([1], 1);

        var batchRepoMock = new Mock<IRepository<Batch>>();

        var batch = _fixture.Create<Batch>();
        batch.Quantity = 10;
        batch.QuantityLoaned = 1;
        batch.QuantityDamaged = 1;
        batch.QuantityMissing = 1;

        batchRepoMock.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct))
            .ReturnsAsync(new List<Batch>
            {
                batch
            })
            .Verifiable();

        var memberRepoMock = new Mock<IRepository<Member>>();
        memberRepoMock.Setup(s =>
                s.FirstOrDefaultAsync(It.IsAny<BaseSpec<Member, MemberDto>>(), _ct))
            .ReturnsAsync(_fixture.Create<MemberDto>())
            .Verifiable();

        var loanSettingRepoMock = new Mock<IRepository<LoanSetting>>();
        loanSettingRepoMock.Setup(s =>
                s.FirstOrDefaultAsync(It.IsAny<BaseSpec<LoanSetting>>(), _ct))
            .ReturnsAsync((LoanSetting?)null)
            .Verifiable();


        _unitOfWorkMock.Setup(s => s.Repository<Batch>())
            .Returns(batchRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Member>())
            .Returns(memberRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<LoanSetting>())
            .Returns(loanSettingRepoMock.Object)
            .Verifiable();

        //Act

        var result = await _sut.Handle(loanBookRequest, _ct);

        //Assert

        result.ShouldBeEquivalentTo(expectedResult);
        batchRepoMock.Verify(v => v.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct),
            Times.Once);
        memberRepoMock.Verify(
            v => v.FirstOrDefaultAsync(It.IsAny<BaseSpec<Member, MemberDto>>(), _ct),
            Times.Once);
        loanSettingRepoMock.Verify(s =>
            s.FirstOrDefaultAsync(It.IsAny<BaseSpec<LoanSetting>>(), _ct), Times.Once);
        _unitOfWorkMock.Verify(v => v.Repository<Batch>());
        _unitOfWorkMock.Verify(v => v.Repository<Member>());
        _unitOfWorkMock.Verify(v => v.Repository<LoanSetting>());
    }

    [Fact]
    public async Task LoaningBook_Should_Return_BusinessLogicErrorError_WhenMemberHasUnreturnedLoan_PastDueDate()
    {
        //Arrange
        ResultModel<Success> expectedResult =
            ResultModel<Success>.Fail(
                new BusinessLogicException("The member has an unreturned loan that is passed is due date"));

        var loanBookRequest = new LoanBookCommand([1], 1);

        var batchRepoMock = new Mock<IRepository<Batch>>();

        var batch = _fixture.Create<Batch>();
        batch.Quantity = 10;
        batch.QuantityLoaned = 1;
        batch.QuantityDamaged = 1;
        batch.QuantityMissing = 1;

        batchRepoMock.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct))
            .ReturnsAsync(new List<Batch>
            {
                batch
            })
            .Verifiable();

        var memberRepoMock = new Mock<IRepository<Member>>();
        memberRepoMock.Setup(s =>
                s.FirstOrDefaultAsync(It.IsAny<BaseSpec<Member, MemberDto>>(), _ct))
            .ReturnsAsync(_fixture.Create<MemberDto>())
            .Verifiable();

        var loanSettingRepoMock = new Mock<IRepository<LoanSetting>>();
        loanSettingRepoMock.Setup(s =>
                s.FirstOrDefaultAsync(It.IsAny<BaseSpec<LoanSetting>>(), _ct))
            .ReturnsAsync(_fixture.Create<LoanSetting>())
            .Verifiable();

        var loanRepoMock = new Mock<IRepository<Loan>>();
        loanRepoMock.Setup(s =>
                s.AnyAsync(It.IsAny<BaseSpec<Loan>>(), _ct))
            .ReturnsAsync(true)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Batch>())
            .Returns(batchRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Member>())
            .Returns(memberRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<LoanSetting>())
            .Returns(loanSettingRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Loan>())
            .Returns(loanRepoMock.Object)
            .Verifiable();

        //Act

        var result = await _sut.Handle(loanBookRequest, _ct);

        //Assert

        result.ShouldBeEquivalentTo(expectedResult);
        batchRepoMock.Verify(v => v.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct),
            Times.Once);
        memberRepoMock.Verify(
            v => v.FirstOrDefaultAsync(It.IsAny<BaseSpec<Member, MemberDto>>(), _ct),
            Times.Once);
        loanSettingRepoMock.Verify(s =>
            s.FirstOrDefaultAsync(It.IsAny<BaseSpec<LoanSetting>>(), _ct), Times.Once);
        loanRepoMock.Verify(s =>
            s.AnyAsync(It.IsAny<BaseSpec<Loan>>(), _ct), Times.Once);
        _unitOfWorkMock.Verify(v => v.Repository<Batch>());
        _unitOfWorkMock.Verify(v => v.Repository<Member>());
        _unitOfWorkMock.Verify(v => v.Repository<LoanSetting>());
        _unitOfWorkMock.Verify(v => v.Repository<Loan>());
    }

    [Fact]
    public async Task LoaningBook_Should_Return_Success_WithCreatedMessage_OnSuccessOperation()
    {
        //Arrange
        ResultModel<Success> expectedResult = Success.Created;
        var loanBookRequest = new LoanBookCommand([1], 1);

        var batchRepoMock = new Mock<IRepository<Batch>>();

        var batch = _fixture.Create<Batch>();
        batch.Quantity = 10;
        batch.QuantityLoaned = 1;
        batch.QuantityDamaged = 1;
        batch.QuantityMissing = 1;

        var batchList = new List<Batch>
        {
            batch
        };
        batchRepoMock.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct))
            .ReturnsAsync(batchList)
            .Verifiable();

        var memberRepoMock = new Mock<IRepository<Member>>();
        memberRepoMock.Setup(s =>
                s.FirstOrDefaultAsync(It.IsAny<BaseSpec<Member, MemberDto>>(), _ct))
            .ReturnsAsync(_fixture.Create<MemberDto>())
            .Verifiable();

        var loanSettingRepoMock = new Mock<IRepository<LoanSetting>>();
        loanSettingRepoMock.Setup(s =>
                s.FirstOrDefaultAsync(It.IsAny<BaseSpec<LoanSetting>>(), _ct))
            .ReturnsAsync(_fixture.Create<LoanSetting>())
            .Verifiable();

        var loanRepoMock = new Mock<IRepository<Loan>>();
        loanRepoMock.Setup(s =>
                s.AnyAsync(It.IsAny<BaseSpec<Loan>>(), _ct))
            .ReturnsAsync(false)
            .Verifiable();

        loanRepoMock.Setup(s => s.AddAsync(It.IsAny<Loan>(), _ct))
            .Verifiable();

        _dueDateCalculatorMock.Setup(s => s.CalculateLoanDueDate(It.IsAny<LoanSetting>(), It.IsAny<DateTime>()))
            .Returns(DateTime.UtcNow.ToDateOnly())
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Batch>())
            .Returns(batchRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Member>())
            .Returns(memberRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<LoanSetting>())
            .Returns(loanSettingRepoMock.Object)
            .Verifiable();

        _unitOfWorkMock.Setup(s => s.Repository<Loan>())
            .Returns(loanRepoMock.Object)
            .Verifiable();

        _eventPublisher.Setup(s => s.Publish(It.IsAny<LoanCreatedMessage>(), _ct))
            .Verifiable();
        //Act

        var result = await _sut.Handle(loanBookRequest, _ct);

        //Assert

        result.ShouldBeEquivalentTo(expectedResult);
        batchRepoMock.Verify(v => v.ToListAsync(It.IsAny<BaseSpec<Batch>>(), _ct),
            Times.Once);
        memberRepoMock.Verify(
            v => v.FirstOrDefaultAsync(It.IsAny<BaseSpec<Member, MemberDto>>(), _ct),
            Times.Once);
        loanSettingRepoMock.Verify(s =>
            s.FirstOrDefaultAsync(It.IsAny<BaseSpec<LoanSetting>>(), _ct), Times.Once);
        loanRepoMock.Verify(s =>
            s.AnyAsync(It.IsAny<BaseSpec<Loan>>(), _ct), Times.Once);

        loanRepoMock.Verify(s => s.AddAsync(It.IsAny<Loan>(), _ct), Times.Once);
        _dueDateCalculatorMock.Verify(s => s.CalculateLoanDueDate(It.IsAny<LoanSetting>(),
                It.IsAny<DateTime>())
            , Times.Once);

        _eventPublisher.Verify(s => s.Publish(It.IsAny<LoanCreatedMessage>(), _ct)
            , Times.Once);

        _unitOfWorkMock.Verify(v => v.Repository<Batch>());
        _unitOfWorkMock.Verify(v => v.Repository<Member>());
        _unitOfWorkMock.Verify(v => v.Repository<LoanSetting>());
        _unitOfWorkMock.Verify(v => v.Repository<Loan>());
    }

    [Theory]
    [MemberData(nameof(DueDateCalculatorInputs))]
    public void LoanDueDateCalculator_ShouldCalculateDueDate_BasedOnGivenLoanSetting(LoanSetting loanSetting,
        DateTime currentDate, DateOnly expectedResult)
    {
        //Arrange
        LoanDueDateCalculatorService dueDateCalculatorService = new LoanDueDateCalculatorService();

        //Act
        var result = dueDateCalculatorService.CalculateLoanDueDate(loanSetting, currentDate);

        //Assess
        Assert.Equal(expectedResult, result);
    }

    public static IEnumerable<object[]> DueDateCalculatorInputs => new List<object[]>
    {
        new object[]
        {
            new LoanSetting
            {
                Id = 1,
                Value = 2,
                IsActive = true,
                LoanDurationUnitType = LoanDurationUnitType.Day,
                CreatedAt = DateTime.UtcNow
            },
            new DateTime(2025, 1, 1),
            new DateOnly(2025, 1, 3)
        },
        new object[]
        {
            new LoanSetting
            {
                Id = 1,
                Value = 3,
                IsActive = true,
                LoanDurationUnitType = LoanDurationUnitType.Week,
                CreatedAt = DateTime.UtcNow
            },
            new DateTime(2025, 1, 1),
            new DateOnly(2025, 1, 22)
        },
        new object[]
        {
            new LoanSetting
            {
                Id = 1,
                Value = 1,
                IsActive = true,
                LoanDurationUnitType = LoanDurationUnitType.Month,
                CreatedAt = DateTime.UtcNow
            },
            new DateTime(2025, 1, 1),
            new DateOnly(2025, 2, 1)
        },
        new object[]
        {
            new LoanSetting
            {
                Id = 1,
                Value = 4,
                IsActive = true,
                LoanDurationUnitType = LoanDurationUnitType.Year,
                CreatedAt = DateTime.UtcNow
            },
            new DateTime(2025, 1, 1),
            new DateOnly(2029, 1, 1)
        }
    };
}
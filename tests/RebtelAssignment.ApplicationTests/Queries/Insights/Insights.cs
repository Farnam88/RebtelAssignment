using AutoFixture;
using Moq;
using RebtelAssignment.Application.Common.Abstractions.Specifications;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Insights.Dtos;
using RebtelAssignment.Application.Core.Insights.Queries;
using RebtelAssignment.Application.Core.Services;
using RebtelAssignment.Application.Data;
using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Shared.DataWrapper;
using Shouldly;

namespace RebtelAssignment.ApplicationTests.Queries.Insights;

public class InsightsTests
{
    private readonly Fixture _fixture;
    private readonly CancellationToken _ct;
    private readonly Mock<ILoanDueDateCalculatorService> _dueDateCalculatorMock;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public InsightsTests()
    {
        _fixture = new();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        // Autofixture v4 doesn't have support for DateOnly/TimeOnly types
        _fixture.Customize<DateOnly>(composer => composer.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));
        _ct = CancellationToken.None;
        _unitOfWorkMock = new();
        _dueDateCalculatorMock = new();
    }

    [Fact]
    public async Task LoaningPatternByBook_Should_Return_PaginatedList_OnSuccess()
    {
        //Arrange
        var lookList = _fixture.CreateMany<BookDto>().ToList();

        var expectedResult = ResultModel<IPaginationResponse<BookDto>>
            .Success(new BasePaginationResponse<BookDto>()
            {
                Items = lookList,
                PageNumber = 1,
                PageSize = 2,
                TotalItems = 5
            });

        Mock<IRepository<Book>> repo = new();

        _unitOfWorkMock.Setup(s => s.Repository<Book>())
            .Returns(repo.Object)
            .Verifiable();

        repo.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Book, BookDto>>(), _ct))
            .ReturnsAsync(lookList)
            .Verifiable();

        repo.Setup(s => s.CountAsync(It.IsAny<BaseSpec<Book, BookDto>>(), _ct))
            .ReturnsAsync(5)
            .Verifiable();

        var sut = new LoaningPatternByBookQueryHandler(_unitOfWorkMock.Object);
        //act

        var result = await sut.Handle(new LoaningPatternByBookQuery(3)
        {
            PageNumber = 1,
            PageSize = 2
        }, _ct);

        //Assert
        result.ShouldBeEquivalentTo(expectedResult);
        repo.Verify(s => s.ToListAsync(It.IsAny<BaseSpec<Book, BookDto>>(), _ct), Times.Once);
        repo.Verify(s => s.CountAsync(It.IsAny<BaseSpec<Book, BookDto>>(), _ct), Times.Once);
    }

    [Fact]
    public async Task TopLoanByBook_Should_Return_PaginatedList_OnSuccess()
    {
        //Arrange
        var lookList = _fixture.CreateMany<TopLoanedBookResponseDto>().ToList();

        var expectedResult = ResultModel<IPaginationResponse<TopLoanedBookResponseDto>>
            .Success(new BasePaginationResponse<TopLoanedBookResponseDto>()
            {
                Items = lookList,
                PageNumber = 1,
                PageSize = 2,
                TotalItems = 5
            });

        Mock<IRepository<Book>> repo = new();

        _unitOfWorkMock.Setup(s => s.Repository<Book>())
            .Returns(repo.Object)
            .Verifiable();

        repo.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Book, TopLoanedBookResponseDto>>(), _ct))
            .ReturnsAsync(lookList)
            .Verifiable();

        repo.Setup(s => s.CountAsync(It.IsAny<BaseSpec<Book, TopLoanedBookResponseDto>>(), _ct))
            .ReturnsAsync(5)
            .Verifiable();

        var sut = new TopLoanedBookQueryHandler(_unitOfWorkMock.Object);
        //act

        var result = await sut.Handle(new TopLoanedBookQuery
        {
            PageNumber = 1,
            PageSize = 2
        }, _ct);

        //Assert
        result.ShouldBeEquivalentTo(expectedResult);
        repo.Verify(s => s.ToListAsync(It.IsAny<BaseSpec<Book, TopLoanedBookResponseDto>>(), _ct), Times.Once);
        repo.Verify(s => s.CountAsync(It.IsAny<BaseSpec<Book, TopLoanedBookResponseDto>>(), _ct), Times.Once);
    }

    [Fact]
    public async Task TopMembersByLoan_Should_Return_PaginatedList_OnSuccess()
    {
        //Arrange
        var lookList = _fixture.CreateMany<TopMembersByLoanResponseDto>().ToList();

        var expectedResult = ResultModel<IPaginationResponse<TopMembersByLoanResponseDto>>
            .Success(new BasePaginationResponse<TopMembersByLoanResponseDto>()
            {
                Items = lookList,
                PageNumber = 1,
                PageSize = 2,
                TotalItems = 5
            });

        Mock<IRepository<Member>> repo = new();

        _unitOfWorkMock.Setup(s => s.Repository<Member>())
            .Returns(repo.Object)
            .Verifiable();

        repo.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Member, TopMembersByLoanResponseDto>>(), _ct))
            .ReturnsAsync(lookList)
            .Verifiable();

        repo.Setup(s => s.CountAsync(It.IsAny<BaseSpec<Member, TopMembersByLoanResponseDto>>(), _ct))
            .ReturnsAsync(5)
            .Verifiable();

        var sut = new TopMembersByLoanQueryHandler(_unitOfWorkMock.Object);
        //act

        var result = await sut.Handle(new TopMembersByLoanQuery
        (new DateTimeRangeDto
        {
            From = DateTime.UtcNow.AddMonths(-6),
            To = DateTime.UtcNow.AddMonths(1)
        })
        {
            PageNumber = 1,
            PageSize = 2
        }, _ct);

        //Assert
        result.ShouldBeEquivalentTo(expectedResult);
        repo.Verify(s => s.ToListAsync(It.IsAny<BaseSpec<Member, TopMembersByLoanResponseDto>>(), _ct), Times.Once);
        repo.Verify(s => s.CountAsync(It.IsAny<BaseSpec<Member, TopMembersByLoanResponseDto>>(), _ct), Times.Once);
    }

    [Fact]
    public async Task MemberReadingPace_Should_Return_MemberReadingPace_OnSuccess()
    {
        //Arrange
        var readingPaceResponse = _fixture.CreateMany<MemberReadingPaceResponseDto>().ToList();

        var expectedResult = ResultModel<IPaginationResponse<MemberReadingPaceResponseDto>>
            .Success(new BasePaginationResponse<MemberReadingPaceResponseDto>()
            {
                Items = readingPaceResponse,
                PageNumber = 1,
                PageSize = 2,
                TotalItems = 2
            });

        Mock<IRepository<Loan>> repo = new();

        _unitOfWorkMock.Setup(s => s.Repository<Loan>())
            .Returns(repo.Object)
            .Verifiable();

        var loanItemAggregatedItems = _fixture.CreateMany<LoanItemAggregatedModel>().ToList();
        repo.Setup(s => s.ToListAsync(It.IsAny<BaseSpec<Loan, LoanItemAggregatedModel>>(), _ct))
            .ReturnsAsync(loanItemAggregatedItems)
            .Verifiable(); ;

        Mock<IReadingPaceCalculatorService> readingPaceCalculatorMock = new();
        readingPaceCalculatorMock.Setup(s => s.CalculatePaceForBook(It.IsAny<List<LoanItemAggregatedModel>>()))
            .Returns(new MemberOverallReadingPaceModel
            {
                OverallReadingPace = 15.2m,
                LoanItems = loanItemAggregatedItems
            })
            .Verifiable();
        
        var sut = new MemberReadingPaceQueryHandler(_unitOfWorkMock.Object,readingPaceCalculatorMock.Object);
        //act

        var result = await sut.Handle(new MemberReadingPaceQuery(1), _ct);

        //Assert
        repo.Verify(s => s.ToListAsync(It.IsAny<BaseSpec<Loan, LoanItemAggregatedModel>>(), _ct), Times.Once);
        readingPaceCalculatorMock.Verify(s => s.CalculatePaceForBook(It.IsAny<List<LoanItemAggregatedModel>>()), Times.Once);
    }
}
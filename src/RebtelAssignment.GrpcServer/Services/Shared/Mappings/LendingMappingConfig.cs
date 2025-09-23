using Common;
using InsightServices;
using Mapster;
using RebtelAssignment.Application.Common.DataTransferObjects;
using RebtelAssignment.Application.Core.Insights.Dtos;
using RebTelAssignment.Domain.Shared.DataWrapper;

namespace RebtelAssignment.GrpcServer.Services.Shared.Mappings;

public class LendingMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BookDto, BookMsg>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Author, src => src.Authors)
            .Map(dest => dest.Subjects, src => src.Subjects);

    }
}

public class InsightMappingsConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MemberReadingPaceResponseDto, MemberReadingPaceResponseMsg>()
            .Map(dest => dest.Books, src => src.Books)
            .Map(dest => dest.OverallReadingPagesPerDay, src => src.PagesPerDay);

        config.NewConfig<TopLoanedBookResponseDto, TopLoanedBooksRow>()
            .Map(dest => dest.BookId, src => src.BookId)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Count, src => src.Count);

        config.NewConfig<BasePaginationResponse<TopLoanedBookResponseDto>, TopLoanedBooksResponseMsg>()
            .Map(dest => dest.Items, src => src.Items)
            .Map(dest => dest.PageNumber, src => src.PageNumber)
            .Map(dest => dest.PageSize, src => src.PageSize)
            .Map(dest => dest.TotalItems, src => src.TotalItems);

        config.NewConfig<BasePaginationResponse<BookDto>, LoaningPatternByBookResponseMsg>()
            .Map(dest => dest.Items, src => src.Items)
            .Map(dest => dest.PageNumber, src => src.PageNumber)
            .Map(dest => dest.PageSize, src => src.PageSize)
            .Map(dest => dest.TotalItems, src => src.TotalItems);
        
        config.NewConfig<TopMembersByLoanResponseDto, TopMembersByLoanCountRow>()
            .Map(dest => dest.MemberId, src => src.MemberId)
            .Map(dest => dest.DisplayName, src => src.DisplayName)
            .Map(dest => dest.LoanCount, src => src.LoanCount);

        config.NewConfig<BasePaginationResponse<TopMembersByLoanResponseDto>, TopMembersByLoanCountResponseMsg>()
            .Map(dest => dest.Items, src => src.Items)
            .Map(dest => dest.PageNumber, src => src.PageNumber)
            .Map(dest => dest.PageSize, src => src.PageSize)
            .Map(dest => dest.TotalItems, src => src.TotalItems);
    }
}
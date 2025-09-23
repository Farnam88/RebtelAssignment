using RebTelAssignment.Domain.Models;
using RebTelAssignment.Domain.Models.Enums;
using RebtelAssignment.Infrastructure.Data;

namespace RebtelAssignment.GrpcServer.Extensions;

/// <summary>
/// Creates in-memory Database
/// </summary>
public static class MigrationInitializer
{
    public static async Task MigrateAndSeedingAsync(this IServiceProvider serviceProvider)
    {
        await using var scopeAsync = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();

        await using var context = scopeAsync.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var logger = scopeAsync.ServiceProvider.GetRequiredService<ILogger<IDbContext>>();

        var hostingLifeTime = scopeAsync.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
        var cancellationToken = hostingLifeTime.ApplicationStopping;
        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.EnsureCreatedAsync(cancellationToken);

        #region Books

        var books = new List<Book>
        {
            new Book
            {
                Title = "Book 1",
                Authors = ["Frederik", "Linda"],
                Subjects = ["Sport", "Running"],
                Id = 1,
            },
            new Book
            {
                Title = "Book 2",
                Authors = ["Johan", "Fred", "Moa"],
                Subjects = ["Mathematics", "Algebra"],
                Id = 2,
            },
            new Book
            {
                Title = "Book 3",
                Authors = ["Sanna", "Jonathan", "Petra"],
                Subjects = ["Mathematics", "Algebra"],
                Id = 3,
            }
        };
        await context.DbSet<Book>().AddRangeAsync(books, hostingLifeTime.ApplicationStopping);
        await context.SaveChangesAsync(hostingLifeTime.ApplicationStopping);

        #endregion

        #region Batches

        var batches = new List<Batch>
        {
            new Batch
            {
                Id = 1,
                BookId = 1,
                Pages = 150,
                Isbn = "Book-1_ISBN-1",
                Publisher = "Book-1_Publisher-1",
                Edition = 1,
                PublishedDate = new DateOnly(1998, 2, 25),
                Quantity = 100,
                QuantityLoaned = 1, //done
                QuantityDamaged = 0,
                QuantityMissing = 0,
                RowVersion = []
            },
            new Batch
            {
                Id = 2,
                BookId = 1,
                Pages = 160,
                Isbn = "Book-1_ISBN-2",
                Publisher = "Book-1_Publisher-2",
                Edition = 2,
                PublishedDate = new DateOnly(2005, 3, 25),
                Quantity = 50,
                QuantityLoaned = 0,
                QuantityDamaged = 0,
                QuantityMissing = 0,
                RowVersion = []
            },
            new Batch
            {
                Id = 3,
                BookId = 2,
                Pages = 300,
                Isbn = "Book-2_ISBN-1",
                Publisher = "Book-2_Publisher-1",
                Edition = 1,
                PublishedDate = new DateOnly(2015, 3, 27),
                Quantity = 20,
                QuantityLoaned = 1,
                QuantityDamaged = 0,
                QuantityMissing = 0,
                RowVersion = []
            },
            new Batch
            {
                Id = 4,
                BookId = 2,
                Pages = 270,
                Isbn = "Book-2_ISBN-2",
                Publisher = "Book-2_Publisher-2",
                Edition = 2,
                PublishedDate = new DateOnly(2015, 5, 27),
                Quantity = 20,
                QuantityLoaned = 1,
                QuantityDamaged = 0,
                QuantityMissing = 0,
                RowVersion = []
            },
            new Batch
            {
                Id = 5,
                BookId = 3,
                Pages = 1000,
                Isbn = "Book-3_ISBN-1",
                Publisher = "Book-3_Publisher-1",
                Edition = 1,
                PublishedDate = new DateOnly(2012, 8, 14),
                Quantity = 20,
                QuantityLoaned = 1, //done
                QuantityDamaged = 0,
                QuantityMissing = 0,
                RowVersion = []
            }
        };
        await context.DbSet<Batch>().AddRangeAsync(batches, hostingLifeTime.ApplicationStopping);
        await context.SaveChangesAsync(hostingLifeTime.ApplicationStopping);

        #endregion

        #region Members

        var members = new List<Member>
        {
            new Member()
            {
                Id = 1,
                DisplayName = "Member 1"
            },
            new Member()
            {
                Id = 2,
                DisplayName = "Member 2"
            },
            new Member()
            {
                Id = 3,
                DisplayName = "Member 3"
            }
        };
        await context.DbSet<Member>().AddRangeAsync(members, hostingLifeTime.ApplicationStopping);
        await context.SaveChangesAsync(hostingLifeTime.ApplicationStopping);

        #endregion

        #region Loan Settings

        var setting = new LoanSetting
        {
            Id = 1,
            Value = 14,
            IsActive = true,
            LoanDurationUnitType = LoanDurationUnitType.Day
        };

        await context.DbSet<LoanSetting>().AddAsync(setting, hostingLifeTime.ApplicationStopping);
        await context.SaveChangesAsync(hostingLifeTime.ApplicationStopping);

        #endregion

        #region Loans

        var loans = new List<Loan>
        {
            new Loan
            {
                Id = 1,
                MemberId = 2,
                LoanedAt = DateTime.SpecifyKind(new DateTime(2025, 7, 23), DateTimeKind.Utc),
                DueAt = new DateOnly(2025, 8, 8),
                LoanItems = new List<LoanItem>
                {
                    new LoanItem
                    {
                        Id = 1,
                        LoanId = 1,
                        BatchId = 1,
                        BookId = 1,
                        ReturnedAt = DateTime.SpecifyKind(new DateTime(2025, 7, 29), DateTimeKind.Utc),
                        ItemStatus = LoanItemStatus.Returned,
                    },
                    new LoanItem
                    {
                        Id = 2,
                        LoanId = 1,
                        BatchId = 5,
                        BookId = 3,
                        ReturnedAt = DateTime.SpecifyKind(new DateTime(2025, 8, 8), DateTimeKind.Utc),
                        ItemStatus = LoanItemStatus.Returned
                    }
                }
            },
            new Loan
            {
                Id = 2,
                MemberId = 3,
                LoanedAt = DateTime.SpecifyKind(new DateTime(2025, 6, 23), DateTimeKind.Utc),
                DueAt = new DateOnly(2025, 7, 8),
                LoanItems = new List<LoanItem>
                {
                    new LoanItem
                    {
                        Id = 3,
                        LoanId = 2,
                        BatchId = 3,
                        BookId = 2,
                        ReturnedAt = DateTime.SpecifyKind(new DateTime(2025, 7, 29), DateTimeKind.Utc),
                        ItemStatus = LoanItemStatus.Damaged
                    }
                }
            },
            new Loan
            {
                Id = 3,
                MemberId = 3,
                LoanedAt = DateTime.SpecifyKind(new DateTime(2025, 9, 23), DateTimeKind.Utc),
                DueAt = new DateOnly(2025, 10, 7),
                LoanItems = new List<LoanItem>
                {
                    new LoanItem
                    {
                        Id = 4,
                        LoanId = 3,
                        BatchId = 4,
                        BookId = 2,
                        ItemStatus = LoanItemStatus.Loaned
                    }
                }
            },
        };

        await context.Set<Loan>().AddRangeAsync(loans, hostingLifeTime.ApplicationStopping);
        await context.SaveChangesAsync(hostingLifeTime.ApplicationStopping);

        #endregion


        #region LoanItems

        // var loanItems=new List<LoanItem>()
        // {
        //     
        // }

        #endregion
    }
}
using AiTutor.Application.Features.Subjects.Queries.GetAllSubjects;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Subjects;

public class GetAllSubjectsHandlerTests
{
    private static TestDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task Handle_WhenNoSubjects_ReturnsEmptyList()
    {
        using var ctx = NewCtx();
        var handler = new GetAllSubjectsQueryHandler(ctx);

        var result = await handler.Handle(new GetAllSubjectsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithSubjects_ReturnsAllMappedToDto()
    {
        using var ctx = NewCtx();
        ctx.Subjects.Add(Subject.Create("Math", "Mathematics", SubjectType.Mathematics));
        ctx.Subjects.Add(Subject.Create("Romanian", "Limba romana", SubjectType.Romanian));
        await ctx.SaveChangesAsync();

        var handler = new GetAllSubjectsQueryHandler(ctx);
        var result = await handler.Handle(new GetAllSubjectsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Should().HaveCount(2);
        result.Data!.Should().Contain(s => s.Name == "Math" && s.Type == SubjectType.Mathematics);
        result.Data!.Should().Contain(s => s.Name == "Romanian");
    }

    [Fact]
    public async Task Handle_PreservesIdAndDescription()
    {
        using var ctx = NewCtx();
        var subj = Subject.Create("Informatics", "Algorithms + DS", SubjectType.Informatics);
        ctx.Subjects.Add(subj);
        await ctx.SaveChangesAsync();

        var handler = new GetAllSubjectsQueryHandler(ctx);
        var result = await handler.Handle(new GetAllSubjectsQuery(), CancellationToken.None);

        var dto = result.Data!.Single();
        dto.Id.Should().Be(subj.Id);
        dto.Description.Should().Be("Algorithms + DS");
        dto.Type.Should().Be(SubjectType.Informatics);
    }
}

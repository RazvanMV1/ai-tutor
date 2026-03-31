using AiTutor.Application.Common.Models;
using FluentAssertions;
using Xunit;

namespace AiTutor.UnitTests.Application.Common;

public class PaginatedListTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        var items = new List<string> { "a", "b", "c" };

        var list = new PaginatedList<string>(items, 10, 1, 3);

        list.Items.Should().BeEquivalentTo(items);
        list.TotalCount.Should().Be(10);
        list.PageNumber.Should().Be(1);
        list.TotalPages.Should().Be(4); // ceil(10/3) = 4
    }

    [Fact]
    public void HasPreviousPage_OnFirstPage_ShouldBeFalse()
    {
        var list = new PaginatedList<string>(new List<string>(), 10, 1, 5);

        list.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void HasPreviousPage_OnSecondPage_ShouldBeTrue()
    {
        var list = new PaginatedList<string>(new List<string>(), 10, 2, 5);

        list.HasPreviousPage.Should().BeTrue();
    }

    [Fact]
    public void HasNextPage_OnLastPage_ShouldBeFalse()
    {
        var list = new PaginatedList<string>(new List<string>(), 10, 2, 5);

        list.HasNextPage.Should().BeFalse(); // totalPages = ceil(10/5) = 2
    }

    [Fact]
    public void HasNextPage_OnFirstPage_ShouldBeTrue()
    {
        var list = new PaginatedList<string>(new List<string>(), 10, 1, 5);

        list.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void TotalPages_WithZeroItems_ShouldBeZero()
    {
        var list = new PaginatedList<string>(new List<string>(), 0, 1, 10);

        list.TotalPages.Should().Be(0);
    }
}

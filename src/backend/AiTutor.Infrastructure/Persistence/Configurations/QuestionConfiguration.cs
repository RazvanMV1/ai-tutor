using AiTutor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiTutor.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Text)
            .IsRequired();

        builder.Property(q => q.CorrectAnswer)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(q => q.Points)
            .IsRequired();

        var comparer = new ValueComparer<List<string>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        builder.Property(q => q.Options)
            .HasConversion(
                options => string.Join("||", options),
                value => value.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList())
            .HasMaxLength(2000)
            .IsRequired()
            .Metadata.SetValueComparer(comparer);
    }
}

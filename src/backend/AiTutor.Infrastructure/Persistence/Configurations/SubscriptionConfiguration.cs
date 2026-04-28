using AiTutor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiTutor.Infrastructure.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(s => s.StartDate)
            .IsRequired();

        builder.Property(s => s.EndDate)
            .IsRequired();

        builder.Property(s => s.IsActive)
            .IsRequired();

        // === Stripe integration fields ===

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(Domain.Enums.SubscriptionStatus.Pending);

        builder.Property(s => s.StripeCustomerId)
            .HasMaxLength(255);

        builder.Property(s => s.StripeSubscriptionId)
            .HasMaxLength(255);

        builder.Property(s => s.StripePriceId)
            .HasMaxLength(255);

        builder.Property(s => s.LastStripeEventId)
            .HasMaxLength(255);

        // Index unic pentru lookup rapid din webhook handlers.
        // Nullable column → unique index acceptă mai multe NULL-uri în PostgreSQL,
        // deci subscription-urile vechi (fără Stripe) nu sunt afectate.
        builder.HasIndex(s => s.StripeSubscriptionId)
            .IsUnique()
            .HasFilter("\"StripeSubscriptionId\" IS NOT NULL");

        // Index non-unique pe StripeCustomerId pentru lookup la customer events.
        builder.HasIndex(s => s.StripeCustomerId);
    }
}

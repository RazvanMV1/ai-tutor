namespace AiTutor.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        if (!email.Contains('@'))
            throw new ArgumentException("Email is not valid.");

        return new Email(email.ToLowerInvariant().Trim());
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is Email other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}

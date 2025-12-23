namespace Domain.Common;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public bool Silindi { get; set; } = false;
}
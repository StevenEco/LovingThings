namespace Ryan.LovingThings.Domain.Core.Entities;
public interface IEntity
{
    object[] GetKeys();
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}
public abstract class Entity : IEntity
{
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {string.Join(", ", GetKeys())}";
    }

    public abstract object[] GetKeys();
}

public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    public virtual TKey Id { get; set; }

    protected Entity()
    {
    }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public bool IsTransient()
    {
        if (EqualityComparer<TKey>.Default.Equals(Id, default))
        {
            return true;
        }

        if (typeof(TKey) == typeof(int))
        {
            return Convert.ToInt32(Id) <= 0;
        }

        if (typeof(TKey) == typeof(long))
        {
            return Convert.ToInt64(Id) <= 0;
        }

        return false;
    }

    public override object[] GetKeys()
    {
        return new object[] { Id! };
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Entity<TKey> other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (other.IsTransient() || IsTransient())
            return false;
        else
            return Id!.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return !IsTransient()
            ? Id!.GetHashCode() ^ 31
            : base.GetHashCode();
    }

    public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
    {
        return Equals(left, null)
            ? Equals(right, null)
            : left.Equals(right);
    }

    public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }
}
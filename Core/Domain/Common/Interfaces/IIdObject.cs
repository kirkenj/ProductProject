namespace Domain.Common.Interfaces
{
    public interface IIdObject<T> where T : struct
    {
        T Id { get; }
    }
}

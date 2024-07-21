namespace Application.Contracts.Infrastructure
{
    public interface IPasswordGenerator
    {
        public int StandartLength { get => 8; }

        public string Generate(int length);
    }
}

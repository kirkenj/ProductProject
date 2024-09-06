namespace Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, string key) : base($"{name} was not found with key {key}") { }
    }
}

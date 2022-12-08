namespace CarbonTodo.Domain.Exceptions;

public class NotFoundEntityAppException : Exception
{
    public NotFoundEntityAppException()
    {
    }

    public NotFoundEntityAppException(string name, int id) : base(
        $"Entity {name} not found. Current id: ${id}")
    {
    }
}
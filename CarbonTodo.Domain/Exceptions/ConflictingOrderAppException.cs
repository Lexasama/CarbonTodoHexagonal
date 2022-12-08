namespace CarbonTodo.Domain.Exceptions
{
    public class ConflictingOrderAppException : Exception
    {
        public ConflictingOrderAppException(int order) : base(
            $"Conflicting order: ${order}")
        {
        }
    }
}
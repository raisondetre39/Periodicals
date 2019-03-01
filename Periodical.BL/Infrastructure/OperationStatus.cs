namespace Periodical.BL.Infrastructure
{
    // Contains details about operation
    public class OperationSatus
    {
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

        public OperationSatus(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
    }
}

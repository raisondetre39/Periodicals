namespace Periodical.BL.Infrastructure
{
    /// <summary>
    /// Claas creates instance to get information about operation status and its details
    /// </summary>
    public class OperationStatus
    {
        public bool Succedeed { get; private set; }

        public string Message { get; private set; }

        public string Property { get; private set; }

        public OperationStatus(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
    }
}

namespace Playground.ReaderWriterExample
{
    [Serializable]
    /// <summary>
    /// Represents a validation error in the application.
    /// </summary>
    public class ValidationError
    {
        public string Code { get; }
        public string Message { get; }
        public string Field { get; }

        public ValidationError(string code, string message, string field)
        {
            Code = code;
            Message = message;
            Field = field;
        }

        public override string ToString() => $"{Field}: {Message} (Code: {Code})";
    }
}

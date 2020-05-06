namespace Take.SmartContacts.Utils
{
    public interface IDataTypeValidator
    {
        DataType DataType { get; }

        bool TryValidate(string input, out string cleanedDocument);
    }
}
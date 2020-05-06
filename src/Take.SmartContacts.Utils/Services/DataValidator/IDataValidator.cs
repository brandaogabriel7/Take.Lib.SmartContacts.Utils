namespace Take.SmartContacts.Utils
{
    public interface IDataValidator
    {
        bool IsValid(string input, DataType types, out ValidatedData validatedData);
    }
}
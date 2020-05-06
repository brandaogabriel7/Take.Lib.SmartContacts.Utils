using System;

namespace Take.SmartContacts.Utils
{
    [Flags]
    public enum DataType
    {
        None = 0,
        Cpf = 1 << 0,
        Cnpj = 1 << 1,
        CarPlate = 1 << 2,
        MercosurCarPlate = 1 << 3
    }
}

namespace GobLangNet.Parsing;

public enum OperationType
{
    Add,
    Sub,
    Mul,
    Div,
    Less,
    More,
    Equal,
    LessOrEqual,
    MoreOrEqual
}

public class OperationToken : ICodeToken
{
    public OperationType OperationType { get; set; }

    public OperationToken(OperationType operationType)
    {
        OperationType = operationType;
    }

    public IEnumerable<byte> GenerateByteCode()
    {
        switch (OperationType)
        {
            case OperationType.Add:
                return new byte[1] { (byte)LanguageOperation.AddFloat };
            case OperationType.Sub:
                return new byte[1] { (byte)LanguageOperation.SubFloat };
            case OperationType.Mul:
                return new byte[1] { (byte)LanguageOperation.MulFloat };
            case OperationType.Div:
                return new byte[1] { (byte)LanguageOperation.DivFloat };
            case OperationType.Less:
                return new byte[1] { (byte)LanguageOperation.AddFloat };
            case OperationType.More:
                return new byte[1] { (byte)LanguageOperation.AddFloat };
            case OperationType.Equal:
                return new byte[1] { (byte)LanguageOperation.AddFloat };
            case OperationType.LessOrEqual:
                return new byte[1] { (byte)LanguageOperation.AddFloat };
            case OperationType.MoreOrEqual:
                return new byte[1] { (byte)LanguageOperation.AddFloat };
            default:
                throw new Exception("Invalid operation passed");
        }
    }
}
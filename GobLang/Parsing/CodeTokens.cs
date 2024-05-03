namespace GobLangNet.Parsing;

public interface ICodeToken
{
    public abstract IEnumerable<byte> GenerateByteCode();
}


public class StringToken : ICodeToken
{
    public string Value { get; set; }

    public StringToken(string value)
    {
        Value = value;
    }

    public IEnumerable<byte> GenerateByteCode()
    {
        throw new NotImplementedException();
    }
}

public class NumberToken : ICodeToken
{
    public float Value { get; set; }

    public NumberToken(float value)
    {
        Value = value;
    }

    public IEnumerable<byte> GenerateByteCode()
    {
        List<byte> bytes = new List<byte>() { (byte)LanguageOperation.PushFloat };
        bytes.AddRange(BitConverter.GetBytes(Value).Reverse());
        return bytes;
    }
}

public class CommandSequenceToken : ICodeToken
{
    public List<ICodeToken> Tokens { get; set; }
    public IEnumerable<byte> GenerateByteCode()
    {
        throw new NotImplementedException();
    }
}
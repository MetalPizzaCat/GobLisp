namespace GobLangNet.Parsing;

public interface CodeToken
{
    public abstract IEnumerable<byte> GenerateByteCode();
}


public class StringToken : CodeToken
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

public class NumberToken : CodeToken
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


public class ExpressionToken : CodeToken
{
    public ExpressionToken(CodeToken header, List<CodeToken> tokens)
    {
        Header = header;
        Tokens = tokens;
    }

    public CodeToken Header { get; set; }
    public List<CodeToken> Tokens { get; set; }

    public IEnumerable<byte> GenerateByteCode()
    {
        List<byte> bytes = new List<byte>();
        foreach (CodeToken token in Tokens)
        {
            bytes.AddRange(token.GenerateByteCode());
        }
        bytes.AddRange(Header.GenerateByteCode());
        return bytes;
    }
}
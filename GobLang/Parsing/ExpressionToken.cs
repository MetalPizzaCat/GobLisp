namespace GobLangNet.Parsing;

public class ExpressionToken : ICodeToken
{
    public ExpressionToken(ICodeToken header, List<ICodeToken> tokens)
    {
        Header = header;
        Tokens = tokens;
    }

    public ICodeToken Header { get; set; }
    public List<ICodeToken> Tokens { get; set; }

    public IEnumerable<byte> GenerateByteCode()
    {
        List<byte> bytes = new List<byte>();
        foreach (ICodeToken token in Tokens)
        {
            bytes.AddRange(token.GenerateByteCode());
        }
        bytes.AddRange(Header.GenerateByteCode());
        return bytes;
    }
}
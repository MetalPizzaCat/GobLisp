
namespace GobLangNet.Parsing;

public class VariableDeclarationOperationToken : ICodeToken
{
    public IEnumerable<VariableDeclarationToken> Variables { get; }

    public VariableDeclarationOperationToken(IEnumerable<VariableDeclarationToken> variables)
    {
        Variables = variables;
    }

    public IEnumerable<byte> GenerateByteCode()
    {
        throw new NotImplementedException();
    }
}

public class VariableDeclarationToken : ICodeToken
{
    public VariableDeclarationToken(string name, ICodeToken defaultValue)
    {
        Name = name;
        DefaultValue = defaultValue;
    }

    public string Name { get; }
    public ICodeToken DefaultValue { get; }
    public IEnumerable<byte> GenerateByteCode()
    {
        throw new NotImplementedException();
    }
}
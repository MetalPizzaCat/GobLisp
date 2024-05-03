namespace GobLangNet.Parsing;
using Sprache;
public static class GobLispParser
{
    private static readonly Parser<OperationToken> _operationParser =
    from opType in Parse.Char('+').Return(OperationType.Add).XOr(Parse.Char('-').Return(OperationType.Sub))
                                                            .XOr(Parse.Char('*').Return(OperationType.Mul))
                                                            .XOr(Parse.Char('/').Return(OperationType.Div))
                                                            .XOr(Parse.Char('>').Return(OperationType.More))
                                                            .XOr(Parse.Char('<').Return(OperationType.Less))
                                                            .XOr(Parse.Char('=').Return(OperationType.Equal))
                                                            .XOr(Parse.String(">=").Return(OperationType.MoreOrEqual))
                                                            .XOr(Parse.String("<=").Return(OperationType.LessOrEqual))
    select new OperationToken(opType);

    private static readonly Parser<StringToken> _textParser =
        from val in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_'))).Token()
        select new StringToken(val);

    private static readonly Parser<NumberToken> _numberParser =
        from op in Parse.Optional(Parse.Char('-').Token())
        from number in Parse.DecimalInvariant.Token()
        select new NumberToken(float.Parse(number) * (op.IsDefined ? -1f : 1f));



    private static readonly Parser<VariableDeclarationToken> _variableDeclarationParser =
        (from name in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_'))).Token()
         from defaultValue in (_numberParser.XOr(_functionParser)).Token()
         select new VariableDeclarationToken(name, defaultValue)).Contained(Parse.Char('('), Parse.Char(')'));

    private static readonly Parser<VariableDeclarationOperationToken> _variableDeclarationOperationParser =
        from letTitle in Parse.String("let").Token()
        from variables in _variableDeclarationParser.Token().XMany()
        select new VariableDeclarationOperationToken(variables);


    private static readonly Parser<ICodeToken> _systemOperationParser =
            _variableDeclarationOperationParser.Or<ICodeToken>(
            from head in _operationParser.XOr<ICodeToken>(_textParser).Token()
            from contents in _functionParser.XOr(_numberParser).XOr(_textParser).Token().XMany()
            select new ExpressionToken(head, contents.ToList()));

    private static readonly Parser<ICodeToken> _functionParser =
    _systemOperationParser
    .Contained(Parse.Char('('), Parse.Char(')'));

    static public ICodeToken GenerateTree(string code)
    {
        return _functionParser.Parse(code);
    }
}
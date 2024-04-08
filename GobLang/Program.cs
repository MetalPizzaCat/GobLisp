namespace GobLangNet;


class Program
{
    static void Main(string[] args)
    {
        string code = "(+ 2 3)";
        GobLangNet.Parsing.ExpressionToken? result = GobLangNet.Parsing.GobLispParser.GenerateTree(code);

        if (result == null)
        {
            return;
        }

    }
}

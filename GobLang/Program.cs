namespace GobLangNet;


class Program
{
    static void Main(string[] args)
    {
        string code = "(let (a 2) (b ( + 3 4)))";
        GobLangNet.Parsing.ICodeToken? result = GobLangNet.Parsing.GobLispParser.GenerateTree(code);

        if (result == null)
        {
            return;
        }

    }
}

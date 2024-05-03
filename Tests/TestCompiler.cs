namespace Tests;

[TestClass]
public class UnitLanguageTests
{

    
    [TestMethod]
    public void TestBasicCodeGeneration()
    {
        string code = "(+ 2 3)";

        GobLangNet.Parsing.ICodeToken? root = GobLangNet.Parsing.GobLispParser.GenerateTree(code);
        Assert.IsNotNull(root);
        IEnumerable<byte> bytes = root.GenerateByteCode();

        List<byte> expected = new List<byte>();
        expected.Add((byte)GobLangNet.LanguageOperation.PushFloat);
        expected.AddRange(BitConverter.GetBytes(2f).Reverse());
        expected.Add((byte)GobLangNet.LanguageOperation.PushFloat);
        expected.AddRange(BitConverter.GetBytes(3f).Reverse());
        expected.Add((byte)GobLangNet.LanguageOperation.AddFloat);

        CollectionAssert.AreEquivalent(expected, bytes.ToList());
    }

    [TestMethod]
    public void TestBasicCodeExecution()
    {
        string code = "(+ 2 3)";

        GobLangNet.Parsing.ICodeToken? root = GobLangNet.Parsing.GobLispParser.GenerateTree(code);
        Assert.IsNotNull(root);
        IEnumerable<byte> bytes = root.GenerateByteCode();

        GobLangNet.ExecutionMachine executionMachine = new GobLangNet.ExecutionMachine(new GobLangNet.OperationList(bytes));
        while (executionMachine.Step()) ;
        Assert.AreEqual(5f, BitConverter.Int32BitsToSingle(executionMachine.ProgramStack.Peek()));
    }


    [TestMethod]
    public void TestBasicNestedCodeExecution()
    {
        string code = "(+ (+ 2 8) (- 3 -3))";

        GobLangNet.Parsing.ICodeToken? root = GobLangNet.Parsing.GobLispParser.GenerateTree(code);
        Assert.IsNotNull(root);
        IEnumerable<byte> bytes = root.GenerateByteCode();

        GobLangNet.ExecutionMachine executionMachine = new GobLangNet.ExecutionMachine(new GobLangNet.OperationList(bytes));
        while (executionMachine.Step()) ;
        Assert.AreEqual((2f + 8f) + (3f - -3f), BitConverter.Int32BitsToSingle(executionMachine.ProgramStack.Peek()));
    }
}
namespace Tests;

[TestClass]
public class UnitLanguageTests
{

    [TestMethod]
    public void TestStackStorage()
    {
        GobLangNet.OperationQueue operation = new GobLangNet.OperationQueue();
        operation.PushInt(4);

        int val = operation.PopInt();

        Assert.AreEqual(4, val);
    }

    [TestMethod]
    public void TestStackStorageFloat()
    {
        GobLangNet.OperationQueue operation = new GobLangNet.OperationQueue();
        operation.PushFloat(4.2f);
        byte[]? f = BitConverter.GetBytes(4.2f);

        CollectionAssert.AreEquivalent(f, operation.Bytes);
        float val = operation.PopFloat();

        Assert.AreEqual(4.2f, val);
    }

    [TestMethod]
    public void TestBasicExecution()
    {
        GobLangNet.OperationQueue operation = new GobLangNet.OperationQueue();
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(4);
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(5);
        operation.Push(GobLangNet.LanguageOperation.AddInt);
        GobLangNet.ExecutionMachine machine = new GobLangNet.ExecutionMachine(operation);

        while (machine.Step()) ;
        Assert.AreEqual(9, machine.ProgramStack.Peek());
    }

    [TestMethod]
    public void TestFloatExecution()
    {
        GobLangNet.OperationQueue operation = new GobLangNet.OperationQueue();
        operation.Push(GobLangNet.LanguageOperation.PushFloat);
        operation.PushFloat(4.4f);
        operation.Push(GobLangNet.LanguageOperation.PushFloat);
        operation.PushFloat(5.45f);
        operation.Push(GobLangNet.LanguageOperation.AddFloat);
        GobLangNet.ExecutionMachine machine = new GobLangNet.ExecutionMachine(operation);

        while (machine.Step()) ;

        Assert.AreEqual(4.4f + 5.45f, BitConverter.Int32BitsToSingle(machine.ProgramStack.Peek()));
    }

    [TestMethod]
    public void TestBasicCodeGeneration()
    {
        string code = "(+ 2 3)";

        GobLangNet.Parsing.ExpressionToken? root = GobLangNet.Parsing.GobLispParser.GenerateTree(code);
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

        GobLangNet.Parsing.ExpressionToken? root = GobLangNet.Parsing.GobLispParser.GenerateTree(code);
        Assert.IsNotNull(root);
        IEnumerable<byte> bytes = root.GenerateByteCode();

        GobLangNet.ExecutionMachine executionMachine = new GobLangNet.ExecutionMachine(new GobLangNet.OperationQueue(bytes));
        while (executionMachine.Step()) ;
        Assert.AreEqual(5f, BitConverter.Int32BitsToSingle(executionMachine.ProgramStack.Peek()));
    }
}
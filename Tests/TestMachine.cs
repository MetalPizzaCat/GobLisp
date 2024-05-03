namespace Tests;

[TestClass]
public class UnitLanguageMachineTests
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
    public void TestNativeFuncCall()
    {
        /*
            push 69
            call 0
        */
        GobLangNet.OperationQueue operation = new GobLangNet.OperationQueue();
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(69);
        operation.Push(GobLangNet.LanguageOperation.CallFunc);
        operation.PushInt(0);
        GobLangNet.ExecutionMachine machine = new GobLangNet.ExecutionMachine(operation);
        List<string> output = new();

        machine.OnValuePrinted += output.Add;

        while (machine.Step()) ;
        Assert.AreEqual(output.FirstOrDefault(), "69");
    }

     [TestMethod]
    public void TesFib()
    {
        /*
            push 69
            call 0
        */
        GobLangNet.OperationQueue operation = new GobLangNet.OperationQueue();
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(69);
        operation.Push(GobLangNet.LanguageOperation.CallFunc);
        operation.PushInt(0);
        GobLangNet.ExecutionMachine machine = new GobLangNet.ExecutionMachine(operation);
        List<string> output = new();

        machine.OnValuePrinted += output.Add;

        while (machine.Step()) ;
        Assert.AreEqual(output.FirstOrDefault(), "69");
    }

}
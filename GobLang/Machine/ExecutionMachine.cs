namespace GobLangNet;

public enum LanguageOperation
{
    None,
    AddInt,
    SubInt,
    MulInt,
    PushInt,
    AddFloat,
    SubFloat,
    MulFloat,
    DivFloat,

    PushFloat,
    /// <summary>
    /// Ends the execution
    /// </summary>
    Stop,
}

public enum DataType
{
    /// <summary>
    /// Value that can be either true or false
    /// </summary>
    Boolean,
    Int,
    Float,
    Address,
}

public class ExecutionMachine
{
    public List<uint> Variables { get; private set; } = new();
    public OperationQueue Operations { get; private set; }

    public Stack<int> ProgramStack { get; private set; } = new();

    public ExecutionMachine(OperationQueue program)
    {
        Operations = program;
    }

    private int PopIntFromOperationQueue()
    {
        byte[] bytes = new byte[4];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Operations.Pop();
        }

        return BitConverter.ToInt32(bytes, 0);
    }

    private float PopFloatFromOperationQueue()
    {
        byte[] bytes = new byte[4];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Operations.Pop();
        }

        return BitConverter.ToSingle(bytes, 0);
    }

    public bool Step()
    {
        if (Operations.IsEmpty())
        {
            return false;
        }
        LanguageOperation operation = (LanguageOperation)Operations.Pop();
        switch (operation)
        {
            case LanguageOperation.None:
                break;
            case LanguageOperation.AddFloat:
                ProgramStack.Push(BitConverter.SingleToInt32Bits(BitConverter.Int32BitsToSingle(ProgramStack.Pop()) +BitConverter.Int32BitsToSingle(ProgramStack.Pop())));
                break;
            case LanguageOperation.AddInt:
                ProgramStack.Push(ProgramStack.Pop() + ProgramStack.Pop());
                break;
            case LanguageOperation.SubInt:
                break;
            case LanguageOperation.MulInt:
                ProgramStack.Push(ProgramStack.Pop() * ProgramStack.Pop());
                break;
            case LanguageOperation.Stop:
                return false;
            case LanguageOperation.PushInt:
                ProgramStack.Push(Operations.PopInt());
                break;

            case LanguageOperation.PushFloat:
                ProgramStack.Push(BitConverter.SingleToInt32Bits(Operations.PopFloat()));
                break;
            default:
                throw new NotImplementedException($"{operation.ToString("D")} is not implemented");
        }
        return true;
    }
}
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
    /// Pops a value from the operation stack and stores it in variable array
    /// </summary>
    SetFloat,
    /// <summary>
    /// Gets value from variable array and pushes it on the stack
    /// </summary>
    GetFloat,
    /// <summary>
    /// Calls the function with id stored in next byte
    /// </summary>
    CallFunc,
    /// <summary>
    /// Ends the execution
    /// </summary>
    Stop,
}

public enum DataType
{
    /// <summary>
    /// Equivalent of void in c
    /// </summary>
    None,
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
    public delegate void ValuePrinted(string val);

    public event ValuePrinted? OnValuePrinted;
    /// <summary>
    /// All variables that can be utilized during execution<para/>
    /// Although by default system has a strick 32 variable limit this can be changed to any other system if necessary 
    /// </summary>
    public int[] Variables { get; private set; } = new int[32];
    public OperationQueue Operations { get; private set; }

    public Stack<int> ProgramStack { get; private set; } = new();


    public List<FunctionData> Functions { get; }
    public List<Func<List<int>, int?>> NativeFunctions { get; }

    public ExecutionMachine(OperationQueue program)
    {
        Operations = program;

        Functions = new()
        {
            new FunctionData("print",true, 0, 1)
        };
        NativeFunctions = new()
        {
            // first function is a print function that just uses c# Console
            (args) =>
            {
                Console.WriteLine(args[0]);
                OnValuePrinted?.Invoke(args[0].ToString());
                return null;
            }
        };
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


    private void CallFunction()
    {
        byte id = Operations.Pop();
        if (id > Functions.Count)
        {
            throw new IndexOutOfRangeException("Id of function is larger that amount of functions present");
        }
        FunctionData func = Functions[id];
        List<int> arguments = new List<int>();
        for (int i = 0; i < func.ArgumentCount; i++)
        {
            arguments.Add(ProgramStack.Pop());
        }
        if (func.Native)
        {
            NativeFunctions[func.Id](arguments);
        }
        else
        {
            throw new NotImplementedException("Non native functions are not yet implemented");
        }
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
            case LanguageOperation.Stop:
                return false;
            case LanguageOperation.AddFloat:
                ProgramStack.Push(BitConverter.SingleToInt32Bits(BitConverter.Int32BitsToSingle(ProgramStack.Pop()) + BitConverter.Int32BitsToSingle(ProgramStack.Pop())));
                break;
            case LanguageOperation.SubFloat:
                {
                    float b = BitConverter.Int32BitsToSingle(ProgramStack.Pop());
                    float a = BitConverter.Int32BitsToSingle(ProgramStack.Pop());
                    ProgramStack.Push(BitConverter.SingleToInt32Bits(a - b));
                    break;
                }
            case LanguageOperation.AddInt:
                ProgramStack.Push(ProgramStack.Pop() + ProgramStack.Pop());
                break;
            case LanguageOperation.SubInt:
                break;
            case LanguageOperation.MulInt:
                ProgramStack.Push(ProgramStack.Pop() * ProgramStack.Pop());
                break;
            case LanguageOperation.PushInt:
                ProgramStack.Push(Operations.PopInt());
                break;

            case LanguageOperation.PushFloat:
                ProgramStack.Push(BitConverter.SingleToInt32Bits(Operations.PopFloat()));
                break;
            case LanguageOperation.SetFloat:
                Variables[Operations.Pop()] = ProgramStack.Pop();
                break;
            case LanguageOperation.GetFloat:
                ProgramStack.Push(Variables[Operations.Pop()]);
                break;
            case LanguageOperation.CallFunc:
                CallFunction();
                break;
            default:
                throw new NotImplementedException($"{Enum.GetName(typeof(LanguageOperation), operation)} is not implemented");
        }
        return true;
    }
}
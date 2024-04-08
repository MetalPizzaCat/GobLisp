namespace GobLangNet;

public class OperationQueue
{

    public Queue<byte> Bytes { get; private set; }

    public OperationQueue(IEnumerable<byte> bytes)
    {
        Bytes = new Queue<byte>(bytes);
    }

    public OperationQueue()
    {
        Bytes = new Queue<byte>();
    }

    public void PushInt(int value)
    {
        for (int i = 3; i >= 0; i--)
        {
            Bytes.Enqueue((byte)(((0xFF << (i * 8)) & value) >> (i * 8)));
        }
    }

    public void Push(byte value)
    {
        Bytes.Enqueue(value);
    }

    public void Push(LanguageOperation value)
    {
        Bytes.Enqueue((byte)value);
    }

    public void PushFloat(float value)
    {
        uint val = BitConverter.SingleToUInt32Bits(value);
        for (int i = 3; i >= 0; i--)
        {
            Bytes.Enqueue((byte)(((0xFF << (i * 8)) & val) >> (i * 8)));
        }
    }

    public int PopInt()
    {
        int result = 0;
        for (int i = 3; i >= 0; i--)
        {
            result |= ((int)Bytes.Dequeue()) << (i * 8);
        }
        return result;
    }

    public float PopFloat()
    {
        uint result = 0;
        for (int i = 3; i >= 0; i--)
        {
            result |= ((uint)Bytes.Dequeue()) << (i * 8);
        }
        return BitConverter.UInt32BitsToSingle(result);
    }

    public byte Pop()
    {
        return Bytes.Dequeue();
    }

    public bool IsEmpty()
    {
        return Bytes.Count == 0;
    }
}
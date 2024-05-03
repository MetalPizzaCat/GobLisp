namespace GobLangNet;

public class OperationList
{
    public int ProgramCounter { get; set; }

    public List<byte> Bytes { get; private set; }

    public OperationList(IEnumerable<byte> bytes)
    {
        Bytes = bytes.ToList();
    }

    public OperationList()
    {
        Bytes = new List<byte>();
    }

    public void PushInt(int value)
    {
        for (int i = 3; i >= 0; i--)
        {
            Bytes.Add((byte)(((0xFF << (i * 8)) & value) >> (i * 8)));
        }
    }

    public void Push(byte value)
    {
        Bytes.Add(value);
    }

    public void Push(LanguageOperation value)
    {
        Bytes.Add((byte)value);
    }

    public void PushFloat(float value)
    {
        uint val = BitConverter.SingleToUInt32Bits(value);
        for (int i = 3; i >= 0; i--)
        {
            Bytes.Add((byte)(((0xFF << (i * 8)) & val) >> (i * 8)));
        }
    }

    public int PopInt()
    {
        int result = 0;
        for (int i = 3; i >= 0; i--)
        {
            result |= ((int)Bytes[ProgramCounter++]) << (i * 8);
        }
        return result;
    }

    public float PopFloat()
    {
        uint result = 0;
        for (int i = 3; i >= 0; i--)
        {
            result |= ((uint)Bytes[ProgramCounter++]) << (i * 8);
        }
        return BitConverter.UInt32BitsToSingle(result);
    }

    public byte Pop()
    {
        return Bytes[ProgramCounter++];
    }

    public bool IsEmpty()
    {
        return Bytes.Count == ProgramCounter;
    }
}
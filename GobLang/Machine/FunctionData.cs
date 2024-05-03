namespace GobLangNet;

/// <param name="Name"> Developer friendly name used for debugging purposes</param>
/// <param name="Native"> If true a native function will be called  </param>
/// <param name="Id"></param>
/// <param name="ArgumentCount"> The amount of values to pop from the stack </param>
public record FunctionData(string Name, bool Native, int Id, int ArgumentCount);

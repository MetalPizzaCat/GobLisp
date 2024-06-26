﻿namespace GobLangNet;


class Program
{
    static void Main(string[] args)
    {
        GobLangNet.OperationList operation = new GobLangNet.OperationList();
        /*
            push 1
            set 1
        */
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(1);
        operation.Push(GobLangNet.LanguageOperation.SetInt);
        operation.Push(1);
        /*
            ; set c to a + b = 0 + 1
            push 1
            set 2
        */
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(1);
        operation.Push(GobLangNet.LanguageOperation.SetInt);
        operation.Push(2);
        /*
            ; start loop
            push 2
            set 3
        */
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(2);
        operation.Push(GobLangNet.LanguageOperation.SetInt);
        operation.Push(3);
        /*
            ; check loop
            get 3
            push 40
        */
        operation.Push(GobLangNet.LanguageOperation.GetInt);
        operation.Push(3);
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(40);
        /*
            check:
            if_more_or_equal $end
        */
        operation.Push(GobLangNet.LanguageOperation.IfMoreOrEqual);
        operation.PushInt(63);
        /*
            get 0
            get 1
            add
            set 2
        */
        operation.Push(GobLangNet.LanguageOperation.GetInt);
        operation.Push((byte)0);
        operation.Push(GobLangNet.LanguageOperation.GetInt);
        operation.Push(1);
        operation.Push(GobLangNet.LanguageOperation.AddInt);
        operation.Push(GobLangNet.LanguageOperation.SetInt);
        operation.Push(2);
        /*
            get 1
            set 0
        */
        operation.Push(GobLangNet.LanguageOperation.GetInt);
        operation.Push(1);
        operation.Push(GobLangNet.LanguageOperation.SetInt);
        operation.Push((byte)0);
        /*
            get 2
            set 1
        */
        operation.Push(GobLangNet.LanguageOperation.GetInt);
        operation.Push(2);
        operation.Push(GobLangNet.LanguageOperation.SetInt);
        operation.Push(1);

        /*
            get 3
            push 1
            add
            set 3
        */
        operation.Push(GobLangNet.LanguageOperation.GetInt);
        operation.Push(3);
        operation.Push(GobLangNet.LanguageOperation.PushInt);
        operation.PushInt(1);
        operation.Push(LanguageOperation.AddInt);
        operation.Push(LanguageOperation.SetInt);
        operation.Push(3);
        /*
            goto $check
        */
        operation.Push(GobLangNet.LanguageOperation.GoTo);
        operation.PushInt(21);

        /*
            get 2
            call 0
        */
        operation.Push(GobLangNet.LanguageOperation.GetInt);
        operation.Push(2);
        operation.Push(GobLangNet.LanguageOperation.CallFunc);
        operation.PushInt(0);
        operation.Push(GobLangNet.LanguageOperation.Stop);

        File.WriteAllBytes("./bin/Debug/net7.0/code.bin", operation.Bytes.ToArray());
        GobLangNet.ExecutionMachine machine = new GobLangNet.ExecutionMachine(operation);
        List<string> output = new();

        machine.OnValuePrinted += output.Add;

        while (machine.Step()) ;

        Console.WriteLine($"Expected: 63245986, Got: {output[0]}");
    }
}

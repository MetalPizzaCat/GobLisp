namespace Tests;

[TestClass]
public class SpecialTests
{

    [TestMethod]
    public void TesFib()
    {
        /*
        this is based on this code

        program name
    implicit none
    
    integer :: start
    integer :: end

    integer :: a
    integer :: b
    integer :: c

    integer :: i

    a = 0
    b = 1
    c = a + b

    start = 2
    end = 40

    do i = start, end
        c = a + b
        a = b
        b = c
    end do

    print *, 'result: ', b

end program name
        */
        /*
            ; a is var0
            ; b is var1
            ; c is var2
            ; i is var3

            ; set b to 1
            push 1
            set 1
            ; set c to a + b = 0 + 1
            push 1
            set 2
            ; start loop
            push 2
            set 3
            ; check loop
            get 3
            push 40
            check:
            if_more_or_equal $end
            get 0
            get 1
            add
            set 2
            get 1
            set 0
            get 2
            set 1
            goto $check
            end: 
            stop
        */
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
        operation.PushInt(41);
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
        operation.Push(GobLangNet.LanguageOperation.AddInt);
        operation.Push(GobLangNet.LanguageOperation.SetInt);
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
        GobLangNet.ExecutionMachine machine = new GobLangNet.ExecutionMachine(operation);
        List<string> output = new();

        machine.OnValuePrinted += output.Add;

        while (machine.Step()) ;
        Assert.AreEqual("102334155", output.FirstOrDefault());
    }

}
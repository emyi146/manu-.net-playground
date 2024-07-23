using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

try
{
    MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain();

}
catch (Exception e)
{
    Console.WriteLine(e.Message + " ### " + e.GetType() + " ### \n" + e.StackTrace);
}

// See https://aka.ms/new-console-template for more information
static void MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain()
{
    try
    {
        CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA();
    }
    catch (Exception e)
    {
        //throw;
        CallingMiddlewareEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE(e);
    }
}
static void CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA()
{
    try
    {
        CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB();
    }
    catch (BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN e)
    {
        var newEx = new Exception("Exception in CallingA", e);
        ExceptionDispatchInfo.SetRemoteStackTrace(newEx, e.StackTrace);
        ExceptionDispatchInfo.Capture(newEx).Throw();
    }
}

static void CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB()
{
    throw new BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN();
}

static void CallingMiddlewareEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE(Exception exception)
{
    exception.ThrowInnerExceptionIfAny();
}

public static class ExceptionExtensions
{
    public static void ThrowInnerExceptionIfAny(this Exception exception)
    {
        var exceptionToThrow = exception!.InnerException ?? exception!;

        try
        {
            exception.CopyFieldTo(exceptionToThrow, "_stackTrace");
            exception.CopyFieldTo(exceptionToThrow, "_remoteStackTraceString");
        }
        finally
        {
            ExceptionDispatchInfo.Capture(exceptionToThrow).Throw();
        }
    }

}
public static class TypeExtensions
{
    public static void CopyFieldTo<T>(this T source, T target, string fieldName) where T : class
    {
        if (source == target) return;

        var type = typeof(T);
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var field = type.GetField(fieldName, flags);
        field?.SetValue(target, field.GetValue(source));
    }
}

public class BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN : Exception { }


/*
 * regular throw GOOD (but wrong exception type)
Exception in CallingA ### System.Exception ###
at Program.<< Main >$> g__CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB | 0_2() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 45
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 30
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 36
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 18
   at Program.<Main>$(String[] args) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 5
*/

/* GOOD replacing _stackStrace and _remoteStackTraceString
Exception of type 'BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN' was thrown. ### BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN ###
   at Program.<<Main>$>g__CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB|0_2() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 43
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 31
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 37
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 19
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingMiddlewareEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE|0_3(Exception exception) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 58
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 24
   at Program.<Main>$(String[] args) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 6
*/

/* BAD replacing stackStrace missing l19
Exception of type 'BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN' was thrown. ### BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN ###
   at Program.<<Main>$>g__CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB|0_2() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 43
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 31
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB|0_2() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 43
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 31
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingMiddlewareEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE|0_3(Exception e) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 59
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 24
   at Program.<Main>$(String[] args) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 6


/* BAD --- almost missing l30
 * *throw replacing stackStrace
Exception of type 'BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN' was thrown. ### BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN ###
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 37
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 19
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingMiddlewareEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE|0_3(Exception e) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 57
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 24
   at Program.<Main>$(String[] args) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 6
*/

/* BAD missing l18
 * * *throw e.InnerException after replacing stackStrace
Exception of type 'BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN' was thrown. ### BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN ###
   at Program.<<Main>$>g__CallingMiddlewareEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE|0_3(Exception e) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 57
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 24
   at Program.<Main>$(String[] args) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 6


/*
 *throw e BAD missing l18
Exception in CallingA ### System.Exception ###
   at Program.<<Main>$>g__CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB|0_2() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 46
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 31
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 23
   at Program.<Main>$(String[] args) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 5
*/

/* missing l18
 * * ExceptionDispatchInfo.Capture(e.InnerException).Throw(); GOOD
Exception of type 'BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN' was thrown. ### BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN ###
   at Program.<<Main>$>g__CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB|0_2() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 42
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 30
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingMiddlewareEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE|0_3(Exception e) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 47
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 23
   at Program.<Main>$(String[] args) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 5
*/

/*
 * * ExceptionDispatchInfo.SetRemoteStackTrace(newEx, e.StackTrace); GOOD
Exception of type 'BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN' was thrown. ### BeautifulExceptionNNNNNNNNNNNNNNNNNNNNNNNNN ###
   at Program.<<Main>$>g__CallingBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB|0_2() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 42
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 30
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA|0_1() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 36
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 18
--- End of stack trace from previous location ---
   at Program.<<Main>$>g__CallingMiddlewareEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE|0_3(Exception e) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 50
   at Program.<<Main>$>g__MMMMMMMMMMMMMMMMMMMMMMMMMMMMMain|0_0() in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 23
   at Program.<Main>$(String[] args) in C:\GIT\github\manu-.net-playground\_ManuPlayground\Program.cs:line 5
*/
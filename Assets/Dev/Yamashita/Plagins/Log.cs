using UnityEngine;
using System.Runtime.CompilerServices;

public static class Log
{
    public static void DebugThrow ( string message = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0,
        [CallerMemberName] string member = "" )
    {
        Debug.Log( $"{file}:{line}-{member}\n{message}" );
    }
}

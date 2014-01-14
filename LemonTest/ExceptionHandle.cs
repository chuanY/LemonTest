namespace LemonTest
{
    using System;

    internal static class ExceptionHandle
    {
        internal static int HandleEx(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            return 2;
        }
    }
}
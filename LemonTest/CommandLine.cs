namespace LemonTest
{
    using LemonDefine.Enum;
    using LemonDefine.Interface;
    using System;
    using System.Collections.Generic;

    internal static class CommandLine
    {
        public static List<String> AssemName { set; get; }
        public static List<CasePriority> PLeave { set; get; }
        public static ITestLog OutputLog { set; get; }
        public static List<String> CaseList { set; get; }
    }
}
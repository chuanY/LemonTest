namespace LemonTest
{
    using LemonDefine.Enum;
    using LemonDefine.Log;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class ParseArg
    {
        /// <summary>
        /// Parse Arguments String
        /// </summary>
        /// <param name="args"></param>
        public static bool ParseArgs(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: LemonTest TestDLL [case name]");
                return false;
            }

            var dllPath = args[0];

            if (!File.Exists(dllPath))
            {
                Console.WriteLine("Error: Assembly {0} not found", dllPath);
                return false;
            }

            CommandLine.CaseList = new List<string>();
            CommandLine.AssemName = new List<string>();
            CommandLine.PLeave = new List<CasePriority>();

            if (CommandLine.OutputLog == null)
                CommandLine.OutputLog = new Log2XmlImp();

            if (args.Length > 0)
                return ParseOptionArg(args);

            return true;
        }

        /// <summary>
        /// Parse All option args
        /// </summary>
        /// <param name="args">Arg list</param>
        /// <returns>Parse succ</returns>
        private static bool ParseOptionArg(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                    GetFlagArgs(args[i]);
                else if (args[i].Contains(".dll"))
                {
                    CommandLine.AssemName.Add(args[i]);
                }
                else
                {
                    CommandLine.CaseList.Add(args[i]);
                }
            }

            return true;
        }

        /// <summary>
        /// Parse flag option
        /// </summary>
        private static void GetFlagArgs(string arg)
        {
            switch (arg)
            {
                case "-nunit":
                    CommandLine.OutputLog = new Log2Nunit();
                    break;
                case "-P0":
                    if (!CommandLine.PLeave.Contains(CasePriority.P0))
                        CommandLine.PLeave.Add(CasePriority.P0);
                    break;
                case "-P1":
                    if (!CommandLine.PLeave.Contains(CasePriority.P1))
                        CommandLine.PLeave.Add(CasePriority.P1);
                    break;
                case "-P2":
                    if (!CommandLine.PLeave.Contains(CasePriority.P2))
                        CommandLine.PLeave.Add(CasePriority.P2);
                    break;
                case "-P3":
                    if (!CommandLine.PLeave.Contains(CasePriority.P3))
                        CommandLine.PLeave.Add(CasePriority.P3);
                    break;
                case "-P4":
                    if (!CommandLine.PLeave.Contains(CasePriority.P3))
                        CommandLine.PLeave.Add(CasePriority.P4);
                    break;
            }
        }
    }
}
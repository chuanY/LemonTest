namespace LemonDefine.Interface
{
    using Enum;
    using System.Collections.Generic;
    using System;

    public interface ITestLog
    {
        void Init(string assemName, int caseCount);
        void Finish(List<Type> passList, List<Type> errorList, List<Type> failedList, List<Type> notRunList);
        void StartCase(string name, string caption);
        void EndCase(CaseResult result);
        void LogAsset(bool isFailed, string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
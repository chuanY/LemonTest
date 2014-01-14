namespace LemonDefine.Attribute
{
    using Enum;
    using System;

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class CaseResultAttribute : System.Attribute
    {
        public CaseResultAttribute(CaseResult caseResult)
        {
            CaseResult = caseResult;
        }

        public CaseResult CaseResult { get; set; }
    }
}
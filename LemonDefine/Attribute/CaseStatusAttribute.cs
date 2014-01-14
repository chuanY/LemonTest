namespace LemonDefine.Attribute
{
    using Enum;
    using System;

    public class CaseStatusAttribute : Attribute
    {
        public CaseStatusAttribute(CaseStatus caseStatus)
        {
            CaseStatus = caseStatus;
        }

        public CaseStatus CaseStatus { get; set; }
    }
}
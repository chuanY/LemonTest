namespace LemonDefine.Attribute
{
    using System;
    using Enum;

    public class CaseStatusAttribute : Attribute
    {
        public CaseStatusAttribute(CaseStatus caseStatus)
        {
            CaseStatus = caseStatus;
        }

        public CaseStatus CaseStatus { get; set; }
    }
}

namespace LemonDefine.Attribute
{
    using System;
    using Enum;

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class CasePriorityAttribute : Attribute
    {
        public CasePriorityAttribute(CasePriority casePriority)
        {
            CasePriority = casePriority;
        }

        public CasePriority CasePriority { get; set; }
    }
}
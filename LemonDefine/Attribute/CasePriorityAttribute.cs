namespace LemonDefine.Attribute
{
    using Enum;
    using System;

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
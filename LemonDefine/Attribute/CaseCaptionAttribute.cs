namespace LemonDefine.Attribute
{
    using System;

    public class CaseCaptionAttribute : Attribute
    {
        public CaseCaptionAttribute(String caseCaption)
        {
            CaseCaption = caseCaption;
        }

        public string CaseCaption { get; set; }
    }
}

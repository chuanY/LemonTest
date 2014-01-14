namespace LemonDefine.Enum
{
    public enum LemonExceptionType
    {
        EnvSetUp,
        CaseDestoryEnv,       //case导致ITestAssembly异常崩溃
        LoadTestAssem,
        OtherFaild             //其他原因
    }
}
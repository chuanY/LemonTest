namespace LemonDefine.Exception
{
    using Enum;

    public class TestEnvException : System.Exception
    {
        public LemonExceptionType ExceptionType { get; set; }

        public TestEnvException(LemonExceptionType type)
        {
            ExceptionType = type;
            if (type == LemonExceptionType.CaseDestoryEnv)
            {
                _message = "测试用例导致测试环境异常崩溃";
            }
            else if (type == LemonExceptionType.OtherFaild)
            {
                _message = "其它未知原因导致测试环境异常崩溃";
            }

        }

        public TestEnvException(LemonExceptionType type, string msg)
        {
            ExceptionType = type;
            _message = msg;
        }

        private readonly string _message;

        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }
}
namespace LemonTest
{
    using System;

    public class Entrance
    {
        /// <summary>
        /// ERRORLEVELdefine  0 - 成功
        ///                   1 - 参数解析失败
        ///                   2 - 测试用例设置和运行错误
        ///                   3 - 测试环境准备错误
        /// </summary>
        private static int Main(string[] args)
        {
            if (!ParseArg.ParseArgs(args))
                return 1;

            foreach (var item in CommandLine.AssemName)
            {
                try
                {
                    LemonMix.Instance.LemonInit(item);

                    LemonMix.Instance.LemonExcute();

                    LemonMix.Instance.LemonFinish();
                }
                catch (Exception e)
                {
                    return ExceptionHandle.HandleEx(e);
                }
            }

            return 0;
        }
    }
}
namespace LemonTest
{
    using System;
    using System.Linq;
    using LemonDefine.Enum;
    using LemonDefine.Exception;
    using LemonDefine.Log;

    internal class LemonMix
    {
        private TestProject TestProject;

        private static LemonMix _instance;

        private LemonMix()
        {
        }

        internal static LemonMix Instance
        {
            get { return _instance ?? (_instance = new LemonMix()); }
        }

        /// <summary>
        /// 结束的时候打log和清理状态
        /// </summary>
        internal void LemonFinish()
        {
            if (TestProject.Cases.Count != 0 && TestProject.TestAssmInstance != null)
                TestProject.TestAssmInstance.TestCleanup();

            var passed = from t in TestProject.Cases
                where t.result == CaseResult.Pass
                select t.CaseType;
            var error = from t in TestProject.Cases
                where t.result == CaseResult.Error
                        select t.CaseType;
            var failed = from t in TestProject.Cases
                where t.result == CaseResult.Failed
                         select t.CaseType;
            var notrun = from t in TestProject.Cases
                where t.result == CaseResult.NotRun
                         select t.CaseType;

            TestLog.Instance.Finish(passed.ToList(), error.ToList(), failed.ToList(), notrun.ToList());
        }

        /// <summary>
        /// execute test cases
        /// </summary>
        internal void LemonExcute()
        {
            foreach (var @case in TestProject.Cases)
            {
                @case.ExcuteCase();
            }
        }

        /// <summary>
        /// init test env
        /// </summary>
        /// <returns>Cases List</returns>
        internal void LemonInit(string assName)
        {
            try
            {
                TestProject = new TestProject(assName);
            }
            catch (Exception e)
            {
                throw new TestEnvException(LemonExceptionType.LoadTestAssem, e.Message);
            }

            TestLog.Instance.Init(TestProject.asm.GetName().Name, TestProject.Cases.Count, CommandLine.OutputLog);

            if (TestProject.Cases.Count == 0 || TestProject.TestAssmInstance == null)
                return;
            if (!TestProject.TestAssmInstance.TestSetUp())
            {
                throw new TestEnvException(LemonExceptionType.EnvSetUp);
            }
        }
    }
}
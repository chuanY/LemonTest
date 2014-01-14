namespace LemonTest
{
    using LemonDefine;
    using LemonDefine.Attribute;
    using LemonDefine.Enum;
    using LemonDefine.Exception;
    using LemonDefine.Interface;
    using LemonDefine.Log;
    using System;
    using System.Linq;

    /// <summary>
    /// 定义一个Case的实体, 包含运行时及定义信息
    /// </summary>
    internal class CaseEntity
    {
        public Type CaseType;

        public ITestCase testCase;

        public String CaseCaption = string.Empty;

        public CaseResult result = CaseResult.NotRun;

        public CaseStatus Status = CaseStatus.InProgress;

        public CaseEntity(Type @case)
        {
            CaseType = @case;
            var status = @case.GetCustomAttributes(typeof(CaseStatusAttribute), false);
            var captions = @case.GetCustomAttributes(typeof(CaseCaptionAttribute), false);

            var caseCaptionAttribute = (CaseCaptionAttribute)captions.FirstOrDefault();
            CaseCaption = caseCaptionAttribute != null ? caseCaptionAttribute.CaseCaption : "DefultName";

            var caseStatusAttribute = (CaseStatusAttribute)status.FirstOrDefault();
            if (caseStatusAttribute != null)
            {
                Status = caseStatusAttribute.CaseStatus;
            }

            testCase = (ITestCase)Activator.CreateInstance(CaseType);
        }

        /// <summary>
        /// 执行当前测试用例
        /// </summary>
        /// <returns>返回运行结果</returns>
        public void ExcuteCase()
        {
            TestAsset.Instance.IsCaseFaild = false;
            TestLog.Instance.StartCase(CaseType.FullName, CaseCaption);
            if (Status != CaseStatus.Complete)
            {
                result = CaseResult.NotRun;
                TestLog.Instance.EndCase(result);
                return;
            }

            try
            {
                testCase.OnBegin();
                testCase.OnRun();
                result = TestAsset.Instance.IsCaseFaild ? CaseResult.Failed : CaseResult.Pass;
            }
            catch (Exception e)
            {
                TestLog.Instance.LogError(e.Message);
                result = CaseResult.Error;
            }

            try
            {
                testCase.OnEnd();

                if (testCase is IDisposable)
                    ((IDisposable)testCase).Dispose();
            }
            catch (TestEnvException ex)
            {
                result = CaseResult.Error;
                TestLog.Instance.LogError(ex.ToString() + '-' + ex.Message);

                throw;
            }
            catch (Exception e)
            {
                TestLog.Instance.LogError(e.Message);
                result = CaseResult.Error;
            }

            finally
            {
                TestLog.Instance.EndCase(result);
            }
        }
    }
}
namespace LemonTest
{
    using LemonDefine.Attribute;
    using LemonDefine.Enum;
    using LemonDefine.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class TestProject
    {
        public Assembly asm;
        public CaseStatus Status = CaseStatus.InProgress;
        public List<CaseEntity> Cases;
        public ITestAssembly TestAssmInstance;

        public TestProject(string pathName)
        {
            LoadAssembly(pathName);
            GetCasesFromAsm(asm);
            SelectDefinePlevelCases(ref Cases);
        }

        /// <summary>
        /// Load Test Assembly file, get ITestAssembly Interface
        /// </summary>
        private void LoadAssembly(string dllPath)
        {
            asm = Assembly.LoadFrom(dllPath);

            var assms = (from type in asm.GetTypes()
                where type.IsClass && (type.GetInterface("ITestAssembly") != null)
                select type).ToList();

            if (assms.Count > 0)
            {
                TestAssmInstance = (ITestAssembly) Activator.CreateInstance(assms[0]);
            }
        }

        /// <summary>
        /// 获取Assembly中所有的继承了ITestCase接口的用例
        /// </summary>
        private void GetCasesFromAsm(Assembly asmInstans)
        {
            if (CommandLine.CaseList.Count != 0)
            {
                Cases = (from type in asmInstans.GetTypes()
                    where type.IsClass && (type.GetInterface("ITestCase") != null)
                    where !type.IsAbstract
                    where
                        CommandLine.CaseList.Exists(name => type.Name == name) ||
                        CommandLine.CaseList.Exists(name => type.FullName == name)
                    orderby type.Name
                    select new CaseEntity(type)).ToList();
            }
            else
            {
                Cases = (from type in asmInstans.GetTypes()
                    where type.IsClass && (type.GetInterface("ITestCase") != null)
                    where !type.IsAbstract
                    orderby type.Name
                    select new CaseEntity(type)).ToList();
            }

        }

        /// <summary>
        /// 筛选符合参数指定的优先级的测试用例
        /// </summary>
        private void SelectDefinePlevelCases(ref List<CaseEntity> cases)
        {
            if (CommandLine.PLeave.Count == 0)
                return;

            cases = (from type in cases
                where type.CaseType.GetCustomAttributes(typeof (CasePriorityAttribute), false).Length != 0
                where
                    CommandLine.PLeave.Contains(
                        ((CasePriorityAttribute)
                            type.CaseType.GetCustomAttributes(typeof (CasePriorityAttribute), false)[0])
                            .CasePriority)
                select type).ToList();
        }
    }
}
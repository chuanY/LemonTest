namespace LemonDefine.Log
{
    using Enum;
    using Interface;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;

    public class TestLog
    {
        private static TestLog _instance;
        private static ITestLog _outputLog;
        private readonly Log2PrintImp _log2Console;

        private TestLog()
        {
            _log2Console = new Log2PrintImp();
        }

        public static TestLog Instance
        {
            get { return _instance ?? (_instance = new TestLog()); }
        }

        public void Init(string assemName, int caseCount, ITestLog outputLog)
        {
            _outputLog = outputLog;
            _log2Console.Init(assemName, caseCount);
            _outputLog.Init(assemName, caseCount);
        }

        public void Finish(List<Type> passList, List<Type> errorList, List<Type> failedList, List<Type> notRunList)
        {
            _outputLog.Finish(passList, errorList, failedList, notRunList);
            _log2Console.Finish(passList, errorList, failedList, notRunList);
        }

        public void StartCase(string name, string caption)
        {
            _log2Console.StartCase(name, caption);
            _outputLog.StartCase(name, caption);
        }

        public void EndCase(CaseResult result)
        {
            _log2Console.EndCase(result);
            _outputLog.EndCase(result);
        }

        public void LogAsset(bool isFailed, string message)
        {
            _log2Console.LogAsset(isFailed, message);
            _outputLog.LogAsset(isFailed, message);
        }

        public void LogInfo(string message)
        {
            _outputLog.LogInfo(message);
            _log2Console.LogInfo(message);
        }

        public void LogWarning(string message)
        {
            _outputLog.LogWarning(message);
            _log2Console.LogWarning(message);
        }

        public void LogError(string message)
        {
            _outputLog.LogError(message);
            _log2Console.LogError(message);
        }
    }

    public class Log2XmlImp : ITestLog
    {
        private XDocument _xDoc;
        private XElement _assemblyEle;
        private XElement _currentCase;
        private String _xmlFile;

        public void Init(string assemName, int caseCount)
        {
            var timeStr = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            _xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));

            _assemblyEle = new XElement(assemName,
                new XElement("StartTime") {Value = timeStr},
                new XElement("TestCases",
                    new XAttribute("CaseCount", caseCount))
                );
            _xDoc.Add(_assemblyEle);

            var timeFormatStr = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            _xmlFile = assemName + timeFormatStr + ".xml";
        }

        public void Finish(List<Type> passList, List<Type> errorList, List<Type> failedList, List<Type> notRunList)
        {
            var timeStr = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            int caseCount = passList.Count + errorList.Count + failedList.Count + notRunList.Count;
            _assemblyEle.Add(
                new XElement("CaseCount") {Value = caseCount.ToString(CultureInfo.InvariantCulture)},
                new XElement("PassedCount") {Value = passList.Count.ToString(CultureInfo.InvariantCulture)},
                new XElement("ErroredCount") {Value = errorList.Count.ToString(CultureInfo.InvariantCulture)},
                new XElement("FailedCount") {Value = failedList.Count.ToString(CultureInfo.InvariantCulture)},
                new XElement("NotRunCount") {Value = notRunList.Count.ToString(CultureInfo.InvariantCulture)},
                new XElement("EndTimeCount") {Value = timeStr}
                );

            _xDoc.Save(_xmlFile);
        }

        public void StartCase(string name, string caption)
        {
            var timeStr = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            var casesEle = _assemblyEle.Element("TestCases");
            var newCase = new XElement("TestCase",
                new XAttribute("CaseName", name),
                new XElement("StartTime") {Value = timeStr},
                new XElement("Caption") {Value = caption});

            if (casesEle != null)
                casesEle.Add(newCase);

            _currentCase = newCase;
        }

        public void EndCase(CaseResult result)
        {
            var timeStr = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            _currentCase.Add(
                new XElement("Result") {Value = result.ToString()},
                new XElement("EndTime") {Value = timeStr}
                );
        }

        public void LogAsset(bool isFailed, string message)
        {
            CaseResult result = isFailed ? CaseResult.Failed : CaseResult.Pass;

            _currentCase.Add(
                new XElement("Assert" + result.ToString()) {Value = message}
                );
        }

        public void LogInfo(string message)
        {
            _currentCase.Add(
                new XElement("Info") {Value = message}
                );
        }

        public void LogWarning(string message)
        {
            _currentCase.Add(
                new XElement("Warning") {Value = message}
                );
        }

        public void LogError(string message)
        {
            _currentCase.Add(
                new XElement("Error") {Value = message}
                );
        }
    }

    public class Log2Nunit : ITestLog
    {
        private XDocument _xDoc;
        private XElement _assemblyEle;
        private XElement _resultsEle;
        private XElement _currentCase;
        private String _xmlFile;

        private DateTime _suiteStartTime;
        private DateTime _caseStartTime;

        public void Init(string assemName, int caseCount)
        {
            _suiteStartTime = DateTime.Now;
            _xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));

            _resultsEle = new XElement("test-results",
                new XAttribute("name", assemName),
                new XAttribute("date", _suiteStartTime.ToString("yyyy-MM-dd")),
                new XAttribute("time", _suiteStartTime.ToString("HH:mm:ss"))
                );

            _assemblyEle = new XElement("test-suite",
                new XElement("results"),
                new XAttribute("name", assemName)
                );

            _resultsEle.Add(_assemblyEle);
            _xDoc.Add(_resultsEle);

            _suiteStartTime = DateTime.Now;

            var timeFormatStr = _suiteStartTime.ToString("yyyy-MM-dd-HH-mm-ss");
            _xmlFile = assemName + timeFormatStr + "nunit" + ".xml";
        }

        public void Finish(List<Type> passList, List<Type> errorList, List<Type> failedList, List<Type> notRunList)
        {
            int errorCount = errorList.Count;
            int passCount = passList.Count;
            int failedCount = failedList.Count;
            int notRunCount = notRunList.Count;

            DateTime finishTime = DateTime.Now;
            var time = finishTime - _suiteStartTime;

            _assemblyEle.Add(
                new XAttribute("success", ((errorCount + failedCount) == 0).ToString()),
                new XAttribute("time", time.TotalSeconds.ToString(CultureInfo.InvariantCulture))
                );

            _resultsEle.Add(
                new XAttribute("total",
                    (passCount + errorCount + failedCount + notRunCount).ToString(CultureInfo.InvariantCulture)),
                new XAttribute("failures", (errorCount + failedCount).ToString(CultureInfo.InvariantCulture)),
                new XAttribute("not-run", notRunCount.ToString(CultureInfo.InvariantCulture))
                );

            _xDoc.Save(_xmlFile);
        }

        public void StartCase(string name, string caption)
        {
            _caseStartTime = DateTime.Now;
            var casesEle = _assemblyEle.Element("results");
            var newCase = new XElement("test-case",
                new XAttribute("name", name));

            if (casesEle != null)
                casesEle.Add(newCase);

            _currentCase = newCase;
        }

        public void EndCase(CaseResult result)
        {
            DateTime endTime = DateTime.Now;
            var time = endTime - _caseStartTime;

            _currentCase.Add(
                new XAttribute("executed", (!result.Equals(CaseResult.NotRun)).ToString()),
                new XAttribute("success", result.Equals(CaseResult.Pass).ToString()),
                new XAttribute("time", time.TotalSeconds.ToString(CultureInfo.InvariantCulture))
                );
        }

        public void LogAsset(bool isFailed, string message)
        {
            if (!isFailed)
                return;

            var failureEle = new XElement("failure",
                new XElement("message", new XCData(message)),
                new XElement("stack-trace", new XCData(message))
                );
            _currentCase.Add(failureEle);
            //CaseResult result = isFailed ? CaseResult.Failed : CaseResult.Pass;

            //_currentCase.Add(
            //    new XElement("Assert" + result.ToString()) { Value = message }
            //    );
        }

        public void LogInfo(string message)
        {
            //_currentCase.Add(
            //    new XElement("Info") { Value = message }
            //    );
        }

        public void LogWarning(string message)
        {
            //_currentCase.Add(
            //    new XElement("Warning") { Value = message }
            //    );
        }

        public void LogError(string message)
        {
            //_currentCase.Add(
            //    new XElement("Error") { Value = message }
            //    );
        }
    }

    public class Log2PrintImp : ITestLog
    {
        public void Init(string assemName, int caseCount)
        {
            Console.Title = "I'm Lemon";
            Console.WindowWidth = 137;
            Console.WindowHeight = 37;

            Console.WriteLine("Run Test Project {0} Count {1}", assemName, caseCount);
        }

        public void Finish(List<Type> passList, List<Type> errorList, List<Type> failedList, List<Type> notRunList)
        {

            int caseCount = passList.Count + errorList.Count + failedList.Count + notRunList.Count;
            var resultStr = string.Format("All:{0} Pass:{1} Error:{2} Failed:{3} NotRun:{4}",
                caseCount, passList.Count, errorList.Count, failedList.Count, notRunList.Count);

            if (errorList.Count + failedList.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Failed And Error Case List");
            }
            foreach (var item in errorList)
            {
                Console.WriteLine("Case:{0} - {1}", item.FullName, CaseResult.Error);
            }
            foreach (var item in failedList)
            {
                Console.WriteLine("Case:{0} - {1}", item.FullName, CaseResult.Failed);
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine(resultStr);
        }

        public void StartCase(string name, string caption)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("{0} - {1}", name, caption);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void EndCase(CaseResult result)
        {
            switch (result)
            {
                case CaseResult.Pass:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case CaseResult.Failed:
                case CaseResult.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case CaseResult.NotRun:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }

            Console.WriteLine("Result {0}", result);

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void LogAsset(bool isFailed, string message)
        {
            CaseResult result;
            if (isFailed)
            {
                result = CaseResult.Failed;
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                result = CaseResult.Pass;
                Console.ForegroundColor = ConsoleColor.Cyan;
            }

            Console.WriteLine("Asset {0}： {1}", message, result.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Info:{0}：", message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning:{0}：", message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error:{0}：", message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
namespace LemonDefine
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Log;

    public class TestAsset
    {
        private static TestAsset _instance;
        public bool IsCaseFaild = false;

        public static TestAsset Instance
        {
            get { return _instance ?? (_instance = new TestAsset()); }
        }

        public static void AssetFalse(bool? condition, string assetMsg = "No Message")
        {
            bool isFaild = false;
            var callInfoStr = Instance.GetCallInfo(assetMsg);

            if (condition == true)
            {
                isFaild = true;
                SetCaseFaild();
            }

            TestLog.Instance.LogAsset(isFaild, callInfoStr);
        }

        public static void AssetTrue(bool? condition, string assetMsg = "No Message")
        {
            bool isFaild = false;
            var callInfoStr = Instance.GetCallInfo(assetMsg);

            if (condition != true)
            {
                isFaild = true;
                SetCaseFaild();
            }

            TestLog.Instance.LogAsset(isFaild, callInfoStr);
        }

        public static void AssetEq<T>(T obj1, T obj2, string assetMsg = "No Message")
        {
            bool isFaild = false;
            var callInfoStr = Instance.GetCallInfo(assetMsg);

            if (obj1.Equals(null) || !obj1.Equals(obj2))
            {
                isFaild = true;
                SetCaseFaild();
            }

            TestLog.Instance.LogAsset(isFaild, callInfoStr);
        }

        public static void AssetNEq<T>(T obj1, T obj2, string assetMsg = "No Message")
        {
            bool isFaild = false;
            var callInfoStr = Instance.GetCallInfo(assetMsg);

            if (obj1.Equals(null) || obj1.Equals(obj2))
            {
                isFaild = true;
                SetCaseFaild();
            }

            TestLog.Instance.LogAsset(isFaild, callInfoStr);
        }

        public static void AssetGT<T>(T obj1, T obj2, string assetMsg = "No Message")
            where T : IComparable
        {
            bool isFaild = false;
            var callInfoStr = Instance.GetCallInfo(assetMsg);

            if (obj1.Equals(null) || obj1.CompareTo(obj2) <= 0)
            {
                isFaild = true;
                SetCaseFaild();
            }

            TestLog.Instance.LogAsset(isFaild, callInfoStr);
        }

        public static void AssetLT<T>(T obj1, T obj2, string assetMsg = "No Message")
            where T : IComparable
        {
            bool isFaild = false;
            var callInfoStr = Instance.GetCallInfo(assetMsg);

            if (obj1.Equals(null) || obj1.CompareTo(obj2) >= 0)
            {
                isFaild = true;
                SetCaseFaild();
            }

            TestLog.Instance.LogAsset(isFaild, callInfoStr);
        }

        public static void AssetNull(object obj, string assetMsg = "No Message")
        {
            bool isFaild = false;
            var callInfoStr = Instance.GetCallInfo(assetMsg);

            if (obj != null)
            {
                isFaild = true;
                SetCaseFaild();
            }

            TestLog.Instance.LogAsset(isFaild, callInfoStr);
        }

        public static void AssetNotNull(object obj, string assetMsg = "No Message")
        {
            bool isFaild = false;
            var callInfoStr = Instance.GetCallInfo(assetMsg);

            if (obj == null)
            {
                isFaild = true;
                SetCaseFaild();
            }

            TestLog.Instance.LogAsset(isFaild, callInfoStr);
        }

        private static void SetCaseFaild()
        {
            //if (t.GetInterface("ITestCase") == null)
            //{
            //    Console.WriteLine(t.ToString());
            //    return;
            //}

            Instance.IsCaseFaild = true;

        }

        private string GetCallInfo(string assetMsg)
        {
            // 获取调用者的Type
            var frame = new StackTrace(true).GetFrame(2);

            int lineNum = frame.GetFileLineNumber();

            var name = frame.GetFileName() ?? "Unknow File";
            var fileName = name.Split('\\').Last();

            var callInfoStr = string.Format("{0} Line:{1} Asset：{2}", fileName, lineNum, assetMsg);

            return callInfoStr;
        }
    }
}
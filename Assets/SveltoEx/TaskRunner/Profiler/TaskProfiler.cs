using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

//This profiler is based on the Entitas Visual Debugging tool 
//https://github.com/sschmid/Entitas-CSharp

namespace Svelto.Tasks.Profiler
{
    public sealed class TaskProfiler
    {
        static readonly Stopwatch _stopwatch = new Stopwatch();

        internal static readonly Dictionary<string, TaskInfo> taskInfos =
            new Dictionary<string, TaskInfo>();

        public static bool MonitorUpdateDuration(IEnumerator enumerator, int threadID)
        {
            bool value = MonitorUpdateDuration(enumerator, " ThreadID: ".FastConcat(threadID));

            return value;
        }

        public static bool MonitorUpdateDuration(IEnumerator enumerator)
        {
            bool value = MonitorUpdateDuration(enumerator, " MainThread: ");

            return value;
        }

        public static void ResetDurations()
        {
            taskInfos.Clear();
        }

        static bool MonitorUpdateDuration(IEnumerator enumerator, string threadInfo)
        {
            TaskInfo info;

            bool result;
            string name = enumerator.ToString().FastConcat(enumerator.GetHashCode());

            lock (_safeDictionary)
            {
                if (taskInfos.TryGetValue(name, out info) == false)
                {
                    info = new TaskInfo(name);

                    info.AddUpdateDuration(_stopwatch.Elapsed.TotalMilliseconds);
                    info.AddThreadInfo(threadInfo);

                    taskInfos.Add(name, info);
                }
                else
                {
                    info.AddUpdateDuration(_stopwatch.Elapsed.TotalMilliseconds);
                    info.AddThreadInfo(threadInfo);
                }
            }
            
            _stopwatch.Start();
            result = enumerator.MoveNext();
            _stopwatch.Reset();

            return result;
        }

        static object _safeDictionary = new object();
    }
}
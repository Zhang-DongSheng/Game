using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 控制台
    /// </summary>
    public class ConsoleView : ViewBase
    {
        [SerializeField] private List<ConsoleBase> views;

        [SerializeField] private PrefabTemplate prefab;

        private readonly List<ItemConsoleToggle> toggles = new List<ItemConsoleToggle>();

        private int count;

        private void Awake()
        {
            count = views.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= toggles.Count)
                {
                    var item = prefab.Create<ItemConsoleToggle>();

                    item.Refresh(new ToggleParameter()
                    {
                        index = i,
                        name = views[i].Name,
                        callback = OnClickToggle,
                    });
                    toggles.Add(item);
                }
                views[i].Initialize();
            }
            OnClickToggle(0);
        }

        private void Update()
        {
            count = views.Count;

            for (int i = 0; i < count; i++)
            {
                views[i].Refresh(Time.deltaTime);
            }
        }

        private void OnClickToggle(int index)
        {
            count = toggles.Count;

            for (int i = 0; i < count; i++)
            {
                toggles[i].Select(index);
            }
            count = views.Count;

            for (int i = 0; i < count; i++)
            {
                views[i].SetActive(i == index);
            }
        }

        private void OnDestroy()
        {
            count = views.Count;

            for (int i = 0; i < count; i++)
            {
                views[i].Dispose();
            }
        }
    }
}
/*
namespace Console
{
    public class GameConsole : MonoBehaviour
    {

        private IEnumerator Save__Log(LogType ignore)
        {
            using (FileStream fs = new FileStream(GameConfig.Path_Log, FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs);

                for (int i = 0; i < log_data.Count; i++)
                {
                    if (log_data[i].type >= ignore)
                    {
                        sw.WriteLine(log_data[i].message);
                        sw.WriteLine(log_data[i].source);
                    }
                }

                sw.Dispose();
            }

            yield return null;
        }

        private IEnumerator Save__Profiler(int second)
        {
            Profiler.logFile = GameConfig.Path_Profiler;
            Profiler.enabled = true;
            Profiler.enableBinaryLog = true;

            for (int i = 0; i < second; i++)
            {
                yield return new WaitForSeconds(1);
            }

            Profiler.enableBinaryLog = false;

            yield return null;
        }

        #region Function
        public void Clear_Log()
        {
            log_data.Clear();
        }

        public void Save_Log(LogType ignore = LogType.Log)
        {
            StartCoroutine(Save__Log(ignore));
        }

        public void Save_Profiler(int second)
        {
            StartCoroutine(Save__Profiler(second));
        }

        public void Save_ScreenCapture()
        {
            ScreenCapture.CaptureScreenshot(GameConfig.Path_ScreenCapture, 0);
        }

        public void ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            string[] rule = command.ToLower().Split(' ');

            if (rule.Length > 1)
            {
                switch (rule[0])
                {
                    case "get":
                        Debug.Log("获取");
                        break;
                    case "level":
                        Debug.Log("关卡");
                        break;
                    default:
                        Debug.LogWarningFormat("暂未支持该类型命令:{0}", rule[0]);
                        break;
                }
            }
            else
            {
                switch (rule[0])
                {
                    case "levelup":
                        Debug.Log("升级");
                        break;
                    default:
                        Debug.LogWarningFormat("暂未支持该类型命令:{0}", rule[0]);
                        break;
                }
            }
        }
    }
}
*/
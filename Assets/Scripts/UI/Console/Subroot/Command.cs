using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    internal class Command
    {
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

using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor.Window
{
    class AutomationScript : CustomWindow
    {
        private string path;

        private string content;

        private Vector2 area;

        private readonly List<Member> members = new List<Member>();

        private readonly StringBuilder builder = new StringBuilder();
        [MenuItem("Assets/Create/Script", false, 81)]
        protected static void Open()
        {
            Open<AutomationScript>("简单脚本生成工具");
        }

        protected override void Init()
        {
            path = "NewScript";
        }

        protected override void Refresh()
        {
            GUILayout.BeginHorizontal();
            {
                #region Left Menu
                GUILayout.BeginVertical(GUILayout.Width(50));
                {
                    foreach (var value in Enum.GetValues(typeof(MemberType)))
                    {
                        MemberType member = (MemberType)value;

                        switch (member)
                        {
                            case MemberType.Constructor:
                            case MemberType.Custom:
                            case MemberType.TypeInfo:
                            case MemberType.NestedType:
                            case MemberType.All:
                                break;
                            default:
                                if (GUILayout.Button(member.ToString()))
                                {
                                    members.Add(new Member()
                                    {
                                        access = AccessModifiers.Public,
                                        member = member,
                                        name = string.Format("{0}{1}", member, index.value),
                                        order = members.Count,
                                        ID = index.value++
                                    });
                                }
                                break;
                        }
                    }
                }
                GUILayout.EndVertical();
                #endregion

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical();
                {
                    GUILayout.Label(string.Format("成员数量：{0}", members.Count));

                    scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(Height - 150));
                    {
                        for (int i = 0; i < members.Count; i++)
                        {
                            RefreshMember(members[i]);
                        }
                    }
                    GUILayout.EndScrollView();

                    GUILayout.Label("预览");

                    area = GUILayout.BeginScrollView(area);
                    {
                        GUILayout.TextArea(content, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                #region Right Menu
                GUILayout.BeginVertical(GUILayout.Width(150));
                {
                    path = GUILayout.TextField(path);

                    if (GUILayout.Button("刷新"))
                    {
                        content = Preview(members.ToArray());
                    }
                    if (GUILayout.Button("生成"))
                    {
                        Create(path, members.ToArray());
                    }
                    if (GUILayout.Button("重置"))
                    {
                        if (members.Count > 0)
                        {
                            members.Clear();
                        }
                        index.value = 0;
                    }
                }
                GUILayout.EndVertical();
                #endregion
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshMember(Member member)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(string.Format("*{0}", member.order), GUILayout.Width(30));

                    member.access = (AccessModifiers)EditorGUILayout.EnumPopup(member.access, GUILayout.Width(90));
                    member.variable = (VariableType)EditorGUILayout.EnumPopup(member.variable, GUILayout.Width(90));

                    switch (member.variable)
                    {
                        case VariableType.Array:
                        case VariableType.List:
                        case VariableType.Stack:
                        case VariableType.Dictionary:
                            member.assistant = (VariableType)EditorGUILayout.EnumPopup(member.assistant, GUILayout.Width(60));
                            break;
                    }
                    member.name = GUILayout.TextField(member.name);

                    if (GUILayout.Button("↑", GUILayout.Width(30)))
                    {
                        int index = members.FindIndex(x => x.ID == member.ID);

                        if (index != -1)
                        {
                            Sort(index, -1);
                        }
                    }
                    if (GUILayout.Button("↓", GUILayout.Width(30)))
                    {
                        int index = members.FindIndex(x => x.ID == member.ID);

                        if (index != -1)
                        {
                            Sort(index, 1);
                        }
                    }
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        member.preview = !member.preview;
                    }
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        if (EditorUtility.DisplayDialog("删除", string.Format("确认删除 {0}", member.name), "Yes", "No"))
                        {
                            int index = members.FindIndex(x => x.ID == member.ID);

                            if (index != -1)
                            {
                                members.RemoveAt(index);
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();

                if (member.preview)
                {
                    switch (member.member)
                    {
                        case MemberType.Event:
                            RefreshMemberField(member);
                            break;
                        case MemberType.Field:
                            RefreshMemberField(member);
                            break;
                        case MemberType.Method:
                            RefreshMemberMethod(member);
                            break;
                        case MemberType.Property:
                            RefreshMemberProperty(member);
                            break;
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private void RefreshMemberField(Member member)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(member.Code, GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshMemberMethod(Member member)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(member.Code, GUILayout.ExpandWidth(true));
                member.content = EditorGUILayout.TextArea(member.content);
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshMemberProperty(Member member)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(member.Code, GUILayout.ExpandWidth(true));
                member.content = EditorGUILayout.TextArea(member.content);
            }
            GUILayout.EndHorizontal();
        }

        private string Preview(params Member[] parameters)
        {
            return Builder(parameters);
        }

        private void Create(string script, params Member[] parameters)
        {
            string path = Utility.Path.New(string.Format("{0}/{1}.cs", Application.dataPath, script));

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter writer = new StreamWriter(stream);

                writer.Write(Builder(parameters));

                writer.Flush(); writer.Dispose();
            }
            AssetDatabase.Refresh();
        }

        private string Builder(params Member[] parameters)
        {
            builder.Clear();
            builder.AppendLine("using System;");
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine("using UnityEngine.UI;");
            builder.AppendLine("using System.Collections;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine();
            builder.AppendLine("namespace Game");
            builder.AppendLine("{");
            builder.AppendLine(string.Format("\tpublic class {0}: MonoBehaviour", path));
            builder.AppendLine("\t{");

            int count = parameters.Length;

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    builder.AppendLine(parameters[i].Code);
                }
            }
            else
            {
                builder.AppendLine("\t\t");
            }
            builder.AppendLine("\t}");
            builder.AppendLine("}");
            return builder.ToString();
        }

        private void Sort(int index, int value)
        {
            int _index = index + value;

            if (_index > -1 && _index < members.Count)
            {
                Member member = members[index];

                members.RemoveAt(index);

                members.Insert(_index, member);
            }
        }
    }

    public class Member
    {
        public int ID;

        public string name;

        public int order;

        public bool preview;

        public AccessModifiers access;

        public MemberType member;

        public VariableType variable;

        public VariableType assistant;

        public string content;

        private readonly StringBuilder builder = new StringBuilder();

        public string Code
        {
            get
            {
                builder.Clear();

                switch (member)
                {
                    case MemberType.Event:
                        {
                            builder.Append("\t\t");
                            builder.Append(CodeUtils.Modifiers(access));
                            builder.Append(" ");
                            builder.Append("Action");
                            if (variable != VariableType.None)
                            {
                                builder.Append("<");
                                builder.Append(CodeUtils.Variable(variable, assistant));
                                builder.Append(">");
                            }
                            builder.Append(" ");
                            builder.Append(name);
                            builder.Append(";");
                        }
                        break;
                    case MemberType.Field:
                        {
                            builder.Append("\t\t");
                            builder.Append(CodeUtils.Modifiers(access));
                            builder.Append(" ");
                            builder.Append(CodeUtils.Variable(variable, assistant));
                            builder.Append(" ");
                            builder.Append(name);
                            builder.Append(";");
                        }
                        break;
                    case MemberType.Method:
                        {
                            builder.Append("\t\t");
                            builder.Append(CodeUtils.Modifiers(access));
                            builder.Append(" ");
                            builder.Append(CodeUtils.Variable(variable, assistant));
                            builder.Append(" ");
                            builder.Append(name);
                            builder.Append("()");
                            builder.AppendLine();
                            builder.Append("\t\t");
                            builder.AppendLine("{");
                            builder.Append("\t\t\t\t");
                            if (string.IsNullOrEmpty(content))
                            {
                                if (variable != VariableType.None)
                                {
                                    builder.AppendLine("return default;");
                                }
                                else
                                {
                                    builder.AppendLine();
                                }
                            }
                            else
                            {
                                builder.AppendLine(content);
                            }
                            builder.Append("\t\t");
                            builder.Append("}");
                        }
                        break;
                    case MemberType.Property:
                        {
                            builder.Append("\t\t");
                            builder.Append("private");
                            builder.Append(" ");
                            builder.Append(CodeUtils.Variable(variable, assistant));
                            builder.Append(" ");
                            builder.Append(string.Format("_{0};", name.ToLower()));
                            builder.AppendLine();
                            builder.Append("\t\t");
                            builder.Append(CodeUtils.Modifiers(access));
                            builder.Append(" ");
                            builder.Append(CodeUtils.Variable(variable, assistant));
                            builder.Append(" ");
                            builder.Append(name);
                            builder.AppendLine();
                            builder.Append("\t\t");
                            builder.AppendLine("{");
                            builder.Append("\t\t\t\t");
                            builder.AppendLine("get");
                            builder.Append("\t\t\t\t");
                            builder.AppendLine("{");
                            builder.Append("\t\t\t\t\t\t");
                            builder.AppendLine(string.Format("return _{0};", name.ToLower()));
                            builder.Append("\t\t\t\t");
                            builder.AppendLine("}");
                            builder.Append("\t\t\t\t");
                            builder.AppendLine("set");
                            builder.Append("\t\t\t\t");
                            builder.AppendLine("{");
                            if (!string.IsNullOrEmpty(content))
                            {
                                builder.Append("\t\t\t\t\t\t");
                                builder.AppendLine(content);
                            }
                            builder.Append("\t\t\t\t\t\t");
                            builder.AppendLine(string.Format("_{0} = value;", name.ToLower()));
                            builder.Append("\t\t\t\t");
                            builder.AppendLine("}");
                            builder.Append("\t\t");
                            builder.Append("}");
                        }
                        break;
                }
                return builder.ToString();
            }
        }
    }
}
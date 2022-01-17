using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace UnityEditor.Define
{
    public static class CodeUtils
    {
        public static string Modifiers(AccessModifiers access)
        {
            switch (access)
            {
                case AccessModifiers.Public:
                    return "public";
                case AccessModifiers.ProtectedInternal:
                    return "protected internal";
                case AccessModifiers.Protected:
                    return "protected";
                case AccessModifiers.Internal:
                    return "internal";
                case AccessModifiers.PrivateProtected:
                    return "private protected";
                case AccessModifiers.Private:
                    return "private";
                default: return string.Empty;
            }
        }

        public static string Variable(VariableType variable, VariableType assistant)
        {
            switch (variable)
            {
                case VariableType.Array:
                    return string.Format("{0}[]", Variable(assistant));
                case VariableType.List:
                    return string.Format("List<{0}>", Variable(assistant));
                case VariableType.Stack:
                    return string.Format("Stack<{0}>", Variable(assistant));
                case VariableType.Dictionary:
                    return string.Format("Dictionary<string, {0}>", Variable(assistant));
                default:
                    return Variable(variable);
            }
        }

        public static string Variable(VariableType variable)
        {
            switch (variable)
            {
                case VariableType.None:
                    return "void";
                case VariableType.Byte:
                    return "byte";
                case VariableType.Bool:
                    return "bool";
                case VariableType.Char:
                    return "char";
                case VariableType.Int:
                    return "int";
                case VariableType.Float:
                    return "float";
                case VariableType.Double:
                    return "double";
                case VariableType.String:
                    return "string";
                case VariableType.Array:
                    return "void";
                case VariableType.List:
                    return "byte";
                case VariableType.Stack:
                    return "bool";
                case VariableType.Dictionary:
                    return "";
                default:
                    goto case VariableType.None;
            }
        }
    }
    /// <summary>
    /// 访问修饰符
    /// </summary>
    public enum AccessModifiers
    {
        Public,             //同一程序集中的任何其他代码或引用该程序集的其他程序集都可以访问该类型或成员
        ProtectedInternal,  //该类型或成员可由对其进行声明的程序集或另一程序集中的派生 class 中的任何代码访问
        Protected,          //只有同一 class 或者从该 class 派生的 class 中的代码可以访问该类型或成员
        Internal,           //同一程序集中的任何代码都可以访问该类型或成员，但其他程序集中的代码不可以
        PrivateProtected,   //该类型或成员可以通过从 class 派生的类型访问，这些类型在其包含程序集中进行声明
        Private,            //只有同一 class 或 struct 中的代码可以访问该类型或成员
    }

    public enum MemberType
    {
        Constructor = 1,
        Event = 2,
        Field = 4,
        Method = 8,
        Property = 16,
        TypeInfo = 32,
        Custom = 64,
        NestedType = 128,
        All = 191
    }

    public enum VariableType
    {
        None,
        Bool,
        Byte,
        Char,
        Int,
        Float,
        Double,
        Long,
        String,
        Array,
        List,
        Stack,
        Dictionary,
    }
}
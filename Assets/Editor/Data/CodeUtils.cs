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
    /// �������η�
    /// </summary>
    public enum AccessModifiers
    {
        Public,             //ͬһ�����е��κ�������������øó��򼯵��������򼯶����Է��ʸ����ͻ��Ա
        ProtectedInternal,  //�����ͻ��Ա���ɶ�����������ĳ��򼯻���һ�����е����� class �е��κδ������
        Protected,          //ֻ��ͬһ class ���ߴӸ� class ������ class �еĴ�����Է��ʸ����ͻ��Ա
        Internal,           //ͬһ�����е��κδ��붼���Է��ʸ����ͻ��Ա�������������еĴ��벻����
        PrivateProtected,   //�����ͻ��Ա����ͨ���� class ���������ͷ��ʣ���Щ����������������н�������
        Private,            //ֻ��ͬһ class �� struct �еĴ�����Է��ʸ����ͻ��Ա
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
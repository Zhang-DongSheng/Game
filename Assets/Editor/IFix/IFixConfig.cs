using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace IFix
{
    [Configure]
    public class IFixConfig
    {
        [IFix]
        static IEnumerable<Type> hotfix
        {
            get
            {
                var types = (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                             where
                             (
                                type.Namespace != null &&
                                type.Namespace.StartsWith("Game") && !type.IsGenericType
                             )
                             select type);
                return new List<Type>(types);
            }
        }
        [Filter]
        static bool Filter(System.Reflection.MethodInfo methodInfo)
        {
            return methodInfo.DeclaringType.FullName.Contains("Editor");
        }
        [MenuItem("InjectFix/Generate Warp Code")]
        static void GeneWarpCode()
        {
            var types = hotfix;

            HashSet<Type> typeSet = new HashSet<Type>();
            foreach (var type in types)
            {
                if (type.IsNestedPrivate)
                {
                    continue;
                }
                if (typeof(Delegate).IsAssignableFrom(type))
                {
                    typeSet.Add(type);
                    continue;
                }
                if (type.IsInterface)
                {
                    typeSet.Add(type);
                }
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var method in methods)
                {
                    if (method.IsGenericMethod)
                    {
                        continue;
                    }
                    var parameters = method.GetParameters();
                    foreach (var parameter in parameters)
                    {
                        if (typeof(Delegate).IsAssignableFrom(parameter.ParameterType) && !parameter.ParameterType.IsByRef)
                        {
                            typeSet.Add(parameter.ParameterType);
                        }
                    }
                }
            }
        }
    }
}
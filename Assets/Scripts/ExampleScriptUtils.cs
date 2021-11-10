using System.Linq;
using System.Reflection;
using UnityEditor;

namespace QuickEye.HowToAsync
{
    public static class ExampleScriptUtils
    {
        private static readonly TypeCache.MethodCollection AllExampleMethods =
            TypeCache.GetMethodsWithAttribute<ExampleMethodAttribute>();

        // [MenuItem("Test/Test")]
        // public static void test()
        // {
        //     foreach (var method in GetAllExampleMethods(Selection.activeObject as MonoScript))
        //     {
        //         Debug.Log($"MES: {method.Name}");
        //     }
        //
        //     Debug.Log($"MES: End");
        // }
        public static (MethodInfo method, ExampleMethodAttribute att)[] GetAllExampleMethods(MonoScript script)
        {
            var type = script.GetClass();
            return AllExampleMethods.Where(m => m.DeclaringType == type)
                .Select(m => (m, m.GetCustomAttribute<ExampleMethodAttribute>())).ToArray();
        }
    }
}
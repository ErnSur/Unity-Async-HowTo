using UnityEditor;
using UnityEngine.Events;

namespace QuickEye.HowToAsync
{
    public class ExampleElementModel
    {
        public readonly string title;
        private readonly MonoScript script;

        public UnityAction RunCodeExample { get; }
    
        public UnityAction OpenScript => () => AssetDatabase.OpenAsset(script);

        public ExampleElementModel(string exampleTitle, MonoScript monoScript, UnityAction runExample)
        {
            title = exampleTitle;
            script = monoScript;
            RunCodeExample = runExample;
        }
    }
}
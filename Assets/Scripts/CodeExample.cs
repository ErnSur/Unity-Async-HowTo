using UnityEngine.Events;

namespace QuickEye.HowToAsync
{
    public class CodeExample
    {
        public readonly string Name;
        public readonly UnityAction RunCodeAction;

        public CodeExample(string name, UnityAction runCodeAction)
        {
            Name = name;
            RunCodeAction = runCodeAction;
        }
    }
}
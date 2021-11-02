using QuickEye.Utility;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class CodeExampleListView : MonoBehaviour
    {
        [SerializeField]
        private Container<CodeExampleElement> exampleList;

        public void Setup(params CodeExample[] examples)
        {
            exampleList.Clear();
            foreach (var example in examples)
            {
                exampleList.AddNew().Init(example);
            }
        }
    }
}
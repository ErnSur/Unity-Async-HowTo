using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace QuickEye.HowToAsync
{
    public class CodeExampleElement : MonoBehaviour
    {
        [SerializeField]
        private Button runCodeButton, openScriptButton;

        [SerializeField]
        private Text label;

        public void Init(CodeExample example)
        {
            name = 
            label.text = example.Name;
            runCodeButton.onClick.AddListener(example.RunCodeAction);
        }
    }
}
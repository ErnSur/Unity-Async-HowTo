using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.HowToAsync
{
    public class CodeExampleElement : MonoBehaviour
    {
        [SerializeField]
        private Button runCodeButton, openScriptButton;

        [SerializeField]
        private TMP_Text label;

        public void Init(ExampleElementModel exampleElementModel)
        {
            name = 
            label.text = exampleElementModel.title;
            runCodeButton.onClick.AddListener(exampleElementModel.RunCodeExample);
            openScriptButton.onClick.AddListener(exampleElementModel.OpenScript);
        }
    }
}
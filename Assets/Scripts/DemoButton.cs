using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace QuickEye.HowToAsync
{
    public class DemoButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private Text label;

        public void Init(string title, UnityAction action)
        {
            name = 
            label.text = title;
            button.onClick.AddListener(action);
        }
    }
}
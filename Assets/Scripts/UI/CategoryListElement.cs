using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.HowToAsync
{
    public class CategoryListElement : MonoBehaviour
    {
        [SerializeField]
        private Text categoryLabel;

        [SerializeField]
        private Button button;

        public void Init(ExampleCategory data, Action onClick)
        {
            categoryLabel.text = data.Title;
            button.onClick.AddListener(onClick.Invoke);
        }
    }
}
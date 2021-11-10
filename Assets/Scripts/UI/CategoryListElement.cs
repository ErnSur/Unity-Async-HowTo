using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace QuickEye.HowToAsync
{
    public class CategoryListElement : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text categoryLabel;

        [FormerlySerializedAs("button")]
        [SerializeField]
        private Toggle toggle;

        public void Init(ExampleCategory data,ToggleGroup group, Action onClick)
        {
            categoryLabel.text = data.title;
            toggle.group = group;
            toggle.onValueChanged.AddListener(_=>onClick?.Invoke());
            toggle.onValueChanged.AddListener(_=>onClick?.Invoke());
        }
    }
}
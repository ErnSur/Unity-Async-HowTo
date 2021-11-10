using System;
using QuickEye.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.HowToAsync
{
    public class CategoryListView : MonoBehaviour
    {
        public event Action<ExampleCategory> SelectionChanged;

        [SerializeField]
        private Container<CategoryListElement> categoryList;

        [SerializeField]
        private ToggleGroup toggleGroup;

        public void Init(params ExampleCategory[] categories)
        {
            foreach (var category in categories)
            {
                categoryList.AddNew().Init(category, toggleGroup, () => SelectionChanged?.Invoke(category));
            }
        }
    }
}
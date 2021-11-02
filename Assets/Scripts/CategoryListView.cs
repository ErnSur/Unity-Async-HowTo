using System;
using QuickEye.Utility;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class CategoryListView : MonoBehaviour
    {
        public event Action<ExampleCategory> SelectionChanged;

        [SerializeField]
        private Container<CategoryListElement> categoryList;

        public void Init(params ExampleCategory[] categories)
        {
            foreach (var category in categories)
            {
                categoryList.AddNew().Init(category, () => SelectionChanged?.Invoke(category));
            }
        }
    }
}
using System.Collections.Generic;

namespace QuickEye.HowToAsync
{
    public class ExampleCategory
    {
        public string Title;
        public CodeExample[] Examples;

        public ExampleCategory(string title, params CodeExample[] examples)
        {
            Title = title;
            Examples = examples;
        }
    }
}
using System;

namespace QuickEye.HowToAsync
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExampleMethodAttribute : Attribute
    {
        public string Title { get; }
        public ExampleType ExampleType { get; }

        public ExampleMethodAttribute(string title, ExampleType type = ExampleType.Good) => (Title, ExampleType) = (title, type);
    }

    public enum ExampleType
    {
        Good,
        ImportantToRemember,
        Bad,
    }
}
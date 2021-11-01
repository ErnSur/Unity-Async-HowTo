using System;

namespace QuickEye.HowToAsync
{
    public class DemoException : Exception
    {
        public DemoException() : base("Demo Exception, should be visible inside the console.") { }
    }
}
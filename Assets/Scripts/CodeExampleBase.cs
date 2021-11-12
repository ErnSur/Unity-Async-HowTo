using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public abstract class CodeExampleBase
    {
        protected static readonly Logger tl = new (new ThreadLogger());
        
    }
}
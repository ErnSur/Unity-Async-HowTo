using UnityEngine;

namespace QuickEye.HowToAsync
{
    public static class RichTextExtensions
    {
        public static string In(this string s,Color color)
        {
            var hex = ColorUtility.ToHtmlStringRGBA(color);
            return $"<color=#{hex}>{s}</color>";
        }
        
        public static string Bold(this string s)
        {
            return $"<b>{s}</b>";
        }
    }
}
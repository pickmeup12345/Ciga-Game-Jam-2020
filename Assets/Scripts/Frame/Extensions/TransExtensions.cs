using UnityEngine;

namespace CjGameDevFrame.Common
{
    public static class TransExtensions
    {
        public static void DestroyChildren(this Transform trans)
        {
            for (var i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }
    }
}
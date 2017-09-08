using UnityEngine;

namespace KaijuRL.Extensions
{
    static class ColorExt
    {
        public static Color Grayscale(float tint)
        {
            return Color.Lerp(Color.white, Color.black, tint);
        }
    }
}

using System;
using UnityEngine;

namespace HappyUnity.Data
{
    [System.Serializable]
    public class ColorData
    {
        public float r;
        public float g;
        public float b;
        public float a;
        
        private static readonly int Color = Shader.PropertyToID("_Color");

        public ColorData()
        {
        }

        public static implicit operator Color(ColorData data)
        {
            return new Color(data.r, data.g, data.b, data.a);
        }

        public void New(Color obj)
        {
            r = obj.r;
            g = obj.g;
            b = obj.b;
            a = obj.a;
        }

        public static implicit operator ColorData(Color color)
        {
            return new ColorData {r = color.r, g = color.g, b = color.b, a = color.a};
        }
    
        public static implicit operator ColorData(Material mat)
        {
            if(!mat.HasProperty(Color))
                throw new NullReferenceException("Material has no _Color property");

            var color = mat.GetColor(Color);
            return new ColorData {r = color.r, g = color.g, b = color.b, a = color.a};
        }
    }
}

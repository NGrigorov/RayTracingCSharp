using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.Materials
{
    public class Diffuse : Material
    {
        public Diffuse(float intensity, float brigthtness, float[] color)
        {
            this.Intensity = intensity;
            this.Brightness = brigthtness;
            this.Color = color;
        }
    }
}

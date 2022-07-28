using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.Materials
{
    class Mirror : Material
    {
        public Mirror(float intensity, float brigthtness, float[] color, float Reflectiveness)
        {
            this.Intensity = intensity;
            this.Brightness = brigthtness;
            this.Color = color;
            this.Reflectiveness = Reflectiveness;
        }
    }
}

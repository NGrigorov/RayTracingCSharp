using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.Materials
{
    class Refractive : Material
    {
        public Refractive(float[] color, float[] absoprtion)
        {
            this.Color = color;
            this.Absorption = absoprtion;
        }
    }
}

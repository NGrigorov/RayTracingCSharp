using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.Lights
{
    public class PointLight : Light
    {
        public PointLight(float Intensity, Vector3 Position)
        {
            this.Intensity = Intensity;
            this.Position = Position;
        }
    }
}

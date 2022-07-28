using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    public abstract class Material
    {
        private float intensity;
        private float brightness;
        private float[] color;
        private float reflectiveness;
        private float[] absorption;
        public float Intensity { get => intensity; set => intensity = value; }
        public float Brightness { get => brightness; set => brightness = value; }
        public float[] Color { get => color; set => color = value; }
        public float Reflectiveness { get => reflectiveness; set => reflectiveness = value; }
        public float[] Absorption { get => absorption; set => absorption = value; }
    }
}

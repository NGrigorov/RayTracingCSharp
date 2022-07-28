using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    public abstract class Light
    {
        private float intensity;
        private Vector3 position;
        public Vector3 Position { get => position; set => position = value; }
        public float Intensity { get => intensity; set => intensity = value; }
    }
}

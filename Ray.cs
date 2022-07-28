using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    public class Ray
    {
        public Vector3 origin;
        public Vector3 direction;
        public Ray()
        {
            origin = new Vector3(0f, 0f, 0f);
            origin = new Vector3(0f, 0f, -1f);
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public Vector3 GetAt(float t)
        {
            return this.origin + this.direction * t;
        }
    }
}

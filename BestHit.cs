using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    public class BestHit
    {
        public Vector3 hitPoint;
        public Vector3 normal;
        public float t;
        public float[] color;
        public HittableObjects hitObject;
        public bool faceSide;

        public void SetfaceSide(Ray ray, Vector3 normal)
        {
            faceSide = Vector3.Dot(ray.direction, normal) < 0;
            this.normal = faceSide ? normal : -normal;
        }
    }
}

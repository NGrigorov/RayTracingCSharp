using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    public abstract class HittableObjects
    {
        public abstract bool Hit(Ray ray, BestHit hit);
        public abstract Vector3 GetNormals(Vector3 intersectionPoint);

        public abstract float HitNormal();

        public abstract float[] GetColor(Vector3 hitPoint);

        public abstract Material GetMaterial();
    }
}

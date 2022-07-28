using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template.Solids
{
    class Square : HittableObjects
    { //TODO
        Vector3 min;
        Vector3 max;
        Vector3 position;
        public Material mat;

        public Square(Vector3 min, Vector3 max, Vector3 position, Material mat)
        {
            this.min = min;
            this.max = max;
            this.position = position;
            this.mat = mat;
        }

        public override float[] GetColor(Vector3 hitPoint)
        {
            return new float[] { 1, 0, 0 };
        }
        public override Material GetMaterial()
        {
            return this.mat;
        }

        public override Vector3 GetNormals(Vector3 intersectionPoint)
        {
            Vector3 direction = intersectionPoint - position;
            float biggestValue = float.NaN;

            for (int i = 0; i < 3; i++)
            {
                if (float.IsNaN(biggestValue) || biggestValue < Math.Abs(direction[i]))
                {
                    biggestValue = Math.Abs(direction[i]);
                }
            }

            if (biggestValue == 0)
            {
                return new Vector3(0, 0, 0);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Math.Abs(direction[i]) == biggestValue)
                    {
                        float[] normal = new float[] { 0, 0, 0 };
                        normal[i] = direction[i] > 0 ? 1 : -1;

                        return new Vector3(normal[0], normal[1], normal[2]);
                    }
                }
            }

            return new Vector3(0, 0, 0);
        }

        public override bool Hit(Ray ray, BestHit hit)
        {
            float t1 = 0, t2 = 0, tnear = float.NegativeInfinity, tfar = float.PositiveInfinity, temp;
            bool intersectFlag = true;
            Vector3 rayDirection = ray.direction;
            Vector3 rayOrigin = ray.origin;
            Vector3 b1 = min;
            Vector3 b2 = max;

            for (int i = 0; i < 3; i++)
            {
                if (rayDirection[i] == 0)
                {
                    if (rayOrigin[i] < b1[i] || rayOrigin[i] > b2[i])
                        intersectFlag = false;
                }
                else
                {
                    t1 = (b1[i] - rayOrigin[i]) / rayDirection[i];
                    t2 = (b2[i] - rayOrigin[i]) / rayDirection[i];
                    if (t1 > t2)
                    {
                        temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }
                    if (t1 > tnear)
                        tnear = t1;
                    if (t2 < tfar)
                        tfar = t2;
                    if (tnear > tfar)
                        intersectFlag = false;
                    if (tfar < 0)
                        intersectFlag = false;
                }
            }
            if (intersectFlag)
            {
                hit.t = t1 > t2 ? t1 : t2;
                hit.hitPoint = ray.GetAt(hit.t);
                hit.normal = GetNormals(hit.hitPoint);
                hit.SetfaceSide(ray, hit.normal);
                hit.color = GetColor(hit.hitPoint);
                hit.hitObject = this;
                return true;
            }
            else
                return false;
        }

        public override float HitNormal()
        {
            throw new NotImplementedException();
        }
    }
}

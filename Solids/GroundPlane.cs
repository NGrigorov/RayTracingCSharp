using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using template.Materials;

namespace template.Solids
{
    class GroundPlane : HittableObjects
    {
        Vector3 position;
        Vector3 normals;
        float d;
        bool isCheckered;
        public Material mat;
        public GroundPlane()
        {
            this.position = new Vector3(0,0,0);
            this.normals = new Vector3(0, 1, 0);
            this.d = 1;
            this.mat = new Mirror(1f,1f,new float[] { 1, 1, 1 },1);
        }

        public GroundPlane(Vector3 position, bool isCheckered, Material mat)
        {
            this.position = position;
            this.normals = new Vector3(0, 1, 0);
            this.d = 1;
            this.isCheckered = isCheckered;
            this.mat = mat;
        }

        public override float[] GetColor(Vector3 hitPoint)
        {
            float[] temp = new float[3];
            if (this.isCheckered)
            {
                if (((int)hitPoint.X ^ 1) == (int)hitPoint.X + 1 ^ ((int)hitPoint.Z ^ 1) == (int)hitPoint.Z + 1)
                {
                    temp[0] = 0.6f;
                    temp[1] = 0.6f;
                    temp[2] = 0.6f;
                    return temp;
                }
                else
                {
                    temp[0] = 0.8f;
                    temp[1] = 0.8f;
                    temp[2] = 0.8f;
                    return temp;
                }
            }
            Vector3 norm = GetNormals(hitPoint);
            temp[0] = norm.X;
            temp[1] = norm.Y;
            temp[2] = norm.Z;
            return temp;
        }

        public override Material GetMaterial()
        {
            return this.mat;
        }

        public override Vector3 GetNormals(Vector3 intersectionPoint)
        {
            return this.normals;
        }

        public override bool Hit(Ray ray, BestHit hit)
        {
            float t = -(Vector3.Dot(ray.origin, this.normals) + this.d) / (Vector3.Dot(ray.direction.Normalized(), this.normals));
            if (t >= 0) {
                hit.t = t;
                hit.hitPoint = ray.GetAt(hit.t);
                hit.normal = GetNormals(hit.hitPoint);
                hit.SetfaceSide(ray, hit.normal);
                hit.color = GetColor(hit.hitPoint);
                hit.hitObject = this;
                return true;
            }
            

            return false;
        }

        public override float HitNormal()
        {
            throw new NotImplementedException();
        }
    }
}

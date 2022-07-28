using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using template.Materials;
using Template;

namespace template.Solids
{
    class Sphere : HittableObjects
    {
        public Vector3 center;
        public float diameter;
        public Material mat;

        //Red Unit-Sphere at 0,0,0
        public Sphere()
        {
            this.center = new Vector3();
            this.diameter = 2;
            mat = new Diffuse(1f,0.5f, new float[] { 1f, 0, 0 });
        }
        public Sphere(Vector3 center, float radius, Material mat)
        {
            this.center = center;
            this.diameter = radius * radius;
            this.mat = mat;
        }

        public override float[] GetColor(Vector3 hitPoint)
        {
            float[] temp = new float[3];
            temp[0] = this.mat.Color[0];
            temp[1] = this.mat.Color[1];
            temp[2] = this.mat.Color[2];
            return temp;
        }

        public override Vector3 GetNormals(Vector3 intersectionPoint)
        {
            return (intersectionPoint - center) / (diameter / 2);
        }
        public override Material GetMaterial()
        {
            return this.mat;
        }

        public override bool Hit(Ray ray, BestHit hit)
        {
            Vector3 intersection = ray.origin - center;
            float a = Vector3.Dot(ray.direction, ray.direction);
            float b = 2 * Vector3.Dot(ray.direction, intersection);
            float c = Vector3.Dot(intersection, intersection) - diameter;
            float discriminant = b * b - 4 * a * c;
            float t0, t1;
            if (discriminant < 0) return false;
            else if (discriminant == 0) t0 = t1 = -0.5f * b / a;
            discriminant = (float)Math.Sqrt(discriminant);

            //float t0 = (-b - discriminant) / (2.0f * a);
            //float t1 = (-b + discriminant) / (2.0f * a);

            float q = (b > 0) ? -0.5f * (b + discriminant) : -0.5f * (b - discriminant);
            t0 = q / a;
            t1 = c / q;
            if (t0 < 0 && t1 < 0) return false;

            hit.t = t0 > t1 ? t1 : t0;
            hit.hitPoint = ray.GetAt(hit.t);
            hit.normal = GetNormals(hit.hitPoint).Normalized();
            hit.SetfaceSide(ray, hit.normal);
            hit.color = GetColor(hit.hitPoint);
            hit.hitObject = this;
            return true;
        }

        public override float HitNormal()
        {
            return 1;
        }
    }
}

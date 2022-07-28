using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    public class Camera
    {
        //Camera
        public float fieldOfVision;
        public float aspectRatio;
        public Vector3 cameraPosition;
        public Vector3 viewDirection;
        public Vector3 screenCenter;
        public Vector3 c0, c1, c2;
        public Matrix4 cameraToWorld;
        public Vector3 right, up, forward;
        public Ray[] rays;

        //Rendering
        public int samplesPerPixel;

        public Camera(float fieldOfVision, float aspectRatio, Vector3 cameraPosition, Vector3 viewDirection, int samplesPerPixel)
        {
            cameraToWorld = Matrix4.LookAt(cameraPosition, viewDirection, new Vector3(0, 1, 0));
            this.cameraPosition = cameraPosition;
            right = new Vector3((float)Math.Ceiling(this.cameraToWorld[0, 0]), (float)Math.Ceiling(this.cameraToWorld[0, 1]), (float)Math.Ceiling(this.cameraToWorld[0, 2]));
            up = new Vector3((float)Math.Ceiling(this.cameraToWorld[1, 0]), (float)Math.Ceiling(this.cameraToWorld[1, 1]), (float)Math.Ceiling(this.cameraToWorld[1, 2]));
            forward = new Vector3((float)Math.Ceiling(this.cameraToWorld[2, 0]), (float)Math.Ceiling(this.cameraToWorld[2, 1]), (float)Math.Ceiling(this.cameraToWorld[2, 2]));
            this.viewDirection = viewDirection;

            this.fieldOfVision = fieldOfVision;
            this.aspectRatio = aspectRatio;
            screenCenter = this.cameraPosition + this.fieldOfVision * this.viewDirection;

            c0 = screenCenter + new Vector3(-1,-1,0);
            c1 = screenCenter + new Vector3(1, -1, 0);
            c2 = screenCenter + new Vector3(-1, 1, 0);
            this.samplesPerPixel = samplesPerPixel; //TODO
        }

        public void GenerateCameraRays(int size)
        {
            rays = new Ray[size];
            for (int i = 0; i < size; i++)
            {
                rays[i] = new Ray();
            }
        }

        public void TranslateCamera(Vector3 direction)
        {
            Vector3 x, y, z;
            x = new Vector3((float)Math.Ceiling(this.cameraToWorld[0, 0]), (float)Math.Ceiling(this.cameraToWorld[0, 1]), (float)Math.Ceiling(this.cameraToWorld[0, 2]));
            y = new Vector3((float)Math.Ceiling(this.cameraToWorld[1, 0]), (float)Math.Ceiling(this.cameraToWorld[1, 1]), (float)Math.Ceiling(this.cameraToWorld[1, 2]));
            z = new Vector3((float)Math.Ceiling(this.cameraToWorld[2, 0]), (float)Math.Ceiling(this.cameraToWorld[2, 1]), (float)Math.Ceiling(this.cameraToWorld[2, 2]));

            Vector3 delta = direction.X * x + direction.Y * y + direction.Z * z;
            this.cameraToWorld = this.cameraToWorld * Matrix4.CreateTranslation(delta);
            this.cameraPosition = this.cameraToWorld.ExtractTranslation();

        }

        public void ChangeFoV(float a)
        {
            this.fieldOfVision += a;
            screenCenter = this.cameraPosition + this.fieldOfVision * this.viewDirection;

            c0 = screenCenter + (right + up);
            c1 = screenCenter + (-right + up);
            c2 = screenCenter + (right - up);
        }

        public void RotateCameraY(Vector3 direction) // TODO: STill not workign correctly
        {
            this.cameraToWorld *= Matrix4.CreateFromAxisAngle(this.right, direction.X);
            //this.cameraToWorld *= Matrix4.CreateFromAxisAngle(this.forward, direction.Z);

            c0 = VectorMultMatrix(c0, cameraToWorld);
            c1 = VectorMultMatrix(c1, cameraToWorld);
            c2 = VectorMultMatrix(c2, cameraToWorld);
        }

        public void RotateCameraX(Vector3 direction) // TODO: STill not workign correctly
        {
            this.cameraToWorld *= Matrix4.CreateFromAxisAngle(this.up, direction.Y * 0.01f);

            c0 = VectorMultMatrix(c0, cameraToWorld);
            c1 = VectorMultMatrix(c1, cameraToWorld);
            c2 = VectorMultMatrix(c2, cameraToWorld);
        }

        public Vector3 VectorMultMatrix(Vector3 a, Matrix4 b) //For rotation
        {
            Vector3 result;
            result.X = a.X * b[0, 0] + a.Y * b[0, 1] + a.Z * b[0, 2];
            result.Y = a.X * b[1, 0] + a.Y * b[1, 1] + a.Z * b[1, 2];
            result.Z = a.X * b[2, 0] + a.Y * b[2, 1] + a.Z * b[2, 2];
            return result;
        }
    }
}

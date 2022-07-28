using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using template;
using template.Lights;
using template.Materials;
using template.Solids;
namespace Template
{
	public class Sprite
	{
		Surface bitmap;
		static public Surface target;
		int textureID;
		public Sprite( string fileName )
		{
			bitmap = new Surface( fileName );
			textureID = bitmap.GenTexture();
		}
		public void Draw( float x, float y, float scale = 1.0f )
		{
			GL.BindTexture( TextureTarget.Texture2D, textureID );
			GL.Enable( EnableCap.Blend );
			GL.BlendFunc( BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha );
			GL.Begin( PrimitiveType.Quads );
			float u1 = (x * 2 - 0.5f * scale * bitmap.width) / target.width - 1;
			float v1 = 1 - (y * 2 - 0.5f * scale * bitmap.height) / target.height;
			float u2 = ((x + 0.5f * scale * bitmap.width) * 2) / target.width - 1;
			float v2 = 1 - ((y + 0.5f * scale * bitmap.height) * 2) / target.height;
			GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex2( u1, v2 );
			GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex2( u2, v2 );
			GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex2( u2, v1 );
			GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex2( u1, v1 );
			GL.End();
			GL.Disable( EnableCap.Blend );
		}
	}
	public class Surface
	{
		public int width, height;
		public int [] pixels;
		static bool fontReady = false;
		static Surface font;
		static int [] fontRedir;

		//Camera Stuff
		public Camera sceneCam;

		//Scene
		List<HittableObjects> scene;
		GroundPlane gp;
		Sphere sp, sp1,sp2, sp3;

		//Lights
		List<Light> sceneLights;
		PointLight pLight;

		//Rendering
		public bool hasRendered;
		public int globalReflectionDepth;
		public Surface( int w, int h )
		{
			//Window
			width = w;
			height = h;
			pixels = new int[w * h];

			//Camera
			sceneCam = new Camera(2, 16.0f/9.0f, new Vector3(0, 0, 0), new Vector3(0, 0, 1),60);
			sceneCam.GenerateCameraRays(w * h);
			//Samples per pixel TODO;
			hasRendered = false;
			globalReflectionDepth = 4;
			//Lights
			sceneLights = new List<Light>();
			pLight = new PointLight(1.1f, new Vector3(0f, 6f, 2f));
			sceneLights.Add(pLight);

			//Scene
			scene = new List<HittableObjects>();
			gp = new GroundPlane(new Vector3(0f, 0f, 1f), true, new Mirror(0.75f, 0.5f, new float[] { 1, 1, 0 },0.55f));
			sp = new Sphere(new Vector3(1.6f, 0f, 4f), 0.8f, new Diffuse(0.5f,0.5f, new float[] { 1, 0, 0 })); //R sphere
			sp1 = new Sphere(new Vector3(0f, 0.25f, 4f), 0.8f, new Refractive(new float[] { 0, 1, 0 },new float[] { 0.8f,0.6f,0.5f}));//Refractive
			//sp1 = new Sphere(new Vector3(0f, 0.25f, 4f), 0.8f, new Mirror(0.5f, 0.5f, new float[] { 0, 1, 0 }, 1f));//Mirror
			sp2 = new Sphere(new Vector3(0f, 2f, 4f), 0.8f, new Mirror(0.5f, 0.5f, new float[] { 1, 1, 0 }, 1f));//Mirror sphere
			sp3 = new Sphere(new Vector3(-1.6f, 0f, 4f), 0.8f, new Diffuse(0.5f,0.5f, new float[] { 0, 0, 1 }));//B sphere
			
			scene.Add(gp);
			scene.Add(sp);
			scene.Add(sp1);
			scene.Add(sp2);
			scene.Add(sp3);
			

		}
		public Surface( string fileName )
		{
 			Bitmap bmp = new Bitmap( fileName );
			width = bmp.Width;
			height = bmp.Height;
			pixels = new int[width * height];
			BitmapData data = bmp.LockBits( new Rectangle( 0, 0, width, height ), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			IntPtr ptr = data.Scan0;
			System.Runtime.InteropServices.Marshal.Copy( data.Scan0, pixels, 0, width * height );
			bmp.UnlockBits( data );
		}
		public int GenTexture()
		{
			int id = GL.GenTexture();
			GL.BindTexture( TextureTarget.Texture2D, id );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear );
			GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, pixels );
			return id;
		}
		public void Clear( int c )
		{
			for( int s = width * height, p = 0; p < s; p++ ) pixels[p] = c;
		}
		public void Box( int x1, int y1, int x2, int y2, int c )
		{
			int dest = y1 * width;
			for( int y = y1; y <= y2; y++, dest += width ) 
			{
				pixels[dest + x1] = c;
				pixels[dest + x2] = c;
			}
			int dest1 = y1 * width;
			int dest2 = y2 * width;
			for( int x = x1; x <= x2; x++ )
			{
				pixels[dest1 + x] = c;
				pixels[dest2 + x] = c;
			}
		}
		public void Bar( int x1, int y1, int x2, int y2, int c )
		{
			int dest = y1 * width;
			for( int y = y1; y <= y2; y++, dest += width ) for( int x = x1; x <= x2; x++ )
			{
				pixels[dest + x] = c;
			}
		}
		public void Line( int x1, int y1, int x2, int y2, int c )
		{
			if ((x1 < 0) || (y1 < 0) || (x2 < 0) || (y2 < 0) ||
				(x1 >= width) || (x2 >= width) || (y1 >= height) || (y2 >= height)) return;
			if (Math.Abs( x2 - x1 ) > Math.Abs( y2 - y1 ))
			{
				if (x2 < x1) { int h = x1; x1 = x2; x2 = h; h = y2; y2 = y1; y1 = h; }
				int l = x2 - x1;
				int dy = ((y2 - y1) * 8192) / l;
				y1 *= 8192;
				for( int i = 0; i < l; i++ )
				{
					pixels[x1++ + (y1 / 8192) * width] = c;
					y1 += dy;
				}
			}
			else
			{
				if (y2 < y1) { int h = x1; x1 = x2; x2 = h; h = y2; y2 = y1; y1 = h; }
				int l = y2 - y1;
				int dx = ((x2 - x1) * 8192) / l;
				x1 *= 8192;
				for( int i = 0; i < l; i++ )
				{
					pixels[x1 / 8192 + y1++ * width] = c;
					x1 += dx;
				}
			}
		}
		public void Plot( int pixelId, int c )
		{
			pixels[pixelId] = c;
		}
		public void Print( string t, int x, int y, int c )
		{
			if (!fontReady)
			{
				font = new Surface( "../../assets/font.png" );
				string ch = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+={}[];:<>,.?/\\ ";
				fontRedir = new int[256];
				for( int i = 0; i < 256; i++ ) fontRedir[i] = 0;
				for( int i = 0; i < ch.Length; i++ )
				{
					int l = (int)ch[i];
					fontRedir[l & 255] = i;
				}
				fontReady = true;
			}
			for( int i = 0; i < t.Length; i++ )
			{
				int f = fontRedir[(int)t[i] & 255];
				int dest = x + i * 12 + y * width;
				int src = f * 12;
				for( int v = 0; v < font.height; v++, src += font.width, dest += width ) for( int u = 0; u < 12; u++ )
				{
					if ((font.pixels[src + u] & 0xffffff) != 0) pixels[dest + u] = c;
				}
			}
		}
		public void Trace()
        {
            float u, v;
            int counter = 0;
            BestHit hit = new BestHit();
            float[] color;
            for (int i = width - 1; i >= 0; i--)
            {
                u = (float)i / width;
                for (int j = 0; j < height; j++, counter++)
                {
                    v = (float)j / height;

                    Vector3 point = sceneCam.c0 + v * (sceneCam.c1 - sceneCam.c0) + u * (sceneCam.c2 - sceneCam.c0);
                    Vector3 direction = point / point.Length;

					sceneCam.rays[counter].origin = sceneCam.cameraPosition;
					sceneCam.rays[counter].direction = direction;
					color = Trace(sceneCam.rays[counter], hit);

					if (color != null)
                        Plot(counter, Utility.RGBtoHex(color[0], color[1], color[2]));
                    else
                        Plot(counter, new Color4(v, u, 0.25f, 1).ToArgb());
                }
            }
            hasRendered = true;//TODO
        }	

		public float[] Trace(Ray ray, BestHit hit)
		{
			if (!FindIntersection(ray, hit)) return null; //No skybox so i'm using uv to generate rgb colors if its null
			float[] temp = hit.color;
			hit.color = DirectIllumination(hit);

            if (hit.hitObject.GetMaterial() is Mirror)
				hit.color = Reflect(ray, hit, globalReflectionDepth, hit.hitObject.GetMaterial().Reflectiveness);

			if (hit.hitObject.GetMaterial() is Refractive)
				hit.color = Refract(ray, hit);

			return Utility.MulColor(hit.color,temp); //Multiplied Color
			//return hit.color; //Clear color
		}

		public bool FindIntersection(Ray ray, BestHit hit)
		{
			BestHit Olddistance = new BestHit();
			Olddistance.t = float.PositiveInfinity;

			int index = -1;
            int counter = 0;
            foreach (HittableObjects item in scene)
			{
				if (item.Hit(ray, hit) )
				{
                    if (hit.t <= Olddistance.t)
                    {
                        Olddistance.t = hit.t;
                        index = counter;
                    }
				}
                counter++;


            }
            if (index != -1)
            {
				scene[index].Hit(ray, hit);
                return true;
            }
            return false;
		}

		public float[] DirectIllumination(BestHit hit)
		{
            BestHit lightHit = new BestHit();
			Vector3 direction = Vector3.Normalize(pLight.Position - hit.hitPoint);
			Ray lightRay = new Ray(hit.hitPoint + direction * 0.01f, direction);
            float t = Vector3.Dot(hit.normal, lightRay.direction);
            if (FindIntersection(lightRay, lightHit) && t > -1f)
            {
				hit.color[0] *= 0.75f; //arbitrary shadow values
				hit.color[1] *= 0.75f;
				hit.color[2] *= 0.75f;
				return hit.color;
            }
			hit.color[0] = pLight.Intensity * hit.color[0];
			hit.color[1] = pLight.Intensity * hit.color[1];
			hit.color[2] = pLight.Intensity * hit.color[2];
            return hit.color;
        }

		public float[] Reflect(Ray ray, BestHit hit, int depth, float totalRefle)
        {
			if (depth <= 0) return hit.color;//change this to just the color variable for a multiplication result.
			BestHit reflected = new BestHit();
			Vector3 direction = Vector3.Normalize(ray.direction - 2 * Vector3.Dot(ray.direction, hit.normal) * hit.normal);
			Ray reflectedRay = new Ray(hit.hitPoint + direction * 0.1f, direction);

			//hit.color = Utility.MulColor(hit.color, totalRefle);

			if (!FindIntersection(reflectedRay, reflected)) return hit.color;

			reflected.color = DirectIllumination(reflected); //I still havent figured out how to save hard data like this so im left to recalcuate if the point is in shadow again.
			
			reflected.color = Utility.MulColor(reflected.color, totalRefle);//Reduce brightness as more reflections are happening
			if (reflected.hitObject.GetMaterial() is Refractive)
				return Refract(reflectedRay, reflected);

			if (reflected.hitObject.GetMaterial() is Diffuse) 
				return reflected.color;

			hit.color[0] *= reflected.color[0];
			hit.color[1] *= reflected.color[1];
			hit.color[2] *= reflected.color[2];

			return Reflect(reflectedRay, reflected, depth-1, reflected.hitObject.GetMaterial().Reflectiveness * totalRefle);
        }

		public float[] Refract(Ray ray, BestHit hit)
		{
			float angle = (float)Math.Min(Vector3.Dot(-ray.direction.Normalized(), hit.normal), 1f);
			Vector3 dirIn = (hit.faceSide ? 3f : 1/3f) * (ray.direction.Normalized() + angle * hit.normal); //3 for somewhat a diamond
			Vector3 dirOut = hit.normal * (float)(-Math.Sqrt(Math.Abs(dirIn.LengthSquared)));
			Vector3 direction = dirIn + dirOut;
			
			Ray refracted = new Ray(ray.direction.Normalized() + direction * 0.1f, direction);
			float lenght = Utility.Vec3Distance(hit.hitPoint, refracted.origin);
			BestHit refHit = new BestHit();

			if (!FindIntersection(refracted, refHit)) 
			{ hit.color[0] = refracted.origin.X; hit.color[1] = refracted.origin.Y; hit.color[2] = refracted.origin.Z; } //I dont have a skybox so this 
			else if (hit.hitObject == refHit.hitObject)																	 //is one option
			{ hit.color[0] = refracted.origin.X; hit.color[1] = refracted.origin.Y; hit.color[2] = refracted.origin.Z; }
			else if (refHit.hitObject.GetMaterial() is Mirror) hit.color = Reflect(refracted, refHit, globalReflectionDepth, refHit.hitObject.GetMaterial().Reflectiveness);
			else
			{
				refHit.color = DirectIllumination(refHit);

				hit.color[0] = refHit.color[0];
				hit.color[1] = refHit.color[1];
				hit.color[2] = refHit.color[2];
			}

			hit.color[0] *= (float)Math.Exp(-hit.hitObject.GetMaterial().Absorption[0] * lenght); //Absorption
			hit.color[1] *= (float)Math.Exp(-hit.hitObject.GetMaterial().Absorption[1] * lenght);
			hit.color[2] *= (float)Math.Exp(-hit.hitObject.GetMaterial().Absorption[2] * lenght);

			return hit.color;
		}
	}
}

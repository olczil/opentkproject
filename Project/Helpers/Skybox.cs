//using System;
//using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.GraphicsLibraryFramework;
//using OpenTK.Windowing.Desktop;
//using Project.Helpers;
//using System.Drawing;
//using GlmSharp;
//using StbImageSharp;

//namespace Project.Helpers
//{
//    public class SkyBox
//    {

//        private static readonly float SIDE_LENGTH = 200;

//        private readonly float[] vertices = new float[]
//        {
//            -SIDE_LENGTH / 2, -SIDE_LENGTH / 2, -SIDE_LENGTH / 2,
//            -SIDE_LENGTH / 2, -SIDE_LENGTH / 2,  SIDE_LENGTH / 2,
//             SIDE_LENGTH / 2, -SIDE_LENGTH / 2,  SIDE_LENGTH / 2,
//             SIDE_LENGTH / 2, -SIDE_LENGTH / 2, -SIDE_LENGTH / 2,
//            -SIDE_LENGTH / 2,  SIDE_LENGTH / 2, -SIDE_LENGTH / 2,
//            -SIDE_LENGTH / 2,  SIDE_LENGTH / 2,  SIDE_LENGTH / 2,
//             SIDE_LENGTH / 2,  SIDE_LENGTH / 2,  SIDE_LENGTH / 2,
//             SIDE_LENGTH / 2,  SIDE_LENGTH / 2, -SIDE_LENGTH / 2
//        };

//        // counter clockwise specification, faces facing inward
//        private readonly uint[] indices = new uint[]
//        {
//            0, 1, 2,    0, 2, 3,    0, 5, 1,    0, 4, 5,
//            1, 6, 2,    1, 5, 6,    2, 7, 3,    2, 6, 7,
//            3, 4, 0,    3, 7, 4,    4, 6, 5,    4, 7, 6
//        };

//        private int vboID, vaoID;

//        private List<string> ImageFaces
//        {
       
//        public struct LoadFaces
//        {
//            public void GetImageFaces()
//            {
//                ImageFaces = new List<string>();
//                string dir = "/Users/olakrason/projects/HouseProject/Project/CubemapPhotos/";
//                ImageFaces.Add(Path.Combine(dir, "posz.jpg"));
//                ImageFaces.Add(Path.Combine(dir, "negz.jpg"));
//                ImageFaces.Add(Path.Combine(dir, "posy.jpg"));
//                ImageFaces.Add(Path.Combine(dir, "negy.jpg"));
//                ImageFaces.Add(Path.Combine(dir, "posx.jpg"));
//                ImageFaces.Add(Path.Combine(dir, "negx.jpg"));
//            }

//            }

//        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
//        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
//        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
//        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
//        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

//        GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

//            }
//        }


//        private readonly Shader skyboxShader;

//        public SkyBox()
//        {
//            vaoID = GLUtils.GenVAO();
//            vboID = GLUtils.GenVBO(vertices);
//            GLUtils.VaoFloatAttrib(vaoID, vboID, 0, 3, 3, 0);
//            GLUtils.AttachEBO(vaoID, indices);

//            skyboxShader = new Shader("/Users/olakrason/projects/HouseProject/Project/Helpers/Shaders/skybox.vert", "/Users/olakrason/projects/HouseProject/Project/Helpers/Shaders/skybox.frag");

//        }


//        public void Render(Window window)
//        {
//            skyboxShader.Use();
//           //skyboxShader.SetVector3("cameraPos", window.activeCam.Position);
//           // skyboxShader.SetMatrix4("cameraMatrix", window.GetProjectionMatrix());

//            GLUtils.DrawIndexedVAO(vaoID, indices.Length);
//        }

//    }
//}

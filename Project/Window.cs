using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Project.Helpers;
using System.Drawing;
using GlmSharp;
using Models;
using Shaders;

namespace Project
{
    public class Window : GameWindow
    {

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        private Camera camera;
        static bool firstMove = true;
        static Vector2 lastPos;

        Texture texture;

       

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.5f, 0.5f, 0.9f, 1.0f);


            DemoShaders.InitShaders("/Users/olakrason/projects/HouseProject/Project/Helpers/Shaders/");

            GL.Enable(EnableCap.DepthTest);


            camera = new Camera(Vector3.UnitZ * 15, Size.X / (float)Size.Y);

            CursorState = CursorState.Grabbed;
            texture = Texture.LoadFromFile("/Users/olakrason/projects/HouseProject/Project/bricks.png");
            texture.Use(TextureUnit.Texture0);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            mat4 P = mat4.Perspective(glm.Radians(50.0f), 1, 1, 50); //Wylicz macierz rzutowania
            mat4 V = mat4.LookAt(new vec3(0, 0, -3), new vec3(0, 0, 0), new vec3(0, 1, 0)); //Wylicz macierz widoku

            DemoShaders.spTextured.Use();

            GL.UniformMatrix4(DemoShaders.spTextured.U("P"), 1, false, P.Values1D); //Wyślij do zmiennej jednorodnej P programu cieniującego wartość zmiennej P zadeklarowanej powyżej
            GL.UniformMatrix4(DemoShaders.spTextured.U("V"), 1, false, V.Values1D); //Wyślij do zmiennej jednorodnej V programu cieniującego wartość zmiennej V zadeklarowanej powyżej

            mat4 M = mat4.Rotate(0, new vec3(0, 1, 0)) * mat4.Rotate(0, new vec3(1, 0, 0)); //Macierz modelu to iloczyun macierzy obrotu wokół osi Y i X.
            GL.UniformMatrix4(DemoShaders.spTextured.U("M"), 1, false, M.Values1D); //Wyślij do zmiennej jednorodnej M programu cieniującego wartość zmiennej M zadeklarowanej powyżej

            GL.EnableVertexAttribArray(DemoShaders.spTextured.A("vertex")); //Aktywuj przesyłanie danych do atrybutu vertex
            GL.EnableVertexAttribArray(DemoShaders.spTextured.A("texCoord")); //Aktywuj przesyłanie danych do atrybutu texCoord            

            GL.VertexAttribPointer(DemoShaders.spTextured.A("vertex"), 4, VertexAttribPointerType.Float, false, 0, MyTeapot.vertices); //Dane do atrybutu vertex pobieraj z tablicy MyCube.vertices
            GL.VertexAttribPointer(DemoShaders.spTextured.A("texCoord"), 2, VertexAttribPointerType.Float, false, 0, MyTeapot.texCoords); //Dane do atrybutu vertex pobieraj z tablicy MyCube.texCoords

            GL.DrawArrays(PrimitiveType.Triangles, 0, MyTeapot.vertexCount); //Rysuj model

            GL.DisableVertexAttribArray(DemoShaders.spTextured.A("vertex")); //Wyłącz przesyłanie danych do atrybutu vertex 
            GL.DisableVertexAttribArray(DemoShaders.spTextured.A("texCoord")); //Wyłącz przesyłanie danych do atrybutu texCoord


            SwapBuffers();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                camera.Position -= camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                camera.Position += camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            // Get the mouse state
            var mouse = MouseState;

            if (firstMove) // This bool variable is initially set to true.
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }

        }

        // In the mouse wheel function, we manage all the zooming of the camera.
        // This is simply done by changing the FOV of the camera.
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            camera.Fov -= e.OffsetY;
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            camera.AspectRatio = Size.X / (float)Size.Y;


        }

    }
}


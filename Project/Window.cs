﻿using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Project.Helpers;
using System.Drawing;
using GlmSharp;
using Models;

namespace Project
{
    public class Window : GameWindow
    {
        private Shader shader;
        private Vector2 lastPos;

        private Matrix4 view;
        private Matrix4 projection;
        private Matrix4 rotation;

        private float rotationRads = 0;

        static float speed_y; //Prędkość obrotu wokół osi Y [rad/s]
        static float speed_x; //Prędkość obrotu wokół osi X [rad/s]

        private Cube[] cubes = new Cube[] {
            // (center_x, center_y, center_z, width(x), height(y), depth(z), testure_path, texture_unit)
            // grass
            new Cube(0, -1f, 0, 50, 0.2f, 50, "/Users/olakrason/projects/HouseProject/Project/grass-2.png"),
            //pavement
            new Cube(0, -0.9f, 5f, 0.8f, 0.05f, 10.0f, "/Users/olakrason/projects/HouseProject/Project/pavement.png"),


            // sides (walls)
            new Cube(0, 0, 0, 5, 2f, 4f, "/Users/olakrason/projects/HouseProject/Project/wall-tex.jpg"),
            // door
            new Cube(0, -0.4f, 2.02f, 0.8f, 1.2f, 0.02f, "/Users/olakrason/projects/HouseProject/Project/door-2.jpg"),
            // front left side window
            new Cube(-1.4f, 0f, 2f, 0.8f, 0.8f, 0.02f, "/Users/olakrason/projects/HouseProject/Project/window-2.png"),
            // front right side window
            new Cube(1.4f, 0f, 2f, 0.8f, 0.8f, 0.02f, "/Users/olakrason/projects/HouseProject/Project/window-2.png"),

            // 2nd floor
            new Cube(0, 2f, 0, 5, 2f, 4f, "/Users/olakrason/projects/HouseProject/Project/wall-tex.jpg"),
            // front left side window
            new Cube(-1.4f, 2, 2f, 0.8f, 0.8f, 0.02f, "/Users/olakrason/projects/HouseProject/Project/window-2.png"),
            // front right side window
            new Cube(1.4f, 2, 2f, 0.8f, 0.8f, 0.02f, "/Users/olakrason/projects/HouseProject/Project/window-2.png"),

            //chimney
            new Cube(-1.5f, 3.5f, 0.1f, 0.5f, 2f, 0.5f, "/Users/olakrason/projects/HouseProject/Project/chimney.jpg"),
        };

        private Pyramid roof = new Pyramid(new Punkt { x = 0, y = 2.9f, z = 0 }, 5.5f, 2f, 4.5f, "/Users/olakrason/projects/HouseProject/Project/roof.jpg");

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.93f, 0.97f, 0.97f, 1.0f);


            GL.Enable(EnableCap.DepthTest);

            foreach (Cube cube in cubes)
            {
                cube.vbo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, cube.vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, cube.vertices.Length * sizeof(float), cube.vertices, BufferUsageHint.StaticDraw);

                cube.vao = GL.GenVertexArray();
                GL.BindVertexArray(cube.vao);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                shader = new Shader("/Users/olakrason/projects/HouseProject/Project/Helpers/Shaders/shader.vert", "/Users/olakrason/projects/HouseProject/Project/Helpers/Shaders/shader.frag");
                shader.Use();

                var positionLocation = shader.GetAttribLocation("aPosition");
                GL.EnableVertexAttribArray(positionLocation);
                GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                var texCoordLocation = shader.GetAttribLocation("aTexCoord");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

                cube.LoadTexture();
                cube.texture.Use(TextureUnit.Texture0);

                shader.SetInt("texture0", 0);
            }

            foreach (Triangle side in roof.triangles)
            {

                side.vbo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, side.vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, side.vertices.Length * sizeof(float), side.vertices, BufferUsageHint.StaticDraw);

                side.vao = GL.GenVertexArray();
                GL.BindVertexArray(side.vao);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                GL.EnableVertexAttribArray(0);

                shader = new Shader("/Users/olakrason/projects/HouseProject/Project/Helpers/Shaders/shader.vert", "/Users/olakrason/projects/HouseProject/Project/Helpers/Shaders/shader.frag");
                shader.Use();

                // Because there's now 5 floats between the start of the first vertex and the start of the second,
                // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
                // This will now pass the new vertex array to the buffer.
                var vertexLocation = shader.GetAttribLocation("aPosition");
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                // Next, we also setup texture coordinates. It works in much the same way.
                // We add an offset of 3, since the texture coordinates comes after the position data.
                // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
                var texCoordLocation = shader.GetAttribLocation("aTexCoord");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

                side.LoadTexture();
                side.texture.Use(TextureUnit.Texture0);

                shader.SetInt("texture0", 0);
            }


            view = Matrix4.CreateTranslation(0.0f, -2f, -20.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);

        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var model = Matrix4.Identity * rotation;

            foreach (Cube cube in cubes)
            {
                GL.BindVertexArray(cube.vao);

                cube.texture.Use(TextureUnit.Texture0);

                shader.Use();

                shader.SetMatrix4("model", model);
                shader.SetMatrix4("view", view);
                shader.SetMatrix4("projection", projection);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }


            foreach (Triangle side in roof.triangles)
            {
                GL.BindVertexArray(side.vao);
                side.texture.Use(TextureUnit.Texture0);

                shader.Use();
                shader.SetMatrix4("model", model);
                shader.SetMatrix4("view", view);
                shader.SetMatrix4("projection", projection);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }
            

            SwapBuffers();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            rotationRads += speed_y;
            Matrix4.CreateRotationY(rotationRads, out rotation);

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.Left))
            {
                speed_y = 0.001f;
            }

            if (input.IsKeyDown(Keys.Right))
            {
                speed_y = -0.001f;
            }

            if (input.IsKeyReleased(Keys.Left))
            {
                speed_y = 0;
            }

            if (input.IsKeyReleased(Keys.Right))
            {
                speed_y = 0;
            }
        }
    }
}


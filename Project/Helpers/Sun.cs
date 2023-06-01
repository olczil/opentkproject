using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Project.Helpers;

namespace Project
{
    public class Sun
    {
        public int vbo { get; set; }
        public int vao { get; set; }

        public Sun(float center_x, float center_y, float center_z, float width, float height, float depth)
        {
            float left_x = center_x - (width / 2);
            float right_x = center_x + (width / 2);
            float top_y = center_y + (height / 2);
            float bottom_y = center_y - (height / 2);
            float close_z = center_z + (depth / 2);
            float far_z = center_z - (depth / 2);

            vertices = new float[] {
                left_x,  bottom_y, far_z,
                right_x, bottom_y, far_z,
                right_x, top_y,    far_z,
                right_x, top_y,    far_z,
                left_x,  top_y,    far_z,
                left_x,  bottom_y, far_z,

                left_x,  bottom_y, close_z,
                right_x, bottom_y, close_z,
                right_x, top_y,    close_z,
                right_x, top_y,    close_z,
                left_x,  top_y,    close_z,
                left_x,  bottom_y, close_z,

                left_x,  top_y,    close_z,
                left_x,  top_y,    far_z,
                left_x,  bottom_y, far_z, 
                left_x,  bottom_y, far_z,  
                left_x,  bottom_y, close_z,
                left_x,  top_y,    close_z,

                right_x, top_y,    close_z,
                right_x, top_y,    far_z,  
                right_x, bottom_y, far_z,  
                right_x, bottom_y, far_z,  
                right_x, bottom_y, close_z,
                right_x, top_y,    close_z,

                left_x,  bottom_y, far_z,  
                right_x, bottom_y, far_z,  
                right_x, bottom_y, close_z,
                right_x, bottom_y, close_z,
                left_x,  bottom_y, close_z,
                left_x,  bottom_y, far_z,  

                left_x,  top_y,    far_z,
                right_x, top_y,    far_z,
                right_x, top_y,    close_z,
                right_x, top_y,    close_z,
                left_x,  top_y,    close_z,
                left_x,  top_y,    far_z,
            };
        }

        public float[] vertices { get; }
    }
}
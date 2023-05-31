using System;
namespace Project.Helpers
{
	public class Roof
	{
        public int vbo { get; set; }
        public int vao { get; set; }
        private string texturePath;
        public Texture texture { get; set; }

        public Roof(float center_x, float center_y, float center_z, float width, float height, float depth, string texturePath)
        {
            this.texturePath = texturePath;
            float left_x = center_x - (width / 2);
            float right_x = center_x + (width / 2);
            float top_y = center_y + (height / 2);
            float bottom_y = center_y - (height / 2);
            float close_z = center_z + (depth / 2);
            float far_z = center_z - (depth / 2);

            vertices = new float[] {

                left_x, bottom_y, far_z, //Bottom-left vertex
                right_x, bottom_y, far_z, //Bottom-right vertex
                0.0f,  top_y, 0.0f,  //Top vertex

                left_x, bottom_y, close_z, 
                right_x, bottom_y, close_z, 
                0.0f,  top_y, 0.0f, 

                left_x, bottom_y, far_z,
                left_x, bottom_y, close_z,
                0.0f, top_y, 0.0f,

                right_x, bottom_y, far_z,
                right_x, bottom_y, close_z,
                0.0f, top_y, 0.0f,

                right_x, bottom_y, far_z,
                right_x, bottom_y, close_z,
                0.0f, top_y, 0.0f,

                left_x, bottom_y, close_z,
                left_x, bottom_y, far_z,
                0.0f, top_y, 0.0f,

            };
        }

        public void LoadTexture()
        {
            texture = Texture.LoadFromFile(texturePath);
        }

        public float[] vertices { get; }


        public readonly uint[] indices =
        {
            0, 1, 2,
            0, 2, 3,
            0, 1, 4,
            1, 2, 4,
            2, 3, 4,
            3, 0, 4
        };

        float[] texCoords = {
    0.0f, 0.0f,  // lower-left corner  
    1.0f, 0.0f,  // lower-right corner
    0.5f, 1.0f   // top-center corner
};

    }
}



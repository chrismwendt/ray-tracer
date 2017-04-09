class scene_class
{
    private string input_text;
    private int input_text_position;

    public string input_path;
    public string output_path;
    private int samples_processed_index;
    public int samples_processed;
    public int ray_depth;
    public int image_width;
    public int image_height;
    public double radiance_clamp;
    public camera_class camera;
    public double[] sky_radiance;
    public System.Collections.Generic.List<double[]> vertex;
    public System.Collections.Generic.List<triangle_class> triangle;
    public System.Collections.Generic.List<sphere_class> sphere;
    public System.Collections.Generic.List<material_class> material;

    public scene_class ()
    {
        input_text = "";
        input_text_position = 0;

        output_path = "";
        samples_processed_index = 0;
        samples_processed = 0;
        ray_depth = 0;
        image_width = 0;
        image_height = 0;
        radiance_clamp = 0;
        camera = new camera_class ();
        sky_radiance = new double[3];
        vertex = new System.Collections.Generic.List<double[]> ();
        triangle = new System.Collections.Generic.List<triangle_class> ();
        sphere = new System.Collections.Generic.List<sphere_class> ();
        material = new System.Collections.Generic.List<material_class> ();
    }

    public void load (string input_path_input)
    {
        input_path = input_path_input;

        input_text_position = 0;

        vertex.Clear ();
        triangle.Clear ();
        sphere.Clear ();
        material.Clear ();

        input_text = System.IO.File.ReadAllText (input_path);

        output_path = text_read_line ();
        samples_processed_index = input_text_position;

        while (input_text[samples_processed_index] == '\n'
            || input_text[samples_processed_index] == '\r'
            || input_text[samples_processed_index] == ' ')
        {
            samples_processed_index++;
        }

        samples_processed = (int)text_read_number ();
        ray_depth = (int)text_read_number ();
        image_width = (int)text_read_number ();
        image_height = (int)text_read_number ();
        radiance_clamp = (double)text_read_number ();

        for (int i = 0; i < 3; i++)
        {
            camera.position[i] = (double)text_read_number ();
        }
        for (int i = 0; i < 3; i++)
        {
            camera.vector_look[i] = (double)text_read_number ();
        }
        for (int i = 0; i < 3; i++)
        {
            camera.vector_up[i] = (double)text_read_number ();
        }

        camera.view_angle_width = (double)text_read_number ();
        camera.view_angle_height = (double)text_read_number ();

        for (int i = 0; i < 3; i++)
        {
            sky_radiance[i] = (double)text_read_number ();
        }

        read_vertices ();
        read_triangles ();
        read_spheres ();
        read_materials ();
        
        camera.initialize (image_width, image_height);
    }

    public void write_samples_processed ()
    {
        string string1 = "";
        string string2 = "";
        int holder = input_text_position;

        string1 = input_text.Substring (0, samples_processed_index - 1);

        input_text_position = samples_processed_index;

        text_read_number ();

        string2 = input_text.Substring (input_text_position);

        input_text_position = holder;

        input_text = string1 + '\r' + samples_processed.ToString () + string2;

        System.IO.File.WriteAllText (input_path, input_text);
    }

    private void read_vertices ()
    {
        while (text_read_letter () == 'v')
        {
            double[] vertex_temporary;
            vertex_temporary = new double[3];

            for (int i = 0; i < 3; i++)
            {
                vertex_temporary[i] = text_read_number ();
            }

            vertex.Add (vertex_temporary);
        }
    }

    private void read_triangles ()
    {
        int index = 0;

        while (text_read_letter () == 't')
        {
            triangle_class triangle_temporary;
            triangle_temporary = new triangle_class ();

            for (int i = 0; i < 3; i++)
            {
                index = (int)text_read_number ();

                for (int n = 0; n < 3; n++)
                {
                    triangle_temporary.vertex[i][n] = vertex[index - 1][n];
                }
            }

            triangle_temporary.material_index = (int)text_read_number ();
            triangle_temporary.initialize ();

            triangle.Add (triangle_temporary);
        }
    }

    private void read_spheres ()
    {
        while (text_read_letter () == 's')
        {
            sphere_class sphere_temporary;
            sphere_temporary = new sphere_class ();

            for (int i = 0; i < 3; i++)
            {
                sphere_temporary.center[i] = text_read_number ();
            }

            sphere_temporary.radius = text_read_number ();

            sphere_temporary.material_index = (int)text_read_number ();

            sphere.Add (sphere_temporary);
        }
    }

    private void read_materials ()
    {
        while (text_read_letter () == 'm')
        {
            material_class material_temporary;
            material_temporary = new material_class ();

            for (int i = 0; i < 3; i++)
            {
                material_temporary.color[i].reflectivity = text_read_number ();
                material_temporary.reflect_glossiness = text_read_number ();
                material_temporary.color[i].refractivity = text_read_number ();
                material_temporary.refract_glossiness = text_read_number ();
                material_temporary.color[i].radiance = text_read_number ();
            }

            material_temporary.refractive_index = text_read_number ();

            material_temporary.calculate_mean ();

            material.Add (material_temporary);
        }
    }

    private char text_read_letter ()
    {
        char output = ' ';

        for (; input_text_position < input_text.Length; input_text_position++)
        {
            if (char.IsLetter (input_text[input_text_position]))
            {
                output = input_text[input_text_position];

                break;
            }
        }

        return output;
    }

    private double text_read_number ()
    {
        string output_string = "";
        double output = 0;

        for (; input_text_position < input_text.Length; input_text_position++)
        {
            if (char.IsDigit (input_text[input_text_position])
             || input_text[input_text_position] == '.'
             || input_text[input_text_position] == '-')
            {
                break;
            }
        }

        for (; input_text_position < input_text.Length; input_text_position++)
        {
            if (!char.IsDigit (input_text[input_text_position])
             && input_text[input_text_position] != '.'
             && input_text[input_text_position] != '-')
            {
                break;
            }

            output_string += input_text[input_text_position].ToString ();
        }

        if (output_string != "")
        {
            output = double.Parse (output_string);
        }

        return output;
    }

    private string text_read_line ()
    {
        string output = "";

        for (; input_text_position < input_text.Length; input_text_position++)
        {
            if (input_text[input_text_position] == '\n'
             || input_text[input_text_position] == '\r')
            {
                break;
            }

            output += input_text[input_text_position].ToString ();
        }

        return output;
    }
}
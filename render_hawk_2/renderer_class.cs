class renderer_class
{
    public scene_class scene;
    System.Drawing.Imaging.BitmapData output_image_bitmap_data;
    System.IntPtr output_image_pointer;
    byte[] output_image_data_byte;
    double[] output_image_data_double;
    public object lock_object;
    private int padding;

    public renderer_class ()
    {
        scene = new scene_class ();

        output_image_bitmap_data = new System.Drawing.Imaging.BitmapData ();
        output_image_pointer = (System.IntPtr)0;
        output_image_data_byte = new byte[0];
        output_image_data_double = new double[0];
        lock_object = new object ();
    }

    public void render_progressive (string input_path, System.ComponentModel.BackgroundWorker background_worker)
    {
        scene.load (input_path);

        //if certain inputs are not valid, return
        if (scene.image_width < 1
         || scene.image_height < 1
         || scene.ray_depth < 1
         || scene.material.Count < 1)
        {
            return;
        }

        System.Random random = new System.Random ();

        System.Drawing.Bitmap output_image = new System.Drawing.Bitmap (scene.image_width,
                                                                        scene.image_height,
                                                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        padding = output_image.Width % 4;

        output_image_data_byte = new byte[output_image.Width * output_image.Height * 3 + (output_image.Height * padding)];
        output_image_data_double = new double[output_image_data_byte.GetLength (0)];

        lock (lock_object)
        {
            if (System.IO.File.Exists (scene.output_path))
            {
                output_image = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile (scene.output_path);

                if (output_image.Width != scene.image_width
                 || output_image.Height != scene.image_height
                 || output_image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                {
                    output_image.Dispose ();

                    output_image = new System.Drawing.Bitmap (scene.image_width,
                                                              scene.image_height,
                                                              System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                }
                else
                {
                    output_image_bitmap_data = output_image.LockBits (new System.Drawing.Rectangle (0, 0, output_image.Width, output_image.Height),
                                                                      System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                                                      output_image.PixelFormat);
                    output_image_pointer = output_image_bitmap_data.Scan0;
                    output_image_data_byte = new byte[output_image_bitmap_data.Stride * output_image_bitmap_data.Height];
                    output_image_data_double = new double[output_image_data_byte.GetLength (0)];

                    System.Runtime.InteropServices.Marshal.Copy (output_image_pointer, output_image_data_byte, 0, output_image_data_byte.GetLength (0));

                    output_image.UnlockBits (output_image_bitmap_data);
                    output_image.Dispose ();

                    for (int i = 0; i < output_image_data_byte.GetLength (0); i++)
                    {
                        output_image_data_double[i] = (double)output_image_data_byte[i];
                    }

                    output_image_data_double = image_class.reverse_linear_clamp (output_image_data_double, scene.radiance_clamp);
                }
            }
        }

        while (!background_worker.CancellationPending)
        {
            output_image = new System.Drawing.Bitmap (scene.image_width,
                                                      scene.image_height,
                                                      System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            output_image_bitmap_data = output_image.LockBits (new System.Drawing.Rectangle (0, 0, output_image.Width, output_image.Height),
                                                              System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                                              output_image.PixelFormat);

            output_image_pointer = output_image_bitmap_data.Scan0;

            output_image_data_double = image_class.average (output_image_data_double, renderer_class.sample_image (scene, random, output_image_data_double.GetLength (0), background_worker), 1 / ((double)scene.samples_processed + 1));
            scene.samples_processed++;
            scene.write_samples_processed ();

            double[] output_image_data_double_view = image_class.linear_clamp (output_image_data_double, scene.radiance_clamp);
            output_image_data_byte = image_class.convert_double_to_byte (output_image_data_double_view);

            System.Runtime.InteropServices.Marshal.Copy (output_image_data_byte, 0, output_image_pointer, output_image_data_byte.GetLength (0));

            output_image.UnlockBits (output_image_bitmap_data);

            lock (lock_object)
            {
                output_image.Save (scene.output_path);
            }
            output_image.Dispose ();

            background_worker.ReportProgress (0, scene.samples_processed);
        }
    }

    private static double[] sample_image (scene_class scene, System.Random random, int length, System.ComponentModel.BackgroundWorker background_worker)
    {
        double[] output_image = new double[length];
        double[] pixel_color = new double[3];
        int i = 0;

        if (background_worker.CancellationPending)
        {
            return output_image;
        }

        for (int y = 0; y < scene.image_height; y++)
        {
            for (int x = 0; x < scene.image_width; x++)
            {
                pixel_color = renderer_class.sample_pixel (scene,
                                                           x,
                                                           y,
                                                           random,
                                                           background_worker);
                
                for (int n = 0; n < 3; n++)
                {
                    output_image[i + n] = pixel_color[2 - n];
                }

                i += 3;
            }

            i += scene.image_width % 4;
        }

        return output_image;
    }

    private static double[] sample_pixel (scene_class scene, int x, int y, System.Random random, System.ComponentModel.BackgroundWorker background_worker)
    {
        double[] output_pixel = new double[3];

        double[] position = new double[3];
        double[] vector = new double[3];

        position = scene.camera.position;
        vector = scene.camera.get_vector (scene, x, y);

        output_pixel = get_total_radiance (scene, x, y, random, position, vector);

        return output_pixel;

        /*
        double[][] output_pixel = new double[5][];
        double[] teh_uber_output_pixel = new double[3];
        for (int i = 0; i < output_pixel.GetLength (0); i++)
        {
            output_pixel[i] = new double[3];
        }


        double[] position = new double[3];
        double[] vector = new double[3];

        for (int i = 0; i < output_pixel.GetLength (0); i++)
        {
            position = scene.camera.position;
            vector = scene.camera.get_vector (scene, x, y);

            output_pixel[i] = get_total_radiance (scene, x, y, random, position, vector);
        }

        teh_uber_output_pixel = output_pixel[0];
        for (int i = 1; i < output_pixel.GetLength (0); i++)
        {
            teh_uber_output_pixel = vector_class.vector_average (teh_uber_output_pixel, output_pixel[i], 1 / ((double)i + 1));
        }

        return teh_uber_output_pixel;
        */
        
    }

    private static double[] get_total_radiance (scene_class scene, int x, int y, System.Random random, double[] position, double[] vector)
    {
        double[] total_color_radiance = new double[3];
        double[] vector_t = new double[3];
        double[] vector_s = new double[3];
        double[] multiplier = new double[3];
        for (int i = 0; i < 3; i++)
        {
            multiplier[i] = 1;
        }
        double medium_refractive_index = 1.0008;
        int triangle_hit = -1;
        int sphere_hit = -1;

        for (int i = 0; i < scene.ray_depth; i++)
        {
            vector_t = vector;
            vector_s = vector;

            triangle_hit = get_closest_triangle (scene, position, ref vector_t, triangle_hit);
            sphere_hit = get_closest_sphere (scene, position, ref vector_s, sphere_hit);

            if (vector_s != vector
             && (vector_t == vector
              || vector_class.vector_is_larger (vector_t, vector_s)))
            {
                vector = vector_s;
                triangle_hit = -1;

                if (sphere_hit != -1)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        total_color_radiance[n] += scene.material[scene.sphere[sphere_hit].material_index - 1].color[n].radiance;
                    }

                    if (i != scene.ray_depth - 1)
                    {
                        position = vector_class.vector_add (position, vector_class.vector_scale (vector, 1 - 0.000000001));
                        vector = scene.material[scene.sphere[sphere_hit].material_index - 1].get_next_direction_sphere (position, vector, scene.sphere[sphere_hit], ref medium_refractive_index, ref multiplier);
                    }
                }
                else
                {
                    for (int n = 0; n < 3; n++)
                    {
                        total_color_radiance[n] += scene.sky_radiance[n];
                    }

                    break;
                }
            }
            else
            {
                vector = vector_t;
                sphere_hit = -1;

                if (triangle_hit != -1)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        total_color_radiance[n] += scene.material[scene.triangle[triangle_hit].material_index - 1].color[n].radiance;
                    }

                    if (i != scene.ray_depth - 1)
                    {
                        position = vector_class.vector_add (position, vector_class.vector_scale (vector, 1 - 0.000000001));
                        vector = scene.material[scene.triangle[triangle_hit].material_index - 1].get_next_direction_triangle (vector, scene.triangle[triangle_hit], ref medium_refractive_index, ref multiplier);
                    }
                }
                else
                {
                    for (int n = 0; n < 3; n++)
                    {
                        total_color_radiance[n] += scene.sky_radiance[n];
                    }

                    break;
                }
            }
        }


        for (int i = 0; i < 3; i++)
        {
            total_color_radiance[i] *= multiplier[i];
        }

        return total_color_radiance;
    }

    private static int get_closest_sphere (scene_class scene, double[] position, ref double[] vector, int exclude)
    {
        int output_sphere = -1;
        double t_previous = double.MaxValue;
        double t = 0;

        for (int i = 0; i < scene.sphere.Count; i++)
        {
            if (i != exclude)
            {
                t = intersection_ray_sphere (position, vector, scene.sphere[i]);

                if (t < t_previous)
                {
                    t_previous = t;

                    output_sphere = i;
                }
            }
        }

        if (t_previous < double.MaxValue)
        {
            vector = vector_class.vector_scale (vector, t_previous);
        }

        return output_sphere;
    }

    private static int get_closest_triangle (scene_class scene, double[] position, ref double[] vector, int exclude)
    {
        int output_triangle = -1;
        double t_previous = double.MaxValue;
        double t = 0;

        for (int i = 0; i < scene.triangle.Count; i++)
        {
            if (i != exclude)
            {
                t = intersection_ray_triangle (position, vector, scene.triangle[i]);
                if (t > 0)
                {
                    t = double.PositiveInfinity;
                }
                else
                {
                    t = -t;
                }

                if (t < t_previous)
                {
                    t_previous = t;

                    output_triangle = i;
                }
            }
        }

        if (t_previous < double.MaxValue)
        {
            vector = vector_class.vector_scale (vector, t_previous);
        }

        return output_triangle;
    }

    public static double intersection_ray_sphere (double[] position, double[] vector, sphere_class sphere)
    {
        double output_t = double.PositiveInfinity;

        position = vector_class.vector_subtract (position, sphere.center);

        double a = vector_class.vector_dot_product (vector, vector);
        double b = 2 * vector_class.vector_dot_product (vector, position);
        double c = vector_class.vector_dot_product (position, position) - (sphere.radius * sphere.radius);

        double discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            return output_t;
        }

        double distance_square_root = System.Math.Sqrt (discriminant);
        double q = 0;

        if (b < 0)
        {
            q = (-b - distance_square_root) * 0.5;
        }
        else
        {
            q = (-b + distance_square_root) * 0.5;
        }

        double t0 = q / a;
        double t1 = c / q;

        if (t0 > t1)
        {
            double temporary = t0;
            t0 = t1;
            t1 = temporary;
        }

        if (t1 < 0)
        {
            return output_t;
        }

        if (t0 < 0)
        {
            output_t = t1;
            return output_t;
        }
        else
        {
            output_t = t0;
            return output_t;
        }
    }

    public static double intersection_ray_triangle (double[] position, double[] vector, triangle_class triangle)
    {
        double output_t;
        double[] vector_t, vector_p, vector_q;
        double determinant, determinant_inverse;
        double t, u, v;
        double tolerance;

        output_t = double.PositiveInfinity;
        vector_t = new double[3];
        vector_p = new double[3];
        vector_q = new double[3];
        determinant = 0;
        determinant_inverse = 0;
        t = 0;
        u = 0;
        v = 0;
        tolerance = 0.000000001;

        vector_p = vector_class.vector_cross_product (vector, triangle.edge_2);

        determinant = vector_class.vector_dot_product (triangle.edge_1, vector_p);
        if (determinant > -tolerance
         && determinant < tolerance)
        {
            return output_t;
        }

        determinant_inverse = 1.0 / determinant;

        vector_t = vector_class.vector_from_2_positions (position, triangle.vertex[0]);

        u = vector_class.vector_dot_product (vector_t, vector_p) * determinant_inverse;
        if (u < 0
         || u > 1)
        {
            return output_t;
        }

        vector_q = vector_class.vector_cross_product (vector_t, triangle.edge_1);

        v = vector_class.vector_dot_product (vector, vector_q) * determinant_inverse;
        if (v < 0
         || v + u > 1)
        {
            return output_t;
        }

        t = vector_class.vector_dot_product (triangle.edge_2, vector_q) * determinant_inverse;

        output_t = t;

        return output_t;
    }
}
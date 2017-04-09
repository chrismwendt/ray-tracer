class material_class
{
    public struct color_struct
    {
        public double reflectivity;
        public double refractivity;
        public double radiance;
    }

    public color_struct[] color;
    public double reflectivity_mean, reflect_glossiness;
    public double refractivity_mean, refract_glossiness, refractive_index;
    System.Random random;

    public material_class ()
    {
        color = new color_struct[3];
        for (int i = 0; i < color.GetLength (0); i++)
        {
            color[i] = new color_struct ();
        }
        reflectivity_mean = 0;
        refractivity_mean = 0;
        reflect_glossiness = 0;
        refract_glossiness = 0;
        refractive_index = 0;
        random = new System.Random ();
    }

    public void calculate_mean ()
    {
        double reflectivity_sum = 0;
        double refractivity_sum = 0;

        for (int i = 0; i < 3; i++)
        {
            reflectivity_sum += color[i].reflectivity;
            refractivity_sum += color[i].refractivity;
        }

        reflectivity_mean = reflectivity_sum / 3;
        refractivity_mean = refractivity_sum / 3;
    }

    public double[] get_next_direction_sphere (double[] input_position, double[] input_vector, sphere_class input_sphere, ref double incidence_ior, ref double[] multiplier)
    {
        double[] output_vector = new double[3];
        double[] random_vector = new double[3];
        double[] sphere_normal = new double[3];
        double[] u = new double[3];
        double[] v = new double[3];
        double a, b, c, angle, radius;

        angle = 2 * System.Math.PI * random.NextDouble ();
        radius = System.Math.Sqrt (random.NextDouble ());

        a = System.Math.Sin (angle) * radius;
        b = System.Math.Cos (angle) * radius;
        c = System.Math.Sqrt (1.0 - (a * a + b * b));

        sphere_normal = input_sphere.get_normal (input_position);

        if (vector_class.vector_dot_product (sphere_normal, input_vector) > 0)
        {
            sphere_normal = vector_class.vector_negative (sphere_normal);
        }

        double[] temporary = new double[3];

        for (int i = 0; i < 3; i++)
        {
            temporary[i] = random.NextDouble ();
        }

        u = vector_class.vector_cross_product (sphere_normal, temporary);
        v = vector_class.vector_cross_product (sphere_normal, u);
        u = vector_class.vector_cross_product (sphere_normal, v);
        u = vector_class.vector_unitize (u);
        v = vector_class.vector_unitize (v);

        random_vector = vector_class.vector_add (vector_class.vector_add (vector_class.vector_scale (v, b),
                                                                          vector_class.vector_scale (u, a)),
                                                 vector_class.vector_scale (sphere_normal, c));

        if (random.NextDouble () * (reflectivity_mean + refractivity_mean) < reflectivity_mean) //reflect
        {
            output_vector = vector_class.vector_average (random_vector,
                                                         get_reflection (input_vector, sphere_normal),
                                                         reflect_glossiness);

            for (int n = 0; n < 3; n++)
            {
                multiplier[n] *= color[n].reflectivity;
            }
        }
        else //refract
        {
            output_vector = vector_class.vector_average (vector_class.vector_negative (random_vector),
                                                         get_refraction (input_vector, sphere_normal, incidence_ior, ref multiplier),
                                                         refract_glossiness);

            for (int n = 0; n < 3; n++)
            {
                multiplier[n] *= color[n].refractivity;
            }

            incidence_ior = refractive_index;
        }

        return output_vector;
    }

    public double[] get_next_direction_triangle (double[] input_vector, triangle_class input_triangle, ref double incidence_ior, ref double[] multiplier)
    {
        double[] output_vector = new double[3];
        double[] random_vector = new double[3];
        double a, b, c, angle, radius;

        angle = 2 * System.Math.PI * random.NextDouble ();
        radius = System.Math.Sqrt (random.NextDouble ());

        a = System.Math.Sin (angle) * radius;
        b = System.Math.Cos (angle) * radius;
        c = System.Math.Sqrt (1.0 - (a * a + b * b));

        if (vector_class.vector_dot_product (input_triangle.normal, input_vector) > 0)
        {
            input_triangle.normal = vector_class.vector_negative (input_triangle.normal);
        }

        random_vector = vector_class.vector_add (vector_class.vector_add (vector_class.vector_scale (input_triangle.v, b),
                                                                          vector_class.vector_scale (input_triangle.u, a)),
                                                 vector_class.vector_scale (input_triangle.normal, c));

        if (random.NextDouble () * (reflectivity_mean + refractivity_mean) < reflectivity_mean) //reflect
        {
            output_vector = vector_class.vector_average (random_vector,
                                                         get_reflection (input_vector, input_triangle.normal),
                                                         reflect_glossiness);

            for (int n = 0; n < 3; n++)
            {
                multiplier[n] *= color[n].reflectivity;
            }
        }
        else //refract
        {
            output_vector = vector_class.vector_average (vector_class.vector_negative (random_vector),
                                                         get_refraction (input_vector, input_triangle.normal, incidence_ior, ref multiplier),
                                                         refract_glossiness);

            for (int n = 0; n < 3; n++)
            {
                multiplier[n] *= color[n].refractivity;
            }

            incidence_ior = refractive_index;
        }

        return output_vector;
    }

    private double[] get_reflection (double[] input_vector, double[] normal)
    {
        double[] output_vector = new double[3];
        double cos_theta = 0;

        input_vector = vector_class.vector_unitize (input_vector);

        cos_theta = vector_class.vector_dot_product (normal, vector_class.vector_negative (input_vector));

        output_vector = vector_class.vector_add (vector_class.vector_scale (normal, 2 * cos_theta), input_vector);

        return output_vector;
    }

    private double[] get_refraction (double[] input_vector, double[] normal, double incidence_ior, ref double[] multiplier)
    {
        double[] output_vector = new double[3];
        double cos_theta1 = 0;
        double cos_theta2 = 0;
        double n1, n2;
        double randomness = random.NextDouble ();
        double temporary = randomness * 6;

        if (temporary < 1)
        {
            multiplier[0] *= temporary;
            multiplier[1] *= 0;
            multiplier[2] *= 0;
        }
        else if (temporary < 2)
        {
            multiplier[0] *= 1;
            multiplier[1] *= temporary - 1;
            multiplier[2] *= 0;
        }
        else if (temporary < 3)
        {
            multiplier[0] *= 1 - (temporary - 2);
            multiplier[1] *= 1;
            multiplier[2] *= 0;
        }
        else if (temporary < 4)
        {
            multiplier[0] *= 0;
            multiplier[1] *= 1;
            multiplier[2] *= temporary - 3;
        }
        else if (temporary < 5)
        {
            multiplier[0] *= 0;
            multiplier[1] *= 1 - (temporary - 4);
            multiplier[2] *= 1;
        }
        else if (temporary < 6)
        {
            multiplier[0] *= 0;
            multiplier[1] *= 0;
            multiplier[2] *= 1 - (temporary - 5);
        }

        for (int i = 0; i < 3; i++)
        {
            multiplier[i] *= 3;
        }

        multiplier[1] *= 1.0 / 1.5;

        n1 = incidence_ior;
        n2 = refractive_index * (1 + (randomness / 6) * (refractive_index / 5));

        if (n1 == n2)
        {
            n2 = 1.0008;
        }

        input_vector = vector_class.vector_unitize (input_vector);

        cos_theta1 = vector_class.vector_dot_product (normal, vector_class.vector_negative (input_vector));
        cos_theta2 = System.Math.Sqrt (1 - System.Math.Pow (n1 / n2, 2) * (1 - System.Math.Pow (cos_theta1, 2)));

        output_vector = vector_class.vector_add (vector_class.vector_scale (input_vector, n1 / n2),
                                                 vector_class.vector_scale (normal, (n1 / n2 * cos_theta1) - cos_theta2));

        return output_vector;
    }
}
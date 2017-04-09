class vector_class
{
    public static bool vector_is_larger (double[] vector_1, double[] vector_2)
    {
        double length_1 = (System.Math.Sqrt (vector_1[0] * vector_1[0] +
                                             vector_1[1] * vector_1[1] +
                                             vector_1[2] * vector_1[2]));

        double length_2 = (System.Math.Sqrt (vector_2[0] * vector_2[0] +
                                             vector_2[1] * vector_2[1] +
                                             vector_2[2] * vector_2[2]));

        if (length_1 > length_2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static double[] vector_average (double[] input_position_1, double[] input_position_2, double weight)
    {
        double[] output_vector = new double[3];

        for (int i = 0; i < 3; i++)
        {
            output_vector[i] = input_position_1[i] + (input_position_2[i] - input_position_1[i]) * weight;
        }

        return output_vector;
    }

    public static double[] vector_from_2_positions (double[] input_position_1, double[] input_position_2)
    {
        double[] output_vector = new double[3];

        for (int i = 0; i < 3; i++)
        {
            output_vector[i] = input_position_2[i] - input_position_1[i];
        }

        return output_vector;
    }

    public static double[] vector_cross_product (double[] input_vector_1, double[] input_vector_2)
    {
        double[] output_vector = new double[3];

        output_vector[0] = (input_vector_1[1] * input_vector_2[2]) - (input_vector_1[2] * input_vector_2[1]);
        output_vector[1] = (input_vector_1[2] * input_vector_2[0]) - (input_vector_1[0] * input_vector_2[2]);
        output_vector[2] = (input_vector_1[0] * input_vector_2[1]) - (input_vector_1[1] * input_vector_2[0]);

        return output_vector;
    }

    public static double vector_dot_product (double[] input_vector_1, double[] input_vector_2)
    {
        double output = 0;

        output = input_vector_1[0] * input_vector_2[0] + 
                 input_vector_1[1] * input_vector_2[1] + 
                 input_vector_1[2] * input_vector_2[2];

        return output;
    }

    public static double[] vector_negative (double[] input_vector)
    {
        double[] output_vector = new double[3];

        output_vector[0] = -input_vector[0];
        output_vector[1] = -input_vector[1];
        output_vector[2] = -input_vector[2];

        return output_vector;
    }

    public static double[] vector_unitize (double[] input_vector)
    {
        double[] output_vector = new double[3];
        double input_vector_length = 0;

        input_vector_length = (System.Math.Sqrt (input_vector[0] * input_vector[0] +
                               input_vector[1] * input_vector[1] +
                               input_vector[2] * input_vector[2]));

        if (input_vector_length != 0)
        {
            output_vector[0] = (input_vector[0] * (1 / input_vector_length));
            output_vector[1] = (input_vector[1] * (1 / input_vector_length));
            output_vector[2] = (input_vector[2] * (1 / input_vector_length));
        }
        else
        {
            return input_vector;
        }

        return output_vector;
    }

    public static double[] vector_add (double[] input_vector_1, double[] input_vector_2)
    {
        double[] output_vector = new double[3];

        output_vector[0] = input_vector_1[0] + input_vector_2[0];
        output_vector[1] = input_vector_1[1] + input_vector_2[1];
        output_vector[2] = input_vector_1[2] + input_vector_2[2];

        return output_vector;
    }

    public static double[] vector_subtract (double[] input_vector_1, double[] input_vector_2)
    {
        double[] output_vector = new double[3];

        output_vector[0] = input_vector_1[0] - input_vector_2[0];
        output_vector[1] = input_vector_1[1] - input_vector_2[1];
        output_vector[2] = input_vector_1[2] - input_vector_2[2];

        return output_vector;
    }

    public static double[] vector_scale (double[] input_vector, double scalar)
    {
        double[] output_vector = new double[3];

        output_vector[0] = input_vector[0] * scalar;
        output_vector[1] = input_vector[1] * scalar;
        output_vector[2] = input_vector[2] * scalar;

        return output_vector;
    }
}
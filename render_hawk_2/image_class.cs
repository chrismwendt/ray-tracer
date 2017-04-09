class image_class
{
    public static double[] average (double[] input_image_1, double[] input_image_2, double weight)
    {
        double[] output_image = new double[input_image_1.GetLength (0)];

        if (input_image_1.GetLength (0) != input_image_2.GetLength (0))
        {
            return input_image_1;
        }

        for (int i = 0; i < output_image.GetLength (0); i++)
        {
            output_image[i] = (input_image_1[i] + ((input_image_2[i] - input_image_1[i]) * weight));
        }

        return output_image;
    }

    public static double[] linear_clamp (double[] input_image, double clamp)
    {
        double[] output_image = new double[input_image.GetLength (0)];

        for (int i = 0; i < output_image.GetLength (0); i++)
        {
            output_image[i] = input_image[i];

            if (output_image[i] > clamp)
            {
                output_image[i] = clamp;
            }
            else if (output_image[i] < 0)
            {
                output_image[i] = 0;
            }

            output_image[i] = ((output_image[i] / clamp) * 255);
        }

        return output_image;
    }

    public static double[] reverse_linear_clamp (double[] input_image, double clamp)
    {
        double[] output_image = new double[input_image.GetLength (0)];

        for (int i = 0; i < output_image.GetLength (0); i++)
        {
            output_image[i] = (input_image[i] / 255.0) * clamp;
        }

        return output_image;
    }

    public static byte[] convert_double_to_byte (double[] input_image)
    {
        byte[] output_image = new byte[input_image.GetLength (0)];

        for (int i = 0; i < output_image.GetLength (0); i++)
        {
            output_image[i] = (byte)input_image[i];
        }

        return output_image;
    }
}
class sphere_class
{
    public double[] center;
    public double radius;
    public int material_index;

    public sphere_class ()
    {
        center = new double[3];
        radius = 0;
        material_index = 0;
    }

    public double[] get_normal (double[] input_position)
    {
        double[] normal = new double[3];

        normal = vector_class.vector_from_2_positions (input_position, center);

        normal = vector_class.vector_unitize (normal);

        return normal;
    }
}
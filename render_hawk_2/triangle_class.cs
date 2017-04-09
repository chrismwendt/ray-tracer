class triangle_class
{
    public double[][] vertex;
    public double[] u, v, normal, edge_1, edge_2;
    public int material_index;

    public triangle_class ()
    {
        vertex = new double[3][];
        vertex[0] = new double[3];
        vertex[1] = new double[3];
        vertex[2] = new double[3];
        u = new double[3];
        v = new double[3];
        normal = new double[3];
        edge_1 = new double[3];
        edge_2 = new double[3];
        material_index = 0;
    }

    public void initialize ()
    {
        edge_1 = vector_class.vector_from_2_positions (vertex[1], vertex[0]);
        edge_2 = vector_class.vector_from_2_positions (vertex[2], vertex[0]);

        u = edge_1;
        normal = get_normal ();
        v = vector_class.vector_cross_product (u, normal);

        u = vector_class.vector_unitize (u);
        v = vector_class.vector_unitize (v);
    }

    public double[] get_normal ()
    {
        double[] output_vector = new double[3];

        output_vector = vector_class.vector_cross_product (edge_1, edge_2);
        output_vector = vector_class.vector_unitize (output_vector);

        return output_vector;
    }
}
class camera_class
{
    public double[] position;
    public double[] vector_look;
    public double[] vector_up;
    public double[] vector_u;
    public double[] vector_v;
    public double view_angle_width;
    public double view_angle_height;
    private System.Random random;

    private double[] output_vector;
    private double x, y, angle_x, angle_y, image_width_half, image_height_half, u_scalar, v_scalar, w_scalar, pi_180_half, cos_angle_y;

    public camera_class ()
    {
        position = new double[3];
        vector_look = new double[3];
        vector_up = new double[3];
        output_vector = new double[3];
        view_angle_width = 0;
        view_angle_height = 0;
        random = new System.Random ();

        pi_180_half = System.Math.PI / 180 * 0.5;
    }

    public double[] get_vector (scene_class scene, int x_pixel, int y_pixel)
    {
        x = (double)x_pixel + random.NextDouble ();
        y = (double)y_pixel + random.NextDouble ();

        angle_x = (((x - image_width_half)) / image_width_half) * (scene.camera.view_angle_width * pi_180_half);
        angle_y = (((image_height_half - y)) / image_height_half) * (scene.camera.view_angle_height * pi_180_half);

        cos_angle_y = System.Math.Cos (angle_y);

        v_scalar = System.Math.Sin (angle_y);
        u_scalar = System.Math.Sin (angle_x) * cos_angle_y;
        w_scalar = System.Math.Cos (angle_x) * cos_angle_y;

        output_vector = vector_class.vector_add (vector_class.vector_add (vector_class.vector_scale (vector_u, u_scalar),
                                                                          vector_class.vector_scale (vector_v, v_scalar)),
                                                 vector_class.vector_scale (vector_look, w_scalar));

        return output_vector;
    }

    public void initialize (int image_width, int image_height)
    {
        vector_u = vector_class.vector_cross_product (vector_look, vector_up);
        vector_v = vector_class.vector_cross_product (vector_u, vector_look);

        vector_look = vector_class.vector_unitize (vector_look);
        vector_u = vector_class.vector_unitize (vector_u);
        vector_v = vector_class.vector_unitize (vector_v);

        if (view_angle_height > 180)
        {
            view_angle_height = 180;
        }

        if (view_angle_width > 360)
        {
            view_angle_width = 360;
        }

        image_width_half = (double)image_width * 0.5;
        image_height_half = (double)image_height * 0.5;
    }
}
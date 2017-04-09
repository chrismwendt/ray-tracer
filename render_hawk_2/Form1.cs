using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace render_hawk
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.OpenFileDialog dialog_open;
        System.Drawing.Bitmap output_image;
        renderer_class renderer;
        System.DateTime time_start;
        System.TimeSpan time_render;

        public Form1 ()
        {
            InitializeComponent ();

            dialog_open = new System.Windows.Forms.OpenFileDialog ();
            dialog_open.InitialDirectory = System.Environment.CurrentDirectory;
            dialog_open.RestoreDirectory = false;

            output_image = new System.Drawing.Bitmap (1, 1);
            renderer = new renderer_class ();
            time_start = System.DateTime.Now;
            time_render = new System.TimeSpan ();
        }

        private void button_process_Click (object sender, EventArgs e)
        {
            if (!System.IO.File.Exists (text_box_input_path.Text))
            {
                System.Windows.Forms.MessageBox.Show ("input file does not exist.", "error");
                return;
            }
            else
            {
                button_process.Text = "processing";
                button_process.Enabled = false;
                button_pause.Text = "pause";
                button_pause.Enabled = true;

                time_start = DateTime.Now;

                background_worker.RunWorkerAsync ();

                this.Invalidate ();
            }
        }

        private void button_pause_Click (object sender, EventArgs e)
        {
            background_worker.CancelAsync ();
        }

        private void button_select_file_Click (object sender, EventArgs e)
        {
            if (dialog_open.ShowDialog (this) == DialogResult.OK)
            {
                text_box_input_path.Text = dialog_open.FileName;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (System.IO.File.Exists(renderer.scene.output_path))
            {
                if (System.IO.File.Exists(renderer.scene.output_path))
                {
                    lock (renderer.lock_object)
                    {
                        output_image = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(renderer.scene.output_path);
                    }

                    e.Graphics.DrawImage(output_image, new System.Drawing.Point(button_process.Location.X, button_process.Location.Y + 30));

                    output_image.Dispose();
                }
            }
        }

        private void background_worker_DoWork (object sender, DoWorkEventArgs e)
        {
            renderer.render_progressive (text_box_input_path.Text, background_worker);
        }

        private void background_worker_ProgressChanged (object sender, ProgressChangedEventArgs e)
        {
            label_sample.Text = "sample: " + (int)e.UserState;

            time_render = DateTime.Now - time_start;
            label_time.Text = time_render.TotalSeconds.ToString ();

            this.Invalidate ();
        }

        private void background_worker_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            time_render = DateTime.Now - time_start;
            label_time.Text = "time: " + time_render.TotalSeconds.ToString ();

            button_process.Enabled = true;
            button_process.Text = "process";
            button_pause.Enabled = false;
            button_pause.Text = "paused";

            this.Invalidate ();
        }
    }
}
namespace render_hawk
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.button_process = new System.Windows.Forms.Button ();
            this.text_box_input_path = new System.Windows.Forms.TextBox ();
            this.button_select_file = new System.Windows.Forms.Button ();
            this.label_time = new System.Windows.Forms.Label ();
            this.button_pause = new System.Windows.Forms.Button ();
            this.background_worker = new System.ComponentModel.BackgroundWorker ();
            this.label_sample = new System.Windows.Forms.Label ();
            this.SuspendLayout ();
            // 
            // button_process
            // 
            this.button_process.Location = new System.Drawing.Point (12, 38);
            this.button_process.Name = "button_process";
            this.button_process.Size = new System.Drawing.Size (120, 22);
            this.button_process.TabIndex = 0;
            this.button_process.Text = "process";
            this.button_process.UseVisualStyleBackColor = true;
            this.button_process.Click += new System.EventHandler (this.button_process_Click);
            // 
            // text_box_input_path
            // 
            this.text_box_input_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.text_box_input_path.Font = new System.Drawing.Font ("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_box_input_path.Location = new System.Drawing.Point (12, 12);
            this.text_box_input_path.Name = "text_box_input_path";
            this.text_box_input_path.Size = new System.Drawing.Size (325, 20);
            this.text_box_input_path.TabIndex = 1;
            this.text_box_input_path.Text = "E:\\programs\\render_hawk\\sphere_material.txt";
            // 
            // button_select_file
            // 
            this.button_select_file.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_select_file.Location = new System.Drawing.Point (343, 12);
            this.button_select_file.Name = "button_select_file";
            this.button_select_file.Size = new System.Drawing.Size (101, 20);
            this.button_select_file.TabIndex = 2;
            this.button_select_file.Text = "select file";
            this.button_select_file.UseVisualStyleBackColor = true;
            this.button_select_file.Click += new System.EventHandler (this.button_select_file_Click);
            // 
            // label_time
            // 
            this.label_time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_time.AutoSize = true;
            this.label_time.Location = new System.Drawing.Point (343, 42);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size (49, 14);
            this.label_time.TabIndex = 3;
            this.label_time.Text = "time: ";
            // 
            // button_pause
            // 
            this.button_pause.Enabled = false;
            this.button_pause.Location = new System.Drawing.Point (138, 38);
            this.button_pause.Name = "button_pause";
            this.button_pause.Size = new System.Drawing.Size (63, 22);
            this.button_pause.TabIndex = 4;
            this.button_pause.Text = "paused";
            this.button_pause.UseVisualStyleBackColor = true;
            this.button_pause.Click += new System.EventHandler (this.button_pause_Click);
            // 
            // background_worker
            // 
            this.background_worker.WorkerReportsProgress = true;
            this.background_worker.WorkerSupportsCancellation = true;
            this.background_worker.DoWork += new System.ComponentModel.DoWorkEventHandler (this.background_worker_DoWork);
            this.background_worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler (this.background_worker_RunWorkerCompleted);
            this.background_worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler (this.background_worker_ProgressChanged);
            // 
            // label_sample
            // 
            this.label_sample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_sample.AutoSize = true;
            this.label_sample.Location = new System.Drawing.Point (207, 42);
            this.label_sample.Name = "label_sample";
            this.label_sample.Size = new System.Drawing.Size (70, 14);
            this.label_sample.TabIndex = 6;
            this.label_sample.Text = "sample: 0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size (456, 436);
            this.Controls.Add (this.label_sample);
            this.Controls.Add (this.button_pause);
            this.Controls.Add (this.label_time);
            this.Controls.Add (this.button_select_file);
            this.Controls.Add (this.text_box_input_path);
            this.Controls.Add (this.button_process);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font ("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "Render Hawk";
            this.Paint += new System.Windows.Forms.PaintEventHandler (this.Form1_Paint);
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.Button button_process;
        private System.Windows.Forms.TextBox text_box_input_path;
        private System.Windows.Forms.Button button_select_file;
        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.Button button_pause;
        private System.ComponentModel.BackgroundWorker background_worker;
        private System.Windows.Forms.Label label_sample;
    }
}


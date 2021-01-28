using System;
using System.Drawing;
using System.Windows.Forms;
using DxLibDLL;

namespace DesktopMascot
{
    class CameraDialog : Form
    {
        Label label_dist;
        Label label_theta;
        Label label_phi;
        TextBox textBox_dist;
        TextBox textBox_theta;
        TextBox textBox_phi;
        TrackBar trackbar_dist;
        TrackBar trackbar_theta;
        TrackBar trackbar_phi;
        Button close_button;

        int val_dist = (int)Form1.camera_dist;
        int val_theta = (int)Form1.camera_theta;
        int val_phi = (int)Form1.camera_phi;

        public CameraDialog()
        {
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.Manual;
            Size = new Size(300, 400);
            Location = new Point(200, 200);
            Text = "Set the Camera Position";

            label_dist = new Label()
            {
                Text = "Distance: ",
                Location = new Point(10, 10),
            };
            textBox_dist = new TextBox()
            {
                Text = val_dist.ToString(),
                Location = new Point(150, 10),
            };
            trackbar_dist = new TrackBar()
            {
                Location = new Point(20, 30),
                Minimum = 5,
                Maximum = 1000,
                Value = val_dist,
                TickFrequency = 100,
                SmallChange = 1,
                LargeChange = 5,
                AutoSize = false,
                Size = new Size(240, 40),
            };
            trackbar_dist.ValueChanged += Trackbar_Dist_ValueChanged;

            label_theta = new Label()
            {
                Text = "Theta Angle: ",
                Location = new Point(10, 100),
            };
            textBox_theta = new TextBox()
            {
                Text = val_theta.ToString(),
                Location = new Point(150, 100),
            };
            trackbar_theta = new TrackBar()
            {
                Location = new Point(20, 120),
                Minimum = -90,
                Maximum = 90,
                Value = val_theta,
                TickFrequency = 30,
                SmallChange = 1,
                LargeChange = 10,
                AutoSize = false,
                Size = new Size(240, 40),
            };
            trackbar_theta.ValueChanged += Trackbar_Theta_ValueChanged;

            label_phi = new Label()
            {
                Text = "Phi Angle: ",
                Location = new Point(10, 200),
            };
            textBox_phi = new TextBox()
            {
                Text = val_phi.ToString(),
                Location = new Point(150, 200),
            };
            trackbar_phi = new TrackBar()
            {
                Location = new Point(20, 220),
                Minimum = -180,
                Maximum = 180,
                Value = val_phi,
                TickFrequency = 60,
                SmallChange = 1,
                LargeChange = 10,
                AutoSize = false,
                Size = new Size(240, 40),
            };
            trackbar_phi.ValueChanged += Trackbar_Phi_ValueChanged;

            close_button = new Button()
            {
                Text = "Close",
                Location = new Point(100, 300),
            };
            close_button.Click += new EventHandler(Close_Button_CLicked);

            this.Controls.AddRange(new Control[] {
                label_dist, label_theta, label_phi, textBox_dist, trackbar_dist,
                textBox_theta, trackbar_theta, textBox_phi, trackbar_phi, close_button
            });
        }

        private void Close_Button_CLicked(object sender, EventArgs e)
        {
            Close();
        }

        private void Trackbar_Dist_ValueChanged(object s, EventArgs e)
        {
            textBox_dist.Text = trackbar_dist.Value.ToString();
            Form1.camera_dist = (float)int.Parse(textBox_dist.Text);
            DX.SetCameraPositionAndTarget_UpVecY(SphereToCube(Form1.camera_dist, Form1.camera_theta, Form1.camera_phi), DX.VGet(0.0f, 10.0f, 0.0f));
        }

        private void Trackbar_Theta_ValueChanged(object s, EventArgs e)
        {
            textBox_theta.Text = trackbar_theta.Value.ToString();
            Form1.camera_theta = (float)int.Parse(textBox_theta.Text);
            DX.SetCameraPositionAndTarget_UpVecY(SphereToCube(Form1.camera_dist, Form1.camera_theta, Form1.camera_phi), DX.VGet(0.0f, 10.0f, 0.0f));
        }

        private void Trackbar_Phi_ValueChanged(object s, EventArgs e)
        {
            textBox_phi.Text = trackbar_phi.Value.ToString();
            Form1.camera_phi = (float)int.Parse(textBox_phi.Text);
            DX.SetCameraPositionAndTarget_UpVecY(SphereToCube(Form1.camera_dist, Form1.camera_theta, Form1.camera_phi), DX.VGet(0.0f, 10.0f, 0.0f));
        }

        public static DX.VECTOR SphereToCube(float dist, float theta, float phi)
        {
            if (theta > 86) theta = 89.0f;
            if (theta < -86) theta = -89.0f;
            float x = dist * (float)Math.Cos(DegToRad(theta)) * (float)Math.Cos(DegToRad(phi - 90));
            float y = dist * (float)Math.Sin(DegToRad(theta)) + 10;
            float z = dist * (float)Math.Cos(DegToRad(theta)) * (float)Math.Sin(DegToRad(phi - 90));
            return DX.VGet(x, y, z);
        }

        private static float DegToRad(float deg)
        {
            return (float)Math.PI * deg / 180;
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopMascot
{
    class PhysicDialog : Form
    {
        private Label label;
        private Label label_Sel;
        private Button button_A;
        private Button button_B;
        private Button button_C;
        private Button close_button;
        private string temp_text;

        public PhysicDialog()
        {
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.Manual;
            Size = new Size(300, 300);
            Location = new Point(200, 200);
            Text = "Physics calc. mode";

            temp_text = "Selected: No Calc.";
            if (Form1.phy_mode == -1)
            {
                temp_text = "Selected: Realtime Calc.";
            }
            if (Form1.phy_mode == 1)
            {
                temp_text = "Selected: Calc. at loading";
            }

            label = new Label()
            {
                Location = new Point(20, 20),
                Text = "Select the physics calc. mode",
                Size = new Size(200, 24),
            };

            button_A = new Button()
            {
                Text = "No calc.",
                Location = new Point(10, 80),
                Size = new Size(80, 25),
            };
            button_A.Click += new EventHandler(A_button_CLicked);

            button_B = new Button()
            {
                Text = "Realtime",
                Location = new Point(100, 80),
                Size = new Size(80, 25),
            };
            button_B.Click += new EventHandler(B_button_CLicked);

            button_C = new Button()
            {
                Text = "At loading",
                Location = new Point(190, 80),
                Size = new Size(80, 25),
            };
            button_C.Click += new EventHandler(C_button_CLicked);

            label_Sel = new Label()
            {
                Text = temp_text,
                Location = new Point(20, 120),
                Size = new Size(200, 25),
            };

            close_button = new Button()
            {
                Text = "Close",
                Location = new Point(150, 200),
            };
            close_button.Click += new EventHandler(Close_button_CLicked);

            Controls.AddRange(new Control[]
            {
                label, label_Sel, button_A, button_B, button_C, close_button
            });
        }

        private void A_button_CLicked(object sender, EventArgs e)
        {
            Form1.SetPhyMode(0);
            label_Sel.Text = "Selected: No Calc.";
        }

        private void B_button_CLicked(object sender, EventArgs e)
        {
            Form1.SetPhyMode(-1);
            label_Sel.Text = "Selected: Realtime";
        }

        private void C_button_CLicked(object sender, EventArgs e)
        {
            Form1.SetPhyMode(1);
            label_Sel.Text = "Selected: At loading";
        }

        private void Close_button_CLicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}

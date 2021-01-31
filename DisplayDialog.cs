using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopMascot
{
    sealed class DisplayDialog : Form
    {
        private Label label;
        private Label label_sel;
        private Button button_n;
        private Button button_t;
        private Button button_close;
        private string temp_text;

        public DisplayDialog()
        {
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.Manual;
            Size = new Size(300, 300);
            Location = new Point(200, 200);
            Text = "Display Mode";

            if (Form1.dsp_flag)
            {
                temp_text = "Selected display mode: 最前面";
            }
            else
            {
                temp_text = "Selected display mode: 標準";
            }

            label = new Label()
            {
                Location = new Point(20, 20),
                Text = "Select the display mode",
                Size = new Size(200, 24),
            };

            button_n = new Button()
            {
                Text = "標準",
                Location = new Point(50, 80),
                AutoSize = true,
            };
            button_n.Click += new EventHandler(N_Button_CLicked);

            button_t = new Button()
            {
                Text = "常に最前面",
                Location = new Point(150, 80),
                AutoSize = true,
            };
            button_t.Click += new EventHandler(T_Button_CLicked);

            label_sel = new Label()
            {
                Text = temp_text,
                Location = new Point(50, 120),
                Size = new Size(200, 25),
            };

            button_close = new Button()
            {
                Text = "Close",
                Location = new Point(150, 200),
            };
            button_close.Click += new EventHandler(Button_Close_CLicked);

            this.Controls.AddRange(new Control[]
            {
                label, label_sel, button_n, button_t, button_close
            });
        }

        void N_Button_CLicked(object sender, EventArgs e)
        {
            Form1.dsp_flag = false;
            Form1.dsp_change_flag = true;
            label_sel.Text = "Selected display mode: Normal";
        }

        void T_Button_CLicked(object sender, EventArgs e)
        {
            Form1.dsp_flag = true;
            Form1.dsp_change_flag = true;
            label_sel.Text = "Selected display mode: TopMost";
        }

        void Button_Close_CLicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}

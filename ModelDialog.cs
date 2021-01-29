using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopMascot
{
    sealed class ModelDialog : Form
    {
        private Label label;
        private Label label_file;
        private Button button_open;
        private Button button_close;

        public ModelDialog()
        {
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.Manual;
            Size = new Size(300, 300);
            Location = new Point(200, 200);
            Text = "モデル";

            label = new Label()
            {
                Location = new Point(20, 20),
                Text = "Select your model.",
                Size = new Size(200, 24),
            };
            label_file = new Label()
            {
                Location = new Point(20, 100),
                Text = "",
                Size = new Size(200, 24),
            };
            button_open = new Button()
            {
                Text = "Open",
                Location = new Point(50, 200),
            };
            button_open.Click += new EventHandler(Open_Button_CLicked);
            button_close = new Button()
            {
                Text = "Close",
                Location = new Point(150, 200),
            };
            button_close.Click += new EventHandler(Close_Button_CLicked);
            Controls.AddRange(new Control[]
            {
                label, label_file, button_open, button_close
            });
        }

        private void Open_Button_CLicked(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            // デフォルトのフォルダの指定
            ofDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            // ダイアログのタイトル
            ofDialog.Title = "モデルの選択";
            ofDialog.Filter = "MMD Model File(*.pmd;*.pmx)|*.pmd;*.pmx";

            // ダイアログの表示
            if (ofDialog.ShowDialog() == DialogResult.OK)
            {
                Form1.ChangeModel(ofDialog.FileName);
                label_file.Text = ofDialog.FileName;
            }
            ofDialog.Dispose();
        }

        private void Close_Button_CLicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}

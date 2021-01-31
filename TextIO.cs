using System.IO;
using System.Windows.Forms;

namespace DesktopMascot
{
    sealed class TextIO
    {
        public string GetModelPath()
        {
            string path = "";
            try
            {
                using(StreamReader sr = new StreamReader("./config/model.txt"))
                {
                    path = sr.ReadLine();
                }
            }
            catch(IOException e)
            {
                MessageBox.Show("ファイルの読み込みに失敗しました。\nタスクトレイからモデル・モーションを選択してください。", "読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return path;
        }

        public void SetModelPath(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("./config/model.txt"))
                {
                    sw.WriteLine(path);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("ファイルの書き込みに失敗しました。", "ファイル書き込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

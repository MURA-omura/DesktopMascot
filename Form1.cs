using System;
using System.Drawing;
using System.Windows.Forms;
using DxLibDLL;

namespace DesktopMascot
{
    public partial class Form1 : Form
    {
        private int modelHandle;

        public float camera_X = 0.0f;
        public float camera_Y = 10.0f;
        public float camera_Z = -30.0f;

        private bool mouse_flag = false;
        private int mouse_orgX;
        private int mouse_orgY;
        private float obj_orgX;
        private float obj_orgY;
        private float obj_orgZ;

        public Form1()
        {
            InitializeComponent(); //フォームの初期設定

            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); //画面サイズの設定
            Text = "DesktopMascot"; //ウインドウの名前を設定

            DX.SetOutApplicationLogValidFlag(DX.FALSE); //Log.txtを生成しないように設定
            DX.SetUserWindow(Handle); //DxLibの親ウインドウをこのフォームに設定
            DX.SetZBufferBitDepth(24); //Zバッファの深度を24bitに変更
            DX.SetCreateDrawValidGraphZBufferBitDepth(24); //裏画面のZバッファの深度を24bitに変更
            DX.SetFullSceneAntiAliasingMode(4, 2); //画面のフルスクリーンアンチエイリアスモードの設定をする
            DX.DxLib_Init(); //DxLibの初期化処理
            DX.SetDrawScreen(DX.DX_SCREEN_BACK); //描画先を裏画面に設定

            modelHandle = DX.MV1LoadModel("Data/さくらみこ2公式mmd_ver1.0/さくらみこ2.pmx"); //3Dモデルの読み込み

            DX.SetCameraNearFar(0.1f, 1000.0f); //奥行0.1～1000をカメラの描画範囲とする
            DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(camera_X, camera_Y, camera_Z), DX.VGet(0.0f, 10.0f, 0.0f)); //第1引数の位置から第2引数の位置を見る角度にカメラを設置
        }

        public void MainLoop()
        {
            DX.ClearDrawScreen(); //裏画面を消す
            DX.DrawBox(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, DX.GetColor(1, 1, 1), DX.TRUE); //背景を設定(透過させる)

            MoveModel();

            DX.MV1DrawModel(modelHandle); //3Dモデルの描画

             //ESCキーを押したら終了
            if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) != 0)
            {
                DX.DxLib_End();
                Close();
            }

            DX.ScreenFlip(); //裏画面を表画面にコピー
        }

        private void MoveModel()
        {

            if ((DX.GetMouseInput() & DX.MOUSE_INPUT_LEFT) != 0)
            {
                int mouse_X, mouse_Y;
                int obj_X, obj_Y;
                int dist_X, dist_Y;

                float distance = (float)Math.Sqrt(camera_X * camera_X + camera_Y * camera_Y + camera_Z * camera_Z);
                int temp = DX.GetMousePoint(out mouse_X, out mouse_Y);
                DX.VECTOR pos = DX.ConvWorldPosToScreenPos(DX.MV1GetPosition(modelHandle));
                obj_X = (int)pos.x;
                obj_Y = (int)pos.y;

                if ((Math.Abs(mouse_X - obj_X) < (50 * 30 / distance)) && ((mouse_Y - obj_Y) < (-100 * 30 / distance) && (mouse_Y - obj_Y) > (-700 * 30 / distance)))
                {
                    if (!mouse_flag)
                    {
                        DX.VECTOR ObjPos = DX.MV1GetPosition(modelHandle);
                        obj_orgX = ObjPos.x;
                        obj_orgY = ObjPos.y;
                        obj_orgZ = ObjPos.z;
                        mouse_orgX = mouse_X;
                        mouse_orgY = mouse_Y;
                        mouse_flag = true;
                    }
                    dist_X = mouse_X - mouse_orgX;
                    dist_Y = mouse_Y - mouse_orgY;
                    DX.VECTOR Move_pos = DX.VGet(obj_orgX + (float)dist_X * (distance + 0.1f) / 1000.0f, obj_orgY - (float)dist_Y * (distance + 0.1f) / 1000.0f, obj_orgZ);
                    DX.MV1SetPosition(modelHandle, Move_pos);
                }
            }
            else
            {
                mouse_flag = false;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DX.DxLib_End(); //DxLibの終了処理
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None; //フォームの枠を非表示にする
            TransparencyKey = Color.FromArgb(1, 1, 1); //透過色を設定
        }
    }
}

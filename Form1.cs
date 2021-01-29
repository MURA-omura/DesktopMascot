using System;
using System.Drawing;
using System.Windows.Forms;
using DxLibDLL;

namespace DesktopMascot
{
    sealed public partial class Form1 : Form
    {
        private NotifyIcon notify_icon;

        private static int modelHandle;
        private static string model_path;

        public static float camera_dist = 50.0f;
        public static float camera_theta = 0.0f;
        public static float camera_phi = 0.0f;

        public static int phy_mode = 0;

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
            ShowInTaskbar = false;
            SetComponents();

            DX.SetOutApplicationLogValidFlag(DX.FALSE); //Log.txtを生成しないように設定
            DX.SetUserWindow(Handle); //DxLibの親ウインドウをこのフォームに設定
            DX.SetZBufferBitDepth(24); //Zバッファの深度を24bitに変更
            DX.SetCreateDrawValidGraphZBufferBitDepth(24); //裏画面のZバッファの深度を24bitに変更
            DX.SetFullSceneAntiAliasingMode(4, 2); //画面のフルスクリーンアンチエイリアスモードの設定をする
            DX.DxLib_Init(); //DxLibの初期化処理
            DX.SetDrawScreen(DX.DX_SCREEN_BACK); //描画先を裏画面に設定

            model_path = "Data/さくらみこ2公式mmd_ver1.0/さくらみこ2.pmx";
            modelHandle = DX.MV1LoadModel(model_path); //3Dモデルの読み込み

            DX.SetCameraNearFar(0.1f, 1000.0f); //奥行0.1～1000をカメラの描画範囲とする
            DX.SetCameraPositionAndTarget_UpVecY(CameraDialog.SphereToCube(camera_dist, camera_theta, camera_phi), DX.VGet(0.0f, 10.0f, 0.0f)); //第1引数の位置から第2引数の位置を見る角度にカメラを設置
        }

        public static void SetPhyMode(int phy_flag)
        {
            phy_mode = phy_flag;
            if (phy_flag == -1)
            {
                DX.MV1PhysicsResetState(modelHandle);
                DX.MV1SetLoadModelPhysicsCalcPrecision(0);      // Calculation accuracy  0: Default(60FPS), 1: 120FPS, 2: 240FPS, 3: 480FPS, 4: 960FPS, 5: 1920FPS  
                DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_REALTIME);
            }
            if (phy_flag == 1)
            {
                DX.MV1PhysicsResetState(modelHandle);
                DX.MV1SetLoadModelPhysicsCalcPrecision(0);
                DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_LOADCALC);
            }
            if (phy_flag == 0)
            {
                DX.MV1PhysicsResetState(modelHandle);
                DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_DISABLE);
            }
            modelHandle = DX.MV1LoadModel(model_path);
            //MaxMotions = DX.MV1GetAnimNum(modelHandle);
            //attachIndex = DX.MV1AttachAnim(modelHandle, MotionNo, -1, DX.FALSE);
            //totalTime = DX.MV1GetAttachAnimTotalTime(modelHandle, attachIndex);
        }

        private void SetComponents()
        {
            notify_icon = new NotifyIcon();
            // アイコンの設定
            notify_icon.Icon = new Icon("./DesktopMascot.ico");
            // アイコンを表示する
            notify_icon.Visible = true;
            // アイコンにマウスポインタを合わせたときのテキスト
            notify_icon.Text = "DesktopMascot";

            // コンテキストメニュー
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            
            ToolStripMenuItem cameraToolStripMenuItem = new ToolStripMenuItem();
            cameraToolStripMenuItem.Text = "カメラ位置";
            cameraToolStripMenuItem.Click += CameraToolStripMenuItem_Click;
            contextMenuStrip.Items.Add(cameraToolStripMenuItem);

            ToolStripMenuItem modelToolStripMenuItem = new ToolStripMenuItem();
            modelToolStripMenuItem.Text = "モデル";
            modelToolStripMenuItem.Click += ModelToolStripMenuItem_Click;
            contextMenuStrip.Items.Add(modelToolStripMenuItem);

            ToolStripMenuItem motionToolStripMenuItem = new ToolStripMenuItem();
            motionToolStripMenuItem.Text = "モーション";
            motionToolStripMenuItem.Click += MotionToolStripMenuItem_Click;
            contextMenuStrip.Items.Add(motionToolStripMenuItem);

            ToolStripMenuItem physicToolStripMenuItem = new ToolStripMenuItem();
            physicToolStripMenuItem.Text = "物理演算";
            physicToolStripMenuItem.Click += PhysicToolStripMenuItem_Click;
            contextMenuStrip.Items.Add(physicToolStripMenuItem);

            ToolStripMenuItem exitToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem.Text = "終了";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            contextMenuStrip.Items.Add(exitToolStripMenuItem);

            notify_icon.ContextMenuStrip = contextMenuStrip;

            // NotifyIconのクリックイベント
            notify_icon.Click += NotifyIconMouseClick;
        }

        public void MainLoop()
        {
            DX.ClearDrawScreen(); //裏画面を消す
            DX.DrawBox(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, DX.GetColor(1, 1, 1), DX.TRUE); //背景を設定(透過させる)

            MoveModel();
            DX.MV1DrawModel(modelHandle); //3Dモデルの描画

            DX.ScreenFlip(); //裏画面を表画面にコピー
        }

        private void MoveModel()
        {

            if ((DX.GetMouseInput() & DX.MOUSE_INPUT_LEFT) != 0)
            {
                int mouse_X, mouse_Y;
                int obj_X, obj_Y;
                int dist_X, dist_Y;

                int temp = DX.GetMousePoint(out mouse_X, out mouse_Y);
                DX.VECTOR pos = DX.ConvWorldPosToScreenPos(DX.MV1GetPosition(modelHandle));
                obj_X = (int)pos.x;
                obj_Y = (int)pos.y;

                if ((Math.Abs(mouse_X - obj_X) < (50 * 30 / camera_dist)) && ((mouse_Y - obj_Y) < (-100 * 30 / camera_dist) && (mouse_Y - obj_Y) > (-700 * 30 / camera_dist)))
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
                    DX.VECTOR Move_pos = DX.VGet(obj_orgX + (float)dist_X * (camera_dist + 0.1f) / 1000.0f, obj_orgY - (float)dist_Y * (camera_dist + 0.1f) / 1000.0f, obj_orgZ);
                    DX.MV1SetPosition(modelHandle, Move_pos);
                }
            }
            else
            {
                mouse_flag = false;
            }
        }

        public static void ChangeModel(string model_path)
        {
            modelHandle = DX.MV1LoadModel(model_path);
        }

        public static void ChangeMotion(string motion_path)
        {
            //
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DX.DxLib_End(); //DxLibの終了処理
            Application.Exit();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None; //フォームの枠を非表示にする
            TransparencyKey = Color.FromArgb(1, 1, 1); //透過色を設定
        }

        private void NotifyIconMouseClick(object sender, EventArgs e)
        {
            notify_icon.ContextMenuStrip.Show(Cursor.Position.X - 10, Cursor.Position.Y - 10);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DX.DxLib_End();
            Application.Exit();
        }

        private void CameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraDialog cdialog = new CameraDialog();
            cdialog.Show();             // Show the dialog window for setting camera works
        }

        private void ModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelDialog mddialog = new ModelDialog();
            mddialog.Show();             // Show the dialog window to select the motion
        }

        private void MotionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MotionDialog mtdialog = new MotionDialog();
            mtdialog.Show();             // Show the dialog window to selkect the model
        }

        private void PhysicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PhysicDialog pdialog = new PhysicDialog();
            pdialog.Show();             // Show dialog window to set the physics calc. mode
        }

        /*
        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_DisplayMode dialog1 = new Dialog_DisplayMode();
            dialog1.Show();             // Show dialog window to set the top most display
        }

        private void settingWindowPosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_WindowPos dialog1 = new Dialog_WindowPos();
            DialogResult result = dialog1.ShowDialog();         // Show dialog window to set the position for the dialog window
            if (result == DialogResult.OK)
            {
                WinPos_X = int.Parse(dialog1.textBox_X.Text);
                WinPos_Y = int.Parse(dialog1.textBox_Y.Text);
            }
        }

        private void scheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_Schedule dialog1 = new Dialog_Schedule();
            dialog1.Show();             // Show dialog window to set the motion speed
        }
        */
    }
}

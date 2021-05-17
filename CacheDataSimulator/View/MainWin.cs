using CacheDataSimulator.Controller;
using CacheDataSimulator.Data;
using CacheDataSimulator.Validation;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CacheDataSimulator
{
    public partial class MainWin : Form
    {
        private static bool IsMRU;
        private static bool IsLRU;
        private MainController MainCTRL;
        private bool IsAssembled = false;

        public MainWin()
        {
            //InitializeComponent();
            InitializeComponent();
            MainCTRL = new MainController();
            Init();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        private void ExitBtn_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void ExitBtn_MouseLeave(object sender, System.EventArgs e)
        {
            ExitBtn.BackColor = ColorTranslator.FromHtml("#323233");
        }

        private void MaxBtn_Click(object sender, System.EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                var workingArea = Screen.FromHandle(Handle).WorkingArea;
                MaximizedBounds = new Rectangle(0, 0, workingArea.Width, workingArea.Height);
                WindowState = FormWindowState.Maximized;
            }
            else
                WindowState = FormWindowState.Normal;
        }

        private void MinBtn_Click(object sender, System.EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void WinTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void BuildBtn_MouseHover(object sender, System.EventArgs e)
        {
            BuildBtn.BackColor = ColorTranslator.FromHtml("#8EBC00");
            BuildBtn.Image = Properties.Resources.b_build;
        }

        private void BuildBtn_MouseLeave(object sender, System.EventArgs e)
        {
            BuildBtn.BackColor = ColorTranslator.FromHtml("#2D2D30");
            BuildBtn.Image = Properties.Resources.build;
        }

        private void FullExecBtn_MouseHover(object sender, System.EventArgs e)
        {
            FullExecBtn.BackColor = ColorTranslator.FromHtml("#8EBC00");
            FullExecBtn.Image = Properties.Resources.b_full_exec;
        }

        private void FullExecBtn_MouseLeave(object sender, System.EventArgs e)
        {
            FullExecBtn.BackColor = ColorTranslator.FromHtml("#2D2D30");
            FullExecBtn.Image = Properties.Resources.full_exec;
        }

        private void SingleStepBtn_MouseHover(object sender, System.EventArgs e)
        {
            SingleStepBtn.BackColor = ColorTranslator.FromHtml("#8EBC00");
            SingleStepBtn.Image = Properties.Resources.b_sstep;
        }

        private void SingleStepBtn_MouseLeave(object sender, System.EventArgs e)
        {
            SingleStepBtn.BackColor = ColorTranslator.FromHtml("#2D2D30");
            SingleStepBtn.Image = Properties.Resources.sstep;
        }

        private void SaveBtn_MouseHover(object sender, System.EventArgs e)
        {
            SaveBtn.BackColor = ColorTranslator.FromHtml("#8EBC00");
            SaveBtn.Image = Properties.Resources.b_save;
        }

        private void SaveBtn_MouseLeave(object sender, System.EventArgs e)
        {
            SaveBtn.BackColor = ColorTranslator.FromHtml("#2D2D30");
            SaveBtn.Image = Properties.Resources.save;
        }

        private void ExitBtn_MouseHover(object sender, System.EventArgs e)
        {
            ExitBtn.BackColor = Color.Red;
        }

        private void ExitBtn_ClientSizeChanged(object sender, System.EventArgs e)
        {
            ExitBtn.BackColor = ColorTranslator.FromHtml("#323233");
        }

        private void RegisterSGTabBtn_Click(object sender, System.EventArgs e)
        {
            RegisterTab.SetTemplateDT(MainCTRL.GenerateRegisterSGDT());
            RegisterTab.Show();
            DataTab.Hide();
            TextTab.Hide();
            RegisterSGTabBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            RegisterSGTabBtn.FlatAppearance.BorderSize = 0;
            RegisterSGTabBtn.NotifyDefault(false);
            RegisterTabPanel.BackColor = ColorTranslator.FromHtml("#94BC00");
            DataTabPanel.BackColor = ColorTranslator.FromHtml("#1A1A1A");
            TextTabPanel.BackColor = ColorTranslator.FromHtml("#1A1A1A");
        }

        private void TextSGTabBtn_Click(object sender, System.EventArgs e)
        {
            RegisterTab.Hide();
            DataTab.Hide();
            TextTab.Show();
            RegisterTabPanel.BackColor = ColorTranslator.FromHtml("#1A1A1A");
            DataTabPanel.BackColor = ColorTranslator.FromHtml("#1A1A1A");
            TextTabPanel.BackColor = ColorTranslator.FromHtml("#94BC00");
        }

        private void DataSGTabBtn_Click(object sender, System.EventArgs e)
        {
            RegisterTab.Hide();
            DataTab.Show();
            TextTab.Hide();
            RegisterTabPanel.BackColor = ColorTranslator.FromHtml("#1A1A1A");
            DataTabPanel.BackColor = ColorTranslator.FromHtml("#94BC00");
            TextTabPanel.BackColor = ColorTranslator.FromHtml("#1A1A1A");
        }

        private void BlockSizeTxt_Enter(object sender, System.EventArgs e)
        {
            if (BlockSizeTxt.Text == "Enter Block Size")
            {
                BlockSizeTxt.Text = "";
            }
        }

        private void BlockSizeTxt_Leave(object sender, System.EventArgs e)
        {
            if (BlockSizeTxt.Text == "")
            {
                BlockSizeTxt.Text = "Enter Block Size";
                BlockSizeTxt.ForeColor = Color.LightGray;
            }
        }

        private void CacheSizeTxt_Enter(object sender, System.EventArgs e)
        {
            if (CacheSizeTxt.Text == "Enter Cache Size")
            {
                CacheSizeTxt.Text = "";
            }
        }

        private void CacheSizeTxt_Leave(object sender, System.EventArgs e)
        {
            if (CacheSizeTxt.Text == "")
            {
                CacheSizeTxt.Text = "Enter Cache Size";
                CacheSizeTxt.ForeColor = Color.Silver;
            }
        }

        private void LruBtn_Click(object sender, System.EventArgs e)
        {
            LruBtn.BackColor = ColorTranslator.FromHtml("#8EBC00");
            MruBtn.BackColor = ColorTranslator.FromHtml("#58585A");
            IsLRU = true;
            IsMRU = false;
        }

        private void MruBtn_Click(object sender, System.EventArgs e)
        {
            LruBtn.BackColor = ColorTranslator.FromHtml("#58585A");
            MruBtn.BackColor = ColorTranslator.FromHtml("#8EBC00");
            IsLRU = false;
            IsMRU = true;
        }

        private void Init()
        {
            MainCTRL.GenerateRegister();
            RegisterTab.SetTemplateDT(MainCTRL.GenerateRegisterSGDT());
            IsLRU = true;
            StaticData.sysDataLst = FileController.ReadSystemData();
        }

        private void OpenFileBtn_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog opDialogBox = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse RISC-V Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "asm",
                Filter = "asm files (*.asm)|*.asm",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (opDialogBox.ShowDialog() == DialogResult.OK)
            {
                filePathTxt.Text = opDialogBox.FileName;
                filePathTxt.Focus();
                filePathTxt.SelectionStart = filePathTxt.Text.Length;

                CodeEditorTxt.Text = string.Join("\r\n", FileController.ReadFile(filePathTxt.Text, false));
                CodeEditorCtrl.SetCodeEditorRTB(CodeEditorTxt.Text);
            }
            else
            {
                filePathTxt.Text = "Select *.asm file.";
            }
        }

        private void BuildBtn_Click(object sender, System.EventArgs e)
        {
            try
            {
                //string err = MainCTRL.BuildSourceCode(BlockSizeTxt.Text, CacheSizeTxt.Text, 
                //    CodeEditorTxt.Text, IsMRU, IsLRU);
                string err = MainCTRL.BuildSourceCode(BlockSizeTxt.Text, CacheSizeTxt.Text,
                    CodeEditorCtrl.GetCodeEditorRTB(), IsMRU, IsLRU);
                if (string.IsNullOrEmpty(err))
                {
                    UpdateErrorLog(ValidateInput.NoErr());
                    DataTab.SetTemplateDT(MainCTRL.GenerateDataSGDT());
                    TextTab.SetTemplateDT(MainCTRL.GenerateTextSGDT());
                    TextTab.SetColumnWidth();
                    IsAssembled = true;
                }
                else
                {
                    UpdateErrorLog(err);
                    IsAssembled = false;
                }
            }
            catch (Exception ex)
            {
                IsAssembled = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateErrorLog(string errorMsg)
        {
            ErrorLogTxt.Text = ErrorLogTxt.Text
                + errorMsg + "\r\n";
            ErrorLogTxt.SelectionStart = ErrorLogTxt.Text.Length;
            ErrorLogTxt.ScrollToCaret();
        }

        private void SingleStepBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsAssembled)
                {
                    foreach (var code in MainCTRL.txSG)
                    {
                        MainCTRL.rxSG = OperationController.ExecuteOperation(code, MainCTRL.dxSG, MainCTRL.rxSG);
                        RegisterTab.SetTemplateDT(MainCTRL.GenerateRegisterSGDT());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

using CacheDataSimulator.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CacheDataSimulator.View
{
    public enum ScrollBarType : uint
    {
        SbHorz = 0,
        SbVert = 1,
        SbCtl = 2,
        SbBoth = 3
    }

    public enum Message : uint
    {
        WM_VSCROLL = 0x0115
    }

    public enum ScrollBarCommands : uint
    {
        SB_THUMBPOSITION = 4
    }

    public partial class CodeEditor : UserControl
    {
        public CodeEditor()
        {
            InitializeComponent();
        }

        public void SetCodeEditorRTB(string code)
        {
            CodeEditRTB.Text = code;
        }
        public  string GetCodeEditorRTB()
        {
            return CodeEditRTB.Text.Trim();
        }

        private void CodeEditRTB_TextChanged(object sender, EventArgs e)
        {
            string[] arr = GetWords(CodeEditRTB.Text, @"\.(\S+)");
            foreach (var s in arr)
            {
                this.CheckKeyword(s, Color.Aquamarine, 0);
            }

            arr = GetWords(CodeEditRTB.Text, @"\#\s*(\S+(?:(?!\n)\s)*)*");
            foreach (var s in arr)
            {
                this.CheckKeyword(s, Color.LightGreen, 0);
            }

            foreach (var s in StaticData.sysDataLst)
            {
                foreach (var op in s.OpList)
                {
                    this.CheckKeyword(op.ToLower(), Color.Khaki, 0);
                }
            }

            foreach (var s in StaticData.sysDataLst)
            {
                foreach (var op in s.OpList)
                {
                    this.CheckKeyword(op.ToUpper(), Color.Khaki, 0);
                }
            }

            int numLines = CodeEditRTB.Text.Count(c => c.Equals('\n')) + 1;
            AddLabel(numLines);
        }

        private void CheckKeyword(string word, Color color, int startIndex)
        {
            if (this.CodeEditRTB.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.CodeEditRTB.SelectionStart;

                while ((index = this.CodeEditRTB.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.CodeEditRTB.Select((index + startIndex), word.Length);
                    this.CodeEditRTB.SelectionColor = color;
                    this.CodeEditRTB.Select(selectStart, 0);
                    this.CodeEditRTB.SelectionColor = Color.Silver;
                }
            }
        }
        
        private string[] GetWords(string input, string format)
        {
            var arr = Regex.Matches(input, format)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToArray();

            return arr;
        }

        private void AddLabel(int n)
        {
            string tmp = string.Empty;
            for (int i = 1; i < n + 1; i++)
            {
                tmp += i.ToString() + "\r\n";
            }
            LineNumberRTB.Text = tmp;
        }

        [DllImport("User32.dll")]
        public extern static int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("User32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private void CodeEditRTB_VScroll(object sender, EventArgs e)
        {
            int nPos = GetScrollPos(CodeEditRTB.Handle, (int)ScrollBarType.SbVert);
            nPos <<= 16;
            uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
            SendMessage(LineNumberRTB.Handle, (int)Message.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));
        }
    }
}

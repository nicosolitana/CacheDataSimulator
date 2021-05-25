using CacheDataSimulator.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WinFormsSyntaxHighlighter;

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
            Init();
        }

        public void Init()
        {
            var syntaxHighlighter = new SyntaxHighlighter(CodeEditRTB);

            // single-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"#.*?$", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(ColorTranslator.FromHtml("#597C49"), false, true));

            // double quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\""([^""]|\""\"")+\"""), new SyntaxStyle(ColorTranslator.FromHtml("#D9CE67")));

            // single quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\'([^']|\'\')+\'"), new SyntaxStyle(ColorTranslator.FromHtml("#D9CE67")));

            // Labels and Local Variables
            syntaxHighlighter.AddPattern(new PatternDefinition(@"[A-Za-z0-9]+\:"), new SyntaxStyle(ColorTranslator.FromHtml("#F4285F")));

            // Directive
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\.(\S+)"), new SyntaxStyle(ColorTranslator.FromHtml("#B47BB0")));

            // keywords1
            syntaxHighlighter.AddPattern(new PatternDefinition("lb", "lh", "lw", "LB", "LH", "LW"), new SyntaxStyle(ColorTranslator.FromHtml("#1390AD")));

            // keywords1
            syntaxHighlighter.AddPattern(new PatternDefinition("sw", "sh", "sb", "SW", "SH", "SB"), new SyntaxStyle(ColorTranslator.FromHtml("#1390AD")));

            // keywords1
            syntaxHighlighter.AddPattern(new PatternDefinition("addi", "slti", "ADDI", "SLTI"), new SyntaxStyle(ColorTranslator.FromHtml("#1390AD")));

            // keywords1
            syntaxHighlighter.AddPattern(new PatternDefinition("add", "slt", "sub", "ADD", "SLT", "SUB"), new SyntaxStyle(ColorTranslator.FromHtml("#1390AD")));

            // keywords1
            syntaxHighlighter.AddPattern(new PatternDefinition("beq", "bne", "blt", "bge", "BEQ", "BNE", "BLT", "BGE"), new SyntaxStyle(ColorTranslator.FromHtml("#1390AD")));
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]

        public static extern bool LockWindowUpdate(IntPtr hWndLock);

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
            try
            {
                //LockWindowUpdate(CodeEditRTB.Handle);
                
                //string[] arr = GetWords(CodeEditRTB.Text, @"\.(\S+)");
                //foreach (var s in arr)
                //{
                //    this.CheckKeyword(s, ColorTranslator.FromHtml("#f1c40f"), 0);
                //}

                //// Labels and Variables
                //arr = GetWords(CodeEditRTB.Text, @"[A-Za-z0-9]+\:");
                //foreach (var s in arr)
                //{
                //    this.CheckKeyword(s, ColorTranslator.FromHtml("#3498db"), 0);
                //}

                //foreach (var s in StaticData.sysDataLst)
                //{
                //    this.CheckKeyword(s.Operation.ToLower(), ColorTranslator.FromHtml("#9b59b6"), 0);
                //}

                //foreach (var s in StaticData.sysDataLst)
                //{
                //    this.CheckKeyword(s.Operation.ToUpper(), ColorTranslator.FromHtml("#9b59b6"), 0);
                //}

                //// Comment
                //arr = GetWords(CodeEditRTB.Text, @"\#\s*(\S+(?:(?!\n)\s)*)*");
                //foreach (var s in arr)
                //{
                //    this.CheckKeyword(s, ColorTranslator.FromHtml("#2ecc71"), 0);
                //}

                int numLines = CodeEditRTB.Text.Count(c => c.Equals('\n')) + 1;
                AddLabel(numLines);
            }
            finally
            {
                //LockWindowUpdate(IntPtr.Zero);
            }
        }

        //private void CheckKeyword(string word, Color color, int startIndex)
        //{
        //    if (this.CodeEditRTB.Text.Contains(word))
        //    {
        //        int index = -1;
        //        int selectStart = this.CodeEditRTB.SelectionStart;

        //        while ((index = this.CodeEditRTB.Text.IndexOf(word, (index + 1))) != -1)
        //        {
        //            this.CodeEditRTB.Select((index + startIndex), word.Length);
        //            this.CodeEditRTB.SelectionColor = color;
        //            this.CodeEditRTB.Select(selectStart, 0);
        //            this.CodeEditRTB.SelectionColor = ColorTranslator.FromHtml("#ecf0f1");
        //        }
        //    }
        //}


        //private string[] GetWords(string input, string format)
        //{
        //    var arr = Regex.Matches(input, format)
        //    .Cast<Match>()
        //    .Select(m => m.Value)
        //    .ToArray();

        //    return arr;
        //}

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

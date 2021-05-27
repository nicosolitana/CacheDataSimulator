using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinFormsSyntaxHighlighter;

namespace CacheDataSimulator.View
{
    public partial class ErrorLog : UserControl
    {
        public ErrorLog()
        {
            InitializeComponent();
            Init();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

        public void Init()
        {
            var syntaxHighlighter = new SyntaxHighlighter(ErrorLogRTB);
            syntaxHighlighter.AddPattern(new PatternDefinition(@"ERROR:[\s]*[A-Za-z0-9\s\:]+"), new SyntaxStyle(ColorTranslator.FromHtml("#D54545")));
            syntaxHighlighter.AddPattern(new PatternDefinition(@"-(\s)+[A-Za-z0-9\s\(\)]*."), new SyntaxStyle(ColorTranslator.FromHtml("#FEBEBE")));
            syntaxHighlighter.AddPattern(new PatternDefinition(@"FOLLOW:[[(\s\S)+]+]."), new SyntaxStyle(ColorTranslator.FromHtml("#0DBA78")));
            syntaxHighlighter.AddPattern(new PatternDefinition(@"ASSEMBLE:[(\s\S)+]+!"), new SyntaxStyle(ColorTranslator.FromHtml("#D9CE67")));
            syntaxHighlighter.AddPattern(new PatternDefinition(@"SUCCESS:[(\s\S)+]+."), new SyntaxStyle(ColorTranslator.FromHtml("#D9CE67")));
        }

        public void SetErrMsg(string msg)
        {
            ErrorLogRTB.Text = ErrorLogRTB.Text + msg;
            SendMessage(ErrorLogRTB.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
            ErrorLogRTB.SelectionStart = ErrorLogRTB.Text.Length;
        }

    }
}

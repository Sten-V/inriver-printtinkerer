using System.IO;
using System.Text;
using System.Windows.Controls;

namespace StarRepublic.Ipmc.PrintTinkerer.Application
{
    public class TextBoxWriter : TextWriter
    {
        private readonly TextBox _textBox;

        public TextBoxWriter(TextBox textBox)
        {
            _textBox = textBox;
        }

        public override void Write(char value)
        {
        }

        public override void Write(string value)
        {
            if (_textBox.Dispatcher.CheckAccess())
            {
                _textBox.Text += value;
                _textBox.ScrollToEnd();
            }
            else
            {
                _textBox.Dispatcher.Invoke(() =>
                {
                    _textBox.Text += value;
                    _textBox.ScrollToEnd();
                });
            }
        }

        public override Encoding Encoding => Encoding.ASCII;
    }
}
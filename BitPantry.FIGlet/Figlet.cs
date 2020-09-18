using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BitPantry.FIGlet
{
    public class Figlet
    {
        private Dictionary<char, string[]> _alphabet;
        private Font _font;

        public Figlet()
        {
            _font = Font.Standard;
        }

        public Figlet(Font font)
        {
            _font = font;
        }

        public Figlet(string fontFilePath)
        {
            _font = new Font(fontFilePath);
        }

        public string ToAsciiArt(string strText)
        {
            var res = "";
            for (int i = 1; i <= _font.Height; i++)
            {
                foreach (var car in strText)
                {
                    res += this.GetCharacter(car, i);
                }
                res += Environment.NewLine;
            }
            return res;
        }

        private string GetCharacter(char car, int line)
        {
            var start = _font.CommentLines + ((Convert.ToInt32(car) - 32) * _font.Height);
            var temp = _font.Lines[start + line];
            var lineending = temp[temp.Length - 1];
            var rx = new Regex(@"\" + lineending + "{1,2}$");
            temp = rx.Replace(temp, "");
            return temp.Replace(_font.HardBlank, " ");
        }
    }
}

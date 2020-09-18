using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BitPantry.FIGlet
{
    public class Font
    {
        public string Signature { get; private set; }
        public string HardBlank { get; private set; }
        public int Height { get; private set; }
        public int BaseLine { get; private set; }
        public int MaxLenght { get; private set; }
        public int OldLayout { get; private set; }
        public int CommentLines { get; private set; }
        public int PrintDirection { get; private set; }
        public int FullLayout { get; set; }
        public int CodeTagCount { get; set; }
        public List<string> Lines { get; set; }

        internal Font(string fontFilePath)
        {
            using (var fso = File.Open(fontFilePath, FileMode.Open))
                LoadFont(fso);
        }

        internal Font(Stream stream) { LoadFont(stream); }

        private void LoadFont(Stream fontStream)
        {
            var _fontData = new List<string>();
            using (var reader = new StreamReader(fontStream))
            {
                while (!reader.EndOfStream)
                {
                    _fontData.Add(reader.ReadLine());
                }
            }
            LoadLines(_fontData);
        }

        private void LoadLines(List<string> fontLines)
        {
            Lines = fontLines;
            var configString = Lines.First();
            var configArray = configString.Split(' ');
            Signature = configArray.First().Remove(configArray.First().Length - 1);
            if (Signature == "flf2a")
            {
                HardBlank = configArray.First().Last().ToString();
                Height = configArray.GetIntValue(1);
                BaseLine = configArray.GetIntValue(2);
                MaxLenght = configArray.GetIntValue(3);
                OldLayout = configArray.GetIntValue(4);
                CommentLines = configArray.GetIntValue(5);
                PrintDirection = configArray.GetIntValue(6);
                FullLayout = configArray.GetIntValue(7);
                CodeTagCount = configArray.GetIntValue(8);
            }
        }

        #region FONT FACTORY FUNCTIONS

        public static Font Standard => GetFont("standard");
        public static Font Isometric3 => GetFont("isometric3");
        public static Font Larry3d => GetFont("larry3d");
        public static Font Rectangles => GetFont("rectangles");
        public static Font Big => GetFont("big");
        public static Font Small => GetFont("small");

        private static Font GetFont(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"BitPantry.FIGlet.Fonts.{resourceName}.flf"))
                return new Font(stream);
        }

        #endregion
    }
}

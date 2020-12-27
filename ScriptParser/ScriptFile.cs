using System.IO;
using System.Text.RegularExpressions;

namespace ScriptParser
{
    internal class ScriptFile
    {
        public string FilePath { get; }
        public string FileName { get => Path.GetFileName(FilePath); }
        public int ChapterIndex { get => int.Parse(FileName.Substring(2, 2)); }
        public bool IsStoryScript
        {
            get => 
                new Regex(@"^[A-Z]{2}[0-9]{2}_[0-9]{2}[A-Z]_[0-9]{2}\..+")
                .IsMatch(FileName);
        }

        public ScriptFile(string filePath)
        {
            //RN01_02A_00.msb.txt
            FilePath = filePath;
        }

    }
}
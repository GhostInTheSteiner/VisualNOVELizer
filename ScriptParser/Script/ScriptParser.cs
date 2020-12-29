using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptParser
{
    class ScriptParser
    {
        private Dictionary<int, string> chapters;
        private ParseMode parseMode;
        private int maxParagraphLength;

        public ScriptParser(Dictionary<int, string> chapters, ParseMode parseMode, int maxParagraphLength)
        {
            this.chapters = chapters;
            this.parseMode = parseMode;
            this.maxParagraphLength = maxParagraphLength;
        }

        internal ScriptBook Parse(string directory)
        {
            //t = ScriptText()
            //p = ScriptParagraph()
            //l = ScriptLine()

            //each file: LinkedList < ScriptFile >

            //    if file.Value.ChapterIndex > file.Previous.Value.ChapterIndex
            //        t.AddChapter(chapters.ById(file.Value.ChapterIndex))

            //    else
            //      t.AddSection()

            //    each line: LinkedList<string>

            //        if line.Value.Person != line.Previous.Value.Person
            //            t.AddParagraph(p)
            //            p = ScriptParagraph()

            //        l = ScriptLine(line.Value, parseMode)
            //        p.Add(l)

            var files =
                new LinkedList<ScriptFile>(Directory
                    .GetFiles(directory)
                    .Select(currentFile => new ScriptFile(currentFile))
                    .Where(currentFile => currentFile.IsStoryScript));

            var file = files.First;

            var book = new ScriptBook();
            var paragraph = new ScriptParagraph();

            while (file != null)
            {
                var previousIndex = file.Previous?.Value.ChapterIndex ?? -1;

                if (file.Value.ChapterIndex > previousIndex)
                {
                    book.AddChapter(chapters[file.Value.ChapterIndex]);
                }
                else
                {
                    book.AddSection();
                }

                var lines =
                    new LinkedList<ScriptLine>(File
                        .ReadAllLines(file.Value.FilePath)
                        .Select(line_ => new ScriptLine(line_, parseMode)));

                var line = lines.First;

                while (line != null)
                {
                    string previousPerson = line.Previous?.Value.Person ?? line.Value.Person;

                    if (paragraph.Lines.Count() > maxParagraphLength) //enforce new paragraph
                    {
                        book.AddParagraph(paragraph);
                        paragraph = new ScriptParagraph();
                    }
                    else if (line.Value.Person.Equals(previousPerson))
                    {
                        //pass
                    }
                    else
                    {
                        book.AddParagraph(paragraph);
                        paragraph = new ScriptParagraph();
                    }

                    paragraph.AddLine(line.Value);

                    line = line.Next;
                }

                file = file.Next;
            }

            return book;
        }
    }
}

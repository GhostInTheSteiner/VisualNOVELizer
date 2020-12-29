using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptParser
{
    //two subsequent ... should lead to new line
    class ScriptParser
    {
        private Dictionary<int, ChapterTitle> chapters;
        private ParseMode parseMode;
        private int maxParagraphLength;
        private string title;

        public ScriptParser(string title, Dictionary<int, ChapterTitle> chapters, int maxParagraphLength, ParseMode parseMode)
        {
            this.chapters = chapters;
            this.parseMode = parseMode;
            this.maxParagraphLength = maxParagraphLength;
            this.title = title;
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

            var book = new ScriptBook(title);
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

                LinkedList<ScriptLine> lines = getScriptLines(file);

                var line = lines.First;

                while (line != null)
                {
                    var previousPerson = line.Previous?.Value.PersonID ?? line.Value.PersonID;
                    var previousEllipsisStyle = line.Previous?.Value.EllipsisStyle ?? line.Value.EllipsisStyle;

                    if (paragraph.Count() > maxParagraphLength) //enforce new paragraph
                    {
                        book.AddParagraph(paragraph);
                        paragraph = new ScriptParagraph();
                    }
                    else if ( //subsequent ... should be split into two paragraphs
                        line.Value.EllipsisStyle == EllipsisStyle.OnBegin &&
                        previousEllipsisStyle == EllipsisStyle.OnEnd
                    )
                    {
                        book.AddParagraph(paragraph);
                        paragraph = new ScriptParagraph();
                    }
                    else if (line.Value.PersonID.Equals(previousPerson))
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

                book.AddParagraph(paragraph);
                paragraph = new ScriptParagraph();

                file = file.Next;
            }

            return book;
        }

        private LinkedList<ScriptLine> getScriptLines(LinkedListNode<ScriptFile> file)
        {
            var linesArray = File.ReadAllLines(file.Value.FilePath);
            var lines = new LinkedList<ScriptLine>();

            for (int i = 0; i < linesArray.Length; i++)
            {
                //if (i > 0)
                //    lines.AddLast(new ScriptLine(linesArray[i], linesArray[i - 1], parseMode));

                //else
                    lines.AddLast(new ScriptLine(linesArray[i], parseMode));
            }

            return lines;
        }
    }
}

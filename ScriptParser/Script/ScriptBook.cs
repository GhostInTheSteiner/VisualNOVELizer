using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScriptParser
{
    internal class ScriptBook : IEnumerable<KeyValuePair<ChapterTitle, List<List<ScriptParagraph>>>>
    {
        public string Title { get; }

        Dictionary<ChapterTitle, List<List<ScriptParagraph>>> scriptText;
        Queue<ChapterTitle> chaptersOrdered;
        ChapterTitle currentChapter;

        public ScriptBook(string title)
        {
            scriptText = new Dictionary<ChapterTitle, List<List<ScriptParagraph>>>();
            chaptersOrdered = new Queue<ChapterTitle>();
            currentChapter = new ChapterTitle("<no chapters added>", "<no subchapters added>");

            Title = title;
        }

        internal void AddParagraph(ScriptParagraph paragraph)
        {
            if (scriptText.Count == 0)
                throw new Exception("No chapters found. Therefore a section can't be added!");

            else if (scriptText[currentChapter].Count == 0)
                throw new Exception("No sections found. Therefore a paragraph can't be added!");

            else
            {
                scriptText[currentChapter].Last().Add(paragraph);
            }
        }

        internal void AddSection()
        {
            if (scriptText.Count == 0)
                throw new Exception("No chapters found. Therefore a section can't be added!");
            
            else
            {
                //add to current chapter
                scriptText[currentChapter].Add(new List<ScriptParagraph>()); // follow-up section
            }
        }

        internal void AddChapter(ChapterTitle chapter)
        {
            if (scriptText.ContainsKey(chapter))
                throw new Exception("Chapter already exists!");

            else
            {
                scriptText[chapter] = new List<List<ScriptParagraph>>();
                scriptText[chapter].Add(new List<ScriptParagraph>()); // initial section

                chaptersOrdered.Enqueue(chapter);
                currentChapter = chapter;
            }
        }

        public IEnumerator<KeyValuePair<ChapterTitle, List<List<ScriptParagraph>>>> GetEnumerator()
        {
            while (chaptersOrdered.Count() > 0)
            {
                var chapterTitle = chaptersOrdered.Dequeue();
                yield return new KeyValuePair<ChapterTitle, List<List<ScriptParagraph>>>(chapterTitle, scriptText[chapterTitle]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            while (chaptersOrdered.Count() > 0)
            {
                var chapterTitle = chaptersOrdered.Dequeue();
                yield return new KeyValuePair<ChapterTitle, List<List<ScriptParagraph>>>(chapterTitle, scriptText[chapterTitle]);
            }
        }
    }

    public class ChapterTitle
    {
        public string MainTitle { get; }
        public string SubTitle { get; }

        public ChapterTitle(string mainTitle, string subTitle)
        {
            MainTitle = mainTitle;
            SubTitle = subTitle;
        }
    }
}
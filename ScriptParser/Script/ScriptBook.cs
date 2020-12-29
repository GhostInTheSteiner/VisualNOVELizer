using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScriptParser
{
    internal class ScriptBook : IEnumerable<KeyValuePair<string, List<List<ScriptParagraph>>>>
    {
        Dictionary<string, List<List<ScriptParagraph>>> scriptText;
        Queue<string> chaptersOrdered;
        string currentChapter;

        public ScriptBook()
        {
            scriptText = new Dictionary<string, List<List<ScriptParagraph>>>();
            chaptersOrdered = new Queue<string>();
            currentChapter = "<no chapters added>";
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

        internal void AddChapter(string chapter)
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

        public IEnumerator<KeyValuePair<string, List<List<ScriptParagraph>>>> GetEnumerator()
        {
            var chapterName = chaptersOrdered.Dequeue();

            yield return new KeyValuePair<string, List<List<ScriptParagraph>>>(chapterName, scriptText[chapterName]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var chapterName = chaptersOrdered.Dequeue();

            yield return new KeyValuePair<string, List<List<ScriptParagraph>>>(chapterName, scriptText[chapterName]);
        }
    }
}
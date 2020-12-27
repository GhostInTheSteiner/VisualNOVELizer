using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptParser
{
    internal class ScriptText
    {
        Dictionary<string, List<List<ScriptParagraph>>> scriptText;
        private Queue<string> chaptersOrdered;
        private string currentChapter;

        public ScriptText()
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



    ////chapter-name -> section-name -> contained paragraphs -> single paragraph
    //+ Paragraphs: Dictionary<string, List<List<Paragraph>>>
    // + AddChapter(name: string)
    // + AddSection()
    // + AddParagraph(paragraph: ScriptParagraph)
        }

        internal KeyValuePair<string, List<List<ScriptParagraph>>> GetNextChapter()
        {
            var chapterName = chaptersOrdered.Dequeue();
            return new KeyValuePair<string, List<List<ScriptParagraph>>>(chapterName, scriptText[chapterName]);
        }
    }
}
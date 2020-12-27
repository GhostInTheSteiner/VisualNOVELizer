using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ApplicationConfiguration(
                (NameValueCollection)ConfigurationManager.GetSection("Parser"),
                (NameValueCollection)ConfigurationManager.GetSection("ParserChapters"),
                (NameValueCollection)ConfigurationManager.GetSection("Writer")
            );

            var composition = new ApplicationComposition(configuration);

            ConversionAPI api = composition.ConversionAPI;

            ScriptText text = api.Parse(args[0]);
            api.Export(text, args[1]);

            /*
            Program()
                +Main()



            ConversionAPI(parser: ScriptParser, writer: DocumentWriter)
                +Parse(directory: string): ScriptText
                    //calls parser.Parse()
                +Export(text: ScriptText, filePath: string)
                ...

            DocumentWriter(documentFormat: DocumentFormat)
                +Export(text: ScriptText, filePath: string)
                    //choses export-method based on documentFormat
                    //executes it
                    //saves returned stream
                -ToHTML(text: ScriptText): Stream
                -ToEPUB(text: ScriptText): Stream
                -ToWord(text: ScriptText): Stream

            ScriptParser(chapters: ScriptChapters, parseMode: ParseMode)
                +Parse(directory: string): ScriptText
                -chapters
                -parseMode
    =
            ScriptText()
                //chapter-name -> section-name -> contained paragraphs -> single paragraph
                +Paragraphs: Dictionary<string, List<List<Paragraph>>>
                +AddChapter(name: string)
                +AddSection()
                +AddParagraph(paragraph: ScriptParagraph)

            ScriptChapters()
                +ById(id: int): string // chapter-name
                +Add(name: string, id: int)

            ScriptParagraph()
                +Lines: List<ScriptLine>
                +Add(line)

            ScriptLine(line: string, parseMode: ParseMode)
                    uses ScriptLineParser
                    creates ScriptLineWord[]
                +Person
                +Content: ScriptLineWord[]

            ParseMode() //enum
                SC3Output
                DoubleColonLine

            ScriptLineParser() //static
                +FromSC3Output(): ScriptLineWord[] //static
                +FromDoubleColonLine(): ScriptLineWord[] //static

            ScriptLineWord()
                +Content: string
                +Style: FontStyle
                +Alignment: TextAlignment

            ScriptFile(filePath: string)
                +IsScript: bool //story script, no menu or boot script
                +Prefix: string
                +ChapterIndex: int
                +SectionCharacter: char
                +SectionIndex: int
                +SubSectionIndex: int
                +FilePath







            Class-Variables:
                chapters: ScriptChapters
                parseMode: ParseMode

            t = ScriptText()
            p = ScriptParagraph()
            l = ScriptLine()

            each file: LinkedList<ScriptFile>

                if file.Value.ChapterIndex > file.Previous.Value.ChapterIndex
                    t.AddChapter(chapters.ById(file.Value.ChapterIndex))

                else
                    t.AddSection()

                each line: LinkedList<string>

                    if line.Value.Person != line.Previous.Value.Person
                        t.AddParagraph(p)
                        p = ScriptParagraph()

                    l = ScriptLine(line.Value, parseMode)
                    p.Add(l)
            */
        }
    }
}

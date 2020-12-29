using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace ScriptParser
{
    internal class HTMLDocumentWriter : IDocumentWriter
    {
        public HTMLDocumentWriter()
        {
        }

        public void Export(ScriptBook book, string filePath)
        {
            using (var writer = new HtmlTextWriter(new StreamWriter(File.Create(filePath))))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Style);

                var cssLines =
                    File.ReadAllLines(
                        Path.Combine(
                            Path.GetDirectoryName(
                                System.Reflection.Assembly.GetEntryAssembly().Location
                            ),
                            "style.css"
                        )
                    );

                writer.WriteLine();

                foreach (var cssLine in cssLines)
                    writer.WriteLine(cssLine);

                writer.RenderEndTag();
                writer.WriteLine();

                writer.RenderBeginTag(HtmlTextWriterTag.H1);
                writer.Write(book.Title);
                writer.RenderEndTag();
                writer.WriteLine();


                writer.AddAttribute(HtmlTextWriterAttribute.Class, "story-area");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                foreach (var chapter in book)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.Write(chapter.Key.MainTitle);
                    writer.RenderEndTag();
                    writer.WriteLine();

                    writer.RenderBeginTag(HtmlTextWriterTag.H2);
                    writer.Write(chapter.Key.SubTitle);
                    writer.RenderEndTag();
                    writer.WriteLine();

                    foreach (var section in chapter.Value)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "story-section");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        foreach (var paragraph in section)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "story-paragraph");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);

                            var lineBuilder = new LineBuilder();

                            foreach (var line in paragraph)
                            {
                                lineBuilder.Append(line);
                                //lineBuilder = concat(lineBuilder, line);
                            }

                            writer.Write(lineBuilder.ToString());
                            writer.RenderEndTag();
                            writer.WriteLine();
                        }

                        writer.RenderEndTag();
                        writer.WriteLine();

                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "story-separator");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.WriteLine("☆☆☆☆☆");
                        writer.RenderEndTag();
                    }
                }
            }
        }

        class LineBuilder
        {
            private StringBuilder builder;
            private ScriptLine previousLine;

            public LineBuilder()
            {
                builder = new StringBuilder();
            }

            public void Append(ScriptLine line)
            {
                //IEnumerable<ScriptLineWord> words = line;

                ////ensure line ends with a dot
                //if (lineBuilder.Length < 2)
                //{
                //    //pass => nothing to trim
                //}
                //else if (lineBuilder[lineBuilder.Length - 1] == '”')
                //{
                //    //remove ” of .”
                //    lineBuilder = lineBuilder.Remove(lineBuilder.Length - 1, 1);
                //    words = line.Content.Skip(1);
                //}
                ////else
                ////{
                ////    //remove .
                ////    lineBuilder = lineBuilder.Remove(lineBuilder.Length - 1, 1);
                ////}

                var previousLineQuoted = previousLine?.IsQuoted ?? false;

                if (previousLineQuoted && line.IsQuoted)
                {
                    //pass => keep opened quotes
                }
                else if (previousLineQuoted && !line.IsQuoted)
                {
                    //close quotes
                    builder.Append("”");
                }
                else if (!previousLineQuoted && line.IsQuoted)
                {
                    //open quotes
                    builder.Append("“");
                }
                else //if (!previousLineQuoted && !line.IsQuoted)
                {
                    //pass => keep closed quotes
                }

                //foreach (var word in words)
                foreach (var word in line)
                {
                    //TODO: Add attributes
                    builder.Append(word.Content);
                    builder.Append(" ");
                }

                previousLine = line;
            }

            public override string ToString()
            {
                if (previousLine.IsQuoted)
                    return builder.ToString().Trim() + "”";

                else
                    return builder.ToString().Trim();
            }
        }

        
    }
}

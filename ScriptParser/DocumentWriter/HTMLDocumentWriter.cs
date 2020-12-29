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
            HtmlTextWriter writer = new HtmlTextWriter(new StreamWriter(File.Create(filePath)));

            foreach (var chapter in book)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H3);
                writer.Write(chapter.Key);
                writer.RenderEndTag();

                foreach (var section in chapter.Value)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "section");

                    foreach(var paragraph in section)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "paragraph");

                        var lineBuilder = new StringBuilder();
                        
                        foreach(var line in paragraph)
                        {
                            foreach(var word in line)
                            {
                                //TODO: Add attributes
                                lineBuilder.Append(word.Content);
                                lineBuilder.Append(" ");
                            }

                            lineBuilder.Append(". ");
                        }

                        writer.Write(lineBuilder.ToString().Trim());
                        writer.RenderEndTag();
                    }

                    writer.RenderEndTag();
                }
            }
        }
    }
}

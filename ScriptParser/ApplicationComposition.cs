using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ScriptParser
{
    internal class ApplicationComposition
    {
        public ConversionAPI ConversionAPI { get; internal set; }

        public ApplicationComposition(ApplicationConfiguration configuration)
        {
            Dictionary<string, ParseMode> parseModeMappings = new Dictionary<string, ParseMode>
            {
                { "DoubleColon", ParseMode.DoubleColon },
                { "SC3Output", ParseMode.SC3Output }
            };

            Dictionary<string, DocumentFormat> documentFormatMappings = new Dictionary<string, DocumentFormat>
            {
                { "EPUB", DocumentFormat.EPUB },
                { "Word", DocumentFormat.Word }
            };

            var parseMode = parseModeMappings[configuration.Parser["ParseMode"]];
            var maxParagraphLength = int.Parse(configuration.Parser["MaxParagraphLength"]);
            var chapters = getChapters(configuration.ParserChapters);
            var format = documentFormatMappings[configuration.Writer["DocumentFormat"]];

            var parser = new ScriptParser(chapters, parseMode, maxParagraphLength);
            var writer = new DocumentWriter(format);

            ConversionAPI = new ConversionAPI(parser, writer);
        }

        private Dictionary<int, string> getChapters(NameValueCollection parserChapters)
        {
            Dictionary<int, string> chapters = new Dictionary<int, string>();
            int i = 1;

            foreach (string chapter in parserChapters)
            {
                chapters.Add(i++, parserChapters[chapter]);
            }

            return chapters;
        }
    }
}
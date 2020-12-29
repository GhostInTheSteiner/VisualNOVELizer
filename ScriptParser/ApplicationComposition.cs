﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ScriptParser
{
    internal class ApplicationComposition
    {
        public ConversionAPI ConversionAPI { get; internal set; }

        public ApplicationComposition(ApplicationConfiguration configuration)
        {
            var parseModeMappings = new Dictionary<string, ParseMode>
            {
                { "DoubleColon", ParseMode.DoubleColon },
                { "SC3Output", ParseMode.SC3Output }
            };

            var documentWriterMappings = new Dictionary<string, Func<IDocumentWriter>>
            {
                { "HTML", () => new HTMLDocumentWriter() },
                { "Word", () => new WordDocumentWriter() }
            };

            var parseMode = parseModeMappings[configuration.Parser["ParseMode"]];
            var maxParagraphLength = int.Parse(configuration.Parser["MaxParagraphLength"]);
            var title = configuration.Parser["BookTitle"];
            var chapters = getChapters(configuration.ParserChapters);
            var formatWriter = documentWriterMappings[configuration.Writer["DocumentFormat"]];

            var parser = new ScriptParser(title, chapters, maxParagraphLength, parseMode);
            var writer = formatWriter();

            ConversionAPI = new ConversionAPI(parser, writer);
        }

        private Dictionary<int, ChapterTitle> getChapters(NameValueCollection parserChapters)
        {
            Dictionary<int, ChapterTitle> chapters = new Dictionary<int, ChapterTitle>();
            int i = 1;

            foreach (string chapter in parserChapters)
            {
                var chapterParts = parserChapters[chapter].Split(';');
                var chapterTitle = new ChapterTitle(chapterParts[0], chapterParts[1]);

                chapters.Add(i++, chapterTitle);
            }

            return chapters;
        }
    }
}
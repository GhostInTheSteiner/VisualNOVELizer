using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScriptParser
{
    internal class ConversionAPI
    {
        private ScriptParser parser;
        private IDocumentWriter writer;

        public ConversionAPI(ScriptParser parser, IDocumentWriter writer)
        {
            this.parser = parser;
            this.writer = writer;
        }

        internal ScriptBook Parse(string directory)
        {
            return parser.Parse(directory);
        }

        internal void Export(ScriptBook book, string filePath)
        {
            writer.Export(book, filePath);
        }
    }
}
using System;
using System.Collections.Generic;

namespace ScriptParser
{
    internal class ScriptParagraph
    {
        public List<ScriptLine> Lines { get; }

        public ScriptParagraph()
        {
            Lines = new List<ScriptLine>();
        }

        internal void AddLine(ScriptLine value)
        {
            //        ScriptParagraph()
            //+ Lines: List < ScriptLine >
            // +Add(line)

            Lines.Add(value);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

namespace ScriptParser
{
    internal class ScriptParagraph : IEnumerable<ScriptLine>
    {
        List<ScriptLine> Lines { get; }

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

        public IEnumerator<ScriptLine> GetEnumerator()
        {
            return ((IEnumerable<ScriptLine>)Lines).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Lines).GetEnumerator();
        }
    }
}
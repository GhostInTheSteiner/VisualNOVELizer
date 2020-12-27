namespace ScriptParser
{
    internal class DocumentWriter
    {
        private DocumentFormat format;

        public DocumentWriter(DocumentFormat format)
        {
            this.format = format;
        }
    }

    internal enum DocumentFormat { EPUB, Word }
}
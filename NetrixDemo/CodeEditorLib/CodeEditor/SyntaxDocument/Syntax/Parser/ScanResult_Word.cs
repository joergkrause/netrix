namespace GuruComponents.CodeEditor.CodeEditor.Syntax.SyntaxDocumentParsers
{

    public class ScanResult_Word
    {
        public Pattern Pattern = null;
        public bool HasContent = false;
        public string Token = "";
        public int Position = 0;
        public PatternList ParentList = null;
    }
}
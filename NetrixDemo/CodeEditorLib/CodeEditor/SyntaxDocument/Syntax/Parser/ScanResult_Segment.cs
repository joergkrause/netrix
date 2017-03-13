//no parsing , just splitting and making whitespace possible
//1 sec to finnish ca 10000 rows

namespace GuruComponents.CodeEditor.CodeEditor.Syntax.SyntaxDocumentParsers
{
    public class ScanResult_Segment
    {
        public Pattern Pattern = null;
        public int Position = 0;
        public bool IsEndSegment = false;
        public bool HasContent = false;
        public string Token = "";
        public BlockType BlockType = null;
        public Segment Segment = null;
        public Scope Scope = null;
    }
}
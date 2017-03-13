using System;

namespace GuruComponents.Netrix 
{

    /// <summary>
    /// Used to inform about the ability to do a command.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public enum HtmlCommandInfo 
    {
        /// <summary>
        /// Command is available.
        /// </summary>
        Enabled = 1,
        /// <summary>
        /// Informs that the state of the command is on or the related element is selected.
        /// </summary>
        Checked = 2,
        /// <summary>
        /// Set if the command is checked (on) and still available.
        /// </summary>
        Both    = 3,
        /// <summary>
        /// Set if the command is a toggle command and is currently active.
        /// </summary>
        Latched = 4,
        /// <summary>
        /// Set if the command is not checked (off) and not available at that point.
        /// </summary>
        None    = 0,
        /// <summary>
        /// Set if the control has not successfully recognized the given command.
        /// </summary>
        /// <remarks>
        /// This does not mean that the command is unavailable. It just means that there is no valid information present.
        /// </remarks>
        Error   = 9
    }

    /// <summary>
    /// These commands can be used to check if the options is on or available.
    /// </summary>
    public enum HtmlCommand
    {
        /// <summary>
        /// Align elements to bottom border in absolute position mode.
        /// </summary>
        AlignBottom             = 1,
        /// <summary>
        /// Align elements to horizontal centered lines in absolute position mode.
        /// </summary>
        AlignHorizontalCenter   = 2,
        /// <summary>
        /// Align elements to left border in absolute position mode.
        /// </summary>
        AlignLeft               = 3,
        /// <summary>
        /// Align elements to right border in absolute position mode.
        /// </summary>
        AlignRight              = 4,
        /// <summary>
        /// Align element to grid lines in absolute position mode.
        /// </summary>
        AlignToGrid             = 5,
        /// <summary>
        /// Align elements to top border in absolute position mode.
        /// </summary>
        Aligntop                = 6,
        /// <summary>
        /// 
        /// </summary>
        AlignVerticalCenter     = 7,
        /// <summary>
        /// 
        /// </summary>
        ArrangeBottom = 8,
        /// <summary>
        /// 
        /// </summary>
        ArrangeRight = 9,
        /// <summary>
        /// 
        /// </summary>
        BringForward = 10,
        /// <summary>
        /// 
        /// </summary>
        BringToFront = 11,
        /// <summary>
        /// 
        /// </summary>
        CenterHorizontally = 12,
        /// <summary>
        /// 
        /// </summary>
        CenterVertically = 13,
        /// <summary>
        /// Delete element.
        /// </summary>
        Delete                  = 17,
        /// <summary>
        /// Set font name (family name).
        /// </summary>
        Fontname                = 18,
        /// <summary>
        /// Set font size in HTML units (1...7).
        /// </summary>
        Fontsize                = 19,
        /// <summary>
        /// Send an element backwards in absolute position mode.
        /// </summary>
        SendBackward            = 32,
        /// <summary>
        /// Send an element to back in absolute position mode.
        /// </summary>
        SendToBack              = 33,
        /// <summary>
        /// Justify the selected paragraph.
        /// </summary>
        JustifyFull             = 50,
        /// <summary>
        /// Set the back color.
        /// </summary>
        BackColor               = 51,
        /// <summary>
        /// Set bold.
        /// </summary>
        Bold                    = 52,
        /// <summary>
        /// Set border color.
        /// </summary>
        BorderColor             = 53,
        /// <summary>
        /// Set fore color of text.
        /// </summary>
        ForeColor               = 55,
        /// <summary>
        /// Set italic.
        /// </summary>
        Italic                  = 56,
        /// <summary>
        /// 
        /// </summary>
        JustifyCenter = 57,
        /// <summary>
        /// 
        /// </summary>
        JustifyGeneral = 58,
        /// <summary>
        /// 
        /// </summary>
        JustifyLeft = 59,
        /// <summary>
        /// 
        /// </summary>
        JustifyRight = 60,
        /// <summary>
        /// Underline text.
        /// </summary>
        Underline               = 63,
        /// <summary>
        /// 
        /// </summary>
        Font = 90,
        /// <summary>
        /// 
        /// </summary>
        StrikeThrough = 91,
        /// <summary>
        /// Delete word under caret.
        /// </summary>
        DeleteWord          =     92,
        /// <summary>
        /// 
        /// </summary>
        ExecPrint = 93,
        /// <summary>
        /// 
        /// </summary>
        JustifyNone = 94,

        /// <summary>
        /// 
        /// </summary>
        InputImage = 2114,
        /// <summary>
        /// 
        /// </summary>
        InputButton = 2115,
        /// <summary>
        /// 
        /// </summary>
        InputReset = 2116,
        /// <summary>
        /// 
        /// </summary>
        InputSubmit = 2117,
        /// <summary>
        /// 
        /// </summary>
        InputUpload = 2118,
        /// <summary>
        /// 
        /// </summary>
        FieldSet = 2119,

        /// <summary>
        /// 
        /// </summary>
        Bookmark = 2123,
        /// <summary>
        /// 
        /// </summary>
        Hyperlink = 2124,
        /// <summary>
        /// 
        /// </summary>
        Unlink = 2125,
        /// <summary>
        /// 
        /// </summary>
        Unbookmark = 2128,

        /// <summary>
        /// 
        /// </summary>
        HorizontalRule = 2150,
        /// <summary>
        /// 
        /// </summary>
        LinebreakNormal = 2151,
        /// <summary>
        /// 
        /// </summary>
        LinebreakLeft = 2152,
        /// <summary>
        /// 
        /// </summary>
        LinebreakRight = 2153,
        /// <summary>
        /// 
        /// </summary>
        LinebreakBoth = 2154,
        /// <summary>
        /// 
        /// </summary>
        NonBreak = 2155,
        /// <summary>
        /// 
        /// </summary>
        InputTextbox = 2161,
        /// <summary>
        /// 
        /// </summary>
        InputTextarea = 2162,
        /// <summary>
        /// 
        /// </summary>
        InputCheckbox = 2163,
        /// <summary>
        /// 
        /// </summary>
        InputRadiobutton = 2164,
        /// <summary>
        /// 
        /// </summary>
        InputDropdownbox = 2165,
        /// <summary>
        /// 
        /// </summary>
        InputListbox = 2166,
        /// <summary>
        /// 
        /// </summary>
        Button = 2167,
        /// <summary>
        /// 
        /// </summary>
        Image = 2168,
        /// <summary>
        /// 
        /// </summary>
        ImageMap = 2171,
        /// <summary>
        /// 
        /// </summary>
        File = 2172,
        /// <summary>
        /// 
        /// </summary>
        Comment = 2173,
        /// <summary>
        /// 
        /// </summary>
        Script = 2174,
        /// <summary>
        /// 
        /// </summary>
        JavaApplet = 2175,
        /// <summary>
        /// 
        /// </summary>
        Plugin = 2176,
        /// <summary>
        /// 
        /// </summary>
        PageBreak = 2177,
        /// <summary>
        /// 
        /// </summary>
        Area = 2178,

        /// <summary>
        /// 
        /// </summary>
        Paragraph = 2180,
        /// <summary>
        /// 
        /// </summary>
        Form = 2181,
        /// <summary>
        /// 
        /// </summary>
        Marquee = 2182,
        /// <summary>
        /// 
        /// </summary>
        List = 2183,
        /// <summary>
        /// 
        /// </summary>
        OrderList = 2184,
        /// <summary>
        /// 
        /// </summary>
        UnorderList = 2185,
        /// <summary>
        /// 
        /// </summary>
        Indent = 2186,
        /// <summary>
        /// 
        /// </summary>
        Outdent = 2187,
        /// <summary>
        /// 
        /// </summary>
        Preformatted = 2188,
        /// <summary>
        /// 
        /// </summary>
        Address = 2189,
        /// <summary>
        /// 
        /// </summary>
        Blink = 2190,
        /// <summary>
        /// 
        /// </summary>
        Div = 2191,
        /// <summary>
        /// Remove block/paragraph format.
        /// </summary>
        RemoveFormat        =    2230,
        /// <summary>
        /// 
        /// </summary>        
        TeleType            =    2232,
        /// <summary>
        /// 
        /// </summary>
        SetBlockFormat = 2234,
        /// <summary>
        /// 
        /// </summary>
        SubScript = 2247,
        /// <summary>
        /// 
        /// </summary>
        SuperScript = 2248,
        /// <summary>
        /// 
        /// </summary>
        CenterAlignPara = 2250,
        /// <summary>
        /// 
        /// </summary>
        LeftAlignPara = 2251,
        /// <summary>
        /// 
        /// </summary>
        RightAlignPara = 2252,
        /// <summary>
        /// 
        /// </summary>
        DirLtr = 2350,
        /// <summary>
        /// 
        /// </summary>
        DirRtl = 2351,
        /// <summary>
        /// 
        /// </summary>
        BlockDirLtr = 2352,
        /// <summary>
        /// 
        /// </summary>
        BlockDirRtl = 2353,
        /// <summary>
        /// 
        /// </summary>
        InlineDirLtr = 2354,
        /// <summary>
        /// 
        /// </summary>
        InlineDirRtl = 2355,
        /// <summary>
        /// 
        /// </summary>
        InsertSpan = 2357,
        /// <summary>
        /// 
        /// </summary>
        MultipleSelection = 2393,
        /// <summary>
        /// 
        /// </summary>
        AbsolutePosition = 2394,
        /// <summary>
        /// 
        /// </summary>
        AbsoluteElement2D = 2395,
        /// <summary>
        /// 
        /// </summary>
        AbsoluteElement1D = 2396,
        /// <summary>
        /// 
        /// </summary>
        AbsolutePositioned = 2397,
        /// <summary>
        /// 
        /// </summary>
        LiveResize = 2398,
        /// <summary>
        /// 
        /// </summary>
        AtomicSelection = 2399,
        /// <summary>
        /// 
        /// </summary>
        AutourlDetectMode = 2400,
        /// <summary>
        /// 
        /// </summary>
        Print = 27,
        /// <summary>
        /// 
        /// </summary>
        ClearSelection = 2007,
        /// <summary>
        /// 
        /// </summary>
        Redo = 29,
        /// <summary>
        /// 
        /// </summary>
        Undo = 43,
        /// <summary>
        /// 
        /// </summary>
        SelectAll = 31,
        /// <summary>
        /// 
        /// </summary>
        Copy = 15,
        /// <summary>
        /// 
        /// </summary>
        Cut = 16,
        /// <summary>
        /// 
        /// </summary>
        Paste = 26,

    }

}
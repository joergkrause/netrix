using System;
using GuruComponents.Netrix.ComInterop ;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner.Elements;

namespace GuruComponents.Netrix.VmlDesigner.Util
{
    /// <summary>
    /// A Sequence of PathCommands
    /// </summary>
    public class PathCommandSequence /*: System.Collections.IEnumerable*/ {

        /// <summary>
        /// Define the possible TokenTypes
        /// </summary>
        public enum PathCommandSequenceToken
        {
            /// <summary>
            /// Not defined
            /// </summary>
            Undefined,
            /// <summary>
            /// Move to
            /// </summary>
            CommandMoveTo,
            /// <summary>
            /// Line from current point to another
            /// </summary>
            CommandLineTo,
            /// <summary>
            /// Close path
            /// </summary>
            CommandClose,
            /// <summary>
            /// End whole command.
            /// </summary>
            CommandEnd,
            /// <summary>
            /// It's a number.
            /// </summary>
            Number,
            /// <summary>
            /// It's a formular
            /// </summary>
            Formula,
            /// <summary>
            /// A  separator char
            /// </summary>
            Separator,
            /// <summary>
            /// A sginificant whitespace.
            /// </summary>
            Whitespace
        }


        /// <summary>
        /// A PathCommand
        /// 
        /// This is a Helper Class for returning command by command
        /// </summary>
        public class PathCommand
        {

            /// <summary>
            /// dDefine the possible Types of PathCommands
            /// </summary>
            public enum PathCommandType
            {
                /// <summary>
                /// Do Nothing
                /// </summary>
                Nothing,
                /// <summary>
                /// Move to
                /// </summary>
                MoveTo,
                /// <summary>
                /// Line to
                /// </summary>
                LineTo,
                /// <summary>
                /// Close the line
                /// </summary>
                CloseLine,
                /// <summary>
                /// End the line
                /// </summary>
                EndLine
            }

            // save the command
            private PathCommandType cmd;

            // save the parameters for this command
            private PathCommandToken[] param;

            /// <summary>
            /// Creates a new PathCommand without Parameters
            /// </summary>
            /// <param name="Cmd">the PathComandType</param>
            public PathCommand(PathCommandType Cmd)
                : this(Cmd, null)
            {
                ;
            }

            /// <summary>
            /// Creates a new PahtCommand with Parameters
            /// </summary>
            /// <param name="Cmd">the PathCommandType</param>
            /// <param name="Param">the Array of PathCommandToken</param>
            public PathCommand(PathCommandType Cmd, PathCommandToken[] Param)
            {
                cmd = Cmd;
                param = Param;
            }

            /// <summary>
            /// Contains the PathComandType
            /// </summary>
            public PathCommandType Command
            {
                get { return cmd; }
            }

            /// <summary>
            /// Contains the Parameters (Array of PathCommandToken)
            /// </summary>
            public PathCommandToken[] Param
            {
                get { return param; }
            }
        }

        /// <summary>
        /// A PathCommand Token
        /// </summary>
        public class PathCommandToken
        {

            private PathCommandSequenceToken type;
            private int val;

            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="Type"></param>
            /// <param name="Value"></param>
            public PathCommandToken(PathCommandSequenceToken Type, int Value)
            {
                type = Type;
                val = Value;
            }

            /// <summary>
            /// Contains the type of this Token
            /// </summary>
            public PathCommandSequenceToken Type
            {
                get { return type; }
            }

            /// <summary>
            /// Value
            /// </summary>
            public int Value
            {
                get { return val; }
            }

            /// <summary>
            /// Converts this Token to a String
            /// </summary>
            /// <returns>the String</returns>
            public override string ToString()
            {
                string ret;
                switch (type)
                {
                    case PathCommandSequenceToken.Number:
                        ret = val.ToString();
                        break;
                    case PathCommandSequenceToken.Formula:
                        ret = "@" + val.ToString();
                        break;
                    default:
                        ret = type.ToString();
                        break;
                }
                return ret;
            }

        }


        /// <summary>
        /// Cuts a Path-string into tokens
        /// </summary>
        public class PathCommandTokenizer
        {

            private string eqn;
            private int pos;


            /// <summary>
            /// Creates a new PathCommandTokenizer from a Path-String
            /// </summary>
            /// <param name="Eqn">the Path-String</param>
            public PathCommandTokenizer(string Eqn)
            {
                pos = 0;
                eqn = Eqn;
            }

            /// <summary>
            /// Gets the next Token.
            /// </summary>
            /// <returns>the next token</returns>
            public PathCommandToken GetNextToken()
            {
                PathCommandToken ret = new PathCommandToken(PathCommandSequenceToken.Undefined, 0);
                int i;

                // we have reached the end of string
                if (pos >= eqn.Length)
                {
                    return null;
                }

#if EXT_PARSER
                // we had to keep one token,
                // return it now
                if (carryForward!=null){
                    ret = carryForward;
                    carryForward=null;
                    return ret ;
                }
#endif

                // get the next char
                switch (eqn[pos])
                {
                    case '-':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        for (i = pos; ((eqn[i] >= '0' && eqn[i] <= '9') || eqn[i] == '-') && (i < eqn.Length); i++) ;
                        string str = eqn.Substring(pos, i - (pos));
                        int tmp = (int)Convert.ToInt32(str);
                        ret = new PathCommandToken(PathCommandSequenceToken.Number, tmp);
                        pos = i;
                        break;
                    case ',':
#if EXT_PARSER
                        // the extended parser for the shorter path strings
                        if (lookBack && !(lastToken == PathCommandSequenceToken.Number || lastToken == PathCommandSequenceToken.Forumla)){
                            lookBack = false ;
                            // insert an additional number zero (behind ',' 'm' 'l')                                
                            ret = new PathCommandToken(PathCommandSequenceToken.Number,0);                            
                        }else{
                            pos++;
                            lastToken = PathCommandSequenceToken.Separator ;
                            carryForward = GetNextToken();
                            if (carryForward.Type == PathCommandSequenceToken.Number || carryForward.Type == PathCommandSequenceToken.Forumla){
                                ret = carryForward ;
                                carryForward = null;
                            }else{
                                // insert an additional number zero and keep carryForward (before 'm' 'l' 'e' 'x')
                                ret = new PathCommandToken(PathCommandSequenceToken.Number,0);    
                            }
                        }
                        lastToken = ret.Type ;
                        break;
#endif
                    case ' ':
                    case '\t':
                        pos++;
                        ret = GetNextToken();

                        break;
                    case 'm':
                        ret = new PathCommandToken(PathCommandSequenceToken.CommandMoveTo, 0);
                        pos++;
                        break;
                    case 'l':
                        ret = new PathCommandToken(PathCommandSequenceToken.CommandLineTo, 0);
                        pos++;
                        break;
                    case 'x':
                        ret = new PathCommandToken(PathCommandSequenceToken.CommandClose, 0);
                        pos++;
                        break;
                    case 'e':
                        ret = new PathCommandToken(PathCommandSequenceToken.CommandEnd, 0);
                        pos++;
                        break;
                    case '@':
                        pos++;
                        for (i = pos; (eqn[i] >= '0' && eqn[i] <= '9') && (i < eqn.Length); i++) ;
                        ret = new PathCommandToken(PathCommandSequenceToken.Formula, Convert.ToInt32(eqn.Substring(pos, i - (pos))));
                        pos = i;
                        break;
                    default:
                        break;
                }
                return ret;
            }

            /// <summary>
            /// Position
            /// </summary>
            public int Position
            {
                get { return pos; }
            }
        }
        // stores the commands
        private System.Collections.ArrayList list = new System.Collections.ArrayList();
        private System.Collections.SortedList xlist = new System.Collections.SortedList();
        private System.Collections.SortedList ylist = new System.Collections.SortedList();

        /// <summary>
        /// Creates a new PathCommandSequence from a IHTMLElement
        /// </summary>
        /// <param name="peer">the IHTMLElement</param>
        /// <remarks>IHTMLElement should be a shape or shapetype</remarks>
        public PathCommandSequence(Interop.IHTMLElement peer)
        {
            IVgPath temp = ((IVgPath)GetAttribute(peer, "path"));
            Parse(temp.v);
        }

        /// <summary>
        /// Creates a new PathCommandSequece from a string
        /// </summary>
        /// <param name="path">the Path-String</param>
        public PathCommandSequence(string path)
        {
            Parse(path);
        }

        /// <summary>
        /// Parses the Path-String
        /// </summary>
        /// <param name="path">the Path-String</param>
        private void Parse(string path)
        {
            PathCommandTokenizer tok = new PathCommandTokenizer(path);
            PathCommand.PathCommandType cmd = PathCommand.PathCommandType.Nothing;
            PathCommandToken[] tmp;
            PathCommandToken t;

            while ((t = tok.GetNextToken()) != null)
            {

                switch (t.Type)
                {
                    case PathCommandSequenceToken.CommandMoveTo:
                        cmd = PathCommand.PathCommandType.MoveTo;
                        tmp = new PathCommandToken[] { tok.GetNextToken(), tok.GetNextToken() };
                        list.Add(new PathCommand(cmd, tmp));
                        break;
                    case PathCommandSequenceToken.CommandLineTo:
                        cmd = PathCommand.PathCommandType.LineTo;
                        tmp = new PathCommandToken[] { tok.GetNextToken(), tok.GetNextToken() };
                        list.Add(new PathCommand(cmd, tmp));
                        break;
                    case PathCommandSequenceToken.Number:
                    case PathCommandSequenceToken.Formula:
                        tmp = new PathCommandToken[] { t, tok.GetNextToken() };
                        list.Add(new PathCommand(cmd, tmp));
                        break;
                    case PathCommandSequenceToken.CommandClose:
                        cmd = PathCommand.PathCommandType.CloseLine;
                        list.Add(new PathCommand(cmd));
                        break;
                    case PathCommandSequenceToken.CommandEnd:
                        cmd = PathCommand.PathCommandType.EndLine;
                        list.Add(new PathCommand(cmd));
                        break;
                }
            }
        }

        // Helperfunction to get an Attribute from the peer-element
        private object GetAttribute(Interop.IHTMLElement peer, string attribute)
        {
            object local2;
            try
            {
                object[] locals = new object[1];
                locals[0] = null;
                peer.GetAttribute(attribute, 0, locals);
                object local1 = locals[0];
                if (local1 is DBNull)
                {
                    local1 = null;
                }
                local2 = local1;
            }
            catch
            {
                local2 = null;
            }
            return local2;
        }


        /// <summary>
        /// Returns the PathCommand at position index
        /// </summary>
        public PathCommand this[int index]
        {
            get { return (PathCommand)list[index]; }
        }

        /// <summary>
        /// The Length of the Pathcommandlist
        /// </summary>
        public int Length
        {
            get { return list.Count; }
        }

        /// <summary>
        /// Returns the surounding rect of the path
        /// </summary>
        public System.Drawing.Rectangle Rect
        {
            get
            {
                System.Drawing.Rectangle ret = new System.Drawing.Rectangle(Left, Top, Right, Bottom);
                return ret;
            }
        }

        /// <summary>
        /// The most left point
        /// </summary>
        public int Left
        {
            get
            {
                int left = ((PathCommand)list[0]).Param[0].Value;
                for (int i = 1; i < list.Count; i++)
                {
                    PathCommand cmd = (PathCommand)list[i];
                    if ((cmd.Command == PathCommand.PathCommandType.LineTo
                        || cmd.Command == PathCommand.PathCommandType.LineTo) && left > cmd.Param[0].Value)
                    {
                        left = cmd.Param[0].Value;
                    }
                }
                return left;
            }
        }

        /// <summary>
        /// Right segment
        /// </summary>
        /// <seealso cref="Top"/>
        public int Right
        {
            get
            {
                int right = ((PathCommand)list[0]).Param[0].Value;
                for (int i = 1; i < list.Count; i++)
                {
                    PathCommand cmd = (PathCommand)list[i];
                    if ((cmd.Command == PathCommand.PathCommandType.LineTo
                        || cmd.Command == PathCommand.PathCommandType.LineTo) && right < cmd.Param[0].Value)
                    {
                        right = cmd.Param[0].Value;
                    }
                }
                return right;
            }
        }

        /// <summary>
        /// Top segment
        /// </summary>
        /// <seealso cref="Right"/>
        public int Top
        {
            get
            {
                int top = ((PathCommand)list[0]).Param[1].Value;
                for (int i = 1; i < list.Count; i++)
                {
                    PathCommand cmd = (PathCommand)list[i];
                    if ((cmd.Command == PathCommand.PathCommandType.LineTo
                        || cmd.Command == PathCommand.PathCommandType.LineTo) && top > cmd.Param[1].Value)
                    {
                        top = cmd.Param[1].Value;
                    }
                }
                return top;
            }
        }

        /// <summary>
        /// Bottom segment
        /// </summary>
        /// <seealso cref="Top"/>
        public int Bottom
        {
            get
            {
                int bottom = ((PathCommand)list[0]).Param[1].Value;
                for (int i = 1; i < list.Count; i++)
                {
                    PathCommand cmd = (PathCommand)list[i];
                    if ((cmd.Command == PathCommand.PathCommandType.LineTo
                        || cmd.Command == PathCommand.PathCommandType.LineTo) && bottom < cmd.Param[1].Value)
                    {
                        bottom = cmd.Param[1].Value;
                    }
                }
                return bottom;
            }
        }

        /// <summary>
        /// Path as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder ret = new System.Text.StringBuilder();
            PathCommand.PathCommandType lastCommand = PathCommand.PathCommandType.Nothing;
            for (int i = 0; i < list.Count; i++)
            {
                int pcount = 0;
                switch (this[i].Command)
                {
                    case PathCommand.PathCommandType.Nothing:
                        break;
                    case PathCommand.PathCommandType.MoveTo:
                        if (lastCommand != this[i].Command)
                        {
                            ret.Append("m");
                        }
                        pcount = 2;
                        break;
                    case PathCommand.PathCommandType.LineTo:
                        if (lastCommand != this[i].Command)
                        {
                            ret.Append("l");
                        }
                        pcount = 2;
                        break;
                    case PathCommand.PathCommandType.CloseLine:
                        ret.Append("x");
                        break;
                    case PathCommand.PathCommandType.EndLine:
                        ret.Append("e");
                        break;
                }
                // only write needed chars
                for (int j = 0; j < pcount; j++)
                {
                    switch (this[i].Param[j].Type)
                    {
                        case PathCommandSequenceToken.Formula:
                            ret.Append(this[i].Param[j].ToString());
                            break;
                        case PathCommandSequenceToken.Number:
                            if ((lastCommand == this[i].Command))
                            {
                                ret.Append(",");
                            }
                            else if (j == 1)
                            {
                                ret.Append(",");
                            }
                            if ((this[i].Param[j].Value) != 0)
                            {
                                ret.Append(this[i].Param[j].ToString());
                            }
                            break;
                        default:
                            // every other should not appear ;-)
                            // may throw an exeption here ?!
                            break;
                    }

                }

                lastCommand = this[i].Command;
            }
            return ret.ToString();
        }

    }
}
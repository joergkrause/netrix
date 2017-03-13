using System;
using GuruComponents.Netrix.VmlDesigner.Elements;
#pragma warning disable 1591
namespace GuruComponents.Netrix.VmlDesigner.Util {
    

    /// <summary>
    /// This ia a EqnNode for the AST of the Equiptions used in the shape and shapetype element.
    /// </summary>
    public class EqnNode {

        public class EqnParserException : Exception {
            public EqnParserException(string str):base(str){;}
        }

        /// <summary>
        /// Define some possible NodeTypes
        /// </summary>
        public enum EqnNodeType {
            Undefined,
            Constant ,
            Variable,
            Adjustment,
            Equation,
            Expression
        }

        /// <summary>
        /// Define the possible Operations
        /// </summary>
        public enum Operation {
            Val,
            Sum,
            Product,
            Mid,
            Abs,
            Min,
            Max,
            If,
            Mod,
            Atan2,
            Sin,
            Cos,
            Cosatan2,
            Sinatan2,
            Sqrt,
            Sumangle,
            Ellipse,
            Tan,
            Undefined
        }

        private EqnNodeType type = EqnNodeType.Undefined ;
        private Operation op = Operation.Undefined ;
        // the owner of this EqnNode
        private CommonShapeElement owner ;

        private EqnNode[] param = null ;

        private double val = 0 ;
        
        /// <summary>
        /// Creates a Undefined EqnNode
        /// </summary>
        /// <param name="Owner">the Owner-Shape</param>
        public EqnNode(CommonShapeElement Owner){
            owner = Owner ;
            type = EqnNodeType.Undefined;
        }

        /// <summary>
        /// Creates a new Constant EqnNode with the value constant.
        /// </summary>
        /// <param name="Owner">the Owner-Shape</param>
        /// <param name="constant">the value for this EqnNode</param>
        public EqnNode(CommonShapeElement Owner,double constant){
            owner = Owner ;
            type=EqnNodeType.Constant ;
            val=constant;
        }

        /// <summary>
        /// Creates a new Variable EqnNode with the number of the variable stored in the value field.
        /// </summary>
        /// <param name="Owner">the Owner-Shape</param>
        /// <param name="variable">the Variablenumber</param>
        public EqnNode(CommonShapeElement Owner,int variable){
            owner = Owner ;
            type=EqnNodeType.Variable ;
            val=variable;
        }

        /// <summary>
        /// Creates a new EqnNode from a string containing a equiption
        /// </summary>
        /// <param name="Owner">the Owner-Shape</param>
        /// <param name="eqn">the equation</param>
        public EqnNode(CommonShapeElement Owner,string eqn){
            owner = Owner ;
            Parse(eqn);
        }

        /// <summary>
        /// Parses the Eqn_string
        /// </summary>
        /// <param name="eqn">the Eqn</param>
        /// <returns>returns the unparsed part of the Eqn-String</returns>
        /// <remarks>This function is used recursive until the whole string is parsed</remarks>
        public string Parse(string eqn){
            int len = 0 ;
            string ret = null;
            // check for last token
            if (eqn==null){
                throw new EqnParserException("Parse an eqn of null not possible!");
            }
            
            // split the tokens
            string[] token = eqn.Split(new char[]{' '},2);
           
            // switch operator
            type = EqnNodeType.Expression ;
            switch (token[0].ToLower()){
                case "val":
                    op = Operation.Val;
                    len = 1 ;
                    break;
                case "sum":
                    op = Operation.Sum;
                    len = 3 ;
                    break;
                case "product":
                    op = Operation.Product;
                    len = 3 ;
                    break;
                case "mid":
                    op = Operation.Mid;
                    len = 2 ;
                    break;
                case "abs":
                    op = Operation.Abs;
                    len = 2 ;
                    break;
                case "min":
                    op = Operation.Min;
                    len = 2 ;
                    break;
                case "max":
                    op = Operation.Max;
                    len = 2 ;
                    break;
                case "if":
                    op = Operation.If;
                    len = 3 ;
                    break;
                case "mod":
                    op = Operation.Mod;
                    len = 3 ;
                    break;
                case "atan2":
                    op = Operation.Atan2;
                    len = 2 ;
                    break;
                case "sin":
                    op = Operation.Sin;
                    len = 2 ;
                    break;
                case "cos":
                    op = Operation.Cos;
                    len = 2 ;
                    break;
                case "cosatan2":
                    op = Operation.Cosatan2;
                    len = 3 ;
                    break;
                case "sinatan2":
                    op = Operation.Sinatan2;
                    len = 3 ;
                    break;
                case "sqrt":
                    op = Operation.Sqrt;
                    len = 1 ;
                    break;
                case "sumangle":
                    op = Operation.Sumangle;
                    len = 3 ;
                    break;
                case "ellipse":
                    op = Operation.Ellipse;
                    len = 3 ;
                    break;
                case "tan":
                    op = Operation.Tan;
                    len = 2 ;
                    break;
                default:
                    if (token[0].StartsWith("@")){
                        type = EqnNodeType.Equation ;
                        val = Convert.ToInt32(token[0].Trim('@'));
                    }else if (token[0].StartsWith("#")){
                        type = EqnNodeType.Adjustment ;
                        val = Convert.ToInt32(token[0].Trim('#'));
                    }else{
                        type = EqnNodeType.Constant ;
                        val = Convert.ToDouble(token[0]);
                    }
                    break;
            }
            if (token.Length>1){
                ret = token[1].Trim();
            }
            if (len>0){
                param = new EqnNode[len];
                for (int i=0;i<len;i++){
                    param[i]=new EqnNode(this.owner);
                    ret = param[i].Parse(ret);
                }
            }
            return ret ;
        }


        /// <summary>
        /// Calulates the value of the eqn
        /// </summary>
        /// <returns>the result of the calulation</returns>
        private double Calulate(){
            double ret = val ;
            switch (op){
                case Operation.Val:
                    ret = val ;
                    break;
                case Operation.Sum:
                    ret = param[0].Value + param[1].Value - param[2].Value;
                    break;
                case Operation.Product:
                    ret = param[0].Value * param[1].Value / param[2].Value;
                    break;
                case Operation.Mid:
                    ret = (param[0].Value + param[1].Value) / 2 ;
                    break;
                case Operation.Abs:
                    ret = Math.Abs(param[0].Value);
                    break;
                case Operation.Min:
                    ret = (param[0].Value < param[1].Value) ? param[0].Value : param[1].Value ;
                    break;
                case Operation.Max:
                    ret = (param[0].Value > param[1].Value) ? param[0].Value : param[1].Value ;
                    break;
                case Operation.If:
                    ret = (param[0].Value > 0) ? param[1].Value : param[2].Value ;
                    break;
                case Operation.Mod :
                    ret = Math.Sqrt(Math.Pow(param[0].Value,2)+Math.Pow(param[1].Value,2)+Math.Pow(param[2].Value,2));
                    break ;
                case Operation.Atan2 :
                    ret = Math.Atan2(param[1].Value,param[0].Value);
                    break ;
                case Operation.Sin :
                    ret = Math.Sin(param[1].Value)*param[0].Value;
                    break ;
                case Operation.Cos :
                    ret = Math.Cos(param[1].Value)*param[0].Value;
                    break ;
                case Operation.Cosatan2 :
                    ret = Math.Cos(Math.Atan2(param[2].Value,param[1].Value))*param[0].Value;
                    break ;
                case Operation.Sinatan2 :
                    ret = Math.Sin(Math.Atan2(param[2].Value,param[1].Value))*param[0].Value;
                    break ;
                case Operation.Sqrt :
                    Math.Sqrt(param[0].Value);
                    break ;
                case Operation.Sumangle :
                    ret = param[0].Value+param[1].Value*Math.Pow(2,16)+param[2].Value*Math.Pow(2,16);
                    break ;
                case Operation.Ellipse :
                    ret = param[2].Value*Math.Sqrt(1-Math.Pow((param[0].Value/param[1].Value),2));
                    break ;
                case Operation.Tan :
                    ret = param[0].Value*Math.Tan(param[1].Value);
                    break ;
                default:
                    break;
            }
            return ret ;
        }

        /// <summary>
        /// The EqnNodeType 
        /// </summary>
        public EqnNodeType Type{
            get { return type ;}
        }   

        /// <summary>
        /// The EqnNodeOperation parameter
        /// </summary>
        public Operation Op{
            get { return op ;}
        }


        /// <summary>
        /// The EqnNodeValue
        /// </summary>
        public double Value{
            get{ 
                double ret = 0 ;
                switch (this.type){
                    // this is a constant , return the value
                    case EqnNodeType.Constant:
                        ret = val ;
                        break;
                    // this is an expression, return the result
                    case EqnNodeType.Expression:
                        ret = Calulate() ;
                        break;
                    // this is an adjuster, return the curent value
                    case EqnNodeType.Adjustment:
                        string[] token=owner.Adj.Value.Split(',');
                        ret=(token.Length>=val) ? Convert.ToInt32(token[(int)val]) : 0 ;
                        break;
                    // this is an other equation return the result of it
                    case EqnNodeType.Equation:
                        FElement f = (FElement)((FormulasElement)owner.GetChild("formulas")).GetChild("f",(int)val);
                        EqnNode e = new EqnNode(owner,((string)f.GetAttribute("eqn"))) ;
                        break;
                    default:
                        // try to return the value
                        ret = val ;
                        break;
                }
                return ret ;
            }
        }
    
        //        static void Main(){
        //            EqnNode n = new EqnNode("sum 4 product 5 6 1 2");
        //            double v = n.Value ;
        //            PathCommandSequence s = new PathCommandSequence("m 0,0 l 100,0,100,100 e x ");
        //            PathCommandSequence s0 = new PathCommandSequence("m 1,1 l 100,1,100,100 e x ");
        //            PathCommandSequence s1 = new PathCommandSequence("m10800,l@0@2@1@2@1@1@2@1@2@0,,10800@2@3@2@4@1@4@1@5@0@5,10800,21600@3@5@4@5@4@4@5@4@5@3,21600,10800@5@0@5@1@4@1@4@2@3@2xe");
        //            Console.WriteLine("*******************************************");
        //            Console.WriteLine(s);
        //            Console.WriteLine(s0);
        //            Console.WriteLine(s1);
        //            Console.WriteLine();
    }
}


//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2011. 04. 24. 13:56:11
//
// The template file is BlankSyntaxNodeVisitor.tt 
//

// disable warnings about missing XML comments
#pragma warning disable 1591 

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// Static CSharpSymbol definitions
  /// </summary>
  // ================================================================================================
  public partial struct CSharpSymbol
  {
    public static CSharpSymbol Abstract = new CSharpSymbol(6);
    public static CSharpSymbol As = new CSharpSymbol(7);
    public static CSharpSymbol Base = new CSharpSymbol(8);
    public static CSharpSymbol Bool = new CSharpSymbol(9);
    public static CSharpSymbol Break = new CSharpSymbol(10);
    public static CSharpSymbol Byte = new CSharpSymbol(11);
    public static CSharpSymbol Case = new CSharpSymbol(12);
    public static CSharpSymbol Catch = new CSharpSymbol(13);
    public static CSharpSymbol Char = new CSharpSymbol(14);
    public static CSharpSymbol Checked = new CSharpSymbol(15);
    public static CSharpSymbol Class = new CSharpSymbol(16);
    public static CSharpSymbol Const = new CSharpSymbol(17);
    public static CSharpSymbol Continue = new CSharpSymbol(18);
    public static CSharpSymbol Decimal = new CSharpSymbol(19);
    public static CSharpSymbol Default = new CSharpSymbol(20);
    public static CSharpSymbol Delegate = new CSharpSymbol(21);
    public static CSharpSymbol Do = new CSharpSymbol(22);
    public static CSharpSymbol Double = new CSharpSymbol(23);
    public static CSharpSymbol Else = new CSharpSymbol(24);
    public static CSharpSymbol Enum = new CSharpSymbol(25);
    public static CSharpSymbol Event = new CSharpSymbol(26);
    public static CSharpSymbol Explicit = new CSharpSymbol(27);
    public static CSharpSymbol Extern = new CSharpSymbol(28);
    public static CSharpSymbol False = new CSharpSymbol(29);
    public static CSharpSymbol Finally = new CSharpSymbol(30);
    public static CSharpSymbol Fixed = new CSharpSymbol(31);
    public static CSharpSymbol Float = new CSharpSymbol(32);
    public static CSharpSymbol For = new CSharpSymbol(33);
    public static CSharpSymbol Foreach = new CSharpSymbol(34);
    public static CSharpSymbol Goto = new CSharpSymbol(35);
    public static CSharpSymbol If = new CSharpSymbol(36);
    public static CSharpSymbol Implicit = new CSharpSymbol(37);
    public static CSharpSymbol In = new CSharpSymbol(38);
    public static CSharpSymbol Int = new CSharpSymbol(39);
    public static CSharpSymbol Interface = new CSharpSymbol(40);
    public static CSharpSymbol Internal = new CSharpSymbol(41);
    public static CSharpSymbol Is = new CSharpSymbol(42);
    public static CSharpSymbol Lock = new CSharpSymbol(43);
    public static CSharpSymbol Long = new CSharpSymbol(44);
    public static CSharpSymbol Namespace = new CSharpSymbol(45);
    public static CSharpSymbol New = new CSharpSymbol(46);
    public static CSharpSymbol Null = new CSharpSymbol(47);
    public static CSharpSymbol Object = new CSharpSymbol(48);
    public static CSharpSymbol Operator = new CSharpSymbol(49);
    public static CSharpSymbol Out = new CSharpSymbol(50);
    public static CSharpSymbol Override = new CSharpSymbol(51);
    public static CSharpSymbol Params = new CSharpSymbol(52);
    public static CSharpSymbol Private = new CSharpSymbol(53);
    public static CSharpSymbol Protected = new CSharpSymbol(54);
    public static CSharpSymbol Public = new CSharpSymbol(55);
    public static CSharpSymbol Readonly = new CSharpSymbol(56);
    public static CSharpSymbol Ref = new CSharpSymbol(57);
    public static CSharpSymbol Return = new CSharpSymbol(58);
    public static CSharpSymbol Sbyte = new CSharpSymbol(59);
    public static CSharpSymbol Sealed = new CSharpSymbol(60);
    public static CSharpSymbol Short = new CSharpSymbol(61);
    public static CSharpSymbol Sizeof = new CSharpSymbol(62);
    public static CSharpSymbol Stackalloc = new CSharpSymbol(63);
    public static CSharpSymbol Static = new CSharpSymbol(64);
    public static CSharpSymbol String = new CSharpSymbol(65);
    public static CSharpSymbol Struct = new CSharpSymbol(66);
    public static CSharpSymbol Switch = new CSharpSymbol(67);
    public static CSharpSymbol This = new CSharpSymbol(68);
    public static CSharpSymbol Throw = new CSharpSymbol(69);
    public static CSharpSymbol True = new CSharpSymbol(70);
    public static CSharpSymbol Try = new CSharpSymbol(71);
    public static CSharpSymbol Typeof = new CSharpSymbol(72);
    public static CSharpSymbol Uint = new CSharpSymbol(73);
    public static CSharpSymbol Ulong = new CSharpSymbol(74);
    public static CSharpSymbol Unchecked = new CSharpSymbol(75);
    public static CSharpSymbol Unsafe = new CSharpSymbol(76);
    public static CSharpSymbol Ushort = new CSharpSymbol(77);
    public static CSharpSymbol Using = new CSharpSymbol(78);
    public static CSharpSymbol Virtual = new CSharpSymbol(79);
    public static CSharpSymbol Void = new CSharpSymbol(80);
    public static CSharpSymbol Volatile = new CSharpSymbol(81);
    public static CSharpSymbol While = new CSharpSymbol(82);
    public static CSharpSymbol And = new CSharpSymbol(83);
    public static CSharpSymbol AndAssignment = new CSharpSymbol(84);
    public static CSharpSymbol Assigment = new CSharpSymbol(85);
    public static CSharpSymbol Colon = new CSharpSymbol(86);
    public static CSharpSymbol Comma = new CSharpSymbol(87);
    public static CSharpSymbol Decrement = new CSharpSymbol(88);
    public static CSharpSymbol DivisionAssignment = new CSharpSymbol(89);
    public static CSharpSymbol Dot = new CSharpSymbol(90);
    public static CSharpSymbol DoubleColon = new CSharpSymbol(91);
    public static CSharpSymbol Equal = new CSharpSymbol(92);
    public static CSharpSymbol GreaterThan = new CSharpSymbol(93);
    public static CSharpSymbol GreaterThanOrEqual = new CSharpSymbol(94);
    public static CSharpSymbol Increment = new CSharpSymbol(95);
    public static CSharpSymbol LeftBrace = new CSharpSymbol(96);
    public static CSharpSymbol LeftBracket = new CSharpSymbol(97);
    public static CSharpSymbol LeftParenthesis = new CSharpSymbol(98);
    public static CSharpSymbol LeftShiftAssignment = new CSharpSymbol(99);
    public static CSharpSymbol LessThan = new CSharpSymbol(100);
    public static CSharpSymbol LeftShift = new CSharpSymbol(101);
    public static CSharpSymbol Minus = new CSharpSymbol(102);
    public static CSharpSymbol SubtractAssignment = new CSharpSymbol(103);
    public static CSharpSymbol ModuloAssignment = new CSharpSymbol(104);
    public static CSharpSymbol NotEqual = new CSharpSymbol(105);
    public static CSharpSymbol Not = new CSharpSymbol(106);
    public static CSharpSymbol OrAssignment = new CSharpSymbol(107);
    public static CSharpSymbol Add = new CSharpSymbol(108);
    public static CSharpSymbol AdditionAssignment = new CSharpSymbol(109);
    public static CSharpSymbol QuestionMark = new CSharpSymbol(110);
    public static CSharpSymbol RightBrace = new CSharpSymbol(111);
    public static CSharpSymbol RightBracket = new CSharpSymbol(112);
    public static CSharpSymbol RightParenthesis = new CSharpSymbol(113);
    public static CSharpSymbol Semicolon = new CSharpSymbol(114);
    public static CSharpSymbol Tilde = new CSharpSymbol(115);
    public static CSharpSymbol Times = new CSharpSymbol(116);
    public static CSharpSymbol MultiplicationAssignment = new CSharpSymbol(117);
    public static CSharpSymbol XorAssignment = new CSharpSymbol(118);
    public static CSharpSymbol Lambda = new CSharpSymbol(119);
    public static CSharpSymbol NullCoalescing = new CSharpSymbol(120);
    public static CSharpSymbol ConditionalOr = new CSharpSymbol(121);
    public static CSharpSymbol ConditionalAnd = new CSharpSymbol(122);
    public static CSharpSymbol Or = new CSharpSymbol(123);
    public static CSharpSymbol Xor = new CSharpSymbol(124);
    public static CSharpSymbol LessThanOrEqual = new CSharpSymbol(125);
    public static CSharpSymbol Division = new CSharpSymbol(126);
    public static CSharpSymbol Modulo = new CSharpSymbol(127);
    public static CSharpSymbol CMemberAccess = new CSharpSymbol(128);	
  }
}

#pragma warning restore 1591

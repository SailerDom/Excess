//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from R.g4 by ANTLR 4.5

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5")]
[System.CLSCompliant(false)]
public partial class RLexer : Lexer {
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, T__28=29, T__29=30, T__30=31, 
		T__31=32, T__32=33, T__33=34, T__34=35, T__35=36, T__36=37, T__37=38, 
		T__38=39, T__39=40, T__40=41, T__41=42, T__42=43, T__43=44, T__44=45, 
		T__45=46, T__46=47, T__47=48, T__48=49, T__49=50, T__50=51, T__51=52, 
		T__52=53, T__53=54, HEX=55, INT=56, FLOAT=57, COMPLEX=58, STRING=59, ID=60, 
		USER_OP=61, NL=62, WS=63;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "T__20", "T__21", "T__22", "T__23", "T__24", 
		"T__25", "T__26", "T__27", "T__28", "T__29", "T__30", "T__31", "T__32", 
		"T__33", "T__34", "T__35", "T__36", "T__37", "T__38", "T__39", "T__40", 
		"T__41", "T__42", "T__43", "T__44", "T__45", "T__46", "T__47", "T__48", 
		"T__49", "T__50", "T__51", "T__52", "T__53", "HEX", "INT", "HEXDIGIT", 
		"FLOAT", "DIGIT", "EXP", "COMPLEX", "STRING", "ESC", "UNICODE_ESCAPE", 
		"OCTAL_ESCAPE", "HEX_ESCAPE", "ID", "LETTER", "USER_OP", "COMMENT", "NL", 
		"WS"
	};


	public RLexer(ICharStream input)
		: base(input)
	{
		Interpreter = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "';'", "'<-'", "'='", "'<<-'", "'[['", "']'", "'['", "'::'", "':::'", 
		"'$'", "'@'", "'^'", "'-'", "'+'", "':'", "'*'", "'/'", "'>'", "'>='", 
		"'<'", "'<='", "'=='", "'!='", "'!'", "'&'", "'&&'", "'|'", "'||'", "'~'", 
		"'->'", "'->>'", "':='", "'function'", "'('", "')'", "'{'", "'}'", "'if'", 
		"'else'", "'for'", "'in'", "'while'", "'repeat'", "'?'", "'next'", "'break'", 
		"'NULL'", "'NA'", "'Inf'", "'NaN'", "'TRUE'", "'FALSE'", "','", "'...'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, "HEX", "INT", "FLOAT", "COMPLEX", 
		"STRING", "ID", "USER_OP", "NL", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "R.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public static readonly string _serializedATN =
		"\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x2\x41\x20A\b\x1\x4"+
		"\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b"+
		"\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4"+
		"\x10\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15"+
		"\t\x15\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A"+
		"\x4\x1B\t\x1B\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x4 "+
		"\t \x4!\t!\x4\"\t\"\x4#\t#\x4$\t$\x4%\t%\x4&\t&\x4\'\t\'\x4(\t(\x4)\t"+
		")\x4*\t*\x4+\t+\x4,\t,\x4-\t-\x4.\t.\x4/\t/\x4\x30\t\x30\x4\x31\t\x31"+
		"\x4\x32\t\x32\x4\x33\t\x33\x4\x34\t\x34\x4\x35\t\x35\x4\x36\t\x36\x4\x37"+
		"\t\x37\x4\x38\t\x38\x4\x39\t\x39\x4:\t:\x4;\t;\x4<\t<\x4=\t=\x4>\t>\x4"+
		"?\t?\x4@\t@\x4\x41\t\x41\x4\x42\t\x42\x4\x43\t\x43\x4\x44\t\x44\x4\x45"+
		"\t\x45\x4\x46\t\x46\x4G\tG\x4H\tH\x4I\tI\x3\x2\x3\x2\x3\x3\x3\x3\x3\x3"+
		"\x3\x4\x3\x4\x3\x5\x3\x5\x3\x5\x3\x5\x3\x6\x3\x6\x3\x6\x3\a\x3\a\x3\b"+
		"\x3\b\x3\t\x3\t\x3\t\x3\n\x3\n\x3\n\x3\n\x3\v\x3\v\x3\f\x3\f\x3\r\x3\r"+
		"\x3\xE\x3\xE\x3\xF\x3\xF\x3\x10\x3\x10\x3\x11\x3\x11\x3\x12\x3\x12\x3"+
		"\x13\x3\x13\x3\x14\x3\x14\x3\x14\x3\x15\x3\x15\x3\x16\x3\x16\x3\x16\x3"+
		"\x17\x3\x17\x3\x17\x3\x18\x3\x18\x3\x18\x3\x19\x3\x19\x3\x1A\x3\x1A\x3"+
		"\x1B\x3\x1B\x3\x1B\x3\x1C\x3\x1C\x3\x1D\x3\x1D\x3\x1D\x3\x1E\x3\x1E\x3"+
		"\x1F\x3\x1F\x3\x1F\x3 \x3 \x3 \x3 \x3!\x3!\x3!\x3\"\x3\"\x3\"\x3\"\x3"+
		"\"\x3\"\x3\"\x3\"\x3\"\x3#\x3#\x3$\x3$\x3%\x3%\x3&\x3&\x3\'\x3\'\x3\'"+
		"\x3(\x3(\x3(\x3(\x3(\x3)\x3)\x3)\x3)\x3*\x3*\x3*\x3+\x3+\x3+\x3+\x3+\x3"+
		"+\x3,\x3,\x3,\x3,\x3,\x3,\x3,\x3-\x3-\x3.\x3.\x3.\x3.\x3.\x3/\x3/\x3/"+
		"\x3/\x3/\x3/\x3\x30\x3\x30\x3\x30\x3\x30\x3\x30\x3\x31\x3\x31\x3\x31\x3"+
		"\x32\x3\x32\x3\x32\x3\x32\x3\x33\x3\x33\x3\x33\x3\x33\x3\x34\x3\x34\x3"+
		"\x34\x3\x34\x3\x34\x3\x35\x3\x35\x3\x35\x3\x35\x3\x35\x3\x35\x3\x36\x3"+
		"\x36\x3\x37\x3\x37\x3\x37\x3\x37\x3\x38\x3\x38\x3\x38\x6\x38\x143\n\x38"+
		"\r\x38\xE\x38\x144\x3\x38\x5\x38\x148\n\x38\x3\x39\x6\x39\x14B\n\x39\r"+
		"\x39\xE\x39\x14C\x3\x39\x5\x39\x150\n\x39\x3:\x3:\x3;\x6;\x155\n;\r;\xE"+
		";\x156\x3;\x3;\a;\x15B\n;\f;\xE;\x15E\v;\x3;\x5;\x161\n;\x3;\x5;\x164"+
		"\n;\x3;\x6;\x167\n;\r;\xE;\x168\x3;\x5;\x16C\n;\x3;\x5;\x16F\n;\x3;\x3"+
		";\x6;\x173\n;\r;\xE;\x174\x3;\x5;\x178\n;\x3;\x5;\x17B\n;\x5;\x17D\n;"+
		"\x3<\x3<\x3=\x3=\x5=\x183\n=\x3=\x3=\x3>\x3>\x3>\x3>\x3>\x3>\x5>\x18D"+
		"\n>\x3?\x3?\x3?\a?\x192\n?\f?\xE?\x195\v?\x3?\x3?\x3?\x3?\a?\x19B\n?\f"+
		"?\xE?\x19E\v?\x3?\x5?\x1A1\n?\x3@\x3@\x5@\x1A5\n@\x3@\x3@\x3@\x5@\x1AA"+
		"\n@\x3\x41\x3\x41\x3\x41\x3\x41\x3\x41\x3\x41\x3\x41\x3\x41\x3\x41\x3"+
		"\x41\x3\x41\x3\x41\x3\x41\x3\x41\x3\x41\x3\x41\x5\x41\x1BC\n\x41\x3\x42"+
		"\x3\x42\x3\x42\x3\x42\x3\x42\x3\x42\x3\x42\x3\x42\x3\x42\x5\x42\x1C7\n"+
		"\x42\x3\x43\x3\x43\x3\x43\x5\x43\x1CC\n\x43\x3\x44\x3\x44\x3\x44\x5\x44"+
		"\x1D1\n\x44\x3\x44\x3\x44\x3\x44\a\x44\x1D6\n\x44\f\x44\xE\x44\x1D9\v"+
		"\x44\x3\x44\x3\x44\x3\x44\x3\x44\a\x44\x1DF\n\x44\f\x44\xE\x44\x1E2\v"+
		"\x44\x5\x44\x1E4\n\x44\x3\x45\x3\x45\x3\x46\x3\x46\a\x46\x1EA\n\x46\f"+
		"\x46\xE\x46\x1ED\v\x46\x3\x46\x3\x46\x3G\x3G\aG\x1F3\nG\fG\xEG\x1F6\v"+
		"G\x3G\x5G\x1F9\nG\x3G\x3G\x3G\x3G\x3H\x5H\x200\nH\x3H\x3H\x3I\x6I\x205"+
		"\nI\rI\xEI\x206\x3I\x3I\x6\x193\x19C\x1EB\x1F4\x2J\x3\x3\x5\x4\a\x5\t"+
		"\x6\v\a\r\b\xF\t\x11\n\x13\v\x15\f\x17\r\x19\xE\x1B\xF\x1D\x10\x1F\x11"+
		"!\x12#\x13%\x14\'\x15)\x16+\x17-\x18/\x19\x31\x1A\x33\x1B\x35\x1C\x37"+
		"\x1D\x39\x1E;\x1F= ?!\x41\"\x43#\x45$G%I&K\'M(O)Q*S+U,W-Y.[/]\x30_\x31"+
		"\x61\x32\x63\x33\x65\x34g\x35i\x36k\x37m\x38o\x39q:s\x2u;w\x2y\x2{<}="+
		"\x7F\x2\x81\x2\x83\x2\x85\x2\x87>\x89\x2\x8B?\x8D\x2\x8F@\x91\x41\x3\x2"+
		"\xF\x4\x2ZZzz\x4\x2NNnn\x5\x2\x32;\x43H\x63h\x4\x2GGgg\x4\x2--//\x4\x2"+
		"$$^^\x4\x2))^^\n\x2$$))\x63\x64hhppttvvxx\x3\x2\x32\x35\x3\x2\x32\x39"+
		"\x4\x2\x30\x30\x61\x61\x4\x2\x43\\\x63|\x4\x2\v\v\"\"\x22C\x2\x3\x3\x2"+
		"\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2"+
		"\x2\x2\x2\r\x3\x2\x2\x2\x2\xF\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2\x2\x13\x3"+
		"\x2\x2\x2\x2\x15\x3\x2\x2\x2\x2\x17\x3\x2\x2\x2\x2\x19\x3\x2\x2\x2\x2"+
		"\x1B\x3\x2\x2\x2\x2\x1D\x3\x2\x2\x2\x2\x1F\x3\x2\x2\x2\x2!\x3\x2\x2\x2"+
		"\x2#\x3\x2\x2\x2\x2%\x3\x2\x2\x2\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2\x2\x2+"+
		"\x3\x2\x2\x2\x2-\x3\x2\x2\x2\x2/\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2\x2\x33"+
		"\x3\x2\x2\x2\x2\x35\x3\x2\x2\x2\x2\x37\x3\x2\x2\x2\x2\x39\x3\x2\x2\x2"+
		"\x2;\x3\x2\x2\x2\x2=\x3\x2\x2\x2\x2?\x3\x2\x2\x2\x2\x41\x3\x2\x2\x2\x2"+
		"\x43\x3\x2\x2\x2\x2\x45\x3\x2\x2\x2\x2G\x3\x2\x2\x2\x2I\x3\x2\x2\x2\x2"+
		"K\x3\x2\x2\x2\x2M\x3\x2\x2\x2\x2O\x3\x2\x2\x2\x2Q\x3\x2\x2\x2\x2S\x3\x2"+
		"\x2\x2\x2U\x3\x2\x2\x2\x2W\x3\x2\x2\x2\x2Y\x3\x2\x2\x2\x2[\x3\x2\x2\x2"+
		"\x2]\x3\x2\x2\x2\x2_\x3\x2\x2\x2\x2\x61\x3\x2\x2\x2\x2\x63\x3\x2\x2\x2"+
		"\x2\x65\x3\x2\x2\x2\x2g\x3\x2\x2\x2\x2i\x3\x2\x2\x2\x2k\x3\x2\x2\x2\x2"+
		"m\x3\x2\x2\x2\x2o\x3\x2\x2\x2\x2q\x3\x2\x2\x2\x2u\x3\x2\x2\x2\x2{\x3\x2"+
		"\x2\x2\x2}\x3\x2\x2\x2\x2\x87\x3\x2\x2\x2\x2\x8B\x3\x2\x2\x2\x2\x8D\x3"+
		"\x2\x2\x2\x2\x8F\x3\x2\x2\x2\x2\x91\x3\x2\x2\x2\x3\x93\x3\x2\x2\x2\x5"+
		"\x95\x3\x2\x2\x2\a\x98\x3\x2\x2\x2\t\x9A\x3\x2\x2\x2\v\x9E\x3\x2\x2\x2"+
		"\r\xA1\x3\x2\x2\x2\xF\xA3\x3\x2\x2\x2\x11\xA5\x3\x2\x2\x2\x13\xA8\x3\x2"+
		"\x2\x2\x15\xAC\x3\x2\x2\x2\x17\xAE\x3\x2\x2\x2\x19\xB0\x3\x2\x2\x2\x1B"+
		"\xB2\x3\x2\x2\x2\x1D\xB4\x3\x2\x2\x2\x1F\xB6\x3\x2\x2\x2!\xB8\x3\x2\x2"+
		"\x2#\xBA\x3\x2\x2\x2%\xBC\x3\x2\x2\x2\'\xBE\x3\x2\x2\x2)\xC1\x3\x2\x2"+
		"\x2+\xC3\x3\x2\x2\x2-\xC6\x3\x2\x2\x2/\xC9\x3\x2\x2\x2\x31\xCC\x3\x2\x2"+
		"\x2\x33\xCE\x3\x2\x2\x2\x35\xD0\x3\x2\x2\x2\x37\xD3\x3\x2\x2\x2\x39\xD5"+
		"\x3\x2\x2\x2;\xD8\x3\x2\x2\x2=\xDA\x3\x2\x2\x2?\xDD\x3\x2\x2\x2\x41\xE1"+
		"\x3\x2\x2\x2\x43\xE4\x3\x2\x2\x2\x45\xED\x3\x2\x2\x2G\xEF\x3\x2\x2\x2"+
		"I\xF1\x3\x2\x2\x2K\xF3\x3\x2\x2\x2M\xF5\x3\x2\x2\x2O\xF8\x3\x2\x2\x2Q"+
		"\xFD\x3\x2\x2\x2S\x101\x3\x2\x2\x2U\x104\x3\x2\x2\x2W\x10A\x3\x2\x2\x2"+
		"Y\x111\x3\x2\x2\x2[\x113\x3\x2\x2\x2]\x118\x3\x2\x2\x2_\x11E\x3\x2\x2"+
		"\x2\x61\x123\x3\x2\x2\x2\x63\x126\x3\x2\x2\x2\x65\x12A\x3\x2\x2\x2g\x12E"+
		"\x3\x2\x2\x2i\x133\x3\x2\x2\x2k\x139\x3\x2\x2\x2m\x13B\x3\x2\x2\x2o\x13F"+
		"\x3\x2\x2\x2q\x14A\x3\x2\x2\x2s\x151\x3\x2\x2\x2u\x17C\x3\x2\x2\x2w\x17E"+
		"\x3\x2\x2\x2y\x180\x3\x2\x2\x2{\x18C\x3\x2\x2\x2}\x1A0\x3\x2\x2\x2\x7F"+
		"\x1A9\x3\x2\x2\x2\x81\x1BB\x3\x2\x2\x2\x83\x1C6\x3\x2\x2\x2\x85\x1C8\x3"+
		"\x2\x2\x2\x87\x1E3\x3\x2\x2\x2\x89\x1E5\x3\x2\x2\x2\x8B\x1E7\x3\x2\x2"+
		"\x2\x8D\x1F0\x3\x2\x2\x2\x8F\x1FF\x3\x2\x2\x2\x91\x204\x3\x2\x2\x2\x93"+
		"\x94\a=\x2\x2\x94\x4\x3\x2\x2\x2\x95\x96\a>\x2\x2\x96\x97\a/\x2\x2\x97"+
		"\x6\x3\x2\x2\x2\x98\x99\a?\x2\x2\x99\b\x3\x2\x2\x2\x9A\x9B\a>\x2\x2\x9B"+
		"\x9C\a>\x2\x2\x9C\x9D\a/\x2\x2\x9D\n\x3\x2\x2\x2\x9E\x9F\a]\x2\x2\x9F"+
		"\xA0\a]\x2\x2\xA0\f\x3\x2\x2\x2\xA1\xA2\a_\x2\x2\xA2\xE\x3\x2\x2\x2\xA3"+
		"\xA4\a]\x2\x2\xA4\x10\x3\x2\x2\x2\xA5\xA6\a<\x2\x2\xA6\xA7\a<\x2\x2\xA7"+
		"\x12\x3\x2\x2\x2\xA8\xA9\a<\x2\x2\xA9\xAA\a<\x2\x2\xAA\xAB\a<\x2\x2\xAB"+
		"\x14\x3\x2\x2\x2\xAC\xAD\a&\x2\x2\xAD\x16\x3\x2\x2\x2\xAE\xAF\a\x42\x2"+
		"\x2\xAF\x18\x3\x2\x2\x2\xB0\xB1\a`\x2\x2\xB1\x1A\x3\x2\x2\x2\xB2\xB3\a"+
		"/\x2\x2\xB3\x1C\x3\x2\x2\x2\xB4\xB5\a-\x2\x2\xB5\x1E\x3\x2\x2\x2\xB6\xB7"+
		"\a<\x2\x2\xB7 \x3\x2\x2\x2\xB8\xB9\a,\x2\x2\xB9\"\x3\x2\x2\x2\xBA\xBB"+
		"\a\x31\x2\x2\xBB$\x3\x2\x2\x2\xBC\xBD\a@\x2\x2\xBD&\x3\x2\x2\x2\xBE\xBF"+
		"\a@\x2\x2\xBF\xC0\a?\x2\x2\xC0(\x3\x2\x2\x2\xC1\xC2\a>\x2\x2\xC2*\x3\x2"+
		"\x2\x2\xC3\xC4\a>\x2\x2\xC4\xC5\a?\x2\x2\xC5,\x3\x2\x2\x2\xC6\xC7\a?\x2"+
		"\x2\xC7\xC8\a?\x2\x2\xC8.\x3\x2\x2\x2\xC9\xCA\a#\x2\x2\xCA\xCB\a?\x2\x2"+
		"\xCB\x30\x3\x2\x2\x2\xCC\xCD\a#\x2\x2\xCD\x32\x3\x2\x2\x2\xCE\xCF\a(\x2"+
		"\x2\xCF\x34\x3\x2\x2\x2\xD0\xD1\a(\x2\x2\xD1\xD2\a(\x2\x2\xD2\x36\x3\x2"+
		"\x2\x2\xD3\xD4\a~\x2\x2\xD4\x38\x3\x2\x2\x2\xD5\xD6\a~\x2\x2\xD6\xD7\a"+
		"~\x2\x2\xD7:\x3\x2\x2\x2\xD8\xD9\a\x80\x2\x2\xD9<\x3\x2\x2\x2\xDA\xDB"+
		"\a/\x2\x2\xDB\xDC\a@\x2\x2\xDC>\x3\x2\x2\x2\xDD\xDE\a/\x2\x2\xDE\xDF\a"+
		"@\x2\x2\xDF\xE0\a@\x2\x2\xE0@\x3\x2\x2\x2\xE1\xE2\a<\x2\x2\xE2\xE3\a?"+
		"\x2\x2\xE3\x42\x3\x2\x2\x2\xE4\xE5\ah\x2\x2\xE5\xE6\aw\x2\x2\xE6\xE7\a"+
		"p\x2\x2\xE7\xE8\a\x65\x2\x2\xE8\xE9\av\x2\x2\xE9\xEA\ak\x2\x2\xEA\xEB"+
		"\aq\x2\x2\xEB\xEC\ap\x2\x2\xEC\x44\x3\x2\x2\x2\xED\xEE\a*\x2\x2\xEE\x46"+
		"\x3\x2\x2\x2\xEF\xF0\a+\x2\x2\xF0H\x3\x2\x2\x2\xF1\xF2\a}\x2\x2\xF2J\x3"+
		"\x2\x2\x2\xF3\xF4\a\x7F\x2\x2\xF4L\x3\x2\x2\x2\xF5\xF6\ak\x2\x2\xF6\xF7"+
		"\ah\x2\x2\xF7N\x3\x2\x2\x2\xF8\xF9\ag\x2\x2\xF9\xFA\an\x2\x2\xFA\xFB\a"+
		"u\x2\x2\xFB\xFC\ag\x2\x2\xFCP\x3\x2\x2\x2\xFD\xFE\ah\x2\x2\xFE\xFF\aq"+
		"\x2\x2\xFF\x100\at\x2\x2\x100R\x3\x2\x2\x2\x101\x102\ak\x2\x2\x102\x103"+
		"\ap\x2\x2\x103T\x3\x2\x2\x2\x104\x105\ay\x2\x2\x105\x106\aj\x2\x2\x106"+
		"\x107\ak\x2\x2\x107\x108\an\x2\x2\x108\x109\ag\x2\x2\x109V\x3\x2\x2\x2"+
		"\x10A\x10B\at\x2\x2\x10B\x10C\ag\x2\x2\x10C\x10D\ar\x2\x2\x10D\x10E\a"+
		"g\x2\x2\x10E\x10F\a\x63\x2\x2\x10F\x110\av\x2\x2\x110X\x3\x2\x2\x2\x111"+
		"\x112\a\x41\x2\x2\x112Z\x3\x2\x2\x2\x113\x114\ap\x2\x2\x114\x115\ag\x2"+
		"\x2\x115\x116\az\x2\x2\x116\x117\av\x2\x2\x117\\\x3\x2\x2\x2\x118\x119"+
		"\a\x64\x2\x2\x119\x11A\at\x2\x2\x11A\x11B\ag\x2\x2\x11B\x11C\a\x63\x2"+
		"\x2\x11C\x11D\am\x2\x2\x11D^\x3\x2\x2\x2\x11E\x11F\aP\x2\x2\x11F\x120"+
		"\aW\x2\x2\x120\x121\aN\x2\x2\x121\x122\aN\x2\x2\x122`\x3\x2\x2\x2\x123"+
		"\x124\aP\x2\x2\x124\x125\a\x43\x2\x2\x125\x62\x3\x2\x2\x2\x126\x127\a"+
		"K\x2\x2\x127\x128\ap\x2\x2\x128\x129\ah\x2\x2\x129\x64\x3\x2\x2\x2\x12A"+
		"\x12B\aP\x2\x2\x12B\x12C\a\x63\x2\x2\x12C\x12D\aP\x2\x2\x12D\x66\x3\x2"+
		"\x2\x2\x12E\x12F\aV\x2\x2\x12F\x130\aT\x2\x2\x130\x131\aW\x2\x2\x131\x132"+
		"\aG\x2\x2\x132h\x3\x2\x2\x2\x133\x134\aH\x2\x2\x134\x135\a\x43\x2\x2\x135"+
		"\x136\aN\x2\x2\x136\x137\aU\x2\x2\x137\x138\aG\x2\x2\x138j\x3\x2\x2\x2"+
		"\x139\x13A\a.\x2\x2\x13Al\x3\x2\x2\x2\x13B\x13C\a\x30\x2\x2\x13C\x13D"+
		"\a\x30\x2\x2\x13D\x13E\a\x30\x2\x2\x13En\x3\x2\x2\x2\x13F\x140\a\x32\x2"+
		"\x2\x140\x142\t\x2\x2\x2\x141\x143\x5s:\x2\x142\x141\x3\x2\x2\x2\x143"+
		"\x144\x3\x2\x2\x2\x144\x142\x3\x2\x2\x2\x144\x145\x3\x2\x2\x2\x145\x147"+
		"\x3\x2\x2\x2\x146\x148\t\x3\x2\x2\x147\x146\x3\x2\x2\x2\x147\x148\x3\x2"+
		"\x2\x2\x148p\x3\x2\x2\x2\x149\x14B\x5w<\x2\x14A\x149\x3\x2\x2\x2\x14B"+
		"\x14C\x3\x2\x2\x2\x14C\x14A\x3\x2\x2\x2\x14C\x14D\x3\x2\x2\x2\x14D\x14F"+
		"\x3\x2\x2\x2\x14E\x150\t\x3\x2\x2\x14F\x14E\x3\x2\x2\x2\x14F\x150\x3\x2"+
		"\x2\x2\x150r\x3\x2\x2\x2\x151\x152\t\x4\x2\x2\x152t\x3\x2\x2\x2\x153\x155"+
		"\x5w<\x2\x154\x153\x3\x2\x2\x2\x155\x156\x3\x2\x2\x2\x156\x154\x3\x2\x2"+
		"\x2\x156\x157\x3\x2\x2\x2\x157\x158\x3\x2\x2\x2\x158\x15C\a\x30\x2\x2"+
		"\x159\x15B\x5w<\x2\x15A\x159\x3\x2\x2\x2\x15B\x15E\x3\x2\x2\x2\x15C\x15A"+
		"\x3\x2\x2\x2\x15C\x15D\x3\x2\x2\x2\x15D\x160\x3\x2\x2\x2\x15E\x15C\x3"+
		"\x2\x2\x2\x15F\x161\x5y=\x2\x160\x15F\x3\x2\x2\x2\x160\x161\x3\x2\x2\x2"+
		"\x161\x163\x3\x2\x2\x2\x162\x164\t\x3\x2\x2\x163\x162\x3\x2\x2\x2\x163"+
		"\x164\x3\x2\x2\x2\x164\x17D\x3\x2\x2\x2\x165\x167\x5w<\x2\x166\x165\x3"+
		"\x2\x2\x2\x167\x168\x3\x2\x2\x2\x168\x166\x3\x2\x2\x2\x168\x169\x3\x2"+
		"\x2\x2\x169\x16B\x3\x2\x2\x2\x16A\x16C\x5y=\x2\x16B\x16A\x3\x2\x2\x2\x16B"+
		"\x16C\x3\x2\x2\x2\x16C\x16E\x3\x2\x2\x2\x16D\x16F\t\x3\x2\x2\x16E\x16D"+
		"\x3\x2\x2\x2\x16E\x16F\x3\x2\x2\x2\x16F\x17D\x3\x2\x2\x2\x170\x172\a\x30"+
		"\x2\x2\x171\x173\x5w<\x2\x172\x171\x3\x2\x2\x2\x173\x174\x3\x2\x2\x2\x174"+
		"\x172\x3\x2\x2\x2\x174\x175\x3\x2\x2\x2\x175\x177\x3\x2\x2\x2\x176\x178"+
		"\x5y=\x2\x177\x176\x3\x2\x2\x2\x177\x178\x3\x2\x2\x2\x178\x17A\x3\x2\x2"+
		"\x2\x179\x17B\t\x3\x2\x2\x17A\x179\x3\x2\x2\x2\x17A\x17B\x3\x2\x2\x2\x17B"+
		"\x17D\x3\x2\x2\x2\x17C\x154\x3\x2\x2\x2\x17C\x166\x3\x2\x2\x2\x17C\x170"+
		"\x3\x2\x2\x2\x17Dv\x3\x2\x2\x2\x17E\x17F\x4\x32;\x2\x17Fx\x3\x2\x2\x2"+
		"\x180\x182\t\x5\x2\x2\x181\x183\t\x6\x2\x2\x182\x181\x3\x2\x2\x2\x182"+
		"\x183\x3\x2\x2\x2\x183\x184\x3\x2\x2\x2\x184\x185\x5q\x39\x2\x185z\x3"+
		"\x2\x2\x2\x186\x187\x5q\x39\x2\x187\x188\ak\x2\x2\x188\x18D\x3\x2\x2\x2"+
		"\x189\x18A\x5u;\x2\x18A\x18B\ak\x2\x2\x18B\x18D\x3\x2\x2\x2\x18C\x186"+
		"\x3\x2\x2\x2\x18C\x189\x3\x2\x2\x2\x18D|\x3\x2\x2\x2\x18E\x193\a$\x2\x2"+
		"\x18F\x192\x5\x7F@\x2\x190\x192\n\a\x2\x2\x191\x18F\x3\x2\x2\x2\x191\x190"+
		"\x3\x2\x2\x2\x192\x195\x3\x2\x2\x2\x193\x194\x3\x2\x2\x2\x193\x191\x3"+
		"\x2\x2\x2\x194\x196\x3\x2\x2\x2\x195\x193\x3\x2\x2\x2\x196\x1A1\a$\x2"+
		"\x2\x197\x19C\a)\x2\x2\x198\x19B\x5\x7F@\x2\x199\x19B\n\b\x2\x2\x19A\x198"+
		"\x3\x2\x2\x2\x19A\x199\x3\x2\x2\x2\x19B\x19E\x3\x2\x2\x2\x19C\x19D\x3"+
		"\x2\x2\x2\x19C\x19A\x3\x2\x2\x2\x19D\x19F\x3\x2\x2\x2\x19E\x19C\x3\x2"+
		"\x2\x2\x19F\x1A1\a)\x2\x2\x1A0\x18E\x3\x2\x2\x2\x1A0\x197\x3\x2\x2\x2"+
		"\x1A1~\x3\x2\x2\x2\x1A2\x1A4\a^\x2\x2\x1A3\x1A5\t\t\x2\x2\x1A4\x1A3\x3"+
		"\x2\x2\x2\x1A5\x1AA\x3\x2\x2\x2\x1A6\x1AA\x5\x81\x41\x2\x1A7\x1AA\x5\x85"+
		"\x43\x2\x1A8\x1AA\x5\x83\x42\x2\x1A9\x1A2\x3\x2\x2\x2\x1A9\x1A6\x3\x2"+
		"\x2\x2\x1A9\x1A7\x3\x2\x2\x2\x1A9\x1A8\x3\x2\x2\x2\x1AA\x80\x3\x2\x2\x2"+
		"\x1AB\x1AC\a^\x2\x2\x1AC\x1AD\aw\x2\x2\x1AD\x1AE\x5s:\x2\x1AE\x1AF\x5"+
		"s:\x2\x1AF\x1B0\x5s:\x2\x1B0\x1B1\x5s:\x2\x1B1\x1BC\x3\x2\x2\x2\x1B2\x1B3"+
		"\a^\x2\x2\x1B3\x1B4\aw\x2\x2\x1B4\x1B5\a}\x2\x2\x1B5\x1B6\x5s:\x2\x1B6"+
		"\x1B7\x5s:\x2\x1B7\x1B8\x5s:\x2\x1B8\x1B9\x5s:\x2\x1B9\x1BA\a\x7F\x2\x2"+
		"\x1BA\x1BC\x3\x2\x2\x2\x1BB\x1AB\x3\x2\x2\x2\x1BB\x1B2\x3\x2\x2\x2\x1BC"+
		"\x82\x3\x2\x2\x2\x1BD\x1BE\a^\x2\x2\x1BE\x1BF\t\n\x2\x2\x1BF\x1C0\t\v"+
		"\x2\x2\x1C0\x1C7\t\v\x2\x2\x1C1\x1C2\a^\x2\x2\x1C2\x1C3\t\v\x2\x2\x1C3"+
		"\x1C7\t\v\x2\x2\x1C4\x1C5\a^\x2\x2\x1C5\x1C7\t\v\x2\x2\x1C6\x1BD\x3\x2"+
		"\x2\x2\x1C6\x1C1\x3\x2\x2\x2\x1C6\x1C4\x3\x2\x2\x2\x1C7\x84\x3\x2\x2\x2"+
		"\x1C8\x1C9\a^\x2\x2\x1C9\x1CB\x5s:\x2\x1CA\x1CC\x5s:\x2\x1CB\x1CA\x3\x2"+
		"\x2\x2\x1CB\x1CC\x3\x2\x2\x2\x1CC\x86\x3\x2\x2\x2\x1CD\x1D0\a\x30\x2\x2"+
		"\x1CE\x1D1\x5\x89\x45\x2\x1CF\x1D1\t\f\x2\x2\x1D0\x1CE\x3\x2\x2\x2\x1D0"+
		"\x1CF\x3\x2\x2\x2\x1D1\x1D7\x3\x2\x2\x2\x1D2\x1D6\x5\x89\x45\x2\x1D3\x1D6"+
		"\x5w<\x2\x1D4\x1D6\t\f\x2\x2\x1D5\x1D2\x3\x2\x2\x2\x1D5\x1D3\x3\x2\x2"+
		"\x2\x1D5\x1D4\x3\x2\x2\x2\x1D6\x1D9\x3\x2\x2\x2\x1D7\x1D5\x3\x2\x2\x2"+
		"\x1D7\x1D8\x3\x2\x2\x2\x1D8\x1E4\x3\x2\x2\x2\x1D9\x1D7\x3\x2\x2\x2\x1DA"+
		"\x1E0\x5\x89\x45\x2\x1DB\x1DF\x5\x89\x45\x2\x1DC\x1DF\x5w<\x2\x1DD\x1DF"+
		"\t\f\x2\x2\x1DE\x1DB\x3\x2\x2\x2\x1DE\x1DC\x3\x2\x2\x2\x1DE\x1DD\x3\x2"+
		"\x2\x2\x1DF\x1E2\x3\x2\x2\x2\x1E0\x1DE\x3\x2\x2\x2\x1E0\x1E1\x3\x2\x2"+
		"\x2\x1E1\x1E4\x3\x2\x2\x2\x1E2\x1E0\x3\x2\x2\x2\x1E3\x1CD\x3\x2\x2\x2"+
		"\x1E3\x1DA\x3\x2\x2\x2\x1E4\x88\x3\x2\x2\x2\x1E5\x1E6\t\r\x2\x2\x1E6\x8A"+
		"\x3\x2\x2\x2\x1E7\x1EB\a\'\x2\x2\x1E8\x1EA\v\x2\x2\x2\x1E9\x1E8\x3\x2"+
		"\x2\x2\x1EA\x1ED\x3\x2\x2\x2\x1EB\x1EC\x3\x2\x2\x2\x1EB\x1E9\x3\x2\x2"+
		"\x2\x1EC\x1EE\x3\x2\x2\x2\x1ED\x1EB\x3\x2\x2\x2\x1EE\x1EF\a\'\x2\x2\x1EF"+
		"\x8C\x3\x2\x2\x2\x1F0\x1F4\a%\x2\x2\x1F1\x1F3\v\x2\x2\x2\x1F2\x1F1\x3"+
		"\x2\x2\x2\x1F3\x1F6\x3\x2\x2\x2\x1F4\x1F5\x3\x2\x2\x2\x1F4\x1F2\x3\x2"+
		"\x2\x2\x1F5\x1F8\x3\x2\x2\x2\x1F6\x1F4\x3\x2\x2\x2\x1F7\x1F9\a\xF\x2\x2"+
		"\x1F8\x1F7\x3\x2\x2\x2\x1F8\x1F9\x3\x2\x2\x2\x1F9\x1FA\x3\x2\x2\x2\x1FA"+
		"\x1FB\a\f\x2\x2\x1FB\x1FC\x3\x2\x2\x2\x1FC\x1FD\bG\x2\x2\x1FD\x8E\x3\x2"+
		"\x2\x2\x1FE\x200\a\xF\x2\x2\x1FF\x1FE\x3\x2\x2\x2\x1FF\x200\x3\x2\x2\x2"+
		"\x200\x201\x3\x2\x2\x2\x201\x202\a\f\x2\x2\x202\x90\x3\x2\x2\x2\x203\x205"+
		"\t\xE\x2\x2\x204\x203\x3\x2\x2\x2\x205\x206\x3\x2\x2\x2\x206\x204\x3\x2"+
		"\x2\x2\x206\x207\x3\x2\x2\x2\x207\x208\x3\x2\x2\x2\x208\x209\bI\x3\x2"+
		"\x209\x92\x3\x2\x2\x2)\x2\x144\x147\x14C\x14F\x156\x15C\x160\x163\x168"+
		"\x16B\x16E\x174\x177\x17A\x17C\x182\x18C\x191\x193\x19A\x19C\x1A0\x1A4"+
		"\x1A9\x1BB\x1C6\x1CB\x1D0\x1D5\x1D7\x1DE\x1E0\x1E3\x1EB\x1F4\x1F8\x1FF"+
		"\x206\x4\t@\x2\b\x2\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}

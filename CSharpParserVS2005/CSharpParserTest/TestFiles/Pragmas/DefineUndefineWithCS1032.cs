#define SymbolA

#undef SymbolB

#undef SymbolB

#define SymbolA

extern alias alias;

#define WrongDefine
#undef WrongUndef

using System.ComponentModel;

#define WrongDefine
#undef WrongUndef

[assembly: Description("Just for test")]

#define WrongDefine
#undef WrongUndef

namespace CSharpParserTest.TestFiles
{
  #define WrongDefine
  #undef WrongUndef
}
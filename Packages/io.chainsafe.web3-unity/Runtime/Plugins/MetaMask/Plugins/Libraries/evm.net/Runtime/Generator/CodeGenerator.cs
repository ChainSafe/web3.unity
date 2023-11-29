using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace evm.net.Generator
{
    public abstract class CodeGenerator
    {
        protected string _cachedString;
        protected StringBuilder builder;
        protected int tabs = 0;
        protected GeneratorContext _context;

        public CodeGenerator()
        {
            builder = new StringBuilder();
        }

        public string ToFunctionName(string name)
        {
            return name[0].ToString().ToLower() == name[0].ToString() ? $"{name[0].ToString().ToUpper()}{name.Substring(1)}" : name;
        }

        public bool IsValidIdentifier(string identifier)
        {
            if (String.IsNullOrEmpty(identifier)) return false;

            // C# keywords: http://msdn.microsoft.com/en-us/library/x53a06bb(v=vs.71).aspx
            var keywords = new[]
            {
                "abstract", "event", "new", "struct",
                "as", "explicit", "null", "switch",
                "base", "extern", "object", "this",
                "bool", "false", "operator", "throw",
                "breal", "finally", "out", "true",
                "byte", "fixed", "override", "try",
                "case", "float", "params", "typeof",
                "catch", "for", "private", "uint",
                "char", "foreach", "protected", "ulong",
                "checked", "goto", "public", "unchekeced",
                "class", "if", "readonly", "unsafe",
                "const", "implicit", "ref", "ushort",
                "continue", "in", "return", "using",
                "decimal", "int", "sbyte", "virtual",
                "default", "interface", "sealed", "volatile",
                "delegate", "internal", "short", "void",
                "do", "is", "sizeof", "while",
                "double", "lock", "stackalloc",
                "else", "long", "static",
                "enum", "namespace", "string"
            };

            // definition of a valid C# identifier: http://msdn.microsoft.com/en-us/library/aa664670(v=vs.71).aspx
            const string formattingCharacter = @"\p{Cf}";
            const string connectingCharacter = @"\p{Pc}";
            const string decimalDigitCharacter = @"\p{Nd}";
            const string combiningCharacter = @"\p{Mn}|\p{Mc}";
            const string letterCharacter = @"\p{Lu}|\p{Ll}|\p{Lt}|\p{Lm}|\p{Lo}|\p{Nl}";
            const string identifierPartCharacter = letterCharacter + "|" +
                                                   decimalDigitCharacter + "|" +
                                                   connectingCharacter + "|" +
                                                   combiningCharacter + "|" +
                                                   formattingCharacter;
            const string identifierPartCharacters = "(" + identifierPartCharacter + ")+";
            const string identifierStartCharacter = "(" + letterCharacter + "|_)";
            const string identifierOrKeyword = identifierStartCharacter + "(" +
                                               identifierPartCharacters + ")*";
            var validIdentifierRegex = new Regex("^" + identifierOrKeyword + "$", RegexOptions.Compiled);
            var normalizedIdentifier = identifier.Normalize();

            // 1. check that the identifier match the validIdentifer regex and it's not a C# keyword
            if (validIdentifierRegex.IsMatch(normalizedIdentifier) && !keywords.Contains(normalizedIdentifier))
            {
                return true;
            }

            // 2. check if the identifier starts with @
            if (normalizedIdentifier.StartsWith("@") && validIdentifierRegex.IsMatch(normalizedIdentifier.Substring(1)))
            {
                return true;
            }

            // 3. it's not a valid identifier
            return false;
        }

        public void WriteLine(string line = null)
        {
            PrependTabs();
            if (!string.IsNullOrWhiteSpace(line))
                builder.Append(line);
            builder.Append("\n");
        }

        public void PrependTabs()
        {
            var tabStr = new string('\t', tabs);
            builder.Append(tabStr);
        }

        public void StartCodeBlock()
        {
            WriteLine("{");
            tabs++;
        }

        public void EndCodeBlock()
        {
            if (tabs == 0)
                throw new InvalidOperationException("Attempted to end code block when none started");
            tabs--;
            WriteLine("}");
        }

        public void CompleteCodeBlocks()
        {
            for (int i = 0; i <= tabs; i++)
            {
                EndCodeBlock();
            }
        }

        protected abstract void DoWrite();
        
        public abstract string Filename { get; }

        public string Write(bool force = false)
        {
            if (!string.IsNullOrWhiteSpace(_cachedString) && !force)
                return _cachedString;
            
            DoWrite();

            _cachedString = builder.ToString();

            return _cachedString;
        }

        public Dictionary<string, string> GenerateAll()
        {
            var dict = new Dictionary<string, string>();
            dict.Add(Filename, ToString());
            
            // Add any additional files
            foreach (var generator in _context.Generators)
            {
                dict.Add(generator.Filename, generator.ToString());
            }

            return dict;
        }

        public string ToString()
        {
            return Write();
        }
    }
}
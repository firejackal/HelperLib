using System.Collections.Generic;

namespace HelperLib
{
    public static class TextTools
    {
        private const char CR = '\r'; // Chr(13);
        private const char LF = '\n'; // Chr(10);

        /// <summary>Taking in account English words being separated by spaces, this function will try to fit as much on one line as possible before making a new line.</summary>
        public static List<string> ParseLines(string text, int charactersPerLine)
        {
            List<string> results = new List<string>();

            // we won't split the text, we need to know where newline characters are.
            string lastWord = "";
            string currentLine = "";
            if(!string.IsNullOrEmpty(text)) {
                for(int index = 1; index <= text.Length; index++) {
                    string chr = text.Substring(index - 1, 1);
                    string nextChr = "";
                    if(index < text.Length) nextChr = text.Substring(index, 1);

                    if(string.Equals(chr, " "))
                        AddWord(ref lastWord, ref currentLine, charactersPerLine, ref results);
                    else if(string.Equals(chr, CR) && string.Equals(nextChr, LF)) {
                        // Both
                        //  (A) We still have a word to add, and need to take that into an account, or
                        //  (B) We will always add a new line, even if we just added one.

                        // automatically add contents to the line, and start a new line
                        AddWord(ref lastWord, ref currentLine, charactersPerLine, ref results);

                        if(!string.IsNullOrEmpty(currentLine)) {
                            results.Add(currentLine);
                            currentLine = "";
                        } else
                            results.Add("");

                        index += 1; //because we also got another character
                    } else
                        // just add to the current, we don't have to worry about anything else.
                        lastWord += chr;
                } //index

                // add the last word or line if any.
                AddWord(ref lastWord, ref currentLine, charactersPerLine, ref results);
                // finish up.
                if(!string.IsNullOrEmpty(currentLine)) {
                    results.Add(currentLine);
                    currentLine = "";
                }
            }

            return results;
        } //ParseLines

        private static void AddWord(ref string word, ref string currentLine, int charactersPerLine, ref List<string> lines)
        {
            // Either
            //  (A) We will add to the current line with no problems, or
            //  (B) This with the current line is beyond the limits, so we start a new line.

            if(!string.IsNullOrEmpty(word) && string.IsNullOrEmpty(currentLine)) {
                currentLine = word;
                word = "";
            } else if(!string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(currentLine)) {
                if((currentLine.Length + 1 + word.Length) > charactersPerLine) {
                    //start new line
                    lines.Add(currentLine);
                    currentLine = word;
                    word = "";
                } else {
                    // add to line
                    currentLine += " " + word;
                    word = "";
                }
            }
        } //AddLine
    } //TextTools
} // HelperLib namespace

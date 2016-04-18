using System.Windows.Forms;
using System.Drawing;
using System;

namespace ChemLab
{
    class Format
    {
        public void ShowBalancedReaction(string reaction, RichTextBox rtbReaction)
        {
            SetStandardFormatting(rtbReaction);
            
            int pos = 0;
            char sym = reaction[pos];
            char nextSym = reaction[pos + 1];
            int len = reaction.Length;

            while (pos < len - 1)
            {
                rtbReaction.AppendText(Convert.ToString(sym));

                if ((char.IsLetter(sym) || sym == ')') && char.IsDigit(nextSym)) SetSubscriptFormatting(rtbReaction);
                else if (char.IsWhiteSpace(sym) && nextSym == '') SetArrowFormatting(rtbReaction);
                else SetStandardFormatting(rtbReaction);

                pos++;
                sym = reaction[pos];
                if (pos < len - 1) nextSym = reaction[pos + 1];
            }

            if (char.IsLetter(sym) && char.IsDigit(nextSym)) SetSubscriptFormatting(rtbReaction);
            rtbReaction.AppendText(Convert.ToString(sym));
            SetStandardFormatting(rtbReaction);
        }

        public string RemoveMultipleSpaces(string text)                                                                             // Премахва ако има повече от един интервал на едно и също място
        {
            text = text.Trim();
            
            int len = text.Length;
            for (int pos = 0; pos < len; pos++)
            {
                char sym = text[pos];
                if (char.IsWhiteSpace(sym))
                {
                    int spacesCounter = 0;
                    do
                    {
                        spacesCounter++;
                        sym = text[pos + spacesCounter];
                    }
                    while (char.IsWhiteSpace(sym));

                    text = text.Remove(pos, spacesCounter - 1);
                    len -= spacesCounter - 1;
                }
            }

            return text;
        }

        public string AddSpacesAroundPlusAndArrowIfNoSuch(string text)                                                              // Добавя интервали около знаците плюс и стрелка, ако няма
        {
            int len = text.Length;
            for (int pos = 0; pos < len - 1; pos++)
            {
                char sym = text[pos];
                char nextSym = text[pos + 1];

                if (!char.IsWhiteSpace(sym) && (nextSym == '+' || nextSym == '') || (sym == '+' || sym == '') && !char.IsWhiteSpace(nextSym))
                {
                    text = text.Insert(pos + 1, " ");
                    pos++;
                    len++;
                }
            }

            return text;
        }

        private void SetStandardFormatting(RichTextBox rtbReaction)                                                                // Връща стандартната големина, цвят и позиция на шрифта
        {
            rtbReaction.SelectionColor = Color.Black;
            rtbReaction.SelectionFont = new Font("Arial Narrow", 16);
            rtbReaction.SelectionCharOffset = 0;
        }

        private void SetSubscriptFormatting(RichTextBox rtbReaction)                                                               // Задава подходящо форматиране на шрифта за изписване на долен индекс
        {
            rtbReaction.SelectionColor = Color.Black;
            rtbReaction.SelectionFont = new Font("Arial Narrow", 10);
            rtbReaction.SelectionCharOffset = -3;
        }

        private void SetArrowFormatting(RichTextBox rtbReaction)                                                                   // Задава подходящо форматиране на шрифта за изписване на стрелката
        {
            rtbReaction.SelectionColor = Color.Black;
            rtbReaction.SelectionFont = new Font("Wingdings 3", 20);
            rtbReaction.SelectionCharOffset = -4;
        }
    }
}

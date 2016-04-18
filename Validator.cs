using System;

namespace ChemLab
{
    class Validator
    {
        private string CommonValidation(string text)                                            // Обща проверка: кодове за грешки от 1 до 9
        {
            if (text == string.Empty) return "Липсва текст.";                                   // Проверка за празен низ

            int len = text.Length;                                                              // Проверка за недопустими символи
            for (int pos = 0; pos < len; pos++)
            {
                char sym = text[pos];
                if (!char.IsLetterOrDigit(sym) && !char.IsWhiteSpace(sym) && sym != '+' && sym != '' && sym != '(' && sym != ')') return "Има недопустим симол.";
            }

            return string.Empty;                                                                // Няма грешка
        }

        public string ValidateMolecule(string molecule)                                         // Проверка на химично съединение: кодове за грешки от 10 до 19
        {
            molecule = molecule.Trim();
            
            string comValRes = CommonValidation(molecule);                                      // Обща проверка
            if (comValRes != string.Empty) return comValRes;

            int moleculeLenght = molecule.Length;

            int pos = 0;                                                                        // Дали има коефициент пред молекулата
            char sym = molecule[pos];
            if (char.IsDigit(sym)) return "Във формулата има коефициент.";

            for (pos = 1; pos < moleculeLenght; pos++)                                          // Дали има коефициент вътре във формулата
            {
                char prevSym = molecule[pos - 1];
                sym = molecule[pos];
                if (char.IsWhiteSpace(prevSym) && char.IsDigit(sym)) return "Във формулата има коефициент.";
            }
            
            if (molecule.Contains("("))                                                         // Дали има коефициент в началото на скобите
            {
                int searchStart = 0;
                pos = molecule.IndexOf('(', searchStart);
                while (pos != -1)
                {
                    pos++;
                    sym = molecule[pos];
                    while (char.IsWhiteSpace(sym))
                    {
                        pos++;
                        sym = molecule[pos];
                    }
                    if (char.IsDigit(sym)) return "Във формулата има коефициент.";

                    searchStart = pos;
                    pos = molecule.IndexOf('(', searchStart);
                }
            }
            
            int bracketsCheck = 0;                                                              // Дали скобите са затворени както трябва
            pos = 0;
            do
            {
                sym = molecule[pos];
                pos++;
                if (sym == '(') bracketsCheck++;
                else if (sym == ')') bracketsCheck--;
            }
            while (bracketsCheck >= 0 && pos < moleculeLenght);
            if (bracketsCheck != 0) return "Скобите не са затворени както трябва.";

            Element element = new Element();                                                    // Дали във формулата има несъществуващ елемент

            int elStartPos = 0;
            int elLen = 0;
            sym = molecule[elStartPos];

            while (elStartPos + elLen < moleculeLenght)
            {
                do
                {
                    elLen++;
                    if (elStartPos + elLen < moleculeLenght) sym = molecule[elStartPos + elLen];
                }
                while (char.IsLower(sym) && elStartPos + elLen < moleculeLenght);

                string symbol = molecule.Substring(elStartPos, elLen);
                if (!element.Exists(symbol)) return "Във формулата има неразпознат химичен елемент.";

                elStartPos += elLen;
                elLen = 0;

                while (!char.IsUpper(sym) && elStartPos < moleculeLenght)
                {
                    elStartPos++;
                    if (elStartPos < moleculeLenght) sym = molecule[elStartPos];
                }
            }

            return string.Empty;                                                                // Няма грешка
        }

        public string ValidateRightPart(string rightPart)                                       // Проверка на дясната част от химичната реакция: кодове за грешки от 20 до 29
        {
            string comValRes = CommonValidation(rightPart);                                     // Обща проверка
            if (comValRes != string.Empty) return comValRes;

            char[] plus = new char[] { '+' };                                                   // Проверка на получените химични съединения
            string[] molecules = rightPart.Split(plus, StringSplitOptions.RemoveEmptyEntries);
            foreach (string molecule in molecules)
            {
                string molValRes = ValidateMolecule(molecule);
                if (molValRes != string.Empty) return molValRes;
            }

            return string.Empty;                                                                // Няма грешка
        }

        public string ValidateReaction(string reaction)                                         // Проверка на цялата химична реакция: кодове за грешки от 30 до 39
        {
            string comValRes = CommonValidation(reaction);                                      // Обща проверка
            if (comValRes != string.Empty) return comValRes;
            
            if (!reaction.Contains("")) return "Реакцията не съдържа знак за равенство.";      // Дали реакцията съдържа стрелка

            char[] arrow = new char[] { '' };                                                  // Дали лявата част съдържа символа "плюс"
            string[] parts = reaction.Split(arrow, StringSplitOptions.RemoveEmptyEntries);
            string leftPart = parts[0];
            if (!leftPart.Contains("+")) return "Лявата част на реакцията не съдържа знак \"плюс\".";

            char[] plus = new char[] { '+' };                                                   // Проверка на химичните съединения от реакцията
            string[] molecules = leftPart.Split(plus, StringSplitOptions.RemoveEmptyEntries);
            foreach (string molecule in molecules)
            {
                string molValRes = ValidateMolecule(molecule);
                if (molValRes != string.Empty) return molValRes;
            }

            string rightPart = parts[1];                                                        // Проверка на дясната част от химичната реакция
            string rightValRes = ValidateRightPart(rightPart);
            if (rightValRes != string.Empty) return rightValRes;

            return string.Empty;                                                                // Няма грешка
        }
    }
}

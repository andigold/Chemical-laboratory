using System;
using System.Collections.Generic;

namespace ChemLab
{
    class Balance
    {
        private string balancedReaction;                                                                                // Изравнената реакция
        public string BalancedReaction
        {
            get { return balancedReaction; }
        }

        public Balance() { }                                                                                            // Конструктор по подразбиране
        
        public Balance(string unbalancedReaction)                                                                       // Конструктор за изравняване на реакция
        {
            List<string> molecules;                                                                                     // Списък от участващите вещества в химичната реакция
            SplitAndCountMolecules(unbalancedReaction, out molecules);                                                  // Разделя уравнението на вещества (молекули)

            List<string> elements;                                                                                      // Списък от участващите елементи в химичната реакция
            SplitAndCountElements(unbalancedReaction, out elements);

            int leftPartMoleculesCount = CountLeftPartMolecules(unbalancedReaction);                                    // Броя на вещества от лявата част (спрямо стрелката) на уравнението
            int[,] matrix = CreateMatrix(elements, molecules, leftPartMoleculesCount);                                  // Въз основа на реакцията се съставя матрица на линейна система от уравнения

            Algebra algebra = new Algebra();
            int[] coefficients = algebra.CalcSolution(matrix);                                                          // Определя се набора от минимални по стойноста цели решения на системата

            balancedReaction = MergeCoefficientsAndMolecules(coefficients, molecules, leftPartMoleculesCount);          // Въз основа на намерените решения се съставя изравнената реакция
        }

        public int SplitAndCountMolecules(string reaction, out List<string> molecules)                                  // Разделя уравнението на вещества (молекули)
        {
            char[] separators = new char[] { '+', '', ' ' };                                                           // За разделители служат символите: плюс, стрелка и интервал
            string[] molArray = reaction.Split(separators, StringSplitOptions.RemoveEmptyEntries);                      // Реакцията се разделя на части (молекули) по зададените символи
            molecules = new List<string>(molArray);                                                                     // Веществата се прехвърлят от масив в динамичен списък

            return molecules.Count;                                                                                     // Освен списъка, методът връща и броя на веществата
        }

        public int SplitAndCountElements(string reaction, out List<string> elements)                                    // Разделя уравнението на химични елементи
        {
            elements = new List<string>();                                                                              // Списък за участващите елементи
            int reactionLenght = reaction.Length;                                                                       // Броя на символите, от които е съставена реакцията

            int elStartPos = 0;                                                                                         // Позицията на първия символ за текущия елемент
            int elLen = 0;                                                                                              // Дължината на низа на текущия елемент
            char sym = reaction[elStartPos];                                                                            // Започва се от началото на реакцията

            while (elStartPos + elLen < reactionLenght)                                                                 // Докато не е достигнат края на реакцията
            {
                do                                                                                                      // Започва четенето на текущия елемент
                {
                    elLen++;                                                                                            // Минава се към следващия символ като се увеличава дължината на елемента
                    if (elStartPos + elLen < reactionLenght) sym = reaction[elStartPos + elLen];                        // Той може да е малка буква, ако все още сме в елемента или друго - ако вече не сме
                }
                while (char.IsLower(sym) && elStartPos + elLen < reactionLenght);                                       // Четенето на елемента продължава докато има малки букви или докато не се достигне до края на уравнението

                string element = reaction.Substring(elStartPos, elLen);                                                 // Въз основа на началната позиция и дължината се отделя подниза на елемента
                if (!elements.Contains(element)) elements.Add(element);                                                 // Отделеният елемент се добавя в списъка, но само ако го няма там

                elStartPos += elLen;                                                                                    // Прескача се към позицията след последния символ на елемента
                elLen = 0;                                                                                              // Изчиства се променливата за дължината на елемента

                while (!char.IsUpper(sym) && elStartPos < reactionLenght)                                               // Търсия се главна буква т.е. началото на следващия елемент, ако не е достигнат края на реакцията
                {
                    elStartPos++;                                                                                       // Актуализира се променливата за начало на елемента
                    if (elStartPos < reactionLenght) sym = reaction[elStartPos];                                        // и променливата за текущия символ от низа на реакцията
                }
            }

            return elements.Count;                                                                                      // Освен списък с елементи, методът връща и техния брой
        }

        public int CountLeftPartMolecules(string reaction)                                                              // Броя на вещества от лявата част (спрямо стрелката) на уравнението
        {
            char[] arrowSeparator = new char[] { '' };                                                                 // Стрелката служи за разделител 
            string[] parts = reaction.Split(arrowSeparator);                                                            // На нейна база се разделя реакцията
            string leftPart = parts[0];                                                                                 // Отделя се лявата част от реакцията

            char[] plusSeparator = new char[] { '+' };
            string[] leftPartMolecules = leftPart.Split(plusSeparator);                                                 // Тя се разделя на съставящите я вещества на база на знака "плюс"

            return leftPartMolecules.Length;                                                                            // Изброяват се получените части (участващи вещества)
        }

        public int[,] CreateMatrix(List<string> elements, List<string> molecules, int leftPartMolCnt)                   // Въз основа на реакцията се съставя матрица на линейна система от уравнения
        {
            int rowsCount = elements.Count;                                                                             // Броят на редовете е равен на броя на участващите химични елементи в реакцията
            int colsCount = molecules.Count;                                                                            // броят на колоните е равен на броя на веществата/коефициентите (те са неизвестните)
            int[,] matrix = new int[rowsCount, colsCount];                                                              // С указаните размери се създавава празна матрица

            int row = 0;                                                                                                // Позицията на текущия ред от матрицата
            foreach (string element in elements)                                                                        // Обхождат се елементите от списъка
            {
                int elementLength = element.Length;                                                                     // Броя на символите в елемента

                int col = 0;                                                                                            // Позицията на текущата колона от матрицата
                foreach (string molecule in molecules)
                {
                    if (!molecule.Contains(element)) matrix[row, col] = 0;                                              // Ако молекулата не съдържа елемента в марицата се записва 0
                    else
                    {
                        string moleculeWithoutBrackets = molecule;                                                      // Променлива за формулата на съединението без скоби
                        if (molecule.Contains("(")) moleculeWithoutBrackets = OpenBrackets(molecule);                   // Премахват се скобите от съединението

                        int moleculeLength = moleculeWithoutBrackets.Length;                                            // Броя на символите в молекулата

                        int searchStartPos = 0;                                                                         // Позицията, от която започва търсенето на елемента в молекулата
                        int elementPos = moleculeWithoutBrackets.IndexOf(element, searchStartPos);                      // Позицията, на която е намерено съвпадение (не е сигурно, обаче, че съвпадението е търсеният елемент)

                        while (elementPos != -1)                                                                        // Прави се обхождане до края на молекулата, понеже елементът може да се среща няколко пъти
                        {
                            if (elementPos < moleculeLength - elementLength)                                            // Ако намереното съвпадение не се намира в края на формулата
                            {
                                char nextSym = moleculeWithoutBrackets[elementPos + elementLength];                     // Следващият символ, след намереното съвпадение

                                if (char.IsDigit(nextSym))                                                              // Ако е цифра - съвпадението е търсения елемент и се намираме в началото на неговия индекс
                                {
                                    matrix[row, col] += (col < leftPartMolCnt) ?                                        // Индексът се превръща в положително иил отрицателно число, в зависимост от позицията си спрямо стрелката в уравнението
                                       ReadAndConvertIndexToNumber(elementPos + elementLength, moleculeWithoutBrackets) :
                                       -ReadAndConvertIndexToNumber(elementPos + elementLength, moleculeWithoutBrackets);
                                }
                                else if (char.IsUpper(nextSym))                                                         // Ако е главна буква - съвпадението е търсения елемент и се намираме на следващия
                                {
                                    matrix[row, col] += (col < leftPartMolCnt) ? 1 : -1;                                // В този случай няма индекс, което означава, че той е 1 или -1 според позицията си спрямо стрелката
                                }

                                searchStartPos = elementPos + elementLength;                                            // Макар и елементът да е намерен, търсенето продължава след него нататък във формулата 
                                elementPos = moleculeWithoutBrackets.IndexOf(element, searchStartPos);                  // защото може той да се среща няколко пъти 
                            }
                            else                                                                                        // Ако намереното съвпадение е последния символ от молекулата, със сигурност е елемента
                            {
                                matrix[row, col] += (col < leftPartMolCnt) ? 1 : -1;                                    // Също така със сигурност няма индекс, т.е. той е плюс или минус 1
                                elementPos = -1;                                                                        // Указва се, че търсенето трябва да спре
                            }
                        } 
                    }
                    col++;
                }
                row++;
            }

            return matrix;                                                                                              // Връща се съставената матрица
        }

        private int ReadAndConvertIndexToNumber(int pos, string molecule)                                               // Индексът се прочита и се превръща в цяло число
        {
            int len = molecule.Length;                                                                                  // Определя се дължината на низа на текущото вещество
            if (pos == len) return 1;                                                                                   // Когато молекулата завършва със скоба, след която няма нищо
            
            string num = string.Empty;                                                                                  // Променливата, в която ще се образува числото съответстващо на индекса
            char sym = molecule[pos];                                                                                   // Текущия символ от индекса
            
            while (char.IsDigit(sym) && pos < len)                                                                      // Докато текущия символ е цифра или не сме излезли от низа на веществото
            {
                num += sym;                                                                                             // Текущият символ се добавя към низа за числото
                pos++;                                                                                                  // Минава се на следващата позиция
                if (pos < len) sym = molecule[pos];                                                                     // и се прочита символа, който е там
            }
            return (num != string.Empty) ? Convert.ToInt32(num) : 1;                                                    // Връща се конструираното число или единица, ако няма индекс след скобите
        }

        private string OpenBrackets(string molecule)                                                                    // Премахват се малките скоби от записва на веществото
        {
            int pos = molecule.IndexOf(')') + 1;                                                                        // Позицията след затварящата скоба
            int indexAfterBrackets = ReadAndConvertIndexToNumber(pos, molecule);                                        // От нея нататък се прочита индекса, който е зад скобите

            pos = molecule.IndexOf('(');                                                                                // Позицията на отварящата скоба
            molecule = molecule.Remove(pos, 1);                                                                         // Премахва се отварящата скоба

            char sym = molecule[pos];                                                                                   // Прочита се символът, който идва на нейно място след като бъде изтрита, т.е. символа на първия елемент от тези, които са били в скобите
            char nextSym = molecule[pos + 1];                                                                           // Следващият символ
            do                                                                                                          // Чете се до достигане на затварящата скоба
            {
                if (indexAfterBrackets != 1)                                                                            // Ако след скобите има индекс, който е по-голям от 1, тогава индексите на елементите в скобите се умножават по него
                {
                    string indexToPush = string.Empty;                                                                  // Низ, където ще се конструира индекса, който трябва да се вмъкне след всеки елемент между скобите

                    if (char.IsDigit(sym))                                                                              // Ако сме попаднали на вътрешен (между скобите) индекс
                    {
                        int index = ReadAndConvertIndexToNumber(pos, molecule);                                         // Той се прочита и превръща в цяло число
                        int len = Convert.ToString(index).Length;                                                       // Преди да се умножи се определя неговата дължина
                        index *= indexAfterBrackets;                                                                    // Умножава се по външния индекс (зад скобите)

                        molecule = molecule.Remove(pos, len);                                                           // Вътрешният индекс се премахва от записа
                        pos -= len;                                                                                     // Понеже е изтрит подниз се актуализира текущата позиция
                        indexToPush = Convert.ToString(index);                                                          // Умноженият индекс се превръща в низ
                        molecule = molecule.Insert(pos + 1, indexToPush);                                               // и се вмъква на актуализираната позиция
                        pos += indexToPush.Length;                                                                      // Позицията отново се актуализира
                    }
                    else if (char.IsUpper(nextSym) || nextSym == ')')                                                   // Ако следва друг елемент или затваряща скоба
                    {
                        indexToPush = Convert.ToString(indexAfterBrackets);                                             // вътрешният индекс е 1 и направо (без умножение)
                        molecule = molecule.Insert(pos + 1, indexToPush);                                               // ще се вмъкне външният индекс
                        pos += indexToPush.Length;
                    }
                }

                pos++;                                                                                                  // Минава се към следваща позиция
                sym = molecule[pos];                                                                                    // Прочита се символът, който е там
                if (pos < molecule.Length - 1) nextSym = molecule[pos + 1];                                             // и следващия след него
            }
            while (sym != ')');

            molecule = molecule.Remove(pos, 1);                                                                                 // След като е достигната затварящата скоба, тя се премахва от веществото,
            if (indexAfterBrackets != 1) molecule = molecule.Remove(pos, Convert.ToString(indexAfterBrackets).Length);          // а след нея и външният индекс
            return molecule;                                                                                                    // Връща се запис без скоби и с извършени умножения на индексите 
        }

        public string MergeCoefficientsAndMolecules(int[] coefficients, List<string> molecules, int leftPartMoleculesCount)
        {
            string reaction = string.Empty;                                                                                     // Низът, в който ще се състави изравнената реакция
            
            int cnt = coefficients.Length;                                                                                      // Броят на коефициентите (веществата) от реакцията
            for (int pos = 0; pos < cnt; pos++)                                                                                 // Обхождат се един по един
            {
                int coef = coefficients[pos];                                                                                   // Прочита се числото на текущия коефициент
                string coefStr = Convert.ToString(coef);                                                                        // Превръща се в низ
                string mol = molecules[pos];                                                                                    // Прочита се съответната му молекула (която ще е зад него)

                if (pos > 0 && pos != leftPartMoleculesCount) reaction += " + ";                                                // Ако не сме в началото на уравнението или на мястото, където ще е стрелката се първо слага знакът "плюс" 
                else if (pos == leftPartMoleculesCount) reaction += "  ";                                                      // Иначе ако сме на мястото за стрелката се слага тя
                if (coef > 1) reaction += coefStr + " ";                                                                        // След това, ако коефициента е по-голям от 1 се добавя и той
                reaction += mol;                                                                                                // и накрая самото вещество
            }

            return reaction;                                                                                                    // Връща се сглобената реакция
        }
    }
}

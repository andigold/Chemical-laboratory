using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace ChemLab
{
    public partial class FmStoichiometry : Form
    {
        bool eventProcessed;                                                                                                            // За избягване рекурсивното извикване на събитийните методи
        
        public FmStoichiometry()
        {
            InitializeComponent();

            CreateHeaderRow();                                                                                                          // Създава заглавния ред на таблицата
            eventProcessed = false;                                                                                                     // Събитието промяна на текста не е обработено
        }

        private void btBalance_Click(object sender, System.EventArgs e)
        {
            Format format = new Format();
            
            string unbalancedReaction = tbReaction.Text;                                                                                // Неизравенетата реакция
            unbalancedReaction = unbalancedReaction.Replace('=', '');                                                                  // Знакът "равно" се заменя със стрелка за да може да се използват методите на класа "Balance"
            unbalancedReaction = format.RemoveMultipleSpaces(unbalancedReaction);                                                       // Премахват се интервалите, ако са повече от един на дадено място
            unbalancedReaction = format.AddSpacesAroundPlusAndArrowIfNoSuch(unbalancedReaction);                                        // Добавят се интервали около знаците плюс и стрелка, ако има нужда

            Validator validator = new Validator();
            string valResult = validator.ValidateReaction(unbalancedReaction);                                                          // Реакцията се проверява за грешки при въвеждане
            if (valResult != string.Empty)
            {
                MessageBox.Show(valResult, "Грешка в реакцията", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Balance balance = new Balance();

                List<string> molecules;                                                                                                 // Списък на веществата участващи в реакцията
                int moleculesCount = balance.SplitAndCountMolecules(unbalancedReaction, out molecules);                                 // и техният брой

                List<string> elements;                                                                                                  // Списък на участващите химични елементи
                balance.SplitAndCountElements(unbalancedReaction, out elements);

                int leftPartMoleculesCount = balance.CountLeftPartMolecules(unbalancedReaction);                                        // Броят на веществата в лявата част на химичната реакция
                int[,] matrix = balance.CreateMatrix(elements, molecules, leftPartMoleculesCount);                                      // Матрицата на уравненията, чийто колони ще се използват за изчисляване на общата моларна маса на веществото

                Algebra algebra = new Algebra();
                int[] coefficients = algebra.CalcSolution(matrix);                                                                      // Изчислените коефициенти

                if (coefficients != null)
                {
                    string balancedReaction = balance.MergeCoefficientsAndMolecules(coefficients, molecules, leftPartMoleculesCount);   // Изравнената реакция

                    rtbReaction.Clear();                                                                                                // Предварително текстовата кутия се изичства
                    format.ShowBalancedReaction(balancedReaction, rtbReaction);                                                         // и в нея се показва форматиранта изравнена реакция

                    tlpStoichiometry.Controls.Clear();                                                                                  // Изчистват се всички контроли от таблицата под изравнената реакция
                    tlpStoichiometry.RowStyles.Clear();                                                                                 // също и добавените редове
                    tlpStoichiometry.RowCount = 1;                                                                                      // Установява се началната стойност за броя редове на таблицата
                    CreateHeaderRow();
                    lbInfo.Visible = false;
                    FillTableLayoutPanel(elements, molecules, matrix, coefficients);                                                    // Таблицата се попълва за новата реакция
                }
                else MessageBox.Show("Реакцията не може да бъде еднозначно изравнена!", "ХимЛаб", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void CreateHeaderRow()                                                                                          // Създава се заглавният ред на таблицата
        {
            CreateAndAddLabel("lbCompound", "Вещество\n");
            CreateAndAddLabel("lbCoefficient", "Коефициент\n");
            CreateAndAddLabel("lbMolarMass", "Молна маса\n(g/mol)");
            CreateAndAddLabel("lbMoles", "Количество вещество\n(mol)");
            CreateAndAddLabel("lbWeight", "Маса\n(g)");
        }

        private void CreateAndAddLabel(string name, string text)                                                                // Създава етикет по зададено име и текст в него
        {
            Label label = new Label();
            label.Name = name;
            label.Width = 180;
            label.Height = 35;
            label.Font = new Font("Arial Narrow", 10);
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleCenter;                                                                    // Центрира текста вътре в етикета
            label.Anchor = AnchorStyles.None;                                                                                   // Етикетът не се закача към никоя от стените на клетката за да стои в центъра
            tlpStoichiometry.Controls.Add(label);                                                                               // Етикетът се добавя към панела
        }

        private void CreateAndAddTextBox(string name, EventHandler eventHandler)                                                // Създава текстова кутия по зададено име и име на метод, който ще обработи събитието смяна на текста
        {
            TextBox textBox = new TextBox();
            textBox.Name = name;
            textBox.Width = 125;
            textBox.Height = 35;
            textBox.Font = new Font("Arial Narrow", 10);
            textBox.Anchor = AnchorStyles.None;
            textBox.TextChanged += new EventHandler(eventHandler);                                                              // Закача се събитийния метод към текстовата кутия
            tlpStoichiometry.Controls.Add(textBox);
        }
        
        private void tbMoles_TextChanged(object sender, EventArgs e)                                                            // При промяна на текста в коя да е текстова кутия за брой молове (количество вещество)
        {
            if (!eventProcessed)                                                                                                // Отначало се приема, че събитието не е обработено и това позволява да започне обработката 
            {
                eventProcessed = true;                                                                                          // Веднага след това се приема, че събитието е обработено за да не могат да се задействат рекурсивните извиквания, които се намират малко по-долу

                TextBox tb = (TextBox)sender;                                                                                   // Текстовата кутия, в която се пише
                string text = tb.Text;                                                                                          // Текстът в нея
                if (text != string.Empty)                                                                                       // Ако има текст се извършват изчисления
                {
                    double moles;
                    bool isNumber = double.TryParse(text, out moles);                                                           // Прочита се стойността на въведените молове (докъдето е стигнато)
                    if (isNumber)                                                                                               // Изчисленията се извършват само, ако въведеното е число
                    {
                        string tbMolesName = tb.Name;                                                                           // Името на текстовата кутия
                        int underlinePos = tbMolesName.IndexOf('_');                                                            // От него се отделя поредният и номер,
                        string tbMolesNumStr = tbMolesName.Substring(underlinePos + 1);                                         // който се намира зад подчертаващата черта
                        int tbMolesNum = Convert.ToInt32(tbMolesNumStr);                                                        // и се превръща в цяло число

                        string lbCoefficientName = "lbCoefficient_" + tbMolesNumStr;                                            // С така определеният номер се знае реда, в който се намираме
                        Label label = (Label)tlpStoichiometry.Controls[lbCoefficientName];                                      // и се прочита етикета съдържащ стойността на съответния коефициент
                        double coef = Convert.ToDouble(label.Text);

                        double multiplier = moles / coef;                                                                       // Изчислява се множителя, с който ще се умножават останалите коефициенти за да се изчислят моловете за останалите вещества, така че да се запази съотношението спрямо въведеното

                        int rowsCount = tlpStoichiometry.RowCount;
                        for (int row = 1; row < rowsCount; row++)                                                               // Обхождат се всички редове
                        {
                            if (row != tbMolesNum)                                                                              // Ако се намираме на ред, който е раличен от текущия
                            {
                                lbCoefficientName = "lbCoefficient_" + Convert.ToString(row);                                   // се прочита неговият коефициент
                                label = (Label)tlpStoichiometry.Controls[lbCoefficientName];
                                coef = Convert.ToDouble(label.Text);

                                tbMolesName = "tbMoles_" + Convert.ToString(row);
                                TextBox tbMoles = (TextBox)tlpStoichiometry.Controls[tbMolesName];
                                moles = coef * multiplier;                                                                      // Изчисляват се моловете чрез текущия коефициент и множителя
                                tbMoles.Text = Convert.ToString(moles);                                                         // и се записват в съответната текстова кутия
                            }

                            string lbMolarMassName = "lbMolarMass_" + Convert.ToString(row);
                            label = (Label)tlpStoichiometry.Controls[lbMolarMassName];
                            double molarMass = Convert.ToDouble(label.Text);                                                    // За всички редове се прочита молната маса на поредното вещество

                            string tbWeightName = "tbWeight_" + Convert.ToString(row);
                            TextBox tbWeight = (TextBox)tlpStoichiometry.Controls[tbWeightName];
                            double weight = molarMass * moles;                                                                  // и се изчислява маса на веществото според количеството вещество и молната му маса 
                            tbWeight.Text = Convert.ToString(weight);
                        }
                    }
                }

                eventProcessed = false;                                                                                         // След като всички текстови кутии са получили стойностите си обработката на събитието може да се позволи
            }
        }

        private void tbWeight_TextChanged(object sender, EventArgs e)                                                           // При промяна на текста в коя да е текстова кутия за масата на веществото
        {
            if (!eventProcessed)
            {
                eventProcessed = true;
                
                TextBox tb = (TextBox)sender;
                string text = tb.Text;
                if (text != string.Empty)
                {
                    double weight;
                    bool isNumber = double.TryParse(text, out weight);
                    if (isNumber)
                    {
                        string tbWeightName = tb.Name;
                        int underlinePos = tbWeightName.IndexOf('_');
                        string tbWeightNumStr = tbWeightName.Substring(underlinePos + 1);
                        int tbWeightNum = Convert.ToInt32(tbWeightNumStr);                                                      // Променената маса на веществото

                        string lbMolarMassName = "lbMolarMass_" + Convert.ToString(tbWeightNumStr);
                        Label label = (Label)tlpStoichiometry.Controls[lbMolarMassName];
                        double molarMass = Convert.ToDouble(label.Text);                                                        // Съответната й молна маса

                        string tbMolesName = "tbMoles_" + Convert.ToString(tbWeightNumStr);
                        TextBox tbMoles = (TextBox)tlpStoichiometry.Controls[tbMolesName];
                        double moles = weight / molarMass;                                                                      // От тях двете се определя количеството вещество и се записва в съответната текстова кутия
                        tbMoles.Text = Convert.ToString(moles);

                        string lbCoefficientName = "lbCoefficient_" + tbWeightNumStr;
                        label = (Label)tlpStoichiometry.Controls[lbCoefficientName];
                        double coef = Convert.ToDouble(label.Text);                                                             // Коефициента,

                        double multiplier = moles / coef;                                                                       // чрез който както в предния метод се определя множителя

                        int rowsCount = tlpStoichiometry.RowCount;
                        for (int row = 1; row < rowsCount; row++)
                        {
                            if (row != tbWeightNum)                                                                             // Изчисленията се правят за останалите редове, понеже в реда, в който става въвеждането сме изчислили количестовто вещество и няма какво друго да се изчислява
                            {
                                lbCoefficientName = "lbCoefficient_" + Convert.ToString(row);
                                label = (Label)tlpStoichiometry.Controls[lbCoefficientName];
                                coef = Convert.ToDouble(label.Text);                                                            // Коефициента

                                tbMolesName = "tbMoles_" + Convert.ToString(row);
                                tbMoles = (TextBox)tlpStoichiometry.Controls[tbMolesName];
                                moles = coef * multiplier;                                                                      // Количеството вещество
                                tbMoles.Text = Convert.ToString(moles);

                                lbMolarMassName = "lbMolarMass_" + Convert.ToString(row);
                                label = (Label)tlpStoichiometry.Controls[lbMolarMassName];
                                molarMass = Convert.ToDouble(label.Text);                                                       // Молната маса на веществото

                                tbWeightName = "tbWeight_" + Convert.ToString(row);
                                TextBox tbWeight = (TextBox)tlpStoichiometry.Controls[tbWeightName];
                                weight = molarMass * moles;                                                                     // Масата на веществото
                                tbWeight.Text = Convert.ToString(weight);
                            }
                        }
                    }
                }

                eventProcessed = false;
            }
        }

        private void FillTableLayoutPanel(List<string> elements, List<string> molecules, int[,] matrix, int[] coefficients)
        {
            int moleculesCount = molecules.Count;                                                                               // Броят на веществата участващи в реакцията
            for (int row = 1; row <= moleculesCount; row++)                                                                     // За всяко вещество се създава нов ред в таблицата
            {
                tlpStoichiometry.RowCount++;                                                                                    // като се актуализира променливата съдържаща общия брой на редовете
                RowStyle rowStyle = new RowStyle(SizeType.Absolute, 40);                                                        // определя се стил за реда, който ще се добави
                tlpStoichiometry.RowStyles.Add(rowStyle);                                                                       // и се добавя празен ред

                string name = "lbCompound_" + Convert.ToString(row);
                string text = molecules[row - 1];
                CreateAndAddLabel(name, text);

                name = "lbCoefficient_" + Convert.ToString(row);
                text = Convert.ToString(coefficients[row - 1]);
                CreateAndAddLabel(name, text);

                name = "lbMolarMass_" + Convert.ToString(row);
                text = CalculateCompoundMolarMass(row - 1, elements, matrix);
                CreateAndAddLabel(name, text);

                name = "tbMoles_" + Convert.ToString(row);
                CreateAndAddTextBox(name, tbMoles_TextChanged);
                
                name = "tbWeight_" + Convert.ToString(row);
                CreateAndAddTextBox(name, tbWeight_TextChanged);
            }
        }

        private string CalculateCompoundMolarMass(int moleculePos, List<string> elements, int[,] matrix)                        // Изчислява молната маса на веществото
        {
            Element element = new Element();
            CultureInfo ci = new CultureInfo("en-US");                                                                          // Понеже молните маси на елементите са записани с точка, вместо с десетична запетая във файла с данните се използва американска нотация за дробните числа

            int row = 0;                                                                                                        // Текущият ред от матрицата
            double compoundMass = 0;                                                                                            // Променливата, в която ще се изчисли молната маса
            foreach (string elmt in elements)                                                                                   // Обхождат се всички елементи, които участват в реакцията, понеже редовете на матрицата съответстват на тях
            {
                string elementMassStr = element.SearchMassBySymbol(elmt);                                                       // За поредния химичен елемент се прочита от файла неговата молна маса
                double elementMass = double.Parse(elementMassStr, ci);                                                          // и от низ се превръща в дробно число

                compoundMass += elementMass * Math.Abs(matrix[row, moleculePos]);                                               // След това се добавя умножена по стойноста от текущия ред и колоната съответстваща на текущото съединение от матрицата
                row++;                                                                                                          // Минава се към следващия ред от матрицата
            }

            return Convert.ToString(compoundMass);                                                                              // Изчислената молна маса се връща като низ  
        }
    }
}

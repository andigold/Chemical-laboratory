using System;
using System.Windows.Forms;
using System.IO;

namespace ChemLab
{
    public partial class FmAddReaction : Form
    {
        public FmAddReaction()
        {
            InitializeComponent();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            Format format = new Format();
            Validator validator = new Validator();
            
            string firstReactantFormula = tbFirstReactant.Text;                                                                         // Формулата на първия реагент
            firstReactantFormula = format.RemoveMultipleSpaces(firstReactantFormula);                                                   // Премахват се интервалите, ако са повече от един на дадено място
            firstReactantFormula = TrimDiatomicIndex(firstReactantFormula);                                                             // Премахват се индексите от двуатомните елементи

            string valResult = validator.ValidateMolecule(firstReactantFormula);                                                        // Проверява се формулата на първия реагент
            if (valResult != string.Empty)
            {
                MessageBox.Show(valResult, "Грешка във формулата на първия реагент", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbFirstReactant.Focus();
            }
            else
            {
                string secondReactantFormula = tbSecondReactant.Text;                                                                   // Формулата на втория реагент
                secondReactantFormula = format.RemoveMultipleSpaces(secondReactantFormula);
                secondReactantFormula = TrimDiatomicIndex(secondReactantFormula);

                valResult = validator.ValidateMolecule(secondReactantFormula);                                                          // Проверява се формулата на втория реагент
                if (valResult != string.Empty)
                {
                    MessageBox.Show(valResult, "Грешка формулата на втория реагент", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbSecondReactant.Focus();
                }
                else
                {
                    string userProducts = tbProducts.Text;                                                                              // Продуктите от реакцията
                    userProducts = format.RemoveMultipleSpaces(userProducts);
                    userProducts = format.AddSpacesAroundPlusAndArrowIfNoSuch(userProducts);                                            // Добавят се интервали около знаците плюс, ако има нужда

                    valResult = validator.ValidateRightPart(userProducts);                                                              // Проверяват се продуктите на реакцията
                    if (valResult != string.Empty)
                    {
                        MessageBox.Show(valResult, "Грешка при продуктите на реакцията", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbProducts.Focus();
                    }
                    else
                    {
                        string firstReactantFolder = Directory.GetCurrentDirectory() + "\\Reactions\\" + firstReactantFormula;          // Папката с реакциите за първия реагент (ако съществува)
                        string secondReactantFile = firstReactantFolder + "\\" + secondReactantFormula + ".txt";                        // Текстовият файл за втория реагент (ако съществува) от папката на първия 

                        if (Directory.Exists(firstReactantFolder)) WriteReactionIfNotExists(secondReactantFile, userProducts);          // Проверява се дали папката съществува и ако съществува се вика метода за запис на продуктите
                        else                                                                                                            // Ако не съществува 
                        {
                            string secondReactantFolder = Directory.GetCurrentDirectory() + "\\Reactions\\" + secondReactantFormula;    // се разменят местата на първия и втория реагент
                            string firstReactantFile = secondReactantFolder + "\\" + firstReactantFormula + ".txt";

                            if (Directory.Exists(secondReactantFolder)) WriteReactionIfNotExists(firstReactantFile, userProducts);      // и отново се проверява
                            else                                                                                                        // Ако и в този случай не съществува
                            {
                                Directory.CreateDirectory(firstReactantFolder);                                                         // Папката се създава със символа на първия реагент
                                WriteProductsToFile(secondReactantFile, userProducts);                                                  // и се извикава методът, който записва продуктите
                            }
                        }

                        tbFirstReactant.Clear();                                                                                        // Изчиства се текстовото поле за първия реагент
                        tbSecondReactant.Clear();                                                                                       // Изчиства се текстовото поле за втория реагент
                        tbProducts.Clear();                                                                                             // Изчиства се текстовото поле за продуктите

                        tbFirstReactant.Focus();                                                                                        // Курсорът се премества в полето за първия реагент за добавяне на нова реакция
                    }
                }
            }
        }

        private string TrimDiatomicIndex(string formula)                                                                    // Премахва индекса 2 от двуатомните елементи
        {
            int posOfLastSymbol = formula.Length - 1;
            return (formula == "O2" || formula == "N2" || formula == "H2" || formula == "F2" || formula == "Cl2" || formula == "Br2" || formula == "I2") ? formula.Remove(posOfLastSymbol) : formula;
        }

        private void WriteReactionIfNotExists(string file, string userProducts)                                             // Записва продуктите в текстовия файл
        {
            if (File.Exists(file))                                                                                          // Ако файлът съществува
            {
                string[] userProductsSplitted = SplitReactionRightPart(userProducts);                                       // Разделя продуктите от въведената реакция
                int userProductsSplittedCount = userProductsSplitted.Length;                                                // и определя техния брой
                
                bool productsExists = false;                                                                                // Дали продуктите ги има отпреди
                string[] lines = File.ReadAllLines(file);                                                                   // Прочитат се всички редове от текстовия файл
                foreach (string line in lines)
                {
                    string[] currentProductsSplitted = SplitReactionRightPart(line);                                        // като всеки един ред също се разделя на отделни продукти
                    int currentProductsSplittedCount = currentProductsSplitted.Length;                                      // определя се броят им

                    bool productsAreEqual = true;                                                                           // Дали са едни и същи въведените продукти с тези от текущия ред
                    if (currentProductsSplittedCount == userProductsSplittedCount)                                          // Ако двата броя съответстват проверката започва
                    {
                        for (int pos = 0; pos < currentProductsSplittedCount; pos++)                                        // Двете групи (на въведените и от текущия ред) се сравняват продукт по продукт
                        {
                            if (currentProductsSplitted[pos] != userProductsSplitted[pos]) productsAreEqual = false;        // Ако се достигне до два различни продукта - това означава, че не съвпадат
                        }
                    }
                    else productsAreEqual = false;                                                                          // Също и ако двата броя са различни

                    if (productsAreEqual) productsExists = true;                                                            // Ако продуктите съвпаднат - означава, че реакцията е въвеждана отпреди
                }

                if (productsExists) MessageBox.Show("Реакцията не е добавена, понеже вече съществува.", "ХимЛаб", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else WriteProductsToFile(file, userProducts);                                                               // Ако я няма се добавят нейните продукти към предходните
            }
            else WriteProductsToFile(file, userProducts);                                                                   // Ако файлът не съществува се създава и в него се записват продуктите
        }

        private string[] SplitReactionRightPart(string rightPart)                                                           // Разделя дясната част от реакцията на отделни продукти
        {
            char[] separators = new char[] { '+', ' ' };                                                                    // като за разделители използва знаците "плюс" и интервал
            string[] products = rightPart.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return products;
        }

        private void WriteProductsToFile(string file, string products)                                                      // Създава текстов файл (ако го няма) или го отваря (ако го има) записва продуктите в него 
        {
            bool fileExists = File.Exists(file);                                                                            // Дали файлът съществува
            using (StreamWriter sw = new StreamWriter(file, true))
            {
                if (fileExists) sw.WriteLine();                                                                             // Ако съществува преминава на нов ред преди да запише продуктите
                sw.Write(products);                                                                                         // записва ги
            }

            MessageBox.Show("Реакцията е въведена.", "ХимЛаб", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

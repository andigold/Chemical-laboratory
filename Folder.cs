using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace ChemLab
{
    class Folder
    {
        public List<string> LoadReactants()                                                                 // Зарежда всички реагенти, за които има въведена информация
        {
            string reactionsFolder = Directory.GetCurrentDirectory() +"\\Reactions";                        // Папката с химичните реакции
            string[] folders = Directory.GetDirectories(reactionsFolder);                                   // Зарежда ги в масив от низове с пълните пътища до папките, заданта част от които са символите на реагентите

            List<string> reactants = new List<string>();
            foreach (string folder in folders)                                                              // Обхожда масива низ по низ
            {                                                                                               // Стандартно търсене:
                int lastIndexOfBackSlash = folder.LastIndexOf('\\');                                        // Определя позицията на последната обърната наклонена черта
                string reactant = folder.Remove(0, lastIndexOfBackSlash + 1);                               // и зарежда динамичния списък като отрязва началната част до намерената последна наклонена черта
                reactants.Add(reactant);
                                                                                                            // Разширено търсене:
                string[] files = Directory.GetFiles(folder);                                                // Прочита файловете от текущата папка
                foreach (string file in files)                                                              // За всеки един файл
                {
                    lastIndexOfBackSlash = file.LastIndexOf('\\');
                    reactant = file.Remove(0, lastIndexOfBackSlash + 1);                                    // Премахва пътя до него от името му
                    int lastDotIndex = reactant.LastIndexOf('.');
                    reactant = reactant.Remove(lastDotIndex, 4);                                            // Премахва разширенитето от името му

                    if (!reactants.Contains(reactant)) reactants.Add(reactant);
                }
            }

            return reactants;
        }

        public Image LoadButtonImageBySymbol(string symbol, bool enabled)
        {
            string fileName = Directory.GetCurrentDirectory() +"\\Buttons\\" + symbol + "_";
            fileName += (enabled) ? "active" : "inactive";
            fileName += ".png";

            Image img = Image.FromFile(fileName);
            return img;
        }

        public List<string> LoadSecondReactants(string firstReactantFormula)                                                // Въз основа на избрания, зарежда веществата, които реагират с него
        {
            List<string> secondReactants = new List<string>();                                                              // Създава празен списък, където ще се заредят названията на реагентите извлечени от пътищата

            string firstReactantFolder = Directory.GetCurrentDirectory() + "\\Reactions\\" + firstReactantFormula;          // и от него намира папката с реагентите
            if (Directory.Exists(firstReactantFolder))                                                                      // Стандартно търсене:
            {
                string[] files = Directory.GetFiles(firstReactantFolder);                                                   // прочита всички файлове от папката
                foreach (string file in files)
                {
                    int lastIndexOfBackSlash = file.LastIndexOf('\\');
                    string secondReactant = file.Remove(0, lastIndexOfBackSlash + 1);

                    int dotIndex = secondReactant.IndexOf('.');                                                             // Намира точката в името на файла
                    secondReactant = secondReactant.Remove(dotIndex);                                                       // за да премахне разширението след нея
                    secondReactants.Add(secondReactant);
                }
            }
                                                                                                                            // Разширено търсене:
            string reactionsFolder = Directory.GetCurrentDirectory() + "\\Reactions";                                       // Определя пътя до папката с реагентите
            string[] folders = Directory.GetDirectories(reactionsFolder);                                                   // Прочита всички папки

            foreach (string folder in folders)                                                                              // Обхожда ги една по една
            {
                string[] files = Directory.GetFiles(folder);                                                                // като зарежда пътищата до файловете във всяка
                foreach (string file in files)                                                                              // Обхожда файловете във всяка папка
                {
                    int lastIndexOfBackSlash = file.LastIndexOf('\\');
                    string currentFirstReactantSymbol = file.Remove(0, lastIndexOfBackSlash + 1);
                    int lastDotIndex = currentFirstReactantSymbol.LastIndexOf('.');
                    currentFirstReactantSymbol = currentFirstReactantSymbol.Remove(lastDotIndex, 4);                        // като отделя името на първия реагент

                    if (currentFirstReactantSymbol == firstReactantFormula)                                                 // Ако отделеното име съвпада с избраното
                    {
                        lastIndexOfBackSlash = folder.LastIndexOf('\\');                                                    // се прочита името на съдържата го папка
                        string secondReactant = folder.Remove(0, lastIndexOfBackSlash + 1);                                 // От нея се определя името на втория реагент
                        if (!secondReactants.Contains(secondReactant)) secondReactants.Add(secondReactant);                 // и ако го няма в списъка се добавя
                    }
                }
            }

            return secondReactants;
        }

        public List<string> LoadProducts(string firstReactantFormula, string secondReactantFormula)         // Зарежда продуктите по зададени реагенти
        {            
            List<string> products = new List<string>();
            string pathToReactions = Directory.GetCurrentDirectory() + "\\Reactions\\";
            string fileName = pathToReactions + firstReactantFormula + "\\" + secondReactantFormula + ".txt";

            if (File.Exists(fileName))                                                                      // Стандартна проверка - първият реагент има своя папка, в която вторият реагент е текстов файл
            {
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    products.Add(line);
                }
            }
            else                                                                                            // Разширена проверка - вторият реагент има папка, а първият е текстов файл в нея
            {
                fileName = pathToReactions + secondReactantFormula + "\\" + firstReactantFormula + ".txt";
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    products.Add(line);
                }
            }

            return products;
        }
    }
}

using System;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace ChemLab
{
    class Element
    {
        public enum GeneralCategories { метал, неметал };                                                                   // Двете основни групи химични елементи

        public string GetFirstReactantFormula()                                                                             // Прочита формулата на първия избран реагент
        {
            FmTable fmTable = (FmTable)Application.OpenForms["FmTable"];
            return fmTable.FirstReactantFormula;
        }

        public void SetFirstReactantFormula(string formula)                                                                // Записва формулата на първия избран реагент
        {
            FmTable fmTable = (FmTable)Application.OpenForms["FmTable"];
            fmTable.FirstReactantFormula = formula;
        }

        public string GetSecondReactantFormula()                                                                            // Прочита формулата на втория избран реагент
        {
            FmTable fmTable = (FmTable)Application.OpenForms["FmTable"];
            return fmTable.SecondReactantFormula;
        }

        public void SetSecondReactantFormula(string formula)                                                                // Записва формулата на втория избран реагент
        {
            FmTable fmTable = (FmTable)Application.OpenForms["FmTable"];
            fmTable.SecondReactantFormula = formula;
        }
        
        public string DetermineGeneralCategory(string symbol)                                                               // По зададен символ определя основната категория на реагента
        {
            string category = SearchCategoryBySymbol(symbol);

            if (category == "алкален метал" || category == "алкалоземен метал" || category == "преходен метал" ||
                category == "друг метал" || category == "лантанид" || category == "актинид") return Convert.ToString(GeneralCategories.метал);

            else return Convert.ToString(GeneralCategories.неметал);
        }

        public XmlDocument LoadAllElements()                                                                                // Прочита и зарежда всички химични елементи от файла
        {
            XmlDocument elements = new XmlDocument();
            elements.Load(Directory.GetCurrentDirectory() + "/elements.xml");
            return elements;
        }

        public bool IsDiatomic(string symbol)                                                                               // По зададен символ проверява дали елемента е двуатомен
        {
            return (symbol == "O" || symbol == "N" || symbol == "H" || symbol == "F" || symbol == "Cl" || symbol == "Br" || symbol == "I") ? true : false;
        }

        public bool Exists(string symbol)                                                                                   // Проверява дали подаденият символ е химичен елемент
        {
            XmlDocument elements = LoadAllElements();
            XmlNode node = elements.SelectSingleNode("elements/element[symbol='" + symbol + "']");
            return (node == null) ? false : true;
        }

        public string SearchCategoryByNumber(string number)                                                                 // По зададен номер определя групата на химичния елемент
        {
            XmlDocument elements = LoadAllElements();
            XmlNode categoryNode = elements.SelectSingleNode("elements/element[number=" + number + "]/category");
            string category = categoryNode.InnerText;
            return category;
        }
        
        public string SearchCategoryBySymbol(string symbol)                                                                 // По зададен символ на химичен елемент определя неговата група
        {
            XmlDocument elements = LoadAllElements();
            XmlNode categoryNode = elements.SelectSingleNode("elements/element[symbol='" + symbol + "']/category");
            string category = categoryNode.InnerText;
            return category;
        }

        public string SearchSymbolByNumber(string number)                                                                   // По зададен атомен номер на химичен елемент определя неговата група
        {
            XmlDocument elements = LoadAllElements();
            XmlNode symbolNode = elements.SelectSingleNode("elements/element[number=" + number + "]/symbol");
            string symbol = symbolNode.InnerText;
            return symbol;
        }

        public string SearchNameByNumber(string number)                                                                     // По зададен атомен номер на химичен елемент определя неговия атомен номер
        {
            XmlDocument elements = LoadAllElements();
            XmlNode nameNode = elements.SelectSingleNode("elements/element[number=" + number + "]/name");
            string name = nameNode.InnerText;
            return name;
        }

        public string SearchNameBySymbol(string symbol)                                                                     // По зададен символ на химичен елемент определя неговото название
        {
            XmlDocument elements = LoadAllElements();
            XmlNode nameNode = elements.SelectSingleNode("elements/element[symbol='" + symbol + "']/name");
            string name = nameNode.InnerText;
            return name;
        }

        public string SearchNumberBySymbol(string symbol)                                                                   // По зададен символ на химичен елемент определя неговия атомен номер
        {
            XmlDocument elements = LoadAllElements();
            XmlNode numberNode = elements.SelectSingleNode("elements/element[symbol='" + symbol + "']/number");
            string number = numberNode.InnerText;
            return number;
        }

        public string SearchMassBySymbol(string symbol)                                                                     // По зададен символ на химичен елемент определя неговата моларна маса
        {
            XmlDocument elements = LoadAllElements();
            XmlNode massNode = elements.SelectSingleNode("elements/element[symbol='" + symbol + "']/mass");
            string mass = massNode.InnerText;
            return mass;
        }
    }
}

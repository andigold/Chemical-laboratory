using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace ChemLab
{
    class Compound
    {
        public XmlDocument LoadAllCompounds()                                                                       // Прочита и зарежда всички химични съединения от файла
        {
            XmlDocument compounds = new XmlDocument();
            compounds.Load(Directory.GetCurrentDirectory() + "/compounds.xml");
            return compounds;
        }

        public List<string> LoadAllFormulasByType(string type)                                                      // Прочита и зарежда всички формули на химични съединения от даден тип
        {
            XmlDocument compounds = LoadAllCompounds();
            XmlNodeList formulaNodes = compounds.SelectNodes("compounds/compound[type='" + type + "']/formula");

            List<string> formulas = new List<string>();
            foreach (XmlNode formulaNode in formulaNodes)
            {
                string formula = formulaNode.InnerText;
                formulas.Add(formula);
            }
            
            return formulas;
        }

        public string SerachTypeByFormula(string formula)                                                           // Търси типа на химичното съединение по неговата формула
        {
            XmlDocument compounds = LoadAllCompounds();
            XmlNode typeNode = compounds.SelectSingleNode("compounds/compound[formula='" + formula + "']/type");
            string type = typeNode.InnerText;
            return type;
        }
        
        public string SearchNameByFormula(string formula)                                                           // Търси наименованието на химичното съединение по неговата формула
        {
            XmlDocument compounds = LoadAllCompounds();
            XmlNode nameNode = compounds.SelectSingleNode("compounds/compound[formula='" + formula + "']/name");
            string name = nameNode.InnerText;
            return name;
        }

        public string SearchFormulaByName(string name)                                                              // Търси химичната формула на съединението по неговото наименование
        {
            XmlDocument compounds = LoadAllCompounds();
            XmlNode formulaNode = compounds.SelectSingleNode("compounds/compound[name='" + name + "']/formula");
            string formula = formulaNode.InnerText;
            return formula;
        }
    }
}

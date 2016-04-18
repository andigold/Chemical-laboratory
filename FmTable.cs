using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace ChemLab
{
    public partial class FmTable : Form
    {
        private const int periodicTableHorizontalOffset = 40;                                           // Отместването на периодичната таблица спрямо лявата рамка на формата 
        private const int periodicTableVerticalOffset = 60;                                             // Отместването на периодичната таблица спрямо горната рамка на формата
        private const int buttonVerticalSpacing = 3;                                                    // Разстоянията между отделните редове
        private const int buttonHorizontalSpacing = 3;                                                  // Разстоянията между отделните колони
        private const int legendGroupsWidth = 40;                                                       // Ширината на цветните правоъгълници от легендата
        private const int legendGroupsHeight = 20;                                                      // Височината на цветните правоъгълници от легендата
        private const int legendVerticalSpacing = 30;                                                   // Разстоянията между цветните правоъгълници от легендата
        private int buttonSize;                                                                         // Размерът на бутоните
        
        private string firstReactantFormula;                                                            // Формаулата на първия избран реагент
        public string FirstReactantFormula
        {
            get { return firstReactantFormula; }
            set { firstReactantFormula = value; }
        }

        private string secondReactantFormula;                                                           // Формаулата на втория избран реагент
        public string SecondReactantFormula
        {
            get { return secondReactantFormula; }
            set { secondReactantFormula = value; }
        }

        private bool controlsAlreadyCreated;                                                            // Създадена ли са: фона, периодичната таблица, полето за реакциите и легендата
        
        public FmTable()
        {
            InitializeComponent();
            controlsAlreadyCreated = false;
        }

        private void tsmiReaction_Click(object sender, EventArgs e)
        {
            if (!controlsAlreadyCreated)
            {
                BackgroundImage = ChemLab.Properties.Resources.background;                              // Сменя се фоновото изображение
                CreateAndShowPeriodicTable();                                                           // Зарежда се информацията за всички химични елементи, създадат се бутоните и се показва таблицата
                rtbReaction.Visible = true;                                                             // Показва се контролата за извеждане на изравнените реакции
                ShowLegends();                                                                          // Показва надписите за периодите и групите

                controlsAlreadyCreated = true;
            }

            firstReactantFormula = string.Empty;                                                        // Изчиства се първият избран реагент
            secondReactantFormula = string.Empty;                                                       // Изчиства се вторият избран реагент

            FmFirstReactantMenu fmFirstReactantMenu = new FmFirstReactantMenu();
            fmFirstReactantMenu.ShowDialog();                                                           // Показва се прозорецът с менюто за избор на първия реагент
        }

        private void tsmiAdd_Click(object sender, EventArgs e)                                          // Добавяне на реакция
        {
            FmAddReaction fmAddReaction = new FmAddReaction();
            fmAddReaction.ShowDialog();
        }

        private void tsmiStoichiometry_Click(object sender, EventArgs e)                                // Изравняване на химични реакции и изчисляване на количествени задачи
        {
            FmStoichiometry fmStoichiometry = new FmStoichiometry();
            fmStoichiometry.ShowDialog();
        }

        private void tsmiHelp_Click(object sender, EventArgs e)                                         // Помощна информация
        {
            FmHelp fmHelp = new FmHelp();
            fmHelp.ShowDialog();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)                                        // Информация за авторите на проекта
        {
            FmAuthors fmAuthors = new FmAuthors();
            fmAuthors.ShowDialog();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button bn = (Button)sender;
            string bnName = bn.Name;                                                                    // Прочита се името на натиснатия бутон
            string number = bnName.Substring(2);                                                        // и се премахва представката "bn" от него за да се определи поредния номер на избрания елемент

            Element element = new Element();
            if (firstReactantFormula == string.Empty)                                                   // Ако все още няма избран първи реагент
            {
                firstReactantFormula = element.SearchSymbolByNumber(number);                            // Определя се символът на първия избран реагент по номера на бутона
                
                FmSecondReactantMenu fmSecondReactantMenu = new FmSecondReactantMenu();
                fmSecondReactantMenu.ShowDialog();                                                      // Показва се прозорецът с менюто за избор на втория реагент
            }
            else                                                                                        // Иначе - потребителят е избрал група от менюто и му се дава възможност да посочи елемент от избраната група
            {
                secondReactantFormula = element.SearchSymbolByNumber(number);                           // Определя се символът на втоприя избран реагент по номера на бутона
                LoadBalanceAndShowEquations();                                                          // Прочитат се реакциите, изравняват се и се показват
            }
        }

        private void pb_MouseEnter(object sender, EventArgs e)                                          // Активира бутоните таблицата съответстващи на посчената група в легендата
        {
            if (firstReactantFormula == string.Empty && secondReactantFormula == string.Empty)          // Методът работи само при начално стартиране на таблицата, когато няма нищо избрано
            {
                PictureBox pb = (PictureBox)sender;                                                     // Посочената картинка от легендата
                string pbName = pb.Name;                                                                // Прочита се името й
                pbName = pbName.Remove(0, 2);

                Element element = new Element();

                foreach (Control control in Controls)
                {
                    if (control is Button)                                                              // Обхождат се всички бутони
                    {
                        Button bn = (Button)control;
                        string bnName = bn.Name;
                        string number = bnName.Substring(2);

                        if (!number.Contains('_'))
                        {
                            string category = element.SearchCategoryByNumber(number);                   // Определя се категорията на всеки един от тях и ако съвпада с категорията на избраната картинка бутона се активира
                            string symbol = element.SearchSymbolByNumber(number);

                            if (category == "алкален метал" && pbName == "AlkaliMetals" ||
                                category == "алкалоземен метал" && pbName == "AlkalineEarthMetals" ||
                                category == "преходен метал" && pbName == "TransitionMetals" ||
                                category == "друг метал" && pbName == "OtherMetals" ||
                                category == "неметал" && pbName == "OtherNonmetals" ||
                                category == "халогенен" && pbName == "Halogens" ||
                                category == "инертен газ" && pbName == "NobleGases" ||
                                category == "лантаноид" && pbName == "Lanthanides" ||
                                category == "актиноид" && pbName == "Actinides") EnableButton(bn, symbol);

                            else DisableButton(bn, symbol);                                             // иначе се деактивира
                        }
                    }
                }
            }
        }

        private void pb_MouseLeave(object sender, EventArgs e)                                          // Връща началното състояние на бутоните, когато мишката напусне съответната картинка от легендата
        {
            if (firstReactantFormula == string.Empty && secondReactantFormula == string.Empty)
            {
                ReloadPeriodicTable();
            }
        }

        private void CreateAndShowPeriodicTable()                                                       // Зарежда се информацията за всички химични елементи, създадат се бутоните и се показва таблицата
        {
            Element element = new Element();
            XmlDocument xml = element.LoadAllElements();                                                // Зареждат се данните за всички елементи
            XmlNodeList nodes = xml.SelectNodes("/elements/element");

            Folder folder = new Folder();
            List<string> reactants = folder.LoadReactants();                                            // Зареждат се символите на всички реагенти, за които има въведена информация
       
            foreach (XmlNode node in nodes)                                                             // Обхождат се последователно
            {
                string number = node["number"].InnerText;                                               // поредния номер
                string symbol = node["symbol"].InnerText;                                               // символа

                Button bn = new Button();                                                               // Създава се нов бутон
                bn.Name = "bn" + number;                                                                // като му се задава име според атомния номер на поредния елемент
                
                int buttonSizeHeight = (Height - periodicTableVerticalOffset - rtbReaction.Height - lbGroupIA.Height - 10) / 10;
                buttonSizeHeight -= buttonVerticalSpacing;
                int buttonSizeWidth = (Width - periodicTableHorizontalOffset - legendGroupsWidth - lbAlkalineEarthMetals.Width - 10) / 18;
                buttonSizeWidth -= buttonHorizontalSpacing;
                buttonSize = (buttonSizeHeight < buttonSizeWidth) ? buttonSizeHeight : buttonSizeWidth;
                
                bn.Size = new Size(buttonSize, buttonSize);

                bn.FlatStyle = FlatStyle.Flat;                                                          // Премахва се 3D стила за бутона
                bn.BackgroundImageLayout = ImageLayout.Zoom;                                            // Картинакта за фон изпълва целия бутон
                bn.Location = DetermineButtonLocation(number);                                          // Определя се неговата позиция

                if (reactants.Contains(symbol)) EnableButton(bn, symbol);                               // Ако има въведени реакции за съответния елемент - бутона е активен
                else DisableButton(bn, symbol);                                                         // иначе - неактивен

                bn.Click += new EventHandler(btn_Click);                                                // Свърза се с метода за обработка на събитието "натискане на бутона"
                Controls.Add(bn);                                                                       // и се добавя към контролите на формата
            }

            DetermineCaptionsAndLegendLocation();                                                       // Определят се позициите на надписите и легендата
        }

        public void ReloadPeriodicTable()
        {
            Element element = new Element();
            
            Folder folder = new Folder();
            List<string> reactants = folder.LoadReactants();                                            // Зареждат се символите на всички реагенти, за които има въведена информация

            foreach (Control control in Controls)
            {
                if (control is Button)
                {
                    Button btn = (Button)control;
                    string buttonName = btn.Name;                                                       // се взема неговото име
                    string number = buttonName.Substring(2);                                            // като от него се отделя атомния номер
                    if (!number.Contains('_'))                                                          // Пропускат се бутоните за лантанидите и актинидите
                    {
                        string symbol = element.SearchSymbolByNumber(number);                           // По номера в името на бутона се определя символа на елемента, на когото съответства бутона
                        if (reactants.Contains(symbol)) EnableButton(btn, symbol);                      // Ако елементът присъства в списъка с реагенти се активира бутона
                        else DisableButton(btn, symbol);                                                // иначе се деактивира
                    }
                }
            }

            firstReactantFormula = string.Empty;                                                        // Изчиства се първият избран реагент
            secondReactantFormula = string.Empty;                                                       // Изчиства се вторият избран реагент
        }

        public void EnableButtonsByReactionsAndGeneralCategory(List<string> reactants, string generalCategory)                          // Активира бутоните според въведените реагенти и избраната група от менюто
        {
            Element element = new Element();

            foreach (Control control in Controls)                                                                                       // Обхождат се всички контроли на формата
            {
                if (control is Button)                                                                                                  // Проверява се дали всеки един от тях е бутон и ако е бутон
                {
                    Button btn = (Button)control;
                    string buttonName = btn.Name;                                                                                       // се взема неговото име
                    string number = buttonName.Substring(2);                                                                            // като от него се отделя атомния номер
                    if (!number.Contains('_'))                                                                                          // Пропускат се бутоните за лантанидите и актинидите
                    {
                        string symbol = element.SearchSymbolByNumber(number);                                                           // По номера в името на бутона се определя символа на елемента, на когото съответства бутона
                        string currentGeneralCategory = element.DetermineGeneralCategory(symbol);                                       // и основната група на този елемент
                        if (reactants.Contains(symbol) && currentGeneralCategory == generalCategory) EnableButton(btn, symbol);         // Ако за бутона има въведен реагент и основната група на елемента съответства на избраната от менюто се активира бутона
                        else DisableButton(btn, symbol);                                                                                // Иначе се деактивира
                    }
                }
            }
        }
        
        public void EnableButtonsByFirstReactantAndGeneralCategory(string generalCategory)                                              // Активира бутоните според избраната група от менюто и елементите, с които реагира първият избран реагент
        {
            Element element = new Element();

            Folder folder = new Folder();
            List<string> secondReactants = folder.LoadSecondReactants(firstReactantFormula);                                            // Зареждат се всички елементи, с които реагира първият избран реагент
            
            if (generalCategory == Convert.ToString(Element.GeneralCategories.неметал))                                                 // Ако избраната група е "неметал"
            {
                secondReactants.Remove("H");                                                                                            // се премахват водорода и кислорода,
                secondReactants.Remove("O");                                                                                            // понеже за тях има специални опции в менюто
            }
            
            foreach (Control control in Controls)                                                                                       // Обхождат се всички контроли на формата
            {
                if (control is Button)                                                                                                  // Проверява се дали всеки един от тях е бутон и ако е бутон
                {
                    Button btn = (Button)control;
                    string buttonName = btn.Name;                                                                                       // се взема неговото име
                    string number = buttonName.Substring(2);                                                                            // като от него се отделя атомния номер
                    if (!number.Contains('_'))                                                                                          // Пропускат се бутоните за лантанидите и актинидите
                    {
                        string symbol = element.SearchSymbolByNumber(number);                                                           // По номера в името на бутона се определя символа на елемента, на когото съответства бутона
                        string currentGeneralCategory = element.DetermineGeneralCategory(symbol);                                       // и основната група на този елемент
                        if (secondReactants.Contains(symbol) && currentGeneralCategory == generalCategory) EnableButton(btn, symbol);   // Ако елемента присъства в списъка от елементи, с които реагира първият избран и неговата основна група съответства на избраната от менюто се активира бутона
                        else DisableButton(btn, symbol);                                                                                // Иначе се деактивира
                    }
                }
            }
        }

        private void EnableButton(Button bn, string symbol)                                                // Активира съответния бутон
        {
            if (!bn.Enabled || bn.Image == null)
            {
                bn.Enabled = true;

                Folder folder = new Folder();
                bn.BackgroundImage = folder.LoadButtonImageBySymbol(symbol, true);
            }
        }
        
        private void DisableButton(Button bn, string symbol)                                               // Деактивира съответния бутон
        {
            if (bn.Enabled)
            {
                bn.Enabled = false;

                Folder folder = new Folder();
                bn.BackgroundImage = folder.LoadButtonImageBySymbol(symbol, false);
            }
        }

        private Point DetermineButtonLocation(string number)                                                // Определя позицията на бутона в таблицата
        {
            int num = 0, linearPos = 0;                                                                     // Поредният номер и линейната позиция в таблицата 

            if (!number.Contains('_'))                                                                      // Ако бутона не е за една от двете специални групи химични елементи
            {
                num = Convert.ToInt32(number) - 1;                                                          // Номерът му се намаля с единица, защото броенето в табличната матрица започва от 0

                if (num < 1) linearPos = num;                                                               // Поради специфичната форма на периодичната таблица се налага разделяне на
                else if (num > 0 && num < 4) linearPos = num + 16;                                          // бутоните на групи според празните места, които се прескачат или
                else if (num > 3 && num < 12) linearPos = num + 26;                                         // или особеностите на тяхното позициониране
                else if (num > 11 && num < 56) linearPos = num + 36;
                else if (num > 55 && num < 71) linearPos = num + 91;
                else if (num > 70 && num < 88) linearPos = num + 22;
                else if (num > 87 && num < 103) linearPos = num + 77;
                else if (num > 102 && num < 118) linearPos = num + 8;
            }
            else if (number == "57_71") linearPos = 92;                                                     // Двата специални бутона за групите на лантанидите
            else if (number == "89_103") linearPos = 110;                                                   // и актинидите

            int row = linearPos / 18;                                                                       // Според линейната позиция се определя реда
            int col = linearPos % 18;                                                                       // и колоната

            Point pos = new Point(periodicTableHorizontalOffset, periodicTableVerticalOffset);              // Позицията се определя въз основа на отместването на таблицата спрямо формата
            pos.X += col * (buttonSize + buttonHorizontalSpacing);                                          // и се мащабира според размерите на бутона и разстоянието между бутоните
            pos.Y += row * (buttonSize + buttonVerticalSpacing);

            return pos;                                                                                     // Връща се изчислената позиция за координатите на горния ляв ъгъл на бутона
        }

        private void DetermineCaptionsAndLegendLocation()                                                   // Опреселя се позицията на надписите и елементите от легендата
        {
            lbPeriod1.Location = new Point((periodicTableHorizontalOffset - lbPeriod1.Width) / 2, periodicTableVerticalOffset + 0 * (buttonSize + buttonVerticalSpacing) + (buttonSize - lbPeriod1.Height) / 2);
            lbPeriod2.Location = new Point((periodicTableHorizontalOffset - lbPeriod2.Width) / 2, periodicTableVerticalOffset + 1 * (buttonSize + buttonVerticalSpacing) + (buttonSize - lbPeriod2.Height) / 2);
            lbPeriod3.Location = new Point((periodicTableHorizontalOffset - lbPeriod3.Width) / 2, periodicTableVerticalOffset + 2 * (buttonSize + buttonVerticalSpacing) + (buttonSize - lbPeriod3.Height) / 2);
            lbPeriod4.Location = new Point((periodicTableHorizontalOffset - lbPeriod4.Width) / 2, periodicTableVerticalOffset + 3 * (buttonSize + buttonVerticalSpacing) + (buttonSize - lbPeriod4.Height) / 2);
            lbPeriod5.Location = new Point((periodicTableHorizontalOffset - lbPeriod5.Width) / 2, periodicTableVerticalOffset + 4 * (buttonSize + buttonVerticalSpacing) + (buttonSize - lbPeriod5.Height) / 2);
            lbPeriod6.Location = new Point((periodicTableHorizontalOffset - lbPeriod6.Width) / 2, periodicTableVerticalOffset + 5 * (buttonSize + buttonVerticalSpacing) + (buttonSize - lbPeriod6.Height) / 2);
            lbPeriod7.Location = new Point((periodicTableHorizontalOffset - lbPeriod7.Width) / 2, periodicTableVerticalOffset + 6 * (buttonSize + buttonVerticalSpacing) + (buttonSize - lbPeriod7.Height) / 2);

            lbGroupIA.Location = new Point(periodicTableHorizontalOffset + 0 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupIA.Width) / 2, 0 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupIA.Height) / 2);
            lbGroupIIA.Location = new Point(periodicTableHorizontalOffset + 1 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupIIA.Width) / 2, 1 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupIIA.Height) / 2);
            lbGroupIIIB.Location = new Point(periodicTableHorizontalOffset + 2 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupIIIB.Width) / 2, 3 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupIIIB.Height) / 2);
            lbGroupIVB.Location = new Point(periodicTableHorizontalOffset + 3 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupIVB.Width) / 2, 3 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupIVB.Height) / 2);
            lbGroupVB.Location = new Point(periodicTableHorizontalOffset + 4 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVB.Width) / 2, 3 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupVB.Height) / 2);
            lbGroupVIB.Location = new Point(periodicTableHorizontalOffset + 5 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVIB.Width) / 2, 3 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupVIB.Height) / 2);
            lbGroupVIIB.Location = new Point(periodicTableHorizontalOffset + 6 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVIIB.Width) / 2, 3 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupVIIB.Height) / 2);
            
            int rightDashPos = 7;
            do
            {
                lbGroupVIII.Text = lbGroupVIII.Text.Insert(1, "─");
                rightDashPos++;
                lbGroupVIII.Text = lbGroupVIII.Text.Insert(rightDashPos, "─");
            }
            while (lbGroupVIII.Width < 3 * buttonSize);
            lbGroupVIII.Location = new Point(periodicTableHorizontalOffset + 8 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVIII.Width) / 2, 3 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupVIII.Height) / 2);
            
            lbGroupIB.Location = new Point(periodicTableHorizontalOffset + 10 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupIB.Width) / 2, 3 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupIB.Height) / 2);
            lbGroupIIB.Location = new Point(periodicTableHorizontalOffset + 11 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupIIB.Width) / 2, 3 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupIIB.Height) / 2);
            lbGroupIIIA.Location = new Point(periodicTableHorizontalOffset + 12 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupIIIA.Width) / 2, 1 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupIIIA.Height) / 2);
            lbGroupIVA.Location = new Point(periodicTableHorizontalOffset + 13 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupIVA.Width) / 2, 1 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupIVA.Height) / 2);
            lbGroupVA.Location = new Point(periodicTableHorizontalOffset + 14 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVA.Width) / 2, 1 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupVA.Height) / 2);
            lbGroupVIA.Location = new Point(periodicTableHorizontalOffset + 15 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVIA.Width) / 2, 1 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupVIA.Height) / 2);
            lbGroupVIIA.Location = new Point(periodicTableHorizontalOffset + 16 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVIIA.Width) / 2, 1 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupVIIA.Height) / 2);
            lbGroupVIIIA.Location = new Point(periodicTableHorizontalOffset + 17 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVIIIA.Width) / 2, 0 * (buttonSize + buttonVerticalSpacing) + (periodicTableVerticalOffset + MainMenuStrip.Height - lbGroupVIIIA.Height) / 2);
            
            pbButtonLegend.Width = 6 * buttonSize;
            pbButtonLegend.Height = 2 * buttonSize;
            pbButtonLegend.Location = new Point(periodicTableHorizontalOffset + 4 * (buttonSize + buttonHorizontalSpacing) + (buttonSize - lbGroupVIB.Width) / 2, periodicTableVerticalOffset);
            
            pbAlkaliMetals.Height = legendGroupsHeight;
            pbAlkaliMetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 0 * (legendGroupsHeight + legendVerticalSpacing));
            pbAlkalineEarthMetals.Height = legendGroupsHeight;
            pbAlkalineEarthMetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 1 * (legendGroupsHeight + legendVerticalSpacing));
            pbTransitionMetals.Height = legendGroupsHeight;
            pbTransitionMetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 2 * (legendGroupsHeight + legendVerticalSpacing));
            pbOtherMetals.Height = legendGroupsHeight;
            pbOtherMetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 3 * (legendGroupsHeight + legendVerticalSpacing));
            pbOtherNonmetals.Height = legendGroupsHeight;
            pbOtherNonmetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 4 * (legendGroupsHeight + legendVerticalSpacing));
            pbHalogens.Height = legendGroupsHeight;
            pbHalogens.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 5 * (legendGroupsHeight + legendVerticalSpacing));
            pbNobleGases.Height = legendGroupsHeight;
            pbNobleGases.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 6 * (legendGroupsHeight + legendVerticalSpacing));
            pbLanthanides.Height = legendGroupsHeight;
            pbLanthanides.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 7 * (legendGroupsHeight + legendVerticalSpacing));
            pbActinides.Height = legendGroupsHeight;
            pbActinides.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing), periodicTableVerticalOffset + 8 * (legendGroupsHeight + legendVerticalSpacing));

            pbAlkaliMetals.Width = legendGroupsWidth;
            lbAlkaliMetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 0 * (legendGroupsHeight + legendVerticalSpacing) - (lbAlkaliMetals.Height - pbAlkaliMetals.Height) / 2);
            pbAlkalineEarthMetals.Width = legendGroupsWidth;
            lbAlkalineEarthMetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 1 * (legendGroupsHeight + legendVerticalSpacing) - (lbAlkalineEarthMetals.Height - pbAlkalineEarthMetals.Height) / 2);
            pbTransitionMetals.Width = legendGroupsWidth;
            lbTransitionMetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 2 * (legendGroupsHeight + legendVerticalSpacing) - (lbTransitionMetals.Height - pbTransitionMetals.Height) / 2);
            pbOtherMetals.Width = legendGroupsWidth;
            lbOtherMetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 3 * (legendGroupsHeight + legendVerticalSpacing) - (lbOtherMetals.Height - pbOtherMetals.Height) / 2);
            pbOtherNonmetals.Width = legendGroupsWidth;
            lbOtherNonmetals.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 4 * (legendGroupsHeight + legendVerticalSpacing) - (lbOtherNonmetals.Height - pbOtherNonmetals.Height) / 2);
            pbHalogens.Width = legendGroupsWidth;
            lbHalogens.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 5 * (legendGroupsHeight + legendVerticalSpacing) - (lbHalogens.Height - pbHalogens.Height) / 2);
            pbNobleGases.Width = legendGroupsWidth;
            lbNobleGases.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 6 * (legendGroupsHeight + legendVerticalSpacing) - (lbNobleGases.Height - pbNobleGases.Height) / 2);
            pbLanthanides.Width = legendGroupsWidth;
            lbLanthanides.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 7 * (legendGroupsHeight + legendVerticalSpacing) - (lbLanthanides.Height - pbLanthanides.Height) / 2);
            pbActinides.Width = legendGroupsWidth;
            lbActinides.Location = new Point(periodicTableHorizontalOffset + 18 * (buttonSize + buttonHorizontalSpacing) + legendGroupsWidth, periodicTableVerticalOffset + 8 * (legendGroupsHeight + legendVerticalSpacing) - (lbActinides.Height - pbActinides.Height) / 2);
        }

        public void LoadBalanceAndShowEquations()
        {
            List<string> equations = CreateListOfBalancedEquations();                                       // Създава списък с изравнени реакции,
            ShowReactions(equations);                                                                       // показва ги
            ReloadPeriodicTable();                                                                          // и презарежда периодичната таблица
        }

        private List<string> CreateListOfBalancedEquations()                                                // Създава списък от изравнени уравнения
        {
            Element element = new Element();

            string firstMolecule = firstReactantFormula;                                                    // Низ за молекулата на първия избран реагент
            if (element.IsDiatomic(firstReactantFormula)) firstMolecule += "2";                             // Проверява дали е двуатомен и ако е такъв долепя двойка до символа му
            
            string leftFromArrow = firstMolecule + " + ";                                                   // Добавя се знак плюс след първия реагент

            string secondMolecule = secondReactantFormula;                                                  // Низ за молекулата на втория избран реагент
            if (element.IsDiatomic(secondReactantFormula)) secondMolecule += "2";                           // Проверява дали е двуатомен и добавя двойка ако е

            leftFromArrow += secondMolecule + "  ";                                                        // След втория реагент се добавя стрелката

            Folder folder = new Folder();
            List<string> products = folder.LoadProducts(firstReactantFormula, secondReactantFormula);       // Зарежда списък с продуктите от химичната реакция
            List<string> balancedReactions = new List<string>(products.Count);                              // Създава се празен списък за изравнените реакции с вместимост равна на предния списък
            foreach (string product in products)                                                            // За всеки продукт на реакцията създава химично уравнение
            {
                string unbalancedReaction = leftFromArrow;                                                  // Низ, в който ще се събере цялата неизравнена реакция
                unbalancedReaction += product;                                                              // След стрелката се добавя поредния продукт

                Balance balance = new Balance(unbalancedReaction);                                          // Реакцията се изравнява,
                string balancedReaction = balance.BalancedReaction;                                         // след което се прочита изравнената реакция

                balancedReactions.Add(balancedReaction);                                                    // и се добавя към съответния списък
            }

            return balancedReactions;                                                                       // Връща се списък от изравнени реакции
        }

        private void ShowReactions(List<string> reactions)                                                  // По зададен списък от уравнения ги показва форматирани
        {
            Element element = new Element();
            
            string firstReactantName = string.Empty;                                                       // Низ за името на първия реагент
            if (element.Exists(firstReactantFormula))                                                      // Ако той е химичен елемент
            {
                firstReactantName = element.SearchNameBySymbol(firstReactantFormula);                       // по неговият символ се определя името му
            }
            else                                                                                            // Ако е вещество
            {
                Compound compound = new Compound();
                firstReactantName = compound.SearchNameByFormula(firstReactantFormula);
            }
            
            string secondReactantName = string.Empty;                                                       // Низ за името на втория реагент
            if (element.Exists(secondReactantFormula))
            {
                secondReactantName = element.SearchNameBySymbol(secondReactantFormula);
            }
            else
            {
                Compound compound = new Compound();
                secondReactantName = compound.SearchNameByFormula(secondReactantFormula);
            }

            Format format = new Format();
            
            string conjunction = "с";                                                                                           // Съюзът "с" между наименованията на първия и втория реагент 
            char secondReactantNameFirstLetter = secondReactantName[0];                                                         // Първата буква от названието на втория реагент
            if (secondReactantNameFirstLetter == 'с' || secondReactantNameFirstLetter == 'з') conjunction += "ъс";              // Ако вторият започва със "с" или "з" съюзът става "със"

            rtbReaction.AppendText("Реакции на " + firstReactantName + " " + conjunction + " " + secondReactantName + ":\n");   // Изписва участващите реагенти над групата от уравнения

            foreach (string reaction in reactions)                                                                              // Прочита уравненията едно по едно
            {
                format.ShowBalancedReaction(reaction, rtbReaction);                                                             // Форматира по подходяща начин изравнената реакция
                rtbReaction.AppendText("\n");                                                                                   // Преминава се на нов ред след текущото уравнение
            }

            rtbReaction.AppendText("\n");                                                                                       // Пропуска се един празен ред между две съседни групи уравнения
            rtbReaction.ScrollToCaret();                                                                                        // Автоматично се превърта надолу до последното изписано, за да може да се види 
        }

        private void ShowLegends()                                                                                              // Показва надписите за периодите и групите, както и легендата
        {
            foreach (Control control in Controls)
            {
                if (control is Label || control is PictureBox)
                {
                    control.Visible = true;
                }
            }
        }
    }
}

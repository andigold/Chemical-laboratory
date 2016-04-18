using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Drawing.Drawing2D;

namespace ChemLab
{
    public partial class FmSecondReactantMenu : Form
    {
        private const int centralMenuItemCircleRadius = 50;                                                                         // Размера на окръжността за централния елемент от менюто
        private const int centralMenuItemFontSize = 11;                                                                             // Големината на шрифта за него
        
        private const int innerMenuCircleRadius = 40;                                                                               // Размера на окръжността за вътрешото меню
        private const int innerMenuRadius = 200;                                                                                    // Разстоянието от центъра до окръжностите за вътрешното меню
        private const int innerMenuFontSize = 11;                                                                                   // Под "вътрешно" меню се разбира това, което е около централния реагент

        private const int outerMenuCircleRadius = 40;                                                                               // Размера на окръжността за външното меню
        public int OuterMenuCircleRadius
        {
            get { return outerMenuCircleRadius; }
        }
        
        private const int outerMenuRadius = 100;                                                                                    // Разстоянието от центъра до окръжностите за външното меню
        public int OuterMenuRadius
        {
            get { return outerMenuRadius; }
        }
        
        private const int outerMenuFontSize = 10;                                                                                   // Под "външно" меню се разбира това, което е около избраната група
        public int OuterMenuFontSize
        {
            get { return outerMenuFontSize; }
        }

        private const int menuLinesThickness = 2;                                                                                   // Дебелина на свързващите линии за менютата

        private Color centralMenuItemForeColor = Color.Wheat;                                                                       // Цвят на текста за централния елемент от менюто
        private Color centralMenuItemBackColor = Color.Maroon;                                                                      // Цвят на кръга за него
        private Color centralMenuItemBorderColor = Color.Olive;                                                                     // Цвят на окръжността за него

        private Color otherMenuItemsForeColor = Color.Maroon;                                                                       // Цвят на текста за останалите елементи от менютата
        private Color otherMenuItemsBackColor = Color.BurlyWood;                                                                    // Цвят на кръга за тях
        private Color otherMenuItemsBorderColor = Color.Olive;                                                                      // Цвят на окръжността за тях

        private Color menuLinesColor = Color.Olive;                                                                                 // Цвят на свързващите линии за менютата

        private Graphics graphics;                                                                                                  // Графичния обект на формата за рисуване върху нея
        private Pen menuLinesPen;                                                                                                   // Инструмент за чертане на линните
        private Point innerMenuCenter;                                                                                              // Координатите на центъра за вътрешното меню

        private Point outerMenuCenter;                                                                                              // Координатите на центъра за външното меню
        public Point OuterMenuCenter
        {
            set { outerMenuCenter = value; }
        }

        private List<Point> innerMenuCenters;                                                                                       // Списък на центровете на елементите на върешното меню

        private List<Point> outerMenuCenters;                                                                                       // Списък на центровете на елементите за външното меню
        public List<Point> OuterMenuCenters
        {
            set { outerMenuCenters = value; }
        }

        private bool choiceHasBeenMade;                                                                                             // Определя дали потребителят е направил избор от менюто
        public bool ChoiceHasBeenMade
        {
            set { choiceHasBeenMade = value; }
        }

        public FmSecondReactantMenu()
        {
            InitializeComponent();

            choiceHasBeenMade = false;                                                                                              // Инициализация на променливата за това дали потребителят е направил своя избор
            graphics = CreateGraphics();                                                                                            // Графичният обект на формата се инициализира за да може да се рисува върху него 
            
            menuLinesPen = new Pen(menuLinesColor, menuLinesThickness);                                                             // Инициализира се цвета на свързващите линните за менютата
            menuLinesPen.DashStyle = DashStyle.DashDotDot;                                                                          // Определя стила на пунктира на линията
            
            outerMenuCenters = new List<Point>();                                                                                   // Инициализира се списъка на центровете на елементите за външното меню
        }

        private void FmSecondReactantMenu_Load(object sender, EventArgs e)                                                          // Зарежда и показва "вътрешното" меню
        {
            Element element = new Element();
            string symbol = element.GetFirstReactantFormula();                                                                      // Символът на първия реагент

            innerMenuCenter = new Point(ClientSize.Width / 2, ClientSize.Height / 2);                                               // Определя позицията на центъра на формата
            DrawCentralReactant(innerMenuCenter);                                                                                   // Показва реагента, който е в средата

            List<string> innerMenuOptions = LoadInnerMenuOptions(symbol);                                                           // Зарежда надписите за "вътрешното" меню
            innerMenuCenters = new List<Point>(innerMenuOptions.Count);                                                             // Определя се броя на центровете за вътрешното меню
            
            DrawCircleMenu(innerMenuCenter, innerMenuRadius, innerMenuCircleRadius, 
                innerMenuFontSize, innerMenuOptions, true, true);                                                                   // По зададени: център, радиус, размери на окръжностите, размер на шрифт, надписи и дали контролът да е активен се изчертава менюто
        }

        private void FmSecondReactantMenu_Paint(object sender, PaintEventArgs e)                                                    // При събитието преначертаване на формата
        {
            DrawInnerMenuLines();                                                                                                   // първо се чертаят свързващите линии на вътрешното меню
            DrawOuterMenuLines();                                                                                                   // после на външното меню
        }

        private void FmSecondReactantMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!choiceHasBeenMade)                                                                                                 // При затваряне на формата, ако потребителят не е избрал нищо
            {
                FmTable fmTable = (FmTable)Application.OpenForms["FmTable"];                                                        // се презарежда формата с периодичната таблица
                fmTable.ReloadPeriodicTable();
            }
        }
        
        private void DrawCentralReactant(Point center)                                                                              // Показва първия реагент в центъра на формата 
        {
            Element element = new Element();
            string symbol = element.GetFirstReactantFormula();
            
            string text = string.Empty;
            if (element.Exists(symbol))
            {
                text = element.SearchNameBySymbol(symbol);
            }
            else
            {
                Compound compound = new Compound();
                text = compound.SearchNameByFormula(symbol);
            }

            RoundButton roundButton = new RoundButton(center, centralMenuItemCircleRadius, text, centralMenuItemFontSize,
                centralMenuItemForeColor, centralMenuItemBackColor, centralMenuItemBorderColor);
            roundButton.Enabled = false;                                                                                            // като определя той да е неактивен
            Controls.Add(roundButton);
        }

        private List<string> LoadInnerMenuOptions(string firstReactantFormula)                                                          // Определя опциите за "вътрешното" меню според избрания реагент
        {
            Element element = new Element();
            Compound compound = new Compound();

            Folder folder = new Folder();
            List<string> secondReactants = folder.LoadSecondReactants(firstReactantFormula);

            List<string> menuOptions = new List<string>();
            foreach (string secondReactant in secondReactants)
            {
                if (element.Exists(secondReactant))
                {
                    string generalCategory = element.DetermineGeneralCategory(secondReactant);
                    if (!menuOptions.Contains(generalCategory)) menuOptions.Add(generalCategory);
                }
                else
                {
                    string type = compound.SerachTypeByFormula(secondReactant);
                    if (!menuOptions.Contains(type)) menuOptions.Add(type);
                }
            }

            return menuOptions;
        }

        public List<string> LoadOuterMenuOptions(string innerMenuChoice)                                                            // Определя опциите за "външното" меню според избраното от "вътрешното"
        {
            Element element = new Element();
            Compound compound = new Compound();

            List<string> menuOptions = new List<string>();
            
            Folder folder = new Folder();
            string firstReactantSymbol = element.GetFirstReactantFormula();
            List<string> secondReactants= folder.LoadSecondReactants(firstReactantSymbol);                                          // Зареждат се всички елементи и вещества, с които реагира първият избран реагент

            List<string> formulas = compound.LoadAllFormulasByType(innerMenuChoice);
            foreach (string formula in formulas)
            {
                string name = compound.SearchNameByFormula(formula);
                if (secondReactants.Contains(formula)) menuOptions.Add(name);
            }

            return menuOptions;                                                                                                     // Така се получава списък с киселини или основи, с които реагира първият реагент
        }

        public void DrawCircleMenu(Point center, int menuRadius, int circleRadius, int fontSize, List<string> menuOptions, bool active, bool isInnerMenu)
        {
            int optionsCount = menuOptions.Count;                                                                                   // Броят на възможностите, т.е. колко окръжности ще се изчертаят 
            double coefToRad = Math.PI / 180;                                                                                       // Множител за превръщане от градуси в радиани, понеже тригонометричните функции работят с радиани
            double angle = 0;                                                                                                       // Инициализация (в градуси) на ъгъла на завъртане, определящ позицията на съответната окръжност

            foreach (string menuOption in menuOptions)                                                                              // Една по една се обхождат подадените опции
            {
                Point location = new Point();                                                                                       // Определя се позицията (в "полярна координатна система") на центъра на всяка окръжност
                location.X = center.X + Convert.ToInt32(menuRadius * Math.Sin(coefToRad * angle));                                  // по-точно: неговата абсциса
                location.Y = center.Y - Convert.ToInt32(menuRadius * Math.Cos(coefToRad * angle));                                  // и ордината

                if (isInnerMenu) innerMenuCenters.Add(location);                                                                    // Координатите на текущия център се добавят към списъка за центровете на вътрешното меню
                else outerMenuCenters.Add(location);                                                                                // или външното, според това коео меню се чертае

                RoundButton roundButton = new RoundButton(location, circleRadius, menuOption, fontSize,
                    otherMenuItemsForeColor, otherMenuItemsBackColor, otherMenuItemsBorderColor);
                
                Controls.Add(roundButton);

                angle += 360 / optionsCount;                                                                                        // Ъгълът се завърта, така че да застане на следващата опция
            }
        }

        private void DrawInnerMenuLines()                                                                                           // Изчертава свързващите линии на вътрешното меню
        {
            foreach (Point point in innerMenuCenters)
            {
                graphics.DrawLine(menuLinesPen, innerMenuCenter, point);
            }
        }

        private void DrawOuterMenuLines()                                                                                           // Изчертава свързващите линии на външното меню
        {
            foreach (Point point in outerMenuCenters)
            {
                graphics.DrawLine(menuLinesPen, outerMenuCenter, point);
            }
        }
    }
}

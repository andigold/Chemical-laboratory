using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace ChemLab
{
    class RoundButton : UserControl
    {
        private Color userControlBackColor = Color.Transparent;                                             // Цвета на фона за потребителския контрол като цяло

        private Color captionColor;                                                                         // Цвета на надписа
        private Color captionStandardColor;                                                                 // Цвета на надписа, когато не е посочен с мишката
        private Color captionMouseOverColor = Color.Wheat;                                                  // Цвета на надписа, когато е посочен с мишката
        
        private Color circleBackColor;                                                                      // Цвета на кръга
        private Color circleBackStandardColor;                                                              // Цвета на кръга, когато не е посчен с мишката
        private Color circleBackMouseOverColor = Color.Maroon;                                              // Цвета на кръга, когато е посочен с мишката

        private Color circleBorderColor;                                                                    // Цвета на окръжността
        private Color circleBorderStandardColor;                                                            // Цвета на окръжността, когато не е посчена с мишката
        private Color circleBorderMouseOverColor = Color.Chocolate;                                         // Цвета на окръжността, когато е посочена с мишката

        private const int circleBorderThickness = 2;                                                        // Дебелината на линията за окръжността
        private const int buttonPadding = 5;                                                                // Празна рамка между окръжността и границите на контрола за да може да се прихване събитието влизане и излизане на мишката 
        private int circleFontSize;                                                                         // Размерът на шрифта за надписа

        private Point circleCenter;                                                                         // Координати на центъра на окръжността
        private int circleRadius;                                                                           // Радиус на окръжността
        
        public RoundButton(Point center, int radius, string text, int fontSize, Color foreColor, Color backColor, Color borderColor)
        {
            circleCenter = center;
            circleRadius = radius;
            circleFontSize = fontSize;
            captionStandardColor = foreColor;
            circleBackStandardColor = backColor;
            circleBorderStandardColor = borderColor;

            Width = 2 * (circleRadius + buttonPadding);                                                     // Ширината на контрола се изчислява на база зададената големина на окръжността
            Height = Width;                                                                                 // Височината е същата като ширината

            center.Offset(-Width / 2, -Height / 2);                                                         // Изчислява се къде трябва да бъде позицията на горния ляв ъгъл на контрола понеже той се чертае спрямо нея
            Location = center;                                                                              // а тя е отместена наляво и нагоре с половината от размера на контрола спрямо неговия център, който всъщност е зададен като входен параметър за позицията му

            captionColor = captionStandardColor;
            circleBackColor = circleBackStandardColor;
            circleBorderColor = circleBorderStandardColor;
            BackColor = userControlBackColor;
            
            text = text.Replace(' ', '\n');                                                                 // Интервалите се заменят със символи за нов ред
            Text = text;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;                                                                                 // Графичният обект се взема от потребителския контрол и затова неговата координатна система се намира в самия него

            Pen circleBorderPen = new Pen(circleBorderColor, circleBorderThickness);                                        // Създава се инструмент за чертане на окръжността
            circleBorderPen.DashStyle = DashStyle.Dash;                                                                     // Определя стила на пунктира на линията
            graphics.DrawEllipse(circleBorderPen, buttonPadding, buttonPadding, 2 * circleRadius, 2 * circleRadius);        // Изчертава се окръжността

            SolidBrush circleBackBrush = new SolidBrush(circleBackColor);                                                   // Изчертава се кръгът
            graphics.FillEllipse(circleBackBrush,
                buttonPadding + circleBorderThickness, buttonPadding + circleBorderThickness,
                2 * (circleRadius - circleBorderThickness), 2 * (circleRadius - circleBorderThickness));

            SolidBrush foreBrush = new SolidBrush(captionColor);                                                            // Цвета на надписа

            Font font = new Font("Arial Narrow", circleFontSize, FontStyle.Regular);                                        // Шрифта за надписа
            SizeF captionSize = e.Graphics.MeasureString(Text, font);                                                       // Определят се размерите на надписа за да може след това той да бъде центриран в рамките на окръжността

            int captionWidth = 2 * (circleRadius - circleBorderThickness);                                                  // Широчината на правоъгълната област за надписа
            int captionHeight = 2 * (circleRadius - circleBorderThickness);                                                 // и височината
            RectangleF captionRect = new Rectangle(buttonPadding + circleBorderThickness, buttonPadding + circleBorderThickness, captionWidth, captionHeight);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;                                                                // Надписът се центрира хоризонтално
            stringFormat.LineAlignment = StringAlignment.Center;                                                            // и вертикално
            
            graphics.DrawString(Text, font, foreBrush, captionRect, stringFormat);                                          // Изписва се надписа
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (MouseInCircle(e.Location))                                                                                  // мишката е навлязла в рамките на кръга
            {
                if (circleBackColor != circleBackMouseOverColor)                                                            // и цвета на фона е различен - за да не се получава постоянно преначертаване, което е излишно
                {
                    captionColor = captionMouseOverColor;
                    circleBackColor = circleBackMouseOverColor;
                    circleBorderColor = circleBorderMouseOverColor;

                    Invalidate();                                                                                           // Изкуствено се предизвиква събитието Paint, което от своя страна извиква метода "OnPaint"
                }
                Cursor.Current = Cursors.Hand;                                                                              // Сменя се вида на курсора за мишката
            }
            else if (circleBackColor != circleBackStandardColor)                                                            // Когато мишката напусне кръга и цвета на фона е различен от стандартния се връща стандартния цвят на фона
            {
                captionColor = captionStandardColor;
                circleBackColor = circleBackStandardColor;
                circleBorderColor = circleBorderStandardColor;

                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)                                                                   // Налага се и това събитие да се обработи, понеже потребителят може да мине много бързо през контрола и да не бъде отчетено както трябва събитието "MouseMove"
        {
            if (circleBackColor != circleBackStandardColor)
            {
                captionColor = captionStandardColor;
                circleBackColor = circleBackStandardColor;
                circleBorderColor = circleBorderStandardColor;

                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (MouseInCircle(e.Location))                                                                                                  // Събитието натискане на бутон на мишката се отчита само, ако контролът е активен и мишката се намира в рамките на кръга
            {
                FmTable fmTable = (FmTable)Application.OpenForms["FmTable"];
                if (fmTable.FirstReactantFormula == string.Empty)
                {
                    FmFirstReactantMenu fmFirstReactantMenu = (FmFirstReactantMenu)Application.OpenForms["FmFirstReactantMenu"];
                    
                    if (Text == "неметал" || Text == "метал")
                    {
                        Folder folder = new Folder();
                        List<string> reactants = folder.LoadReactants();

                        string generalCategory = (Text == "метал") ? "метал" : "неметал";                                                   // Въз основа на надписа се определя основната група елементи, която потребителят е избрал
                        fmTable.EnableButtonsByReactionsAndGeneralCategory(reactants, generalCategory);                                     // Като се знае основната група се филтрират само елементите, за които има въведени реакции и са от същата група

                        fmFirstReactantMenu.ChoiceHasBeenMade = true;                                                                       // Потребителя е направил своя избор
                        fmFirstReactantMenu.Close();                                                                                        // и формата се затваря
                    }
                    else if (Text == "вода")
                    {
                        Element element = new Element();
                        element.SetFirstReactantFormula("H2O");

                        fmFirstReactantMenu.ChoiceHasBeenMade = true;
                        fmFirstReactantMenu.Close();

                        FmSecondReactantMenu fmSecondReactantMenu = new FmSecondReactantMenu();
                        fmSecondReactantMenu.ShowDialog(fmTable);
                    }
                    else if (Text == "киселина" || Text == "основа" || Text == "оксид")                                                     // Ако избраното е "киселина", "основа" или "оксид"
                    {
                        foreach (Control control in fmFirstReactantMenu.Controls)                                                           // се деактивира "вътрешното" меню
                        {
                            if (control is RoundButton)                                                                                     // т.е. всички потребителски контроли показани до момента
                            {
                                RoundButton roundButton = (RoundButton)control;
                                roundButton.Enabled = false;
                            }
                        }

                        fmFirstReactantMenu.OuterMenuCenter = circleCenter;                                                                 // Съхраняват се координатите на центъра на външното меню

                        List<string> outerMenuOptions = fmFirstReactantMenu.LoadOuterMenuOptions(Text);                                     // след това веществата, за които има въведени реакции
                        fmFirstReactantMenu.OuterMenuCenters = new List<Point>(outerMenuOptions.Count);                                     // Определя се броя на центровете за външното меню

                        fmFirstReactantMenu.DrawCircleMenu(circleCenter, fmFirstReactantMenu.OuterMenuRadius,
                            fmFirstReactantMenu.OuterMenuCircleRadius, fmFirstReactantMenu.OuterMenuFontSize,
                            outerMenuOptions, true, false);                                                                                 // и по определения нов център и списък с вещества се изчертава "външното" меню
                    }
                    else                                                                                                                    // Във всички останали случаи, потребителят е избрал опция от външното меню
                    {
                        string text = Text.Replace('\n',' ');                                                                               // Възвръща се оригиналното име на съединението (без знаците за нов ред)
                        
                        Compound compound = new Compound();
                        string formula = compound.SearchFormulaByName(text);

                        Element element = new Element();
                        element.SetFirstReactantFormula(formula);                                                                           // След като е открито съвпадение, то се записва като формула за първия реагент

                        fmFirstReactantMenu.ChoiceHasBeenMade = true;
                        fmFirstReactantMenu.Close();

                        FmSecondReactantMenu fmSecondReactantMenu = new FmSecondReactantMenu();
                        fmSecondReactantMenu.ShowDialog(fmTable);
                    }
                }
                else if (fmTable.SecondReactantFormula == string.Empty)
                {
                    FmSecondReactantMenu fmSecondReactantMenu = (FmSecondReactantMenu)Application.OpenForms["FmSecondReactantMenu"];

                    Element element = new Element();

                    if (Text == "неметал" || Text == "метал")
                    {
                        string generalCategory = (Text == "метал") ? "метал" : "неметал";                                                   // Въз основа на надписа се определя основната група елементи, която потребителят е избрал
                        string firstReactantFormula = element.GetFirstReactantFormula();                                                    // и формулата на първия реагент
                        fmTable.EnableButtonsByFirstReactantAndGeneralCategory(generalCategory);                                            // След това, като се знае първия реагент и основната група за втория се филтрират само елементите, които реагират с първия избран и са от същата основна група 

                        fmSecondReactantMenu.ChoiceHasBeenMade = true;                                                                      // Потребителя е направил своя избор
                        fmSecondReactantMenu.Close();                                                                                       // и формата се затваря
                    }
                    else if (Text == "водород" || Text == "кислород" || Text == "вода")                                                     // Ако потребителят е избрал опция с надпис водород, кислород или вода
                    {
                        switch (Text)
                        {
                            case "водород": element.SetSecondReactantFormula("H"); break;
                            case "кислород": element.SetSecondReactantFormula("O"); break;
                            case "вода": element.SetSecondReactantFormula("H2O"); break;
                        }
                        fmTable.LoadBalanceAndShowEquations();                                                                              // Понеже формулата е ясна, може направо да се премина към изравняване на реакцията

                        fmSecondReactantMenu.ChoiceHasBeenMade = true;                                                                      // и да се затвори формата с менюто
                        fmSecondReactantMenu.Close();
                    }
                    else if (Text == "киселина" || Text == "основа" || Text == "оксид")                                                     // Ако избраното е "киселина", "основа" или "оксид"
                    {
                        foreach (Control control in fmSecondReactantMenu.Controls)                                                          // се деактивира "вътрешното" меню
                        {
                            if (control is RoundButton)                                                                                     // т.е. всички потребителски контроли показани до момента
                            {
                                RoundButton roundButton = (RoundButton)control;
                                roundButton.Enabled = false;
                            }
                        }

                        fmSecondReactantMenu.OuterMenuCenter = circleCenter;                                                                // Съхраняват се координатите на центъра на външното меню

                        List<string> outerMenuOptions = fmSecondReactantMenu.LoadOuterMenuOptions(Text);                                    // след това веществата, с които реагира първият реагент
                        fmSecondReactantMenu.OuterMenuCenters = new List<Point>(outerMenuOptions.Count);                                    // Определя се броя на центровете за външното меню

                        fmSecondReactantMenu.DrawCircleMenu(circleCenter, fmSecondReactantMenu.OuterMenuRadius,
                            fmSecondReactantMenu.OuterMenuCircleRadius, fmSecondReactantMenu.OuterMenuFontSize,
                            outerMenuOptions, true, false);                                                                                 // и по определения нов център и списък с вещества се изчертава "външното" меню
                    }
                    else                                                                                                                    // Във всички останали случаи, потребителят е избрал опция от външното меню
                    {
                        string text = Text.Replace('\n', ' ');                                                                               // Възвръща се оригиналното име на съединението (без знаците за нов ред)
                        
                        Compound compound = new Compound();
                        string formula = compound.SearchFormulaByName(text);

                        element.SetSecondReactantFormula(formula);                                                                          // След като е открито съвпадение, то се записва като формула за втория реагент
                        fmTable.LoadBalanceAndShowEquations();

                        fmSecondReactantMenu.ChoiceHasBeenMade = true;
                        fmSecondReactantMenu.Close();
                    }
                }
            }
        }

        private bool MouseInCircle(Point mouseCoords)                                                                       // Проверка дали мишката се намира в рамките на кръга
        {
            int mouseX = mouseCoords.X - CircleCenterInUserControlCoordinates().X;                                          // Абсцисата на мишката се привежда към координатна система, чийто център е средата на контрола
            int mouseY = mouseCoords.Y - CircleCenterInUserControlCoordinates().Y;                                          // също и ординатата

            int mouseDistFromCenter = Convert.ToInt32(Math.Sqrt(mouseX * mouseX + mouseY * mouseY));                        // Определя се разстоянието от центъра до позицията на мишката
            return (mouseDistFromCenter < circleRadius) ? true : false;                                                     // и ако то е по-малко от радиуса на окръжността, това означава, че мишката се намира в кръга определен от нея
        }

        private Point CircleCenterInUserControlCoordinates()                                                                // Определя координатите на центъра на окръжността
        {
            Point center = new Point();
            center.X = circleRadius + buttonPadding;                                                                        // като има предвид празната рамка между окръжността и границите на потребителския контрол
            center.Y = circleRadius + buttonPadding;
            return center;
        }
    }
}

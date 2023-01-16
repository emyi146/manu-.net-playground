using DesignPatterns.Common;

namespace DesignPatterns.Creational.Factory_AbstractFactory_FactoryMethod.FactoryPattern;

class FactoryPattern
{

    public static void SampleFactoryPattern()
    {
        new NavigationBar();
        new DropdownMenu();
    }

    public class NavigationBar
    {
        public NavigationBar() => ButtonFactory.CreateButton();
    }

    public class DropdownMenu
    {
        public DropdownMenu() => ButtonFactory.CreateButton();
    }

    public class ButtonFactory
    {
        public static Button CreateButton()
        {
            return new Button { Type = "Default Button".Dump() };
        }
    }


    public class Button
    {
        public string Type { get; set; }
    }

}
using DesignPatterns.Common;

namespace DesignPatterns.Creational.Factory_AbstractFactory_FactoryMethod.FactoryPattern;

class AbstractFactoryPattern
{

    public static void SampleAbstractFactoryPattern()
    {
        new NavigationBar(new Android());
        new DropdownMenu(new Apple());
    }

    public class NavigationBar
    {
        public NavigationBar(IUiFactory factory) => factory.CreateButton();
    }

    public class DropdownMenu
    {
        public DropdownMenu(IUiFactory factory) => factory.CreateButton();
    }

    public interface IUiFactory
    {
        public Button CreateButton();
    }

    internal class Apple : IUiFactory
    {
        public Button CreateButton()
        {
            return new Button() { Type = "iOS Button".Dump() };
        }
    }

    internal class Android : IUiFactory
    {
        public Button CreateButton()
        {
            return new Button() { Type = "Android Button".Dump() };
        }
    }

    public class Button
    {
        public string Type { get; set; } = string.Empty;
    }

}

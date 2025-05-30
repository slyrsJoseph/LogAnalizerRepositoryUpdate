using System.Windows;

namespace LogAnalizerWpfClient;

public class MaterialDesignWindow : Window
{
    public MaterialDesignWindow()
    {
        // Подключение поддержки Material Design
        this.SetResourceReference(StyleProperty, "MaterialDesignWindow");
    }
    
}
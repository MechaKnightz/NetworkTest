// -----------------------------------------------------------
//  
//  This file was generated, please do not modify.
//  
// -----------------------------------------------------------
namespace EmptyKeys.UserInterface.Generated {
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.ObjectModel;
    using EmptyKeys.UserInterface;
    using EmptyKeys.UserInterface.Charts;
    using EmptyKeys.UserInterface.Data;
    using EmptyKeys.UserInterface.Controls;
    using EmptyKeys.UserInterface.Controls.Primitives;
    using EmptyKeys.UserInterface.Input;
    using EmptyKeys.UserInterface.Interactions.Core;
    using EmptyKeys.UserInterface.Interactivity;
    using EmptyKeys.UserInterface.Media;
    using EmptyKeys.UserInterface.Media.Effects;
    using EmptyKeys.UserInterface.Media.Animation;
    using EmptyKeys.UserInterface.Media.Imaging;
    using EmptyKeys.UserInterface.Shapes;
    using EmptyKeys.UserInterface.Renderers;
    using EmptyKeys.UserInterface.Themes;
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "2.6.0.0")]
    public partial class MainMenuSettingsUserControl : UserControl {
        
        private Grid e_0;
        
        private TabControl tabControl;
        
        private ComboBox comboBox;
        
        public MainMenuSettingsUserControl() {
            Style style = UserControlStyle.CreateUserControlStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            this.e_0.IsHitTestVisible = true;
            // tabControl element
            this.tabControl = new TabControl();
            this.e_0.Children.Add(this.tabControl);
            this.tabControl.Name = "tabControl";
            this.tabControl.Height = 720F;
            this.tabControl.Width = 1280F;
            this.tabControl.Margin = new Thickness(0F, 0F, 0F, 0F);
            this.tabControl.ItemsSource = Get_tabControl_Items();
            // comboBox element
            this.comboBox = new ComboBox();
            this.e_0.Children.Add(this.comboBox);
            this.comboBox.Name = "comboBox";
            this.comboBox.Height = 111.96F;
            this.comboBox.Margin = new Thickness(896F, 180F, 762F, 0F);
            this.comboBox.VerticalAlignment = VerticalAlignment.Top;
        }
        
        private static System.Collections.ObjectModel.ObservableCollection<object> Get_tabControl_Items() {
            System.Collections.ObjectModel.ObservableCollection<object> items = new System.Collections.ObjectModel.ObservableCollection<object>();
            // e_1 element
            TabItem e_1 = new TabItem();
            e_1.Name = "e_1";
            e_1.Header = "TabItem";
            // e_2 element
            Grid e_2 = new Grid();
            e_1.Content = e_2;
            e_2.Name = "e_2";
            e_2.Background = new SolidColorBrush(new ColorW(51, 51, 51, 255));
            // checkBox element
            CheckBox checkBox = new CheckBox();
            e_2.Children.Add(checkBox);
            checkBox.Name = "checkBox";
            checkBox.Margin = new Thickness(429.666F, 87F, 0F, 0F);
            checkBox.HorizontalAlignment = HorizontalAlignment.Left;
            checkBox.VerticalAlignment = VerticalAlignment.Top;
            checkBox.Content = "Fullscreen";
            items.Add(e_1);
            // e_3 element
            TabItem e_3 = new TabItem();
            e_3.Name = "e_3";
            e_3.Header = "TabItem";
            // e_4 element
            Grid e_4 = new Grid();
            e_3.Content = e_4;
            e_4.Name = "e_4";
            e_4.Background = new SolidColorBrush(new ColorW(51, 51, 51, 255));
            items.Add(e_3);
            return items;
        }
    }
}

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
            // groupBox element
            GroupBox groupBox = new GroupBox();
            e_2.Children.Add(groupBox);
            groupBox.Name = "groupBox";
            groupBox.Height = 200F;
            groupBox.Width = 200F;
            groupBox.Margin = new Thickness(10F, 10F, 0F, 0F);
            groupBox.HorizontalAlignment = HorizontalAlignment.Left;
            groupBox.VerticalAlignment = VerticalAlignment.Top;
            groupBox.Header = "Window";
            // e_3 element
            Grid e_3 = new Grid();
            groupBox.Content = e_3;
            e_3.Name = "e_3";
            // CmbResolutionSetting element
            ComboBox CmbResolutionSetting = new ComboBox();
            e_3.Children.Add(CmbResolutionSetting);
            CmbResolutionSetting.Name = "CmbResolutionSetting";
            CmbResolutionSetting.Height = 31F;
            CmbResolutionSetting.Width = 180F;
            CmbResolutionSetting.Margin = new Thickness(4F, 0F, 0F, 0F);
            CmbResolutionSetting.HorizontalAlignment = HorizontalAlignment.Left;
            CmbResolutionSetting.VerticalAlignment = VerticalAlignment.Top;
            Binding binding_CmbResolutionSetting_ItemsSource = new Binding("DisplayedResolutions");
            binding_CmbResolutionSetting_ItemsSource.Mode = BindingMode.TwoWay;
            CmbResolutionSetting.SetBinding(ComboBox.ItemsSourceProperty, binding_CmbResolutionSetting_ItemsSource);
            // checkBox element
            CheckBox checkBox = new CheckBox();
            e_3.Children.Add(checkBox);
            checkBox.Name = "checkBox";
            checkBox.Margin = new Thickness(5F, 35F, 5F, 0F);
            checkBox.VerticalAlignment = VerticalAlignment.Top;
            checkBox.Content = "Fullscreen";
            Binding binding_checkBox_IsChecked = new Binding("Fullscreen");
            binding_checkBox_IsChecked.Mode = BindingMode.TwoWay;
            checkBox.SetBinding(CheckBox.IsCheckedProperty, binding_checkBox_IsChecked);
            // BtnApplyVideoSettings element
            Button BtnApplyVideoSettings = new Button();
            e_2.Children.Add(BtnApplyVideoSettings);
            BtnApplyVideoSettings.Name = "BtnApplyVideoSettings";
            BtnApplyVideoSettings.Height = 50F;
            BtnApplyVideoSettings.Width = 200F;
            BtnApplyVideoSettings.Margin = new Thickness(0F, 0F, 10F, 10F);
            BtnApplyVideoSettings.HorizontalAlignment = HorizontalAlignment.Right;
            BtnApplyVideoSettings.VerticalAlignment = VerticalAlignment.Bottom;
            BtnApplyVideoSettings.Content = "Apply";
            Binding binding_BtnApplyVideoSettings_Command = new Binding("VideoSettingsApplyCommand");
            BtnApplyVideoSettings.SetBinding(Button.CommandProperty, binding_BtnApplyVideoSettings_Command);
            // BtnBackToMainmenu element
            Button BtnBackToMainmenu = new Button();
            e_2.Children.Add(BtnBackToMainmenu);
            BtnBackToMainmenu.Name = "BtnBackToMainmenu";
            BtnBackToMainmenu.Height = 50F;
            BtnBackToMainmenu.Width = 200F;
            BtnBackToMainmenu.Margin = new Thickness(10F, 0F, 0F, 10F);
            BtnBackToMainmenu.HorizontalAlignment = HorizontalAlignment.Left;
            BtnBackToMainmenu.VerticalAlignment = VerticalAlignment.Bottom;
            BtnBackToMainmenu.Content = "Back";
            Binding binding_BtnBackToMainmenu_Command = new Binding("BackButtonCommand");
            BtnBackToMainmenu.SetBinding(Button.CommandProperty, binding_BtnBackToMainmenu_Command);
            items.Add(e_1);
            return items;
        }
    }
}

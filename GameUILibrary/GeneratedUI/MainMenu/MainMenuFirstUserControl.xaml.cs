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
    public partial class MainMenuFirstUserControl : UserControl {
        
        private Grid e_0;
        
        private Button BtnConnect;
        
        private Button BtnRegister;
        
        private Button BtnSettings;
        
        private Button BtnExit;
        
        private TextBox TempTitleBox;
        
        public MainMenuFirstUserControl() {
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
            this.e_0.Margin = new Thickness(0F, 0F, 10F, 0F);
            // BtnConnect element
            this.BtnConnect = new Button();
            this.e_0.Children.Add(this.BtnConnect);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Height = 70F;
            this.BtnConnect.Width = 200F;
            this.BtnConnect.IsHitTestVisible = true;
            this.BtnConnect.Margin = new Thickness(0F, 255F, 0F, 0F);
            this.BtnConnect.HorizontalAlignment = HorizontalAlignment.Center;
            this.BtnConnect.VerticalAlignment = VerticalAlignment.Top;
            this.BtnConnect.TabIndex = 1;
            this.BtnConnect.Content = "Play";
            this.BtnConnect.CommandParameter = "Click Connect Button";
            Binding binding_BtnConnect_Command = new Binding("PlayButtonCommand");
            this.BtnConnect.SetBinding(Button.CommandProperty, binding_BtnConnect_Command);
            // BtnRegister element
            this.BtnRegister = new Button();
            this.e_0.Children.Add(this.BtnRegister);
            this.BtnRegister.Name = "BtnRegister";
            this.BtnRegister.Height = 70F;
            this.BtnRegister.Width = 200F;
            this.BtnRegister.IsHitTestVisible = true;
            this.BtnRegister.Margin = new Thickness(0F, 330F, 0F, 0F);
            this.BtnRegister.HorizontalAlignment = HorizontalAlignment.Center;
            this.BtnRegister.VerticalAlignment = VerticalAlignment.Top;
            this.BtnRegister.TabIndex = 2;
            this.BtnRegister.Content = "Register";
            this.BtnRegister.CommandParameter = "Click Register Button";
            Binding binding_BtnRegister_Command = new Binding("RegisterButtonCommand");
            this.BtnRegister.SetBinding(Button.CommandProperty, binding_BtnRegister_Command);
            // BtnSettings element
            this.BtnSettings = new Button();
            this.e_0.Children.Add(this.BtnSettings);
            this.BtnSettings.Name = "BtnSettings";
            this.BtnSettings.Height = 70F;
            this.BtnSettings.Width = 200F;
            this.BtnSettings.IsHitTestVisible = true;
            this.BtnSettings.Margin = new Thickness(0F, 405F, 0F, 0F);
            this.BtnSettings.HorizontalAlignment = HorizontalAlignment.Center;
            this.BtnSettings.VerticalAlignment = VerticalAlignment.Top;
            this.BtnSettings.TabIndex = 3;
            this.BtnSettings.Content = "Settings";
            this.BtnSettings.CommandParameter = "Click Settings Button";
            Binding binding_BtnSettings_Command = new Binding("SettingsButtonCommand");
            this.BtnSettings.SetBinding(Button.CommandProperty, binding_BtnSettings_Command);
            // BtnExit element
            this.BtnExit = new Button();
            this.e_0.Children.Add(this.BtnExit);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Height = 35F;
            this.BtnExit.Width = 35F;
            this.BtnExit.Margin = new Thickness(0F, 10F, 10F, 0F);
            this.BtnExit.HorizontalAlignment = HorizontalAlignment.Right;
            this.BtnExit.VerticalAlignment = VerticalAlignment.Top;
            this.BtnExit.TabIndex = 3;
            this.BtnExit.Content = "X";
            this.BtnExit.CommandParameter = "Click Exit Button";
            Binding binding_BtnExit_Command = new Binding("ExitButtonCommand");
            this.BtnExit.SetBinding(Button.CommandProperty, binding_BtnExit_Command);
            // TempTitleBox element
            this.TempTitleBox = new TextBox();
            this.e_0.Children.Add(this.TempTitleBox);
            this.TempTitleBox.Name = "TempTitleBox";
            this.TempTitleBox.Height = 70F;
            this.TempTitleBox.Width = 300F;
            this.TempTitleBox.Margin = new Thickness(0F, 150F, 0F, 0F);
            this.TempTitleBox.HorizontalAlignment = HorizontalAlignment.Center;
            this.TempTitleBox.VerticalAlignment = VerticalAlignment.Top;
            this.TempTitleBox.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.TempTitleBox.BorderThickness = new Thickness(0F, 0F, 0F, 0F);
            this.TempTitleBox.FontSize = 48F;
            this.TempTitleBox.Text = "TerraStructor";
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
        }
    }
}

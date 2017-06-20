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
        
        private Button BtnExit;
        
        private Button BtnRegister;
        
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
            // BtnConnect element
            this.BtnConnect = new Button();
            this.e_0.Children.Add(this.BtnConnect);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Height = 70F;
            this.BtnConnect.Width = 200F;
            this.BtnConnect.IsHitTestVisible = true;
            this.BtnConnect.Margin = new Thickness(0F, 0F, 0F, 500F);
            this.BtnConnect.HorizontalAlignment = HorizontalAlignment.Center;
            this.BtnConnect.VerticalAlignment = VerticalAlignment.Center;
            this.BtnConnect.Content = "Play";
            this.BtnConnect.CommandParameter = "Click Connect Button";
            Binding binding_BtnConnect_Command = new Binding("PlayButtonCommand");
            this.BtnConnect.SetBinding(Button.CommandProperty, binding_BtnConnect_Command);
            // BtnExit element
            this.BtnExit = new Button();
            this.e_0.Children.Add(this.BtnExit);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Height = 35F;
            this.BtnExit.Width = 35F;
            this.BtnExit.Margin = new Thickness(0F, 10F, 10F, 0F);
            this.BtnExit.HorizontalAlignment = HorizontalAlignment.Right;
            this.BtnExit.VerticalAlignment = VerticalAlignment.Top;
            this.BtnExit.Content = "X";
            this.BtnExit.CommandParameter = "Click Exit Button";
            Binding binding_BtnExit_Command = new Binding("ExitButtonCommand");
            this.BtnExit.SetBinding(Button.CommandProperty, binding_BtnExit_Command);
            // BtnRegister element
            this.BtnRegister = new Button();
            this.e_0.Children.Add(this.BtnRegister);
            this.BtnRegister.Name = "BtnRegister";
            this.BtnRegister.Height = 70F;
            this.BtnRegister.Width = 200F;
            this.BtnRegister.IsHitTestVisible = true;
            this.BtnRegister.Margin = new Thickness(860F, 330F, 860F, 0F);
            this.BtnRegister.HorizontalAlignment = HorizontalAlignment.Center;
            this.BtnRegister.VerticalAlignment = VerticalAlignment.Top;
            this.BtnRegister.Content = "Register";
            this.BtnRegister.CommandParameter = "Click Connect Button";
            Binding binding_BtnRegister_Command = new Binding("RegisterButtonCommand");
            this.BtnRegister.SetBinding(Button.CommandProperty, binding_BtnRegister_Command);
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

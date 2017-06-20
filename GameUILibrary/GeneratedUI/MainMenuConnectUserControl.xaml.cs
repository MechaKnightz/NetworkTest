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
    public partial class MainMenuConnectUserControl : UserControl {
        
        private Grid e_0;
        
        private Button ConnectButton;
        
        private TextBox Username;
        
        private PasswordBox Password;
        
        private TextBox IP;
        
        private TextBox Port;
        
        private Button BtnBack;
        
        public MainMenuConnectUserControl() {
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
            // ConnectButton element
            this.ConnectButton = new Button();
            this.e_0.Children.Add(this.ConnectButton);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Height = 80F;
            this.ConnectButton.Margin = new Thickness(660F, 193F, 660F, 0F);
            this.ConnectButton.VerticalAlignment = VerticalAlignment.Top;
            this.ConnectButton.FontSize = 55F;
            this.ConnectButton.Content = "Connect";
            this.ConnectButton.CommandParameter = "Click Connect Button";
            Binding binding_ConnectButton_Command = new Binding("ConnectButtonCommand");
            this.ConnectButton.SetBinding(Button.CommandProperty, binding_ConnectButton_Command);
            // Username element
            this.Username = new TextBox();
            this.e_0.Children.Add(this.Username);
            this.Username.Name = "Username";
            this.Username.Width = 600F;
            this.Username.Margin = new Thickness(660F, 278F, 660F, 722F);
            this.Username.HorizontalAlignment = HorizontalAlignment.Center;
            this.Username.FontSize = 55F;
            Binding binding_Username_Text = new Binding("Username");
            binding_Username_Text.Mode = BindingMode.TwoWay;
            this.Username.SetBinding(TextBox.TextProperty, binding_Username_Text);
            // Password element
            this.Password = new PasswordBox();
            this.e_0.Children.Add(this.Password);
            this.Password.Name = "Password";
            this.Password.Width = 600F;
            this.Password.Margin = new Thickness(660F, 363F, 660F, 637F);
            this.Password.HorizontalAlignment = HorizontalAlignment.Center;
            this.Password.FontSize = 55F;
            Binding binding_Password_Text = new Binding("Password");
            binding_Password_Text.Mode = BindingMode.TwoWay;
            this.Password.SetBinding(PasswordBox.TextProperty, binding_Password_Text);
            // IP element
            this.IP = new TextBox();
            this.e_0.Children.Add(this.IP);
            this.IP.Name = "IP";
            this.IP.Width = 600F;
            this.IP.Margin = new Thickness(660F, 448F, 660F, 552F);
            this.IP.HorizontalAlignment = HorizontalAlignment.Center;
            this.IP.FontSize = 55F;
            this.IP.IsReadOnly = true;
            Binding binding_IP_Text = new Binding("IP");
            binding_IP_Text.Mode = BindingMode.TwoWay;
            this.IP.SetBinding(TextBox.TextProperty, binding_IP_Text);
            // Port element
            this.Port = new TextBox();
            this.e_0.Children.Add(this.Port);
            this.Port.Name = "Port";
            this.Port.Width = 600F;
            this.Port.Margin = new Thickness(660F, 533F, 660F, 467F);
            this.Port.HorizontalAlignment = HorizontalAlignment.Center;
            this.Port.FontSize = 55F;
            this.Port.IsReadOnly = true;
            Binding binding_Port_Text = new Binding("Port");
            binding_Port_Text.Mode = BindingMode.TwoWay;
            this.Port.SetBinding(TextBox.TextProperty, binding_Port_Text);
            // BtnBack element
            this.BtnBack = new Button();
            this.e_0.Children.Add(this.BtnBack);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Height = 80F;
            this.BtnBack.Margin = new Thickness(660F, 0F, 660F, 382F);
            this.BtnBack.VerticalAlignment = VerticalAlignment.Bottom;
            this.BtnBack.FontSize = 55F;
            this.BtnBack.Content = "Back";
            this.BtnBack.CommandParameter = "Click Back Button";
            Binding binding_BtnBack_Command = new Binding("BackButtonCommand");
            this.BtnBack.SetBinding(Button.CommandProperty, binding_BtnBack_Command);
            FontManager.Instance.AddFont("Segoe UI", 55F, FontStyle.Regular, "Segoe_UI_41.25_Regular");
        }
    }
}

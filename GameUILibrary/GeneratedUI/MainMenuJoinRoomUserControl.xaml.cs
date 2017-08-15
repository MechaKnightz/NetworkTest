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
    public partial class MainMenuJoinRoomUserControl : UserControl {
        
        private Grid e_0;
        
        private Button BtnJoinRoom;
        
        private TextBox textBox;
        
        private Button BtnBack2;
        
        public MainMenuJoinRoomUserControl() {
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
            // BtnJoinRoom element
            this.BtnJoinRoom = new Button();
            this.e_0.Children.Add(this.BtnJoinRoom);
            this.BtnJoinRoom.Name = "BtnJoinRoom";
            this.BtnJoinRoom.Height = 80F;
            this.BtnJoinRoom.Width = 600F;
            this.BtnJoinRoom.Margin = new Thickness(0F, 193F, 0F, 0F);
            this.BtnJoinRoom.VerticalAlignment = VerticalAlignment.Top;
            this.BtnJoinRoom.FontSize = 55F;
            this.BtnJoinRoom.Content = "Join room";
            this.BtnJoinRoom.CommandParameter = "Click Join Room Button";
            Binding binding_BtnJoinRoom_Command = new Binding("JoinRoomButtonCommand");
            this.BtnJoinRoom.SetBinding(Button.CommandProperty, binding_BtnJoinRoom_Command);
            // textBox element
            this.textBox = new TextBox();
            this.e_0.Children.Add(this.textBox);
            this.textBox.Name = "textBox";
            this.textBox.Height = 80F;
            this.textBox.Width = 600F;
            this.textBox.Margin = new Thickness(0F, 278F, 0F, 0F);
            this.textBox.VerticalAlignment = VerticalAlignment.Top;
            this.textBox.FontSize = 55F;
            Binding binding_textBox_Text = new Binding("RoomName");
            binding_textBox_Text.Mode = BindingMode.TwoWay;
            this.textBox.SetBinding(TextBox.TextProperty, binding_textBox_Text);
            // BtnBack2 element
            this.BtnBack2 = new Button();
            this.e_0.Children.Add(this.BtnBack2);
            this.BtnBack2.Name = "BtnBack2";
            this.BtnBack2.Height = 80F;
            this.BtnBack2.Width = 600F;
            this.BtnBack2.Margin = new Thickness(0F, 363F, 0F, 0F);
            this.BtnBack2.VerticalAlignment = VerticalAlignment.Top;
            this.BtnBack2.FontSize = 55F;
            this.BtnBack2.Content = "Back";
            this.BtnBack2.CommandParameter = "Click Back Button 2";
            Binding binding_BtnBack2_Command = new Binding("BackButton2Command");
            this.BtnBack2.SetBinding(Button.CommandProperty, binding_BtnBack2_Command);
            FontManager.Instance.AddFont("Segoe UI", 55F, FontStyle.Regular, "Segoe_UI_41.25_Regular");
        }
    }
}

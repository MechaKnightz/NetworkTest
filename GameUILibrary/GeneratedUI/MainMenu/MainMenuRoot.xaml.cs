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
    public partial class MainMenuRoot : UIRoot {
        
        private Grid e_0;
        
        private MainMenuFirstUserControl e_1;
        
        private MainMenuConnectUserControl e_2;
        
        private MainMenuJoinRoomUserControl e_3;
        
        private MainMenuRegisterUserControl e_4;
        
        public MainMenuRoot() : 
                base() {
            this.Initialize();
        }
        
        public MainMenuRoot(int width, int height) : 
                base(width, height) {
            this.Initialize();
        }
        
        private void Initialize() {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            this.e_0.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            // e_1 element
            this.e_1 = new MainMenuFirstUserControl();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            Binding binding_e_1_Visibility = new Binding("FirstMenuVisible");
            binding_e_1_Visibility.Mode = BindingMode.TwoWay;
            this.e_1.SetBinding(MainMenuFirstUserControl.VisibilityProperty, binding_e_1_Visibility);
            // e_2 element
            this.e_2 = new MainMenuConnectUserControl();
            this.e_0.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            Binding binding_e_2_Visibility = new Binding("ConnectMenuVisible");
            binding_e_2_Visibility.Mode = BindingMode.TwoWay;
            this.e_2.SetBinding(MainMenuConnectUserControl.VisibilityProperty, binding_e_2_Visibility);
            // e_3 element
            this.e_3 = new MainMenuJoinRoomUserControl();
            this.e_0.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            Binding binding_e_3_Visibility = new Binding("JoinRoomMenuVisible");
            binding_e_3_Visibility.Mode = BindingMode.TwoWay;
            this.e_3.SetBinding(MainMenuJoinRoomUserControl.VisibilityProperty, binding_e_3_Visibility);
            // e_4 element
            this.e_4 = new MainMenuRegisterUserControl();
            this.e_0.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Margin = new Thickness(0F, -10F, 0F, 10F);
            Binding binding_e_4_Visibility = new Binding("RegisterMenuVisible");
            binding_e_4_Visibility.Mode = BindingMode.TwoWay;
            this.e_4.SetBinding(MainMenuRegisterUserControl.VisibilityProperty, binding_e_4_Visibility);
        }
    }
}

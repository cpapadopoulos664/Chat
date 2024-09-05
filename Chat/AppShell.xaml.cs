using Chat.Views;

namespace Chat
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Front), typeof(Front));
            Routing.RegisterRoute(nameof(SignUp), typeof(SignUp));
            Routing.RegisterRoute(nameof(SignIn), typeof(SignIn));
            Routing.RegisterRoute(nameof(GroupChat), typeof(GroupChat));
        }
    }
}

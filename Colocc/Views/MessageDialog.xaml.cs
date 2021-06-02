using System.Windows;
using System.Windows.Controls;

namespace Colocc.Views
{
    /// <summary>
    /// Interaction logic for MessageDialog
    /// </summary>
    public partial class MessageDialog : UserControl
    {
        public MessageDialog()
        {
            InitializeComponent();
        }
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((Window)sender).DragMove();
        }
    }
}

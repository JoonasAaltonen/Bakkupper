using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackUpper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ControlsWindow _controls = new ControlsWindow();
        public MainWindow()
        {
            InitializeComponent();
            MainWindowGrid.Children.Add(_controls);
            
        }

    }
}

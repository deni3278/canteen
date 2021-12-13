using System.Windows;
using Canteen.Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace Canteen.Management.Views;

public partial class AddItemWindow : Window
{
    public AddItemWindow()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetService<AddItemViewModel>();
    }

    private void OpenFileDialog_OnClick(object sender,
                                        RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Canteen Management",
            Filter = "PNG Files|*.png",
            Multiselect = false,
            CheckFileExists = true,
            CheckPathExists = true
        };

        if (!openFileDialog.ShowDialog()!.Value) return;

        var path = openFileDialog.FileName;
        (DataContext as AddItemViewModel)!.Path = path;
    }

    private void SetDialogResultFalse_OnClick(object sender,
                                              RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void SetDialogResultTrue_OnClick(object sender,
                                             RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace CourseProject;

public partial class EditWindow : Window
{
    public EditWindow(string name)
    {
        InitializeComponent();

        _name = name;
        _name2 = name;
        _dataTableDate = DataBase.ShowTable(_name2 += "_d");
        DataGridDate.ItemsSource = _dataTableDate.DefaultView;
    }
    
    private string _name2;
    private string _name;
    private DataTable _dataTableDate;
    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        DataBase.SaveTable(_name2, _dataTableDate);
        DataBase.GenTableA(_name);
        MessageBox.Show("Success");
        Close();
    }
}
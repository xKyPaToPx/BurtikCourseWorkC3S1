using System.Windows;

namespace CourseProject;

public partial class AddNewDb : Window
{
    public AddNewDb()
    {
        InitializeComponent();
    }

    private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
    {
        DataBase.CreateNewTable(TextBoxNameOfDb.Text);
        MessageBox.Show("Success");
        Close();
    }

    private void ButtonCreateNewDb_OnClick(object sender, RoutedEventArgs e)
    {
        DataBase.CreateDataBase();
        MessageBox.Show("Success");
    }
}
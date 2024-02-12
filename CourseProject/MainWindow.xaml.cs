using System.ComponentModel;
using System.Data;

using System.Windows;
using System.Windows.Controls;


namespace CourseProject;


public partial class MainWindow : Window
{
    private bool isEdit = false;
    public MainWindow()
    {
        InitializeComponent();
        DataBase.GetTables();
        DbComboBox.ItemsSource = DataBase._tables;
    }
    
    private void AddNewButton_OnClick(object sender, RoutedEventArgs e)
    {
        AddNewDb addNewDb = new AddNewDb();
        addNewDb.ShowDialog();
        DbComboBox.ItemsSource = null;
        DbComboBox.ItemsSource = DataBase._tables;
    }

    private DataTable _dt;
    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (DbComboBox.Text != "")
        {
            switch (MessageBox.Show($"Would you like to delete group {DbComboBox.Text}? ", "",
                        MessageBoxButton.YesNo))
            {
                case MessageBoxResult.Yes:
                    DataBase.DeleteTable(DbComboBox.Text);
                    MessageBox.Show("Success");
                    break;
                case MessageBoxResult.No:
                    return;
            }
            DbComboBox.ItemsSource = null;
            DbComboBox.ItemsSource = DataBase._tables;
            DbComboBox.SelectedIndex = 0;
        }
    }
        

    private void EditTablesButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (DbComboBox.Text!="")
        {
            EditWindow editWindow = new EditWindow(DbComboBox.Text);
            editWindow.ShowDialog();
            Update(prevStr);
        }
        
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (DbComboBox.Text != "")
        {
            DataBase.SaveTable($"{DbComboBox.Text}_a",_dt);
            MessageBox.Show("Success");
        }
        
    }
    
    private void DbComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isEdit)
        {
            if (MessageBox.Show("Would u like to save changed?","",MessageBoxButton.YesNo) == MessageBoxResult.Yes)     
            {
                DataBase.SaveTable($"{prevStr}_a",_dt);
                MessageBox.Show("Success");
            }
            
        }
        if (DbComboBox.ItemsSource != null)
        {
            Update(DbComboBox.SelectedItem.ToString());
            prevStr = DbComboBox.SelectedItem.ToString();
        }
        
        
    }

    private string prevStr;
    private void Update(string? s)
    {
        var str = s;
        if (str != "")
        {
            _dt = DataBase.ShowTable(str += "_a");
            DataGridVeiw.ItemsSource = _dt.DefaultView;
        }

        isEdit = false;
    }


    private void DataGridVeiw_OnRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
    {
        isEdit = true;
    }
}
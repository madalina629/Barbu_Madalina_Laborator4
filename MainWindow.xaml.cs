using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoLotModel;
using System.Data.Entity;
using System.Data;

namespace Barbu_Madalina_Laborator4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
             customerViewSource.Source = ctx.Customer.Local;
             ctx.Customer.Load();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;

            btnCancel.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(contract_valueTextBox,
           TextBox.TextProperty);
            BindingOperations.ClearBinding(contract_dateDatePicker,
           DatePicker.SelectedDateProperty);
            Keyboard.Focus(contract_dateDatePicker);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;

            btnCancel.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            customerDataGrid.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(contract_valueTextBox,
           TextBox.TextProperty);
            BindingOperations.ClearBinding(contract_dateDatePicker,
           DatePicker.SelectedDateProperty);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnNext.IsEnabled = true;
            btnPrev.IsEnabled = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim(),
                        Contract_value = Decimal.Parse(contract_valueTextBox.Text.Trim()),
                        Contract_date = contract_dateDatePicker.SelectedDate
                    };
                    ctx.Customer.Add(customer);
                    customerViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
 if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    customer.Contract_value = Decimal.Parse(contract_valueTextBox.Text.Trim());
                    customer.Contract_date = contract_dateDatePicker.SelectedDate;
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                customerViewSource.View.MoveCurrentTo(customer);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customer.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                  {
                     MessageBox.Show(ex.Message);
                  }
            customerViewSource.View.Refresh();
            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
           customerViewSource.View.MoveCurrentToNext();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }
    }
}

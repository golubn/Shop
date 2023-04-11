using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Shop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-6L58BJ1\SQLEXPRESS;Initial Catalog=NewDB;Integrated Security=True");

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void clearData()
        {
            Name.Clear();
            Group.Clear();
            Price.Clear();
            Model.Clear();
            Count.Clear();
            ID_text.Clear();
        }

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from FirstTable", connection);
            DataTable dt = new DataTable();
            connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            connection.Close();
            DataGrid.ItemsSource = dt.DefaultView;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        public bool isValid()
        {
            if (Group.Text == string.Empty)
            {
                MessageBox.Show("Group is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Name.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Model.Text == string.Empty)
            {
                MessageBox.Show("Model is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Price.Text == string.Empty)
            {
                MessageBox.Show("Price is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Count.Text == string.Empty)
            {
                MessageBox.Show("Count is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO FirstTable VALUES(@Group, @Name, @Model, @Price, @Count)", connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", Name.Text);
                    cmd.Parameters.AddWithValue("@Group", Group.Text);
                    cmd.Parameters.AddWithValue("@Model", Model.Text);
                    cmd.Parameters.AddWithValue("@Price", Price.Text);
                    cmd.Parameters.AddWithValue("@Count", Count.Text);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully added", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();

                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Update_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand(@"UPDATE FirstTable SET Name=@Name, Model=@Model, Price=@Price, Count=@Count" + " WHERE ID=@ID", connection);
            cmd.Parameters.AddWithValue("@ID", ID_text.Text);
            cmd.Parameters.AddWithValue("@Name", Name.Text);
            cmd.Parameters.AddWithValue("@Model", Model.Text);
            cmd.Parameters.AddWithValue("@Price", Price.Text);
            cmd.Parameters.AddWithValue("@Count", Count.Text);
            
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Updated successfully", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); clearData();LoadGrid(); }

        }

        private void Delete_Click_1(object sender, RoutedEventArgs e)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("delete from FirstTable WHERE ID = " + ID_text.Text + " ", connection);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product has been deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                connection.Close();
                clearData();
                LoadGrid();
                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Not deleted" + ex.Message);
            }
            finally
            {
                connection.Close();
                clearData();
            }
        }

        private void Found_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from FirstTable WHERE ID = " + ID_text.Text + " ", connection);
            DataTable dt = new DataTable();
            connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            connection.Close();
            DataGrid.ItemsSource = dt.DefaultView;
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from FirstTable ORDER BY Price", connection);
            DataTable dt = new DataTable();
            connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            connection.Close();
            DataGrid.ItemsSource = dt.DefaultView;
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            LoadGrid();
        }
    }
}

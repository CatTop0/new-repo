using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Task_4_Library
{
    public partial class LoginForm : Form
    {
        SqlCommand sqlCommand;
        //Нужно поменять соединение
        SqlConnection connection = new SqlConnection(@"data source=LAPTOP-9SF09P2S\SQLEXPRESS;initial catalog=Library;integrated security=True");
        SqlDataReader dataReader;
        LibraryEntities libraryEntities = new LibraryEntities();
        const string loginPlaceholder = "Введите E-mail";
        const string passwordPlaceholder = "Пароль";
        public LoginForm()
        {
            InitializeComponent();
            CenterToScreen();
        }
        private void EntryButton_Click(object sender, EventArgs e)
        {
            if (PasswordField.Text != string.Empty && LoginField.Text != string.Empty)
            {
                connection.Open();
                sqlCommand = new SqlCommand("SELECT * FROM [User] WHERE UserLogin = @ulogin AND UserPassword = @upassword", connection);
                sqlCommand.Parameters.AddWithValue("@ulogin", LoginField.Text);
                sqlCommand.Parameters.AddWithValue("@upassword", PasswordField.Text);
                dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    //Номера столбцов, из которых принимать данные могут привести к сложности с миграцией базы данных
                    object userRole = dataReader.GetValue(4);
                    object userFullname = dataReader.GetValue(1);
                    if (Convert.ToInt32(userRole) == 1)
                    {
                        BookListForm bookListForm = new BookListForm(userFullname.ToString(), Convert.ToInt32(userRole));
                        bookListForm.Show();
                    }
                    else
                    {
                        BookListForm bookListForm = new BookListForm(userFullname.ToString(), Convert.ToInt32(userRole));
                        bookListForm.Show();
                    }
                    dataReader.Close();
                    this.Hide();
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Пользователя с таким логином и паролем нет. Введите верные данные", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                    dataReader.Close();
                }
            }
            else MessageBox.Show("Пожалуйста, заполните все поля.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoginField_Enter(object sender, EventArgs e)
        {
            if (LoginField.Text == loginPlaceholder)
            {
                LoginField.Text = string.Empty;
                LoginField.ForeColor = Color.Black;
            }
        }

        private void LoginField_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LoginField.Text))
            {
                LoginField.Text = loginPlaceholder;
                LoginField.ForeColor = Color.White;
            }
        }

        private void PasswordField_Enter(object sender, EventArgs e)
        {
            if (PasswordField.Text == passwordPlaceholder)
            {
                PasswordField.Text = string.Empty;
                PasswordField.ForeColor = Color.Black;
                PasswordField.UseSystemPasswordChar = true;
            }
        }

        private void PasswordField_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordField.Text))
            {
                PasswordField.Text = passwordPlaceholder;
                PasswordField.ForeColor = Color.White;
                //Чтобы слово "Пароль" не было скрыто на символы пароля
                PasswordField.UseSystemPasswordChar = false;
            }
        }
    }
}

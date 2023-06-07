using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_4_Library
{
    public partial class LoginForm : Form
    {
        LibraryEntities libraryEntities=new LibraryEntities();
        public LoginForm()
        {
            InitializeComponent();
            CenterToScreen();
        }
        SqlCommand cmd;
        SqlConnection cn;
        SqlDataReader dr;
        private void LoginForm_Load(object sender, EventArgs e)
        {
            cn = new SqlConnection(@"data source=LAPTOP-9SF09P2S\SQLEXPRESS;initial catalog=Library;integrated security=True");

        }
        private void EntryButton_Click(object sender, EventArgs e)
        {
            if (PasswordField.Text != string.Empty || LoginField.Text != string.Empty)
            {
                cn.Open();
                cmd = new SqlCommand("SELECT * FROM [User] WHERE UserLogin = @ulogin AND UserPassword = @upassword", cn);
                cmd.Parameters.AddWithValue("@ulogin", LoginField.Text);
                cmd.Parameters.AddWithValue("@upassword", PasswordField.Text);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    object urole = dr.GetValue(4);
                    object userfullname = dr.GetValue(1);
                    if (Convert.ToInt32(urole) == 1)
                    {
                        BookListForm bookListForm = new BookListForm(userfullname.ToString(), Convert.ToInt32(urole));
                        bookListForm.Show();
                    }
                    else
                    {
                        BookListForm bookListForm = new BookListForm(userfullname.ToString(), Convert.ToInt32(urole));
                        bookListForm.Show();
                    }
                    dr.Close();
                    this.Hide();
                    cn.Close();
                }
                else
                {
                    MessageBox.Show("Пользователя с таким логином и паролем нет. Введите верные данные", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cn.Close();
                    dr.Close();
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
            if (LoginField.Text == "Введите E-mail")
            {
                LoginField.Text = "";
                LoginField.ForeColor = Color.Black;
            }
        }

        private void LoginField_Leave(object sender, EventArgs e)
        {
            if (LoginField.Text == "")
            {
                LoginField.Text = "Введите E-mail";
                LoginField.ForeColor = Color.White;
            }
        }

        private void PasswordField_Enter(object sender, EventArgs e)
        {
            if (PasswordField.Text == "Пароль")
            {
                PasswordField.Text = "";
                PasswordField.ForeColor = Color.Black;
            }
        }

        private void PasswordField_Leave(object sender, EventArgs e)
        {
            if (PasswordField.Text == "")
            {
                PasswordField.Text = "Пароль";
                PasswordField.ForeColor = Color.White;
            }
        }
    }
}

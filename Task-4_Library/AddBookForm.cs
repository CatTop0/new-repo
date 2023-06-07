using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_4_Library
{
    public partial class AddBookForm : Form
    {
        LibraryEntities libraryEntities = new LibraryEntities();
        private bool isAdd;
        Book _book;
        public AddBookForm(Book book, bool isAdd)
        {
            InitializeComponent();
            CenterToScreen();
            this.isAdd = isAdd;
            _book = book;
            if(isAdd==false)
            {
                RegLabel.Text = "Редактирование";
                NameField.Text = _book.BookName;
                AuthorField.Text = _book.BookAuthor;
                NumberField.Text = _book.BookInvenoryNumber.ToString();
            }
            //NameField.Text = _book.BookName;
            //AuthorField.Text = _book.BookAuthor;
            //NumberField.Text = _book.BookInvenoryNumber.ToString();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameField.Text) || string.IsNullOrWhiteSpace(AuthorField.Text) || NumberField.Text == "")
            {
                MessageBox.Show("Заполните все поля", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int num = Convert.ToInt32(NumberField.Text);
            if (isAdd == true)
            {
                try
                {
                    var newBook = new Book
                    {
                        BookName = NameField.Text,
                        BookAuthor = AuthorField.Text,
                        BookInvenoryNumber = Convert.ToInt32(NumberField.Text),
                    };
                    if (libraryEntities.Book.Any(p => p.BookInvenoryNumber.ToString() == NumberField.Text))
                    {
                        MessageBox.Show("Книга с таким инвентарным номером уже существует.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if(1000 > num || num < 100000)
                    {
                        MessageBox.Show("Инвентарный номер должен находиться в промежутке от 1000 до 100000.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    libraryEntities.Book.Add(newBook);
                    libraryEntities.SaveChanges();
                    MessageBox.Show("Книга успешно добавлена!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    BookListForm bookListForm = new BookListForm(null, 1);
                    bookListForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении книги: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (1000 > num || num < 100000)
                {
                    MessageBox.Show("Инвентарный номер должен находиться в промежутке от 1000 до 100000.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var newBook = new Book
                    {
                        BookName = NameField.Text,
                        BookAuthor = AuthorField.Text,
                        BookInvenoryNumber = Convert.ToInt32(NumberField.Text),
                    };
                    libraryEntities.Book.Add(newBook);
                    libraryEntities.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    BookListForm bookListForm = new BookListForm(null, 1);
                    bookListForm.Show();
                }
                //_book.BookName = NameField.Text;
                //_book.BookAuthor = AuthorField.Text;
                //_book.BookInvenoryNumber = Convert.ToInt32(NumberField.Text);
                
            }
            
        }

        private void LogInBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            BookListForm bookListForm = new BookListForm(null, 1);
            bookListForm.Show();
        }

        private void NumberField_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}

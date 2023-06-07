using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_4_Library
{
    public partial class BookListForm : Form
    {
        LibraryEntities libraryEntities = new LibraryEntities();
        private List<Book> allBooks;
        private int userRole;
        private int currentPage = 1;
        private int _itemsPerPage = 3;
        private string _selectedSort = "Без сортировки";
        public BookListForm(string userFullname, int userRole)
        {
            InitializeComponent();
            CenterToScreen();
            this.userRole = userRole;
            FullName.Text= userFullname;

            if (userRole == 1 || userRole == 2)
            {
                addBtn.Visible = true;
            }
            else
            {
                addBtn.Visible = false;
            }
        }

        private void LogInBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void BookListForm_Load(object sender, EventArgs e)
        {
            allBooks = libraryEntities.Book.ToList();
            GenerateCards(tableLayoutPanel1, allBooks, userRole);
            comboBox1.Items.Add("Без сортировки");
            comboBox1.Items.Add("По убыванию даты выдачи");
            comboBox1.SelectedIndex = 0;
        }

        //Лучше это переделать путём организации шаблона через конструктор и применения данных из БД, скрывая шаблон, когда
        public static void GenerateCards(TableLayoutPanel tableLayoutPanel1, List<Book> books, int userRole)
        {
            User user = new User();
            int row = 0;
            foreach (Book book in books)
            {
                Panel bookCard = new Panel();
                bookCard.BorderStyle = BorderStyle.FixedSingle;
                bookCard.Width = tableLayoutPanel1.ClientSize.Width / 1 - 10;
                bookCard.Height = 170;
                Panel separationPanel = new Panel();
                separationPanel.Width = 2;
                separationPanel.Height = bookCard.Height - 20;
                separationPanel.BackColor = Color.Black;
                separationPanel.Top = 5;
                bookCard.Controls.Add(separationPanel);

                System.Windows.Forms.Label bookNameLabel = new System.Windows.Forms.Label();
                bookNameLabel.Text = "Название книги:";
                bookNameLabel.TextAlign = ContentAlignment.MiddleRight;
                bookNameLabel.Top = 0;
                bookCard.Controls.Add(bookNameLabel);

                System.Windows.Forms.Label bookName = new System.Windows.Forms.Label();
                bookName.Text = book.BookName;
                bookName.TextAlign = ContentAlignment.MiddleLeft;
                bookName.Width = bookCard.Width - 10;
                bookName.Top = 0;
                bookName.Left = bookNameLabel.Right + 10;
                bookCard.Controls.Add(bookName);

                System.Windows.Forms.Label authorLabel = new System.Windows.Forms.Label();
                authorLabel.Text = "Автор:";
                authorLabel.TextAlign = ContentAlignment.MiddleRight;
                authorLabel.Top = bookName.Bottom + 0;
                bookCard.Controls.Add(authorLabel);

                System.Windows.Forms.Label bookAuthor = new System.Windows.Forms.Label();
                bookAuthor.Text = book.BookAuthor;
                bookAuthor.TextAlign = ContentAlignment.MiddleLeft;
                bookAuthor.MaximumSize = new Size(bookCard.Width - 250, 0);
                bookAuthor.AutoSize = true;
                bookAuthor.Top = authorLabel.Top + 5;
                bookAuthor.Left = authorLabel.Right + 10;
                bookCard.Controls.Add(bookAuthor);

                System.Windows.Forms.Label bookInvenoryNumberLabel = new System.Windows.Forms.Label();
                bookInvenoryNumberLabel.Text = "Номер:";
                bookInvenoryNumberLabel.TextAlign = ContentAlignment.MiddleRight;
                bookInvenoryNumberLabel.Top = bookAuthor.Bottom + 5;
                bookCard.Controls.Add(bookInvenoryNumberLabel);

                System.Windows.Forms.Label InvenoryNumber = new System.Windows.Forms.Label();
                InvenoryNumber.Text = book.BookInvenoryNumber.ToString();
                InvenoryNumber.TextAlign = ContentAlignment.MiddleLeft;
                InvenoryNumber.Top = bookInvenoryNumberLabel.Top + 0;
                InvenoryNumber.Left = bookNameLabel.Right + 10;
                bookCard.Controls.Add(InvenoryNumber);

                System.Windows.Forms.Button infoButton = new System.Windows.Forms.Button();
                infoButton.Text = "Выбрать книгу";
                infoButton.Name = "infoButton";
                infoButton.Size = new Size(100, 30);
                infoButton.Location = new Point(bookCard.Width - infoButton.Width - 10, bookCard.Height - infoButton.Height - 10);
                infoButton.Top = bookNameLabel.Bottom + 5;
                infoButton.BackColor = Color.FromArgb(224, 224, 224);
                infoButton.FlatStyle = FlatStyle.Flat;
                infoButton.FlatAppearance.BorderSize = 1;
                infoButton.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
                bookCard.Controls.Add(infoButton);
                infoButton.Click += (sender, args) => {
                    LibraryEntities libraryEntities = new LibraryEntities();
                    var bookToDelete = libraryEntities.Book.FirstOrDefault(b => b.BookInvenoryNumber == book.BookInvenoryNumber);
                    if (bookToDelete != null)
                    {
                        DialogResult result = MessageBox.Show("Вы хотите удалить эту книгу?", "Удаление книги", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            libraryEntities.Book.Attach(bookToDelete);
                            libraryEntities.Book.Remove(bookToDelete);
                            libraryEntities.SaveChanges();
                            MessageBox.Show("Книга успешно удалена", "Удаление книги", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if(result == DialogResult.No)
                        {
                            DialogResult result1 = MessageBox.Show("Вы хотите изменить информацию об этой книге?", "Редактирование книги", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result1 == DialogResult.Yes)
                            {
                                libraryEntities.Book.Attach(bookToDelete);
                                libraryEntities.Book.Remove(bookToDelete);
                                AddBookForm addBookForm = new AddBookForm(bookToDelete, false);                                
                                addBookForm.Show();
                                libraryEntities.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Книга не найдена", "Удаление книги", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                infoButton.BringToFront();
                tableLayoutPanel1.Controls.Add(bookCard, row / 1, row % 1);
                row++;

                if (userRole == 1 || userRole == 2)
                {
                    infoButton.Visible = true;
                    
                }
                else
                {
                    infoButton.Visible = false;
                }
            }
        }
        private void UpdateProductCards()
        {
            string searchString = textBox1.Text;
            var books = libraryEntities.Book
           .Where(p => p.BookName.Contains(searchString) || p.BookAuthor.Contains(searchString) ||
               p.BookInvenoryNumber.ToString().Contains(searchString)).ToList();
            if (_selectedSort == "По убыванию даты выдачи")
            {
                books = books.OrderByDescending(p => p.BookName).ToList();
            }

            int startIndex = (currentPage - 1) * _itemsPerPage;
            int endIndex = startIndex + _itemsPerPage;

            books = books.Skip(startIndex).Take(_itemsPerPage).ToList();
            tableLayoutPanel1.Controls.Clear();
            GenerateCards(tableLayoutPanel1, books, userRole);
            int count = books.Count;
        }
        private void leftBtn_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdateProductCards();
            }
        }

        private void RightBtn_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)allBooks.Count / _itemsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                UpdateProductCards();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            UpdateProductCards();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedSort = comboBox1.SelectedItem.ToString();
            UpdateProductCards();
        }
        bool isAdd;
        private void button1_Click(object sender, EventArgs e)
        {
            isAdd = true;
            this.Hide();
            AddBookForm addBookForm = new AddBookForm(null, true);
            addBookForm.Show();
        }
    }
}

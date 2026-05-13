using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace EazyQuizz
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Question> Questions { get; set; }
        public Array DomainItems { get; set; }
        public Array DifficultyItems { get; set; }

        string loggedStudent = "";
        List<Question> currentQuiz = new List<Question>();
        int currentIndex = 0;
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();

            Questions = new ObservableCollection<Question>(DataStorage.LoadQuestions());
            DomainItems = Enum.GetValues(typeof(Domain));
            DifficultyItems = Enum.GetValues(typeof(Difficulty));

            DataContext = this;

            cbQuizDomain.SelectedIndex = 0;
            cbDomain.SelectedIndex = 0;
            cbDifficulty.SelectedIndex = 0;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string name = txtStudentName.Text;
            string pass = txtPassword.Password;

            if (DataStorage.RegisterStudent(name, pass))
            {
                txtLoginStatus.Text = "Student inregistrat cu succes.";
                loggedStudent = name;
            }
            else
            {
                txtLoginStatus.Text = "Studentul exista deja.";
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string name = txtStudentName.Text;
            string pass = txtPassword.Password;

            if (DataStorage.LoginStudent(name, pass))
            {
                txtLoginStatus.Text = "Autentificare reusita.";
                loggedStudent = name;
            }
            else
            {
                txtLoginStatus.Text = "Date gresite.";
            }
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (loggedStudent == "")
            {
                txtQuizStatus.Text = "Trebuie sa fii autentificat.";
                return;
            }

            Domain selectedDomain = (Domain)cbQuizDomain.SelectedItem;

            currentQuiz = Questions
                .Where(q => q.domain == selectedDomain)
                .ToList();

            if (currentQuiz.Count == 0)
            {
                txtQuizStatus.Text = "Nu exista intrebari pentru domeniul ales.";
                return;
            }

            currentIndex = 0;
            score = 0;
            ShowCurrentQuestion();
        }

        private void ShowCurrentQuestion()
        {
            if (currentIndex >= currentQuiz.Count)
            {
                txtQuestion.Text = "Quiz terminat!";
                txtQuizStatus.Text = "Scor final: " + score + "/" + currentQuiz.Count;

                DataStorage.SaveScore(loggedStudent, score, currentQuiz.Count);
                return;
            }

            Question q = currentQuiz[currentIndex];

            txtQuestion.Text = q.text;
            txtQuizStatus.Text = "Intrebarea " + (currentIndex + 1) + " din " + currentQuiz.Count;

            rb1.Content = q.answers[0].text;
            rb2.Content = q.answers[1].text;
            rb3.Content = q.answers[2].text;
            rb4.Content = q.answers[3].text;

            rb1.IsChecked = false;
            rb2.IsChecked = false;
            rb3.IsChecked = false;
            rb4.IsChecked = false;

            imgQuestion.Source = null;

            if (!string.IsNullOrWhiteSpace(q.imagePath))
            {
                try
                {
                    imgQuestion.Source = new BitmapImage(new Uri(q.imagePath, UriKind.RelativeOrAbsolute));
                }
                catch
                {
                    imgQuestion.Source = null;
                }
            }
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuiz.Count == 0 || currentIndex >= currentQuiz.Count)
                return;

            int selected = -1;

            if (rb1.IsChecked == true) selected = 0;
            if (rb2.IsChecked == true) selected = 1;
            if (rb3.IsChecked == true) selected = 2;
            if (rb4.IsChecked == true) selected = 3;

            if (selected == -1)
            {
                txtQuizStatus.Text = "Alege un raspuns.";
                return;
            }

            if (currentQuiz[currentIndex].answers[selected].correct)
                score++;

            currentIndex++;
            ShowCurrentQuestion();
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            Question q = CreateQuestionFromForm();
            Questions.Add(q);
            ClearForm();
        }

        private void UpdateQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (lstQuestions.SelectedItem is Question q)
            {
                q.text = txtQuestionAdmin.Text;
                q.domain = (Domain)cbDomain.SelectedItem;
                q.difficulty = (Difficulty)cbDifficulty.SelectedItem;
                q.imagePath = txtImagePath.Text;
                q.type = QuestionType.Text;

                if (chkImage.IsChecked == true)
                    q.type |= QuestionType.Imagine;

                if (chkMultiple.IsChecked == true)
                    q.type |= QuestionType.RaspunsMultiplu;

                q.answers = GetAnswersFromForm();

                lstQuestions.Items.Refresh();
            }
        }

        private void DeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (lstQuestions.SelectedItem is Question q)
            {
                Questions.Remove(q);
                ClearForm();
            }
        }

        private void SaveQuestions_Click(object sender, RoutedEventArgs e)
        {
            DataStorage.SaveQuestions(Questions.ToList());
            MessageBox.Show("Intrebarile au fost salvate.");
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string text = txtSearch.Text.ToLower();

            var results = Questions
                .Where(q => q.text.ToLower().Contains(text))
                .ToList();

            lstQuestions.ItemsSource = results;
        }

        private void lstQuestions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstQuestions.SelectedItem is Question q)
            {
                txtQuestionAdmin.Text = q.text;
                cbDomain.SelectedItem = q.domain;
                cbDifficulty.SelectedItem = q.difficulty;
                txtImagePath.Text = q.imagePath;

                chkImage.IsChecked = q.type.HasFlag(QuestionType.Imagine);
                chkMultiple.IsChecked = q.type.HasFlag(QuestionType.RaspunsMultiplu);

                if (q.answers.Count >= 4)
                {
                    txtA1.Text = q.answers[0].text;
                    txtA2.Text = q.answers[1].text;
                    txtA3.Text = q.answers[2].text;
                    txtA4.Text = q.answers[3].text;

                    correct1.IsChecked = q.answers[0].correct;
                    correct2.IsChecked = q.answers[1].correct;
                    correct3.IsChecked = q.answers[2].correct;
                    correct4.IsChecked = q.answers[3].correct;
                }
            }
        }

        private Question CreateQuestionFromForm()
        {
            QuestionType type = QuestionType.Text;

            if (chkImage.IsChecked == true)
                type |= QuestionType.Imagine;

            if (chkMultiple.IsChecked == true)
                type |= QuestionType.RaspunsMultiplu;

            return new Question
            {
                text = txtQuestionAdmin.Text,
                domain = (Domain)cbDomain.SelectedItem,
                difficulty = (Difficulty)cbDifficulty.SelectedItem,
                imagePath = txtImagePath.Text,
                type = type,
                answers = GetAnswersFromForm()
            };
        }

        private List<Answer> GetAnswersFromForm()
        {
            return new List<Answer>
            {
                new Answer { text = txtA1.Text, correct = correct1.IsChecked == true },
                new Answer { text = txtA2.Text, correct = correct2.IsChecked == true },
                new Answer { text = txtA3.Text, correct = correct3.IsChecked == true },
                new Answer { text = txtA4.Text, correct = correct4.IsChecked == true }
            };
        }

        private void ClearForm()
        {
            txtQuestionAdmin.Text = "";
            txtImagePath.Text = "";
            txtA1.Text = "";
            txtA2.Text = "";
            txtA3.Text = "";
            txtA4.Text = "";

            correct1.IsChecked = false;
            correct2.IsChecked = false;
            correct3.IsChecked = false;
            correct4.IsChecked = false;

            chkImage.IsChecked = false;
            chkMultiple.IsChecked = false;
        }

        private void LoadScores_Click(object sender, RoutedEventArgs e)
        {
            lstScores.ItemsSource = DataStorage.LoadScores();
        }
    }
}
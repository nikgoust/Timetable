using System.Linq;
using System.Windows;


namespace Medissa
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth(){
            InitializeComponent();
            using (var db = new MembersContext()){
                var membersList = db.Members.Select(i=>i.UserName);
                LoginComboBox.ItemsSource = membersList;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e){
            using (var db = new MembersContext()){
                var membersList =db.Members.ToList();
                foreach (var member in membersList){
                    if (member.UserName == LoginComboBox.Text){
                        if (member.Password == Password.Password){
                            var window = new MainWindow(LoginComboBox.Text, member.Role);
                            window.Show();
                            this.Close();
                        }
                    }
                }
                StateLable.Content = "Неправильный логин или пароль";
            }
            }
    }

    


}


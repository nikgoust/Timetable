using System;
using System.Linq;
using System.Windows;


namespace Medissa
{
    /// <summary>
    /// Логика взаимодействия для AddMembers.xaml
    /// </summary>
    public partial class AddMembers : Window
    {
        public event Action MemberListChanged;
        public AddMembers(){
            InitializeComponent();
            RolesComboBox.ItemsSource = Enum.GetNames(typeof(Enums.Roles));
            RolesComboBox.SelectedItem = RolesComboBox.Items[0];
        }

        private void AddButton_Click(object sender, RoutedEventArgs e){
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Text)){
                StateLable.Content = "Не все поля заполнены";
                return;
            }
            using (var db = new MembersContext()){
                var meberList = db.Members.ToList();
                if (meberList.Any(member => member.UserName == NameTextBox.Text)){
                    StateLable.Content = "Такой пользователь существует";
                    return;
                }
               var newMember = new Member { UserName = NameTextBox.Text, Password = PasswordTextBox.Text, Role = RolesComboBox.Text};
               db.Members.Add(newMember);
               db.SaveChanges();
               StateLable.Content = "Пользователь добавлен";
               MemberListChanged?.Invoke();

            }
        }
    }
}

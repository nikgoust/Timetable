using System;
using System.Linq;
using System.Windows;


namespace Medissa
{
    /// <summary>
    /// Логика взаимодействия для EditMember.xaml
    /// </summary>
    public partial class EditMember : Window
    {
        private readonly string _memberName;
        public event Action MemberListChanged;
        public EditMember(string memberForChange){
            InitializeComponent();
            _memberName = memberForChange.Substring(0, memberForChange.IndexOf(" "));
            using (var db = new MembersContext()){
                var member = db.Members.ToList().Find(l => l.UserName == _memberName);
                textBox.Text = member.UserName;
                textBox1.Text = member.Password;
                comboBox.ItemsSource = Enum.GetNames(typeof(Enums.Roles));
                comboBox.Text = member.Role;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e){
            if (string.IsNullOrEmpty(textBox.Text) || string.IsNullOrEmpty(textBox1.Text)) return;
            using (var db = new MembersContext()){
                var member = db.Members.ToList().Find(l => l.UserName == _memberName);
                member.UserName = textBox.Text;
                member.Password = textBox1.Text;
                member.Role = comboBox.Text;
                db.SaveChanges();
                MemberListChanged?.Invoke();
                this.Close();
            }
        }
    }
}

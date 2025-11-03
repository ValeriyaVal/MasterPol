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

namespace MasterPol
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Partner _currentPartner = new Partner();
        public AddEditPage(Partner SelectedPartner)
        {
            InitializeComponent();

            if (SelectedPartner != null)
                _currentPartner = SelectedPartner;

            _currentPartner = SelectedPartner ?? new Partner();
            DataContext = _currentPartner; // сначала
            ComboType.ItemsSource = MasterPolEntities.GetContext().PartnerType.ToList(); // потом

            //if (_currentPartner.IDTypePartner != null)
            //    ComboType.SelectedValue = _currentPartner.IDTypePartner;

            // Если редактируем, выставляем тип транспорта
            //if (!string.IsNullOrEmpty(_currentPartner.NamePartnerType))
            //    ComboType.SelectedValue = _currentPartner.NamePartnerType;


        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentPartner.NamePartner))
                errors.AppendLine("Укажите наименование партнера");

            if (string.IsNullOrWhiteSpace(_currentPartner.LastNamePartner))
                errors.AppendLine("Укажите фамилию директора");

            if (string.IsNullOrWhiteSpace(_currentPartner.FirstNamePartner))
                errors.AppendLine("Укажите имя директора");

            if (string.IsNullOrWhiteSpace(_currentPartner.PatronymicPartner))
                errors.AppendLine("Укажите отчество директора");

            if (string.IsNullOrWhiteSpace(_currentPartner.EmailParnter))
                errors.AppendLine("Укажите почту партнера");

            if (string.IsNullOrWhiteSpace(_currentPartner.AdressePartner))
                errors.AppendLine("Укажите адрес партнера");

            if (string.IsNullOrWhiteSpace(_currentPartner.NumberPartner))
                errors.AppendLine("Укажите номер партнера");

            if (string.IsNullOrWhiteSpace(_currentPartner.INNPartner))
                errors.AppendLine("Укажите ИНН партнера");

            if (_currentPartner.TopPartner <= 0)
                errors.AppendLine("Рейтинг партнера не может быть равен или меньше 0");

            if (_currentPartner.PartnerType == null)
                errors.AppendLine("Выберите тип партнера");
            //if (string.IsNullOrWhiteSpace(_currentPartner.NameTypePartner))
            //    errors.AppendLine("");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var allPartners = MasterPolEntities.GetContext().Partner.Where(p=> p.NamePartner == _currentPartner.NamePartner && p.IDPartner!=_currentPartner.IDPartner).ToList();

            if (allPartners.Count == 0)
            {
                if (_currentPartner.IDPartner == 0)
                    MasterPolEntities.GetContext().Partner.Add(_currentPartner);

                try
                {
                    MasterPolEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные сохранены");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else MessageBox.Show("Уже существует данный партнер");
            

        }

    }
}

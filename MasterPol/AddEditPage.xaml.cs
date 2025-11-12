using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MasterPol
{
    public partial class AddEditPage : Page
    {
        private Partner _currentPartner = new Partner();

        // Свойство для ComboBox
        public List<PartnerType> PartnerTypes { get; set; }

        public AddEditPage(Partner SelectedPartner)
        {
            InitializeComponent();

            // Загружаем типы партнёров из базы
            PartnerTypes = MasterPolEntities.GetContext().PartnerType.ToList();

            // Если редактируем — используем выбранного, иначе создаём нового
            _currentPartner = SelectedPartner ?? new Partner();

            // Привязываем всё к контексту
            DataContext = this; // чтобы ComboBox видел PartnerTypes
            this.DataContext = _currentPartner;

            // Подключаем ComboBox к источнику вручную (если нужно)
            ComboType.ItemsSource = PartnerTypes;
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

            if (_currentPartner.IDTypePartner <= 0)
                errors.AppendLine("Выберите тип партнера");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var context = MasterPolEntities.GetContext();

            var sameName = context.Partner
                .FirstOrDefault(p => p.NamePartner == _currentPartner.NamePartner && p.IDPartner != _currentPartner.IDPartner);

            if (sameName != null)
            {
                MessageBox.Show("Партнёр с таким названием уже существует");
                return;
            }

            if (_currentPartner.IDPartner == 0)
                context.Partner.Add(_currentPartner);

            try
            {
                context.SaveChanges();
                MessageBox.Show("Данные сохранены");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }
    }
}

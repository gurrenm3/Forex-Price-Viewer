using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
using Forex_Price_Viewer.Extensions;
using Forex_Price_Viewer;

namespace Forex_Price_Viewer.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;

        ForexDownloader forexDownloader = new ForexDownloader();
        List<PriceData> lastPrices = new List<PriceData>();

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }


        private void ChooseDateButton_Click(object sender, RoutedEventArgs e)
        {
            datePicker.IsDropDownOpen = true;
            SetDateLabel(DateTime.Today.ToString().Split(' ')[0]);
        }

        private void Date_Picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDateLabel(e.AddedItems[0].ToString().Split(' ')[0]);
        }

        private void SetDateLabel(string selectedDate)
        {
            Dispatcher.Invoke(() => { DateLabel.Content = $"Date: {selectedDate}"; });
        }



        private void ChooseTimeButton_Click(object sender, RoutedEventArgs e)
        {
            timePicker.IsDropDownOpen = true;

            var time = DateTime.Now.TimeOfDay;
            SetTimeLabel($"{time.Hours}:{time.Minutes.ToString().PadLeft(2, '0')}:{time.Seconds.ToString().PadLeft(2, '0')}");
        }

        private void SetTimeLabel(string selectedTime)
        {
            Dispatcher.Invoke(() => { TimeLabel.Content = $"Time: {selectedTime}"; });
        }

        private void timePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (!e.NewValue.HasValue)
                return;

            var time = e.NewValue.Value.TimeOfDay;
            SetTimeLabel($"{time.Hours}:{time.Minutes.ToString().PadLeft(2,'0')}:{time.Seconds.ToString().PadLeft(2, '0')}");
        }


        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!datePicker.SelectedDate.HasValue || !timePicker.SelectedTime.HasValue)
            {
                MessageBox.Show("Error! You need to enter a date and a time in order to get price data");
                return;
            }

            //var date = DateTime.Now.AddDays(-1);
            var date = datePicker.SelectedDate.Value;
            var time = timePicker.SelectedTime.Value;
            date.AddHours(time.Hour);
            date.AddMinutes(time.Minute);


            var data = await forexDownloader.GetAllPriceData(date);
            var prices = forexDownloader.GetPriceDataFromTime(date, data);

            currencyButtons_StackPanel.Children.Clear();
            lastPrices.Clear();

            foreach (var item in prices)
            {
                lastPrices.Add(item);
                
                var button = new Button();
                button.Content = item.Ticker;
                button.Click += CurrencyButtonChosen_Click;
                currencyButtons_StackPanel.Children.Add(button);
            }

            ChooseCurrencyButton.Visibility = Visibility.Visible;
        }

        private void ChooseCurrencyButton_Click(object sender, RoutedEventArgs e)
        {
            currencyPopup.IsPopupOpen = !currencyPopup.IsPopupOpen;
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine(e.OriginalSource.ToString());
            if (e.OriginalSource is Border)
                return;

            currencyPopup.IsPopupOpen = false;
        }

        private void CurrencyButtonChosen_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var priceData = lastPrices.FirstOrDefault(data => data.Ticker == button.Content.ToString());
            ShowPriceData(priceData);
        }

        private void ShowPriceData(PriceData data)
        {
            CurrencyTextBlock.Text = "Currency: " + data.Ticker;
            DateTextBlock.Text = "Date: " + data.Date;
            TimeTextBlock.Text = "Time: " + data.Time;
            OpenTextBlock.Text = "Open: " + data.Open.ToString();
            HighTextBlock.Text = "High: " + data.High.ToString();
            LowTextBlock.Text = "Low: " + data.Low.ToString();
            CloseTextBlock.Text = "Close: " + data.Close.ToString();
        }
    }
}

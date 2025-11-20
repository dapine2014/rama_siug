using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

using SIUGJ.Models;

namespace SIUGJ.Controls
{
    public partial class HearingEvent : ContentView
    {
        public static BindableProperty CalenderEventCommandProperty =
            BindableProperty.Create(nameof(CalenderEventCommand), typeof(ICommand), typeof(HearingEvent), null);

        public HearingEvent()
        {
            InitializeComponent();
        }

        public ICommand CalenderEventCommand
        {
            get => (ICommand)GetValue(CalenderEventCommandProperty);
            set => SetValue(CalenderEventCommandProperty, value);
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var command = CalenderEventCommand;
            if (BindingContext is Hearing eventModel)
                CalenderEventCommand?.Execute(eventModel);
        }
    }
}


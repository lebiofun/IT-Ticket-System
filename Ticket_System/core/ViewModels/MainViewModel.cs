using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_System.Core;

namespace Ticket_System.core.ViewModels
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand EnterViewCommand { get; set; }
        public RelayCommand RegistrationViewCommand { get; set; }
        public RelayCommand TicketViewCommand { get; set; }
        public RelayCommand SettingViewCommand { get; set; }
        public RelayCommand AdminPanelViewCommand { get; set; }


        public EnterViewModel EnterVM { get; set; }
        public RegistrationViewModel RegistrationVM { get; set; }
        public TickitViewModel TickitVM { get; set; }
        public SettingViewModel SettingVM { get; set; }
        public AdminPanelViewModel AdminPanelVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            EnterVM = new EnterViewModel();
            RegistrationVM = new RegistrationViewModel();
            TickitVM = new TickitViewModel();
            AdminPanelVM = new AdminPanelViewModel();
            SettingVM = new SettingViewModel();
            CurrentView = EnterVM;

            EnterViewCommand = new RelayCommand(o =>
            {
                CurrentView = EnterVM;
            });

            RegistrationViewCommand = new RelayCommand(o =>
            {
                CurrentView = RegistrationVM;
            });
            TicketViewCommand = new RelayCommand(o =>
            {
                CurrentView = TickitVM;
            });
            SettingViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingVM;
            });
            AdminPanelViewCommand = new RelayCommand(o =>
            {
                CurrentView = AdminPanelVM;
            });
        }
        
    }
}

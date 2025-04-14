using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf_App_Navigation_Tutorial.Services;


namespace Wpf_App_Navigation_Tutorial.MVVM.ViewModel
{
    public partial class MainWIndowViewModel(INavigationService navService) : ObservableObject
    {
        

        [ObservableProperty] static  string title = "Main Window";


        public INavigationService NavigationService
        {
            get => navService;
            set
            {
                navService = value;
                OnPropertyChanged();
            }
        }
        [RelayCommand] public void NavigateToHome() => navService.NavigateTo<HomeViewModel>();

        [RelayCommand] public void NavigateToSettings() => navService.NavigateTo<SettingsViewModel>();
        
    }
}

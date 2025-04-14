using CommunityToolkit.Mvvm.ComponentModel;
using Wpf_App_Navigation_Tutorial.MVVM.ViewModel;

namespace Wpf_App_Navigation_Tutorial.Services
{
    public interface INavigationService
    {
        ObservableObject CurrentView { get; }
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    }

    public partial class NavigationService(Func<Type, ObservableObject> viewModelFactory) : ObservableObject, INavigationService
    {
        private readonly Func<Type, ObservableObject> _viewModelFactory = viewModelFactory;

        private ObservableObject _currentView = viewModelFactory.Invoke(typeof(HomeViewModel));
        public ObservableObject CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
        {
            ObservableObject viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }
    }
}
    
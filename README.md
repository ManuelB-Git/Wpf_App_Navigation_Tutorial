# Wpf_App_Navigation_Tutorial

Introduction
This document provides a comprehensive guide to implementing navigation in a WPF application using the MVVM (Model-View-ViewModel) pattern with dependency injection. The tutorial demonstrates a clean, maintainable approach to switching between different views in a WPF application.
Architecture Overview
The application uses:
•	.NET 9 with C# 13
•	MVVM pattern for separation of concerns
•	Community Toolkit MVVM library for simplified MVVM implementation
•	Dependency Injection using Microsoft's DI container
•	Custom NavigationService for view switching
Key Components
1. Navigation Service
The NavigationService is the core component that manages view transitions.

       // Services/NavigationService.cs
       public interface INavigationService
       {
           ObservableObject CurrentView { get; }
           void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
       }

       public partial class NavigationService : ObservableObject, INavigationService
       {
           private readonly Func<Type, ObservableObject> _viewModelFactory;
           private ObservableObject _currentView;
    
           public NavigationService(Func<Type, ObservableObject> viewModelFactory)
           {
               _viewModelFactory = viewModelFactory;
               _currentView = new HomeViewModel();
           }
    
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
   
3. ViewModels
ViewModels represent the logic for each view:
•	MainWindowViewModel - Contains navigation commands
•	HomeViewModel - View model for the home view
•	SettingsViewModel - View model for the settings view
4. Views
The UI components:
•	MainWindow - The application shell with navigation buttons
•	HomeView - A simple user control for the home page
•	SettingsView - A simple user control for the settings page
5. DataTemplates
DataTemplates in App.xaml map ViewModels to Views:
<DataTemplate DataType="{x:Type viewModel:HomeViewModel}">
    <view:HomeView />
</DataTemplate>

<DataTemplate DataType="{x:Type viewModel:SettingsViewModel}">
    <view:SettingsView />
</DataTemplate>
Setup Process
1. Dependency Injection Setup
In App.xaml.cs, we configure the dependency injection container:
        
    public App()
    {
        IServiceCollection services = new ServiceCollection();
    
        // Register the main window and its view model
        services.AddSingleton<MainWindow>(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainWIndowViewModel>()
        });
    
        // Register view models
        services.AddSingleton<MainWIndowViewModel>();
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<SettingsViewModel>();
    
        // Register the navigation service
        services.AddSingleton<INavigationService, NavigationService>();
        
        // Register the view model factory
        services.AddSingleton<Func<Type, ObservableObject>>(
            serviceProvider => viewModelType => 
                (ObservableObject)serviceProvider.GetRequiredService(viewModelType));
    
        _serviceProvider = services.BuildServiceProvider();
    }
2. Application Startup
   
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

3. Main Window Setup
The MainWindow.xaml contains:
•	Navigation buttons in a sidebar
•	A ContentControl bound to the current view

        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="Auto" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>
        
            <StackPanel Grid.Row="0">
                <Button Command="{Binding NavigateToHomeCommand}" Content="Home" />
                <Button Command="{Binding NavigateToSettingsCommand}" Content="Settings" />
            </StackPanel>
        
            <ContentControl Grid.Column="1" Content="{Binding NavigationService.CurrentView}" />
        </Grid>

Navigation Flow
1.	User clicks a navigation button
2.	The corresponding command in MainWindowViewModel executes
3.	The command calls NavigationService.NavigateTo<>() with the target ViewModel type
4.	The navigation service resolves the ViewModel from the DI container
5.	The CurrentView property is updated with the new ViewModel
6.	The UI updates because:
•	ContentControl is bound to NavigationService.CurrentView
•	DataTemplates map each ViewModel type to its corresponding View
Implementation Steps
Step 1: Create the Navigation Service
1.	Create the INavigationService interface
2.	Implement the NavigationService class with:
•	A ViewModel factory to resolve ViewModels
•	A CurrentView property to track the active view
•	A NavigateTo<>() method for navigation
Step 2: Create Views and ViewModels
1.	Create ViewModels for each page (Home, Settings)
2.	Create UserControl views for each page
3.	Create the MainWindowViewModel with navigation commands
Step 3: Setup View Mapping
1.	Add DataTemplates in App.xaml to map ViewModels to Views
2.	Ensure the ContentControl in MainWindow binds to NavigationService.CurrentView
Step 4: Configure Dependency Injection
1.	Register all services, ViewModels, and the ViewModel factory
2.	Configure the application startup
Best Practices
1.	Loose Coupling: ViewModels don't reference Views directly
2.	Single Responsibility: The NavigationService handles only navigation
3.	Dependency Injection: Components are resolved through DI
4.	MVVM Pattern: Proper separation of UI (Views) and logic (ViewModels)
Extending the Application
To add a new page:
1.	Create a new ViewModel extending ObservableObject
2.	Create a corresponding UserControl View
3.	Add a DataTemplate in App.xaml
4.	Register the ViewModel in the DI container
5.	Add a navigation command to MainWindowViewModel
This tutorial demonstrates a clean, maintainable approach to WPF navigation using modern C# features and the MVVM pattern.

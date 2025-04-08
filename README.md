<div align="center">

# 🎯 MVVM Implementation Benchmark

[![Build and Test](https://github.com/vvidov/MVVMBenchmark/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/vvidov/MVVMBenchmark/actions/workflows/build-and-test.yml)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download)
[![Windows](https://img.shields.io/badge/Windows-0078D6?logo=windows)](https://www.microsoft.com/windows)

</div>

This project demonstrates three different approaches to implementing the MVVM (Model-View-ViewModel) pattern in WPF, showcasing the evolution of MVVM implementation techniques from traditional to modern approaches. Each implementation demonstrates different patterns and best practices in WPF development.

## 🏗️ Project Structure

### 📦 Core Projects
- `Models`: Contains the shared domain model (`Person`)
  - Basic domain model with properties and business logic
  - Implements age calculation and data validation
  - Shared across all MVVM implementations

- `Services`: Contains shared services and interfaces
  - MessageBox service abstraction for testing
  - Default and test implementations
  - Enables proper unit testing without UI dependencies

- `ViewModels`: Contains three different MVVM implementations
  - Common interface `IPersonVM` for consistency
  - Three different implementation approaches
  - Demonstrates evolution of MVVM patterns

### 🖥️ Application Projects
- `WpfAppOld`: Traditional MVVM implementation
  - Manual property change notifications
  - Classic command implementation
  - Base ViewModel class approach

- `WpfAppNewStyle`: Modern MVVM with basic source generators
  - Uses CommunityToolkit.Mvvm source generators
  - Simplified property implementation
  - Mix of modern and traditional patterns

- `WpfAppToolkit`: Modern MVVM with full CommunityToolkit.Mvvm features
  - Full use of modern MVVM patterns
  - Source-generated properties and commands
  - Declarative property and command notifications

### 🧪 Testing Project
- `ModelAndVMTests`: Unit tests for models and view models
  - Comprehensive test coverage
  - Tests for all three MVVM implementations
  - Command execution testing
  - Property change notification testing
  - MessageBox interaction testing

### ⚡ Performance Project
- `PerformanceBenchmarks`: Benchmarks comparing MVVM implementations
  - Uses BenchmarkDotNet for accurate measurements
  - Compares property update performance
  - Measures instantiation overhead
  - Memory allocation analysis
  - Command execution benchmarks

## 🔄 MVVM Implementation Approaches

### 1. Traditional MVVM (PersonViewModelOldStyle)
Classic implementation using manual property change notifications and base classes.

```csharp
public class PersonViewModelOldStyle : ViewModelBase, IPersonVM
{
    private readonly IMessageBoxService _messageBoxService;
    private string _firstName;
    
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (SetProperty(ref _firstName, value))
            {
                _person.FirstName = value;
                OnPropertyChanged(nameof(DisplayText));
            }
        }
    }

    public void Reset()
    {
        var result = _messageBoxService.Show("Are you sure?", "Confirm Clear", 
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Today;
        }
    }
}
```

**Characteristics:**
- Manual property implementation
- Explicit backing fields
- Manual property change notifications
- Base class for INotifyPropertyChanged implementation
- Service injection for testability
- More boilerplate code but full control
- Familiar to developers coming from older .NET versions

### 2. Modern MVVM with Basic Source Generators (PersonViewModel)
Uses CommunityToolkit.Mvvm's source generators for properties while keeping manual notifications.

```csharp
public partial class PersonViewModel : ObservableObject, IPersonVM
{
    private readonly IMessageBoxService _messageBoxService;

    [ObservableProperty]
    private string firstName = string.Empty;

    partial void OnFirstNameChanged(string value)
    {
        _person.FirstName = value;
        OnPropertyChanged(nameof(DisplayText));
    }

    [RelayCommand]
    private void Reset()
    {
        var result = _messageBoxService.Show("Are you sure?", "Confirm Clear", 
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Today;
        }
    }
}
```

**Characteristics:**
- Source-generated properties using [ObservableProperty]
- Reduced boilerplate for property declarations
- Manual handling of dependent property notifications
- Partial methods for property change callbacks
- Service injection for testability
- Good balance between control and convenience

### 3. Modern MVVM with Full Toolkit Features (PersonViewModel2)
Leverages all CommunityToolkit.Mvvm features including property notifications and command handling.

```csharp
public partial class PersonViewModel2 : ObservableObject, IPersonVM
{
    private readonly IMessageBoxService _messageBoxService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyPropertyChangedFor(nameof(CanUpdatePerson))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string firstName = string.Empty;

    [RelayCommand]
    private void Reset()
    {
        var result = _messageBoxService.Show("Are you sure?", "Confirm Clear", 
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Today;
        }
    }
}
```

**Characteristics:**
- Full use of source generators
- Declarative property and command notifications
- Automatic dependency tracking
- Service injection for testability
- Minimal boilerplate code
- Modern C# features and patterns

## 🔍 Testing Approach

### Unit Testing with Service Injection
```csharp
public class PersonViewModel2Tests
{
    [Theory]
    [InlineData(MessageBoxResult.Yes, true)]
    [InlineData(MessageBoxResult.No, false)]
    public void Reset_ShouldRespectUserConfirmation(
        MessageBoxResult userChoice, bool shouldReset)
    {
        // Arrange
        string text = string.Empty;
        string caption = string.Empty;
        MessageBoxButton button = MessageBoxButton.OK;

        var messageBoxService = new TestMessageBoxService(
            (messageBoxText, messageBoxCaption, messageBoxButton, image) =>
        {
            text = messageBoxText;
            caption = messageBoxCaption;
            button = messageBoxButton;
            return userChoice;
        });

        var viewModel = new PersonViewModel2(messageBoxService)
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        viewModel.ResetCommand.Execute(null);

        // Assert
        Assert.Equal("Are you sure?", text);
        Assert.Equal("Confirm Clear", caption);
        Assert.Equal(MessageBoxButton.YesNo, button);

        if (shouldReset)
        {
            Assert.Equal(string.Empty, viewModel.FirstName);
            Assert.Equal(string.Empty, viewModel.LastName);
        }
        else
        {
            Assert.Equal("John", viewModel.FirstName);
            Assert.Equal("Doe", viewModel.LastName);
        }
    }
}
```

### Test Coverage Areas
1. **Property Change Notifications**
   - Verify property updates trigger correct notifications
   - Test dependent property updates
   - Validate display text formatting

2. **Command Testing**
   - Test command enable/disable conditions
   - Verify command execution effects
   - Test command state after property changes
   - Mock user interactions via services

3. **Model Integration**
   - Verify ViewModel-Model synchronization
   - Test age calculation logic
   - Validate data flow between layers

## 💡 Best Practices and Recommendations

### 1. Service Abstraction
- Abstract UI dependencies behind interfaces
- Use dependency injection for services
- Create test-specific service implementations
- Keep services focused and single-purpose

### 2. Testing Strategy
- Mock UI interactions using service interfaces
- Test user confirmation scenarios
- Verify service interaction parameters
- Test both positive and negative paths

### 3. Implementation Tips
- Use source generators to reduce boilerplate
- Implement proper service interfaces
- Follow SOLID principles
- Keep ViewModels testable and maintainable

## 📊 Performance Results

```
|--------------------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|
|    OldStyle_PropertyUpdate |  850.0 ns | 12.32 ns |  9.63 ns |  1.00 |    0.00 | 0.1559 |     816 B |
| BasicModern_PropertyUpdate |  825.5 ns | 11.89 ns |  9.28 ns |  0.97 |    0.02 | 0.1450 |     760 B |
| FullToolkit_PropertyUpdate |  890.2 ns | 13.01 ns | 10.15 ns |  1.05 |    0.02 | 0.1657 |     872 B |
```

### Key Findings
- Traditional MVVM excels at object creation
- All implementations have similar notification performance
- Memory overhead differences are minimal
- Toolkit benefits outweigh small performance costs
- Service abstraction has negligible performance impact

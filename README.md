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

- `WinFormsAppToolkit`: Windows Forms MVVM implementation
  - Uses same ViewModels as WPF without modifications
  - Demonstrates platform-independent MVVM
  - Windows Forms data binding capabilities
  - No code-behind approach

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
- Design ViewModels to be platform-independent
- Support both WPF and Windows Forms bindings
- Avoid platform-specific code in ViewModels
- Legacy Windows Forms apps can use modern MVVM

## 📊 Performance Results

### Environment
- BenchmarkDotNet v0.13.12
- Windows 10 (10.0.19045.5608/22H2/2022Update)
- AMD Ryzen 7 5700G with Radeon Graphics
- .NET 8.0.14 (Release), X64 RyuJIT AVX2

### Benchmark Results (Release Configuration)

| Method                             | Mean      | Error    | StdDev   | Ratio | Gen0   | Allocated |
|----------------------------------- |----------:|---------:|---------:|------:|-------:|----------:|
| Basic Modern - Notifications       |  92.75 ns |  1.82 ns |  1.78 ns |  0.38 |      - |       0 B |
| Full Toolkit - Notifications       |  92.07 ns |  1.81 ns |  1.69 ns |  0.38 |      - |       0 B |
| Traditional - Command              |  96.26 ns |  1.94 ns |  2.45 ns |  0.40 | 0.0057 |      48 B |
| Traditional - Create               |  99.10 ns |  2.01 ns |  4.49 ns |  0.40 | 0.0229 |     192 B |
| Basic Modern - Command             | 109.16 ns |  2.18 ns |  3.76 ns |  0.44 | 0.0114 |      96 B |
| Full Toolkit - Command             | 119.55 ns |  2.41 ns |  2.58 ns |  0.49 |      - |       0 B |
| Full Toolkit - Create              | 158.77 ns |  3.23 ns |  3.59 ns |  0.65 | 0.0181 |     152 B |
| Basic Modern - Property Chain      | 207.17 ns |  3.47 ns |  3.71 ns |  0.85 | 0.0105 |      88 B |
| Traditional - Property Chain       | 209.92 ns |  2.12 ns |  1.88 ns |  0.86 | 0.0105 |      88 B |
| Full Toolkit - Property Chain      | 213.61 ns |  2.61 ns |  2.31 ns |  0.87 | 0.0105 |      88 B |
| Full Toolkit - Complete            | 235.51 ns |  2.21 ns |  1.96 ns |  0.96 | 0.0086 |      72 B |
| Basic Modern - Complete            | 238.66 ns |  4.37 ns |  5.69 ns |  0.98 | 0.0086 |      72 B |
| Traditional - Complete             | 244.15 ns |  4.83 ns |  5.16 ns |  1.00 | 0.0143 |     120 B |

### Key Findings

1. Notification Performance
   - Modern approaches excel at property notifications (92ns)
   - Zero allocation for notifications in Full Toolkit
   - ~60% faster than complete operation benchmarks

2. Command Execution
   - Traditional: Fastest but allocates 48B
   - Basic Modern: 13% slower, doubles allocation (96B)
   - Full Toolkit: 24% slower but zero allocation

3. Object Creation
   - Traditional: Fastest (99ns) but highest allocation (192B)
   - Modern approaches: ~60% slower but 20% less memory
   - Consistent between Basic and Full Toolkit

4. Property Chain Updates
   - All implementations within 3% performance range
   - Identical memory allocation (88B)
   - Shows maturity of property notification system

5. Overall Analysis
   - Full Toolkit advantages:
     * Zero-allocation notifications and commands
     * Consistent performance across operations
     * Modern features with minimal overhead
   - Basic Modern benefits:
     * Best balance of performance and features
     * Reduced memory footprint
     * Simple implementation
   - Traditional approach:
     * Fastest raw performance
     * Higher but predictable memory usage
     * Good for memory-constrained scenarios

6. Recommendations
   - New projects: Use Full Toolkit for best features/performance ratio
   - Memory-critical: Consider Basic Modern approach
   - Legacy systems: Traditional approach still viable
   - Platform independence: Full Toolkit ideal for WPF/WinForms

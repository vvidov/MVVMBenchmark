# MVVM Implementation Benchmark

This project demonstrates three different approaches to implementing the MVVM (Model-View-ViewModel) pattern in WPF, showcasing the evolution of MVVM implementation techniques from traditional to modern approaches. Each implementation demonstrates different patterns and best practices in WPF development.

## Project Structure

### Core Projects
- `Models`: Contains the shared domain model (`Person`)
  - Basic domain model with properties and business logic
  - Implements age calculation and data validation
  - Shared across all MVVM implementations

- `ViewModels`: Contains three different MVVM implementations
  - Common interface `IPersonVM` for consistency
  - Three different implementation approaches
  - Demonstrates evolution of MVVM patterns

### Application Projects
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

### Testing Project
- `ModelAndVMTests`: Unit tests for models and view models
  - Comprehensive test coverage
  - Tests for all three MVVM implementations
  - Command execution testing
  - Property change notification testing

### Performance Project
- `PerformanceBenchmarks`: Benchmarks comparing MVVM implementations
  - Uses BenchmarkDotNet for accurate measurements
  - Compares property update performance
  - Measures instantiation overhead
  - Memory allocation analysis
  - Command execution benchmarks

## MVVM Implementation Approaches

### 1. Traditional MVVM (PersonViewModelOldStyle)
Classic implementation using manual property change notifications and base classes.

```csharp
public class PersonViewModelOldStyle : ViewModelBase, IPersonVM
{
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
}
```

**Characteristics:**
- Manual property implementation
- Explicit backing fields
- Manual property change notifications
- Base class for INotifyPropertyChanged implementation
- More boilerplate code but full control
- Familiar to developers coming from older .NET versions

### 2. Modern MVVM with Basic Source Generators (PersonViewModel)
Uses CommunityToolkit.Mvvm's source generators for properties while keeping manual notifications.

```csharp
public partial class PersonViewModel : ObservableObject, IPersonVM
{
    [ObservableProperty]
    private string firstName = string.Empty;

    partial void OnFirstNameChanged(string value)
    {
        _person.FirstName = value;
        OnPropertyChanged(nameof(DisplayText));
    }
}
```

**Characteristics:**
- Source-generated properties using [ObservableProperty]
- Reduced boilerplate for property declarations
- Manual handling of dependent property notifications
- Partial methods for property change callbacks
- Good balance between control and convenience

### 3. Modern MVVM with Full Toolkit Features (PersonViewModel2)
Leverages all CommunityToolkit.Mvvm features including property notifications and command handling.

```csharp
public partial class PersonViewModel2 : ObservableObject, IPersonVM
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText))]
    [NotifyPropertyChangedFor(nameof(CanUpdatePerson))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string firstName = string.Empty;

    [RelayCommand(CanExecute = nameof(CanUpdatePerson))]
    private void Save()
    {
        _person.FirstName = FirstName;
        _person.LastName = LastName;
        _person.DateOfBirth = DateOfBirth;
        // Add actual save implementation here
    }
}
```

**Characteristics:**
- Full use of source generators for properties and commands
- Declarative property change notifications
- Automatic command generation with [RelayCommand]
- Integrated command enable/disable state management
- Minimal boilerplate with maximum functionality
- Modern, clean, and maintainable code

## Key Differences

1. **Code Volume:**
   - Old Style: ~70 lines for basic properties
   - Basic Modern: ~40 lines for the same functionality
   - Full Toolkit: ~30 lines with additional features

2. **Maintainability:**
   - Old Style: More code to maintain, but explicit and clear
   - Basic Modern: Less code, clear property change handling
   - Full Toolkit: Least code, declarative and self-documenting

3. **Features:**
   - Old Style: Basic property change notification
   - Basic Modern: Property generation with manual notifications
   - Full Toolkit: Full property and command generation with automatic notifications

## Running the Applications

Each implementation has its own WPF application project:

```bash
# Run the traditional MVVM implementation
dotnet run --project WpfAppOld

# Run the basic modern implementation
dotnet run --project WpfAppNewStyle

# Run the full toolkit implementation
dotnet run --project WpfAppToolkit
```

## Performance Benchmarks

To run performance benchmarks:

```bash
dotnet run --project PerformanceBenchmarks --configuration Release
```

The benchmarks measure:

1. **Property Updates**
   - Setting multiple properties
   - Calculating dependent properties
   - Property change notification overhead
   - Memory allocations per update

2. **Instantiation Performance**
   - Creation time for each ViewModel type
   - Memory allocation patterns
   - Initialization costs

3. **Memory Analysis**
   - Allocation patterns for each implementation
   - GC pressure comparison
   - Memory footprint differences

Example benchmark output:
```
|                    Method |      Mean |    Error |   StdDev | Ratio | RatioSD |  Gen 0 | Allocated |
|-------------------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|
|     OldStyle_PropertyUpdate |  850.0 ns | 12.32 ns |  9.63 ns |  1.00 |    0.00 | 0.1559 |     816 B |
| BasicModern_PropertyUpdate |  825.5 ns | 11.89 ns |  9.28 ns |  0.97 |    0.02 | 0.1450 |     760 B |
| FullToolkit_PropertyUpdate |  890.2 ns | 13.01 ns | 10.15 ns |  1.05 |    0.02 | 0.1657 |     872 B |
```

## Testing Strategy

### Unit Testing Approach
```csharp
public class PersonViewModel2Tests
{
    [Fact]
    public void Constructor_ShouldInitializeWithEmptyValues()
    {
        var viewModel = new PersonViewModel2();
        Assert.Equal(string.Empty, viewModel.FirstName);
        Assert.Equal(string.Empty, viewModel.LastName);
    }

    [Theory]
    [InlineData("", "Smith", false)]
    [InlineData("John", "", false)]
    [InlineData("John", "Smith", true)]
    public void CanSave_ShouldReturnCorrectValue(string firstName, 
        string lastName, bool expectedCanSave)
    {
        var viewModel = new PersonViewModel2
        {
            FirstName = firstName,
            LastName = lastName
        };
        
        var canExecute = viewModel.SaveCommand.CanExecute(null);
        Assert.Equal(expectedCanSave, canExecute);
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

3. **Model Integration**
   - Verify ViewModel-Model synchronization
   - Test age calculation logic
   - Validate data flow between layers

## XAML Implementation

### View Structure
```xaml
<Grid Margin="10">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <TextBox Text="{Binding FirstName, 
             UpdateSourceTrigger=PropertyChanged}"/>
    
    <Button Content="Save" 
            Command="{Binding SaveCommand}"
            IsEnabled="{Binding CanSave}"/>
</Grid>
```

### Binding Patterns
1. **Property Bindings**
   - Use UpdateSourceTrigger=PropertyChanged for immediate updates
   - Two-way binding for editable fields
   - One-way binding for display-only data

2. **Command Bindings**
   - Direct command binding with RelayCommand
   - Automatic enable/disable through CanExecute
   - Command parameter binding when needed

3. **Validation Display**
   - Error template customization
   - Validation rule implementation
   - INotifyDataErrorInfo support

## Best Practices and Recommendations

### 1. New Projects
- Use the Full Toolkit approach (PersonViewModel2)
- Leverage source generators for reduced boilerplate
- Implement proper command state management
- Use declarative notifications
- Follow SOLID principles

### 2. Existing Projects
- Gradual migration path:
  1. Start with command improvements
  2. Introduce source-generated properties
  3. Add declarative notifications
  4. Refactor to full toolkit patterns
- Can mix approaches within same project
- Focus on maintainable, testable code

### 3. Learning Path
1. **Understanding Basics (Old Style)**
   - Manual property change notification
   - ICommand implementation
   - MVVM fundamentals

2. **Modern Basics (Basic Modern)**
   - Source generators introduction
   - Simplified property implementation
   - Basic command patterns

3. **Advanced Patterns (Full Toolkit)**
   - Declarative notifications
   - Advanced command features
   - Full toolkit capabilities

### 4. Performance Considerations
- Use proper UpdateSourceTrigger settings
- Implement property change notification efficiently
- Consider command execution frequency
- Profile UI updates and bindings

### 5. Testing Strategy
- Write unit tests for ViewModels
- Test command logic thoroughly
- Verify property change notifications
- Test validation rules
- Consider UI automation testing

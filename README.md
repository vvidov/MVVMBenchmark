# MVVM Implementation Benchmark

This project demonstrates three different approaches to implementing the MVVM (Model-View-ViewModel) pattern in WPF, showcasing the evolution of MVVM implementation techniques from traditional to modern approaches.

## Project Structure

- `Models`: Contains the shared domain model (`Person`)
- `ViewModels`: Contains three different MVVM implementations
- `WpfAppOld`: Traditional MVVM implementation
- `WpfAppNewStyle`: Modern MVVM with basic source generators
- `WpfAppToolkit`: Modern MVVM with full CommunityToolkit.Mvvm features
- `ModelAndVMTests`: Unit tests for models and view models

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

## Running Tests

```bash
dotnet test
```

## Recommendations

1. **New Projects:**
   - Use the Full Toolkit approach (PersonViewModel2)
   - Provides the most features with least code
   - Best maintainability and readability

2. **Existing Projects:**
   - Consider gradual migration from Old Style to Modern approaches
   - Can mix approaches within the same project
   - Basic Modern approach is a good intermediate step

3. **Learning MVVM:**
   - Start with Old Style to understand the fundamentals
   - Progress to Basic Modern to learn source generators
   - Finally, adopt Full Toolkit for maximum productivity

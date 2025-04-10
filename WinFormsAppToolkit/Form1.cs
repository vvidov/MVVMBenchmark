using Models;
using Services;
using ViewModels;

namespace WinFormsAppToolkit;

public partial class Form1 : Form
{
    private readonly PersonViewModel2 _viewModel;
    private readonly IMessageBoxService _messageBoxService;

    public Form1()
    {
        InitializeComponent();

        // Create services and view model
        _messageBoxService = new DefaultMessageBoxService();
        _viewModel = new PersonViewModel2(_messageBoxService);

        // Bind text boxes
        txtFirstName.DataBindings.Add(nameof(TextBox.Text), _viewModel, nameof(PersonViewModel2.FirstName), false, DataSourceUpdateMode.OnPropertyChanged);
        txtLastName.DataBindings.Add(nameof(TextBox.Text), _viewModel, nameof(PersonViewModel2.LastName), false, DataSourceUpdateMode.OnPropertyChanged);
        dtpDateOfBirth.DataBindings.Add(nameof(DateTimePicker.Value), _viewModel, nameof(PersonViewModel2.DateOfBirth), false, DataSourceUpdateMode.OnPropertyChanged);

        // Bind display labels
        lblDisplayText.DataBindings.Add(nameof(Label.Text), _viewModel, nameof(PersonViewModel2.DisplayText));


        // Bind buttons
        btnSave.Click += (s, e) => _viewModel.SaveCommand.Execute(null);
        btnReset.Click += (s, e) => _viewModel.ResetCommand.Execute(null);

        // Bind button enabled states
        // Set initial state and update when command can execute changes
        btnSave.Enabled = _viewModel.SaveCommand.CanExecute(null);
        _viewModel.SaveCommand.CanExecuteChanged += (s, e) => btnSave.Enabled = _viewModel.SaveCommand.CanExecute(null);
    }
}

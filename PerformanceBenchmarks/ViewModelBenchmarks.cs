using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using ViewModels;

namespace PerformanceBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class ViewModelBenchmarks
{
    private PersonViewModelOldStyle _oldStyleVm = null!;
    private PersonViewModel _basicModernVm = null!;
    private PersonViewModel2 _fullToolkitVm = null!;
    
    private const string TestFirstName = "John";
    private const string TestLastName = "Doe";
    private static readonly DateTime TestDate = new DateTime(1990, 1, 1);

    [GlobalSetup]
    public void Setup()
    {
        _oldStyleVm = new PersonViewModelOldStyle();
        _basicModernVm = new PersonViewModel();
        _fullToolkitVm = new PersonViewModel2();

        // Initialize with some data to enable validation scenarios
        _oldStyleVm.FirstName = TestFirstName;
        _oldStyleVm.LastName = TestLastName;
        _basicModernVm.FirstName = TestFirstName;
        _basicModernVm.LastName = TestLastName;
        _fullToolkitVm.FirstName = TestFirstName;
        _fullToolkitVm.LastName = TestLastName;
    }

    [Benchmark(Baseline = true, Description = "Traditional MVVM")]
    public void OldStyle_PropertyUpdate()
    {
        _oldStyleVm.FirstName = "John";
        _oldStyleVm.LastName = "Doe";
        _oldStyleVm.DateOfBirth = new DateTime(1990, 1, 1);
        _ = _oldStyleVm.DisplayText;
        _ = _oldStyleVm.Age;
    }

    [Benchmark(Description = "Basic Modern MVVM")]
    public void BasicModern_PropertyUpdate()
    {
        _basicModernVm.FirstName = "John";
        _basicModernVm.LastName = "Doe";
        _basicModernVm.DateOfBirth = new DateTime(1990, 1, 1);
        _ = _basicModernVm.DisplayText;
        _ = _basicModernVm.Age;
    }

    [Benchmark(Description = "Full Toolkit MVVM")]
    public void FullToolkit_PropertyUpdate()
    {
        _fullToolkitVm.FirstName = "John";
        _fullToolkitVm.LastName = "Doe";
        _fullToolkitVm.DateOfBirth = new DateTime(1990, 1, 1);
        _ = _fullToolkitVm.DisplayText;
        _ = _fullToolkitVm.Age;
    }

    [Benchmark(Description = "Traditional MVVM - Create")]
    public PersonViewModelOldStyle OldStyle_Creation()
    {
        return new PersonViewModelOldStyle();
    }

    [Benchmark(Description = "Basic Modern MVVM - Create")]
    public PersonViewModel BasicModern_Creation()
    {
        return new PersonViewModel();
    }

    [Benchmark(Description = "Full Toolkit MVVM - Create")]
    public PersonViewModel2 FullToolkit_Creation()
    {
        return new PersonViewModel2();
    }

    #region Property Chain Benchmarks

    [Benchmark(Description = "Traditional MVVM - Property Chain")]
    public void OldStyle_PropertyChain()
    {
        _oldStyleVm.FirstName = TestFirstName + " Updated";
        _ = _oldStyleVm.DisplayText; // Triggers dependent property
        _ = _oldStyleVm.Age; // Triggers age calculation
    }

    [Benchmark(Description = "Basic Modern MVVM - Property Chain")]
    public void BasicModern_PropertyChain()
    {
        _basicModernVm.FirstName = TestFirstName + " Updated";
        _ = _basicModernVm.DisplayText;
        _ = _basicModernVm.Age;
    }

    [Benchmark(Description = "Full Toolkit MVVM - Property Chain")]
    public void FullToolkit_PropertyChain()
    {
        _fullToolkitVm.FirstName = TestFirstName + " Updated";
        _ = _fullToolkitVm.DisplayText;
        _ = _fullToolkitVm.Age;
    }

    #endregion

    #region Property Notification Benchmarks

    [Benchmark(Description = "Traditional MVVM - Notifications")]
    public void OldStyle_PropertyNotifications()
    {
        _oldStyleVm.FirstName = "";
        _oldStyleVm.LastName = "";
        _oldStyleVm.DateOfBirth = TestDate;
        _ = _oldStyleVm.DisplayText;
        _ = _oldStyleVm.Age;
    }

    [Benchmark(Description = "Basic Modern MVVM - Notifications")]
    public void BasicModern_PropertyNotifications()
    {
        _basicModernVm.FirstName = "";
        _basicModernVm.LastName = "";
        _basicModernVm.DateOfBirth = TestDate;
        _ = _basicModernVm.DisplayText;
        _ = _basicModernVm.Age;
    }

    [Benchmark(Description = "Full Toolkit MVVM - Notifications")]
    public void FullToolkit_PropertyNotifications()
    {
        _fullToolkitVm.FirstName = "";
        _fullToolkitVm.LastName = "";
        _fullToolkitVm.DateOfBirth = TestDate;
        _ = _fullToolkitVm.DisplayText;
        _ = _fullToolkitVm.Age;
    }

    #endregion
}

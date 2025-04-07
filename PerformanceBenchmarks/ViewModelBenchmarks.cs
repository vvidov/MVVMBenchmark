using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using ViewModels;

namespace PerformanceBenchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ViewModelBenchmarks
{
    private PersonViewModelOldStyle _oldStyleVm = null!;
    private PersonViewModel _basicModernVm = null!;
    private PersonViewModel2 _fullToolkitVm = null!;

    [GlobalSetup]
    public void Setup()
    {
        _oldStyleVm = new PersonViewModelOldStyle();
        _basicModernVm = new PersonViewModel();
        _fullToolkitVm = new PersonViewModel2();
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
}

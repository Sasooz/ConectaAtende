using BenchmarkDotNet.Attributes;
using ConectaAtende.Application.Services;
using ConectaAtende.Application.Undo;
using Projeto.Infrastructure.Repositories;

namespace ConectaAtende.Benchmarks;

[MemoryDiagnoser]
public class ContactBenchmarks
{
    private ContactService _service = null!;

    private List<Guid> _ids = new();

    [GlobalSetup]
    public async Task Setup()
    {
        var repository =
            new InMemoryContactRepository();

        var undo =
            new UndoService();

        _service =
            new ContactService(repository, undo);

        for (int i = 0; i < 10000; i++)
        {
            var id = await _service.CreateAsync(
                $"Contact {i}",
                $"1199999{i}");

            _ids.Add(id);
        }
    }

    [Benchmark]
    public async Task InsertContact()
    {
        await _service.CreateAsync(
            "Novo contato",
            Guid.NewGuid().ToString());
    }

    [Benchmark]
    public async Task SearchByName()
    {
        await _service.SearchByNameAsync("Contact");
    }

    [Benchmark]
    public async Task SearchByPhone()
    {
        await _service.SearchByPhoneAsync("1199");
    }

    [Benchmark]
    public async Task UpdateContact()
    {
        var id = _ids[5000];

        await _service.UpdateAsync(
            id,
            "Updated Name",
            "11988888888");
    }
}
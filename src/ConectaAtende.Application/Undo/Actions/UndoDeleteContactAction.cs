using ConectaAtende.Application.Undo;
using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Repositories;

namespace ConectaAtende.Application.Undo.Actions;

public class UndoDeleteContactAction : IUndoAction
{
    private readonly IContactRepository _repository;
    private readonly Contact _contact;

    public UndoDeleteContactAction(
        IContactRepository repository,
        Contact contact)
    {
        _repository = repository;
        _contact = contact;
    }

    public async Task ExecuteAsync()
    {
        await _repository.AddAsync(_contact);
    }
}
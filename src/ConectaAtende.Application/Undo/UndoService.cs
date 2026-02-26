using System.Collections.Concurrent;

namespace ConectaAtende.Application.Undo;

public class UndoService
{
    private readonly ConcurrentStack<IUndoAction> _stack = new();

    public void Register(IUndoAction action)
    {
        _stack.Push(action);
    }

    public async Task<bool> UndoAsync()
    {
        if (_stack.TryPop(out var action))
        {
            await action.ExecuteAsync();
            return true;
        }

        return false;
    }

    public bool HasActions()
    {
        return !_stack.IsEmpty;
    }
}
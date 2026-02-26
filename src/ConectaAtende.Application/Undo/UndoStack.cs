using System.Collections.Concurrent;

namespace ConectaAtende.Application.Undo;

public class UndoStack
{
    private readonly ConcurrentStack<UndoOperation> _stack = new();

    public void Push(UndoOperation operation)
    {
        _stack.Push(operation);
    }

    public async Task<bool> UndoAsync()
    {
        if (_stack.TryPop(out var operation))
        {
            await operation.ExecuteAsync();
            return true;
        }

        return false;
    }

    public bool HasOperations()
    {
        return !_stack.IsEmpty;
    }
}
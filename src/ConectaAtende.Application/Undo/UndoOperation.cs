namespace ConectaAtende.Application.Undo;

public class UndoOperation
{
    public Func<Task> UndoAction { get; }

    public UndoOperation(Func<Task> undoAction)
    {
        UndoAction = undoAction;
    }

    public Task ExecuteAsync()
    {
        return UndoAction();
    }
}
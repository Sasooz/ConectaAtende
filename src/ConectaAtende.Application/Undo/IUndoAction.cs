namespace ConectaAtende.Application.Undo;

public interface IUndoAction
{
    Task ExecuteAsync();
}
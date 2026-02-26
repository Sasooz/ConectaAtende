using ConectaAtende.Domain.Entities;

namespace Projeto.Infrastructure.Persistence.InMemory;

public class RecentContactsList
{
    private readonly LinkedList<Contact> _list = new();
    private readonly Dictionary<Guid, LinkedListNode<Contact>> _map = new();

    private readonly int _capacity;

    public RecentContactsList(int capacity = 10)
    {
        _capacity = capacity;
    }

    public void Add(Contact contact)
    {
        if (_map.TryGetValue(contact.Id, out var existingNode))
        {
            _list.Remove(existingNode);
            _list.AddFirst(existingNode);
            return;
        }

        var node = new LinkedListNode<Contact>(contact);

        _list.AddFirst(node);
        _map[contact.Id] = node;

        if (_list.Count > _capacity)
        {
            var last = _list.Last!;

            _map.Remove(last.Value.Id);
            _list.RemoveLast();
        }
    }

    public List<Contact> GetRecent(int count)
    {
        return _list.Take(count).ToList();
    }

    public void Remove(Guid contactId)
    {
        if (_map.TryGetValue(contactId, out var node))
        {
            _list.Remove(node);
            _map.Remove(contactId);
        }
    }
}
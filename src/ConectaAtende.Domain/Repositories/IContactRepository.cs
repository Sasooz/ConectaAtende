using ConectaAtende.Domain.Entities;

public interface IContactRepository
{
    Task AddAsync(Contact contact);

    Task<Contact?> GetByIdAsync(Guid id);

    Task UpdateAsync(Contact contact);

    Task DeleteAsync(Guid id);

    Task<bool> ExistsByPhoneAsync(string normalizedPhone);

    Task<IEnumerable<Contact>> GetPagedAsync(int page, int pageSize);

    Task<IEnumerable<Contact>> SearchByNameAsync(string name);

    Task<IEnumerable<Contact>> SearchByPhoneAsync(string phone);

    Task<IEnumerable<Contact>> GetRecentAsync(int limit);

    Task<int> CountAsync();
}
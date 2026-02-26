using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Enums;
using ConectaAtende.Domain.Services;

namespace ConectaAtende.Application.Services;

public class DefaultTicketTriagePolicy : ITicketTriagePolicy
{
    public TicketPriority DefinePriority(Ticket ticket)
    {
        var text = $"{ticket.Title} {ticket.Description}".ToLower();

        if (text.Contains("critico"))
            return TicketPriority.Critical;

        if (text.Contains("urgente"))
            return TicketPriority.High;

        if (text.Contains("erro"))
            return TicketPriority.High;

        return TicketPriority.Normal;
    }

    public string DefineCategory(Ticket ticket)
    {
        var text = $"{ticket.Title} {ticket.Description}".ToLower();

        if (text.Contains("pagamento"))
            return "Financeiro";

        if (text.Contains("erro"))
            return "Suporte Técnico";

        if (text.Contains("login"))
            return "Acesso";

        return "Geral";
    }
}
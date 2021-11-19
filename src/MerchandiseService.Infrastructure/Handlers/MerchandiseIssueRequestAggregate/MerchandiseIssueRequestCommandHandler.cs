using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MerchandiseService.Infrastructure.Commands.MerchandiseIssueRequest;

namespace MerchandiseService.Infrastructure.Handlers.MerchandiseIssueRequestAggregate
{
    public class MerchandiseIssueRequestCommandHandler : IRequestHandler<MerchandiseIssueRequestCommand>
    {
        public Task<Unit> Handle(MerchandiseIssueRequestCommand request, CancellationToken cancellationToken)
        {
            // Запрос к stock-api для выдачи мерча
            throw new System.NotImplementedException();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MerchandiseService.Domain.Contracts;
using MerchandiseService.Infrastructure.Repositories.Infrastructure.Exceptions;
using MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;
using Npgsql;

namespace MerchandiseService.Infrastructure.Repositories.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _connectionFactory;

        private NpgsqlTransaction _transaction;

        private readonly IChangeTracker _changeTracker;

        private readonly IPublisher _publisher;

        public UnitOfWork(IDbConnectionFactory<NpgsqlConnection> connectionFactory, NpgsqlTransaction transaction,
            IChangeTracker changeTracker, IPublisher publisher)
        {
            _connectionFactory = connectionFactory;
            _changeTracker = changeTracker;
            _publisher = publisher;
        }


        public void Dispose()
        {
            _transaction?.Dispose();
        }

        public async ValueTask StartTransaction(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                return;
            }

            var connection = await _connectionFactory.CreateConnection(cancellationToken);
            _transaction = await connection.BeginTransactionAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                throw new NoActiveTransactionStartedException();
            }

            var domainEvents = new Queue<INotification>(_changeTracker.TrackedEntities.SelectMany(e =>
            {
                var events = e.DomainEvents;
                e.ClearDomainEvents();
                return events;
            }));

            while (domainEvents.TryDequeue(out var notification))
            {
                _publisher.Publish(notification, cancellationToken);
            }

            await _transaction.CommitAsync(cancellationToken);
        }
    }
}
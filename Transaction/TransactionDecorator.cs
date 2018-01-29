﻿using System;
using System.Threading.Tasks;
using Arta.Entities.DataAccess;

namespace Arta.Infrastructure.Transaction
{
    public class TransactionDecorator<TRequest, TResponse> : IRequestHandlerDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly PartnerContext _context;

        public TransactionDecorator(PartnerContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var response = await next();

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return response;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
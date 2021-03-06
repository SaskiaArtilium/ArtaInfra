﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arta.Infrastructure.Validation
{
    public class ValidationDecorator<TRequest, TResponse> : IRequestHandlerDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

        public ValidationDecorator(IEnumerable<IRequestValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<ApiResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                foreach (var validator in _validators.OrderBy(v => v.Order))
                {
                    var result = await validator.InternalValidate(request);

                    if (result.IsFailure)
                        return ApiResult<TResponse>.Fail(result.HttpStatusCode, result.ErrorMessage, result.ErrorCode);
                }
            }

            return await next();
        }
    }
}
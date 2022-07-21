using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vapie.WebUI.AppCode.Infrastructure
{
    public interface IJsonRequest : IRequest<CommandJsonResponse>
    {
    }
    public interface IJsonRequestHandler<T> : IRequestHandler<T,CommandJsonResponse>
        where T : IRequest<CommandJsonResponse>
    {

    }
}

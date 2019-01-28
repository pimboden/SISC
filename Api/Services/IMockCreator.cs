using System.Collections.Generic;
using Sisc.RestApi.Models;

namespace Sisc.RestApi.Services
{
    public interface IMockCreator
    {
        List<Value> AllValues { get; }
    }
}
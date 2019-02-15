using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sisc.Api.Common.Base;

namespace Sisc.Api.Common
{
    public class Country:DataEntity
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
    }
}

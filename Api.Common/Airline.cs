using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sisc.Api.Common.Base;

namespace Sisc.Api.Common
{
    public class Airline:DataEntity
    {
        [Key]
        public int? Id { get; set; }
        public string Name { get; set; }
        public string IataCode { get; set; }
        public string IcaoCode { get; set; }
    }
}

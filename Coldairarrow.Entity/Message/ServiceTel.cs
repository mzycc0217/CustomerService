using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity
{
    /// <summary>
    /// ServiceTel
    /// </summary>
    [Table("ServiceTel")]
    public class ServiceTel
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public String CreatorId { get; set; }

        /// <summary>
        /// Deleted
        /// </summary>
        public Boolean Deleted { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public String Action { get; set; }

        /// <summary>
        /// DTAE
        /// </summary>
        public DateTime? DTAE { get; set; }

        /// <summary>
        /// state
        /// </summary>
        public Int32? state { get; set; }

        /// <summary>
        /// Data1
        /// </summary>
        public String Data1 { get; set; }

        /// <summary>
        /// Data2
        /// </summary>
        public String Data2 { get; set; }

    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity
{
    /// <summary>
    /// Expression
    /// </summary>
    [Table("Expression")]
    public class Expression
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
        /// Face
        /// </summary>
        public String Face { get; set; }

        /// <summary>
        /// Decimal
        /// </summary>
        public String Decimal { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public Int32 Sort { get; set; }

    }
}
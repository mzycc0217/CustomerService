using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity
{
    /// <summary>
    /// Base_UserDecimal
    /// </summary>
    [Table("Base_UserDecimal")]
    public class Base_UserDecimal
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
        /// UserId
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Openid
        /// </summary>
        public String Openid { get; set; }

        /// <summary>
        /// data1
        /// </summary>
        public String data1 { get; set; }

        /// <summary>
        /// data2
        /// </summary>
        public String data2 { get; set; }

        /// <summary>
        /// data3
        /// </summary>
        public int data3 { get; set; }

        /// <summary>
        /// data4
        /// </summary>
        public String data4 { get; set; }

    }
}
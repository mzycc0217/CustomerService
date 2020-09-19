using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity
{
    /// <summary>
    /// Menu_Servise
    /// </summary>
    [Table("Menu_Servise")]
    public class Menu_Servise
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public String CreatorId { get; set; }

        /// <summary>
        /// Deleted
        /// </summary>
        public Boolean Deleted { get; set; }

        /// <summary>
        /// MenuID
        /// </summary>
        public String MenuID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public String UserID { get; set; }

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
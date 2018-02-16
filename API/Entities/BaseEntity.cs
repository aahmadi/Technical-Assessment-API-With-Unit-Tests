using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Deleted { get; set; } = false;

        public void StampCreation(string creator)
        {
            DateCreated = DateTime.Now;
            CreatedBy = creator;
        }

        public void StampModification(string modifiedBy)
        {
            DateModified = DateTime.Now;
            ModifiedBy = modifiedBy;
        }

        public void Delete(string deletedBy)
        {
            Deleted = true;
            DateModified = DateTime.Now;
            ModifiedBy = deletedBy;
        }

    }
}
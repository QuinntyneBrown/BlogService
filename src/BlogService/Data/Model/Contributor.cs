using System;
using BlogService.Data.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class Contributor: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AvatarUrl { get; set; }

        public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}

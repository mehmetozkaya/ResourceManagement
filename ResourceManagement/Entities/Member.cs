using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagement.Entities
{
    public class Member
    {
        public int MemberID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<MemberComment> MemberComments { get; set; }

    }

    public class Comment
    {
        public int CommentID { get; set; }
        public string Message { get; set; }

        public virtual ICollection<MemberComment> MemberComments { get; set; }
    }

    public class MemberComment
    {
        [Key, Column(Order = 0)]
        public int MemberID { get; set; }
        [Key, Column(Order = 1)]
        public int CommentID { get; set; }

        public virtual Member Member { get; set; }
        public virtual Comment Comment { get; set; }

        public int Something { get; set; }
        public string SomethingElse { get; set; }
    }

}

using System;

namespace UsingIdentity.Core.Entities
{
    public class Post
    {
        public string AuthorId { get; set; }

        public string Content { get; set; }

        public DateTime DatePosted { get; set; }
    }
}

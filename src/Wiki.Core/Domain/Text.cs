using System;
using System.Collections.Generic;

namespace Wiki.Core.Domain
{
    public class Text
    {
        private IList<Suggestion> suggestions = new List<Suggestion>();
        private ISet<TextTag> tags = new HashSet<TextTag>();

        public Text(string title, string content, double version)
        {
            Title = title;
            Content = content;
            Version = version;
            CreatedAt = DateTime.Now;
        }

        public Text(int id)
        {
            Id = id;
            CreatedAt = DateTime.Now;
        }

        public Text(int id, string title, string content, double version) : base()
        {
            Id = id;
            CreatedAt = DateTime.Now;
            Title = title;
            Content = content;
            Version = version;
        }

        protected Text()
        {

        }

        public void SetAuthor(User author)
        {
            Author = author;
        }

        public void SetTags(IEnumerable<TextTag> tags)
        {
            Tags = tags;
        }

        public void SetStatus(TextStatus status)
        {
            Status = status;
        }

        public int Id { get; set; }
        public int ArticleId { get; protected set; }
        public string Title { get; protected set; }
        public User Author { get; protected set; }
        public double Version { get; protected set; }
        public string TextComment { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public User Supervisor { get; protected set; }
        public byte[] Avatar { get; protected set; }

        public IEnumerable<Suggestion> Suggestions
        {
            get => suggestions;
            protected set { }
        }

        public IEnumerable<TextTag> Tags
        {
            get { return tags; }
            set { tags = new HashSet<TextTag>(value); }
        }

        public string Content { get;  set; }

        public TextStatus Status { get;  set; }

        public void SetComment(string textcomment)
        {
            TextComment = textcomment;
        }

        public void SetSupervisor(User supervisor)
        {
            Supervisor = supervisor;
        }

        public void SetAvatar(byte[] avatar)
        {
            Avatar = avatar;
        }

    }

}

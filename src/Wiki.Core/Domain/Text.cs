using System;
using System.Collections.Generic;

namespace Wiki.Core.Domain
{
    public class Text
    {
        private IList<Suggestion> suggestions = new List<Suggestion>();

        public int ArticleId { get; protected set; }
        public int AuthorId { get; protected set; }

        public IEnumerable<Suggestion> Suggestions
        {
            get => suggestions;
            protected set { }
        }

        public string Content { get; protected set; }

        public Status Status { get; protected set; }
    }

    public enum Status { a, b, c }
}
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.DTO
{
    public class TextDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<TextTagDto> Tags { get; set; }
        public UserDto Author { get; set; }
        public UserDto Supervisor { get; set; }
        public TextStatusDto Status { get; set; }
        public double Version { get; set; }
    }
}

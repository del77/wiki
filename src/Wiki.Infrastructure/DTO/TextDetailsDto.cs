﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.DTO
{
    public class TextDetailsDto : TextDto
    {
        public string Content { get; set; }
        public string TextComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] Avatar { get; set; }
    }
}

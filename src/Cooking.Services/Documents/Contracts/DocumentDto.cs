using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Services.Documents.Contracts
{
    public class DocumentDto
    {
        public byte[] Data { get; set; }
        public string Extension { get; set; }
    }
}

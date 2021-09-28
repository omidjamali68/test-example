using System;
using System.Collections.Generic;

namespace Cooking.Entities.Documents
{
    public class Document
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public DocumentStatus Status { get; set; }
    }

    public enum DocumentStatus : short
    {
        Reserve = 1,
        Register = 2
    }
}
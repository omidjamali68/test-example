using System;
using System.Collections.Generic;

namespace Cooking.Entities.Documents
{
    public class Document
    {
        public long Id { get; set; }
        public Guid FileId { get; set; }
        public string FileExtension { get; set; }
        public DocumentStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public enum DocumentStatus : byte
    {
        Registered = 1,
        Reserved = 2,
        Deleted = 3
    }
}
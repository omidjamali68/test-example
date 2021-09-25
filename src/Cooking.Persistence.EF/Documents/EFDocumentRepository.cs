using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cooking.Entities.Documents;
using Cooking.Services.Documents.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cooking.Persistence.EF.Documents
{
    public class EFDocumentRepository : DocumentRepository
    {
        private readonly EFDataContext _context;
        private readonly DbSet<Document> _documents;

        public EFDocumentRepository(EFDataContext context)
        {
            _context = context;
            _documents = context.Documents;
        }

        public void AddDocument(Document document, byte[] fileStream, string fileExtension)
        {
            var fileId = Guid.NewGuid();
            var resolvedFileExtension = fileExtension.TrimStart('.');
            var filename = fileId.ToString("N") + "." + resolvedFileExtension;

            document.FileId = fileId;
            document.FileExtension = resolvedFileExtension;
            _documents.Add(document);
        }

        public async Task RegisterDocument(long documentId)
        {
            var document = await _documents.FirstOrDefaultAsync(_ => _.Id == documentId);
            document.Status = DocumentStatus.Registered;
        }

        public async Task RegisterDocuments(List<long> documentIds)
        {
            var documents = await _documents.Where(_ => documentIds.Any(d => d == _.Id)).ToListAsync();
            documents.ForEach(_ => _.Status = DocumentStatus.Registered);
        }

        public async Task RemoveDocument(long documentId)
        {
            var document = await _documents.FindAsync(documentId);
            document.Status = DocumentStatus.Deleted;
        }

        public async Task RemoveDocuments(List<long> documentsId)
        {
            var documents = await _documents.Where(_ => documentsId.Any(d => d == _.Id)).ToListAsync();
            documents.ForEach(_ => _.Status = DocumentStatus.Deleted);
        }

        public async Task<bool> IsDocumentExist(long documentId)
        {
            return await _documents.AnyAsync(_ => _.Id == documentId);
        }

        public async Task<IList<Document>> FindByIds(IEnumerable<long> ids)
        {
            return await _documents.Where(_ => ids.Contains(_.Id)).ToListAsync();
        }

        public async Task<DocumentFileDto> GetDocumentFileById(long id)
        {
            var documentFile = from document in _documents.Where(_ => _.Id == id)
                select new DocumentFileDto
                {
                    FileExtension = document.FileExtension
                };

            return await documentFile.SingleOrDefaultAsync();
        }

        public async Task<string> GetExtension(long id)
        {
            var document = await _documents.Where(_ => _.Id == id)
                .Select(_ => new {_.FileExtension}).SingleAsync();

            return document.FileExtension;
        }
    }
}
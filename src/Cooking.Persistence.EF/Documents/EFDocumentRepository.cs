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
            _documents = context.Set<Document>();
        }

        public void AddDocument(Document document)
        {
            _documents.Add(document);
        }

        public async Task RegisterDocument(Guid documentId)
        {
            var document = await _documents.FirstOrDefaultAsync(_ => _.Id == documentId);
            document.Status = DocumentStatus.Register;
        }

        public async Task RegisterDocuments(List<Guid> documentIds)
        {
            var documents = await _documents.Where(_ => documentIds.Any(d => d == _.Id)).ToListAsync();
            documents.ForEach(_ => _.Status = DocumentStatus.Register);
        }

        public async Task<DocumentDto?> GetDocumentById(Guid id)
        {
            return await _documents.Where(_ => _.Id == id)
                                   .Select(document => new DocumentDto
                                   {
                                       Extension = document.Extension,
                                       Data = document.Data
                                   })
                                   .SingleOrDefaultAsync();
        }

        public async Task<Document> FindById(Guid id)
        {
            return await _context.Set<Document>().FindAsync(id);
        }

        public async Task DeleteByIds(IList<Guid> documentIds)
        {
            var documents = _documents.Where(_ => documentIds.Contains(_.Id))?.ToList();

            if (documents != null)
                _documents.RemoveRange(documents);

            //var raw = string.Empty;
            //var ids = string.Empty;

            //foreach (var id in documentIds)
            //{
            //    ids += $"N'{id}',";
            //}
            //ids = ids.TrimEnd(',');

            //raw += $"DELETE FROM Documents WHERE Id IN({ids})";

            //if (!string.IsNullOrEmpty(raw))
            //{
            //    await _context.Database.ExecuteSqlRawAsync(raw);
            //}
        }
    }
}
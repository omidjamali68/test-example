using System;
using System.Collections.Generic;
using System.Text;

namespace Cooking.Entities.Recipes
{
    public class StepOperationDocument
    {
        public long Id { get; set; }
        public long StepOperationId { get; set; }
        public StepOperation StepOperation { get; set; }
        public Guid DocumentId { get; set; }
        public string Extension { get; set; }
    }
}

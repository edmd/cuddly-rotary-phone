using Products.Api.Models;

namespace Products.Api.Services
{
    public sealed class QueryResponse
    {
        private Resource[]? resources;

        public QueryResponse()
        {
        }

        public QueryResponse(IReadOnlyCollection<Resource> resources)
        {
        }

        public QueryResponse(IList<Resource> resources)
        {
        }

        public int ItemsPerPage
        {
            get;
            set;
        }

        public IEnumerable<Resource> Resources
        {
            get
            {
                return this.resources;
            }

            set
            {
                this.resources = value.ToArray();
            }
        }

        public int? StartIndex
        {
            get;
            set;
        }

        public int TotalResults
        {
            get;
            set;
        }
    }
}

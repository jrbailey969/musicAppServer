using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Infrastructure
{
    public class DocumentClientFactory
    {
        private const string EndpointUrl = "https://musicapp-20170705.documents.azure.com:443/";
        private const string PrimaryKey = "EJ2NnTnRuzDFM2Lcs567Al3y6RfrG5nyw3Ak0rqBJk9clUM1ivjrApDuIIRbLnPfPCXfXTRxl2s6ozaRTQEdxg==";

        private DocumentClient _client;
        public async Task<DocumentClient> GetClient()
        {
            if (_client == null)
            {
                lock (this)
                {
                    if (_client == null)
                    {
                        _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

                    }
                }
            }

            return _client;
        }
    }
}

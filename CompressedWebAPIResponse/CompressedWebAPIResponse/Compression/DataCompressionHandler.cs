﻿using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CompressedWebAPIResponse.Compression
{
    public class DataCompressionHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith((responseToCompleteTask) =>
            {
                HttpResponseMessage response = responseToCompleteTask.Result;

                if (response.RequestMessage.Headers.AcceptEncoding != null &&
                    response.RequestMessage.Headers.AcceptEncoding.Count > 0)
                {
                    string encodingType = response.RequestMessage.Headers.AcceptEncoding.First().Value;

                    response.Content = new CompressedResponse(response.Content, encodingType);
                }

                return response;
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
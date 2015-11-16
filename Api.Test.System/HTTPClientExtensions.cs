namespace Api.Test.System
{
    using global::System;
    using global::System.Net.Http;
    using global::System.Net.Http.Formatting;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = new ObjectContent<dynamic>(value, new JsonMediaTypeFormatter(), "application/json") };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = new ObjectContent<dynamic>(value, new JsonMediaTypeFormatter(), "application/json") }; // specifying the media type allows the API to deserialize an expected object easily
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("DELETE");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> DeleteAsync<T>(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("DELETE");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, Uri requestUri, HttpContent content)
        {
            var method = new HttpMethod("DELETE");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var method = new HttpMethod("DELETE");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request, cancellationToken);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var method = new HttpMethod("DELETE");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request, cancellationToken);
        }

        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), requestUri) { Content = new ObjectContent<dynamic>(value, new JsonMediaTypeFormatter(), "application/json") };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value)
        {
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), requestUri) { Content = new ObjectContent<dynamic>(value, new JsonMediaTypeFormatter(), "application/json") }; // specifying the media type allows the API to deserialize an expected object easily
            return await client.SendAsync(request);
        }
    }
}

using DDHCenter.Core.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DDHCenter.Core.Utils
{
    public class ApiClient<Model>
    {
        #region Campos
            private static ApiClient<Model> _instance = new ApiClient<Model>();
            private readonly HttpClient client = new HttpClient();
            private readonly string url = Properties.Settings.Default.ApiUrl;
            private string resourceUrl = "";
        #endregion

        #region Propiedades

            public static ApiClient<Model> Instance { get { return _instance; } }

            public string ResourceUrl
            {
                get
                {
                    return resourceUrl;
                }
                set
                {
                    if (resourceUrl != value)
                        resourceUrl = value;
                }
            }

        #endregion

        #region Metodos

            public async Task<List<Model>> GetModelsAsync()
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url + resourceUrl))
                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None))
                {
                    var stream = await response.Content.ReadAsStreamAsync();

                    if (response.IsSuccessStatusCode)
                        return DeserializeJsonFromStream<List<Model>>(stream);

                    var content = await StreamToStringAsync(stream);
                    throw new ApiException { StatusCode = (int)response.StatusCode, Content = content };
                }
            }

            public async Task<Model> GetModelAsync(string overridedUrl = "")
            {
                if (overridedUrl != "")
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("DDHCenter");
                }
                else {
                    overridedUrl = url + resourceUrl;
                }
                using (var request = new HttpRequestMessage(HttpMethod.Get, overridedUrl))
                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None))
                {
                    var stream = await response.Content.ReadAsStreamAsync();

                    if (response.IsSuccessStatusCode)
                        return DeserializeJsonFromStream<Model>(stream);

                    var content = await StreamToStringAsync(stream);
                    throw new ApiException { StatusCode = (int)response.StatusCode, Content = content };
                }
            }

            public async Task DownloadFile(string fileUrl, string localFile) 
            {
               client.DefaultRequestHeaders.UserAgent.ParseAdd("DDHCenter");
               var response = await client.GetAsync(fileUrl);
               using (var fs = new System.IO.FileStream(localFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
               {
                   await response.Content.CopyToAsync(fs).ContinueWith( copyTask => fs.Close() );
               }
            }

            public async Task<Model> PostModelAsync(object content)
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url + resourceUrl))
                using (var httpContent = CreateHttpContent(content))
                {
                    if (Properties.Settings.Default.UserToken != "")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.UserToken);
                    //
                    request.Content = httpContent;
                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false))
                    {
                        var stream = await response.Content.ReadAsStreamAsync();

                        if (response.IsSuccessStatusCode)
                        {

                            return DeserializeJsonFromStream<Model>(stream);
                        }

                        var _content = await StreamToStringAsync(stream);
                        throw new ApiException
                        {
                            StatusCode = (int)response.StatusCode,
                            Content = _content
                        };
                    }
                }
            }

            public async Task<Model> PostImageAsync(FileStream file)
            {
                //
                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StreamContent(file);
                HttpContent content2 = new StringContent("1");
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "fileImg",
                    FileName = Regex.Replace(file.Name, @"[^0-9a-zA-Z:,.\/]+", "")
                };
                form.Add(content, "fileImg");
                form.Add(content2, "send");
                using (var response = await client.PostAsync(resourceUrl, form, CancellationToken.None).ConfigureAwait(false))
                {
                    var stream = await response.Content.ReadAsStreamAsync();

                    if (response.IsSuccessStatusCode)
                    {

                        return DeserializeJsonFromStream<Model>(stream);
                    }

                    var _content = await StreamToStringAsync(stream);
                    throw new ApiException
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = _content
                    };
                }
            }

            private HttpContent CreateHttpContent(object content)
            {
                HttpContent httpContent = null;

                if (content != null)
                {
                    var ms = new MemoryStream();
                    SerializeJsonIntoStream(content, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    httpContent = new StreamContent(ms);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                return httpContent;
            }

            private T DeserializeJsonFromStream<T>(Stream stream)
            {
                if (stream == null || stream.CanRead == false)
                    return default(T);

                using (var sr = new StreamReader(stream))
                using (var jtr = new JsonTextReader(sr))
                {
                    var jr = new JsonSerializer();
                    var searchResult = jr.Deserialize<T>(jtr);
                    return searchResult;
                }
            }

            private async Task<string> StreamToStringAsync(Stream stream)
            {
                string content = null;

                if (stream != null)
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = await sr.ReadToEndAsync();
                    }
                }

                return content;
            }

            private void SerializeJsonIntoStream(object value, Stream stream)
            {
                using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
                using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
                {
                    var js = new JsonSerializer();
                    js.Serialize(jtw, value);
                    jtw.Flush();
                }
            }

        #endregion
    }
}

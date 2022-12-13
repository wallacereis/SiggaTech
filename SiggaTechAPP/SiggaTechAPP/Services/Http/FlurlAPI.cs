using SiggaTechAPP.Helpers;
using SiggaTechAPP.Interfaces;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SiggaTechAPP.Services.Http
{
    public class FlurlAPI<T> : IFlurlAPI<T> where T : class
    {
        public async Task<List<T>> GetAllItemsAsync(string entity)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment(entity)
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsAsync(string entity, string parameter, string value)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}")
                .SetQueryParam(parameter, value)
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<List<T>> GetAllItemsByIdAsync(string entity, int id)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<T> GetItemByIdAsync(string entity, int id)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<T>();
            return result;
        }

        public async Task<T> GetItemByEmailAsync(string entity, string email)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{email}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<T>();
            return result;
        }

        public async Task<List<T>> GetItemByLoginAsync(string entity, string emailParameter, string passwordParameter)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}")
                .SetQueryParams
                (
                    new
                    {
                        email = emailParameter,
                        username = passwordParameter
                    }
                )
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .GetJsonAsync<List<T>>();
            return result;
        }

        public async Task<byte[]> GetFileAsync(string file, string token)
        {
            var result =
                await Constants.BaseAddressPDF
                .AppendPathSegment($"{file}")
                .WithHeader("Accept", "application/json")
                //.WithOAuthBearerToken(token)
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(5))
                .GetBytesAsync();
            return result;
        }

        public async Task<T> AddItemAsync(string entity, T obj)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .PostJsonAsync(obj)
                .ReceiveJson<T>();
            return result;
        }

        public async Task<T> UpdateItemAsync(string entity, int id, T obj)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .PutJsonAsync(obj);
            //.ReceiveJson<T>();
            return null;
        }

        public async Task<T> DeleteItemAsync(string entity, int id)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .DeleteAsync();
            //.ReceiveJson<T>();
            return null;
        }

        public async Task<T> UploadItemAsync(string entity, Stream file, int id)
        {
            var result =
                await Constants.BaseAddressAPI
                .AppendPathSegment($"{entity}/upload/{id}")
                .WithHeader("Accept", "application/json")
                .AllowHttpStatus("400-500")
                .WithTimeout(TimeSpan.FromMinutes(2))
                .PostJsonAsync(file)
                .ReceiveJson<T>();
            return result;
        }
    }
}

using Core.Dtos;
using Core.Interfaces.IClients;
using Core.Models;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;


namespace MvcApp.Clinets
{
    public class UserClient : IUserClient
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public UserClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<UserModel>> GetAll()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{_configuration.GetValue<string>("UserApiUrl")}get-all")
            {
                Headers =
                {
                    { HeaderNames.Accept,"application/json"}
                }
            };

            var httpCleint = _httpClientFactory.CreateClient();
            var response = await httpCleint.SendAsync(httpRequestMessage);
            if (!response.IsSuccessStatusCode)
            {
                return new List<UserModel>();
            }
            var str = await response.Content.ReadAsStringAsync();
            var userModels = JsonConvert.DeserializeObject<List<UserModel>>(str);
            return userModels;
        }

        public async Task<int> CreateUserAsync(UserModel userModel)
        {

            var httpCleint = _httpClientFactory.CreateClient();
            httpCleint.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            var response = await httpCleint.PostAsync($"{_configuration.GetValue<string>("UserApiUrl")}user-create",
                new StringContent(JsonConvert.SerializeObject(userModel), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json));
            if (!response.IsSuccessStatusCode)
            {
                var strError = await response.Content.ReadAsStringAsync();
                return 1;
            }
            var str = await response.Content.ReadAsStringAsync();
            int id = JsonConvert.DeserializeObject<int>(str);
            return id;
        }

        public async Task<string?> DeleteAsync(int id)
        {
            var httpCleint = _httpClientFactory.CreateClient();
            var response = await httpCleint.DeleteAsync($"{_configuration.GetValue<string>("UserApiUrl")}" + id);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var str = await response.Content.ReadAsStringAsync();
            string userId = JsonConvert.DeserializeObject<string>(str);
            return userId;
        }

        public async Task<UserModel?> FindOneAsync(int id)
        {
            var httpCleint = _httpClientFactory.CreateClient();
            var response = await httpCleint.GetAsync(new Uri($"{_configuration.GetValue<string>("UserApiUrl") + id}"));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var str = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(str);
            return userModel;
        }

        public async Task<SearchUserResponseDto?> Search(string searchField)
        {
            var httpCleint = _httpClientFactory.CreateClient();
            httpCleint.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            var response = await httpCleint.GetAsync(new Uri($"{_configuration.GetValue<string>("UserApiUrl")}search/{searchField}"));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var str = await response.Content.ReadAsStringAsync();
            var searchUserResponseDto = JsonConvert.DeserializeObject<SearchUserResponseDto>(str);
            return searchUserResponseDto;
        }

        public async Task<UserModel?> UpdateAsync(UserModel userModel)
        {
            var httpCleint = _httpClientFactory.CreateClient();
            httpCleint.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            var response = await httpCleint.PutAsync(new Uri($"{_configuration.GetValue<string>("UserApiUrl")}update"),
                new StringContent(JsonConvert.SerializeObject(userModel), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var str = await response.Content.ReadAsStringAsync();
            var userModelResult = JsonConvert.DeserializeObject<UserModel>(str);
            return userModelResult;
        }
    }
}

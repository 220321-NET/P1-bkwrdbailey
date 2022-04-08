using System.Net.Http;
using Models;

namespace UI;
public class HttpService
{
    private HttpClient client = new HttpClient();
    private readonly string _apiBaseURL = "https://localhost:7223/api/";

    public HttpService() {
        client.BaseAddress = new URI(_apiBaseURL);
    }
}
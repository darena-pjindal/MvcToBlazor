namespace MVCToBlazor;

public class MyTasksClient
{
    private readonly HttpClient _httpClient;

    public MyTasksClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<List<MyTask>> GetTasksAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<MyTask>>("api/tasks");
    }

    public async Task CreateTaskAsync(MyTask task)
    {
        await _httpClient.PostAsJsonAsync("api/tasks", task);
    }
}
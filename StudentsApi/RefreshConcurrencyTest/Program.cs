using System.Net.Http.Json;

var url = "API_URL";

var refreshToken = "REFRESH_TOKEN";

using var client = new HttpClient();

var request1 = client.PostAsJsonAsync(
    url,
    new { refreshToken });

var request2 = client.PostAsJsonAsync(
    url,
    new { refreshToken });

var responses = await Task.WhenAll(
    request1,
    request2);

for (var i = 0; i < responses.Length; i++)
{
    var response = responses[i];

    Console.WriteLine(
        $"Request {i + 1}: " +
        $"{(int)response.StatusCode} " +
        $"{response.StatusCode}");

    var body = await response.Content.ReadAsStringAsync();

    Console.WriteLine(body);
    Console.WriteLine();
}
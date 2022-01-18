using System.Text.Json.Nodes;
using IdentityModel.Client;

HttpClientHandler clientHandler = new HttpClientHandler();
clientHandler.ServerCertificateCustomValidationCallback =
    (sender, cert, chain, sslPolicyErrors) => { return true; };

// Pass the handler to httpclient(from you are calling api)
HttpClient client = new HttpClient(clientHandler);
var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
if (disco.IsError) {
    Console.WriteLine(disco.Error);
    return;
}

// request token
var tokenResponse = await client.RequestClientCredentialsTokenAsync(
    new ClientCredentialsTokenRequest {
        Address = disco.TokenEndpoint, ClientId = "client", ClientSecret = "secret", Scope = "api1"
    });

if (tokenResponse.IsError) {
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine(tokenResponse.Json);


// call api
var apiClient = new HttpClient(clientHandler);
apiClient.SetBearerToken(tokenResponse.AccessToken);

var response = await apiClient.GetAsync("https://localhost:7203/identity");
if (!response.IsSuccessStatusCode) {
    Console.WriteLine(response.StatusCode);
}
else {
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(JsonArray.Parse(content));
}
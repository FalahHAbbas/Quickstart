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


/*
 *
 http://localhost:5000/connect/authorize?
 client_id=js
 redirect_uri=http%3A%2F%2Fgoogle.com
 response_type=id_token%20token
 scope=openid%20profile%20cinema
 state=a96df82c3d8546c38d841a0a0109c967
 nonce=93056686e50442f8a97cc64f75305b1e
 */

// request token
var tokenResponse = await client.RequestClientCredentialsTokenAsync(
    new ClientCredentialsTokenRequest {
        Address = disco.TokenEndpoint, ClientId = "client", ClientSecret = "secret", Scope = "cinema"
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
// See https://aka.ms/new-console-template for more information
using IdentityModel.Client;
using Newtonsoft.Json;
using Products.Api.Models;
using System.Net.Http.Headers;
using System.Text;

TokenResponse? _jwtToken = null;
int? _skuId = int.MaxValue;

Console.WriteLine("Products Simulator\n");

try
{
    await FetchJwtToken();

    await SubmitProduct();

    await RetrieveProductSuccess();

    await RetrieveProductNotFound();

    await RetrieveProductUnauthorised();

    await SubmitProductBadRequest();

    Console.ReadLine();
    return;
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


async Task FetchJwtToken()
{
    Console.WriteLine("Fetch Jwt Token from Identity Server\n");

    // discover endpoints from metadata
    var client = new HttpClient();
    var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7205");
    if (disco.IsError)
    {
        Console.WriteLine(disco.Error);
    }

    // request token
    var jwtResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = disco.TokenEndpoint,
        ClientId = "merchant",
        ClientSecret = "secret",
        Scope = "Products.Api"
    });

    if (jwtResponse.IsError)
    {
        Console.WriteLine(jwtResponse.Error);
    }

    Console.WriteLine($"JWT Token created: \n{jwtResponse.Json}\n");

    _jwtToken = jwtResponse;

    return;
}

async Task SubmitProduct()
{
    Console.WriteLine("Submit Product for processing to the Gateway\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var product = new CreateProductRequest(123, "Widgets", "Widgets Description", 99.99M, Guid.NewGuid());
    var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("https://localhost:7005/api/products", stringContent);

    Console.WriteLine(response.StatusCode);

    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        CreateProductResponse? productResponse = JsonConvert.DeserializeObject<CreateProductResponse>(content);
        _skuId = productResponse?.Sku; // assign created product id
        Console.WriteLine(content);
        Console.WriteLine($"Product created with Sku: {productResponse?.Sku}\n");
    }

    return;
}

async Task RetrieveProductSuccess()
{
    Console.WriteLine("Retrieve Product information from Gateway (Success)\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);

    var response = await client.GetAsync($"https://localhost:7005/api/products/{_skuId}");

    Console.WriteLine(response.StatusCode);

    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        var product = JsonConvert.DeserializeObject<GetProductResponse>(content);
        Console.WriteLine(content);
        Console.WriteLine($"Product retrieved with Id: {product?.Sku}\n");
    }

    return;
}

async Task RetrieveProductNotFound()
{
    Console.WriteLine("Retrieve Product information from Gateway (Not Found)\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);

    var response = await client.GetAsync($"https://localhost:7005/api/products/{Guid.NewGuid()}");

    Console.WriteLine(response.StatusCode);

    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        var product = JsonConvert.DeserializeObject<GetProductResponse>(content);
        Console.WriteLine(content);
        Console.WriteLine($"Product retrieved with Id: {product?.Sku}\n");
    } else {
        var content = await response.Content.ReadAsStringAsync();
        var product = JsonConvert.DeserializeObject<ErrorDetails>(content);
        Console.WriteLine($"{content}\n");
    }

    return;
}

async Task RetrieveProductUnauthorised()
{
    Console.WriteLine("Retrieve Product information from Gateway (Unauthorised)\n");

    // call api
    var client = new HttpClient();

    var response = await client.GetAsync($"https://localhost:7005/api/products/{_skuId}");

    Console.WriteLine($"{response.StatusCode}\n");

    return;
}

async Task SubmitProductBadRequest()
{
    Console.WriteLine("Submit Product for processing to the Gateway (BadRequest)\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var product = new CreateProductRequest(123, "Widgets", "Widgets Description", 99.99M, Guid.NewGuid());
    var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("https://localhost:7005/api/products", stringContent);

    Console.WriteLine($"{response.StatusCode}\n");

    return;
}
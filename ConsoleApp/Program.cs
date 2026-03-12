
using Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

HttpClient httpClient = new HttpClient();

httpClient.BaseAddress = new Uri("http://localhost:5114/api/");

httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


var result = await httpClient.GetAsync("ShoppingLists");

if (result.StatusCode != System.Net.HttpStatusCode.OK)
{
    Console.WriteLine("Error 1");
}

if (!result.IsSuccessStatusCode)
{
    Console.WriteLine("Error 2");
}


try
{
    result.EnsureSuccessStatusCode();
}
catch (HttpRequestException ex)
{
    Console.WriteLine(ex.Message);
}

//korzystamy z biblioteki standardowej System.Text.Json 
var items = await result.Content.ReadFromJsonAsync<IEnumerable<ShoppingList>>();


var stringContent = await result.Content.ReadAsStringAsync();
Console.WriteLine(stringContent);
//korzystamy z biblioteki Newtonsoft.Json
items = JsonConvert.DeserializeObject<IEnumerable<ShoppingList>>(stringContent);




var item = items.First();

item.Name = "Nowa nazwa";

//System.Text.Json
await httpClient.PutAsJsonAsync($"ShoppingLists/{item.Id}", item);


//Newtonsoft.Json
item.Name = "Jeszcze inna nazwa";
using (StringContent content = new(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json"))
{
    HttpResponseMessage response = await httpClient.PutAsync($"ShoppingLists/{item.Id}", content);
}


    Console.ReadLine();
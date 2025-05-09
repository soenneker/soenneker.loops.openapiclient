[![](https://img.shields.io/nuget/v/soenneker.loops.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.loops.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.loops.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.loops.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.loops.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.loops.openapiclient/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Loops.OpenApiClient
### A Loops (loops.so) .NET client generated from their OpenAPI schema, updated daily

## Installation

```bash
dotnet add package Soenneker.Loops.OpenApiClient
```

## Authentication

To use the Loops API, you'll need an API key. You can generate one in the Loops dashboard under Settings -> API. The API key should never be exposed client-side or to end users.

```csharp
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Loops.OpenApiClient;

// Create an HTTP client with the API key
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer YOUR_API_KEY");

// Create the request adapter
var requestAdapter = new HttpClientRequestAdapter(httpClient);

// Instantiate the main client
var client = new LoopsOpenApiClient(requestAdapter);
```

## Rate Limiting

The Loops API has a baseline rate limit of 10 requests per second per team. The API provides rate limit information in response headers:

- `x-ratelimit-limit`: Maximum requests per second
- `x-ratelimit-remaining`: Remaining requests in the current window

If you exceed the rate limit, you'll receive a 429 Too Many Requests response. Implement retry logic with exponential backoff to handle these cases.

## Usage Examples

### Managing Contacts

```csharp
// Create a new contact
var contactRequest = new ContactRequest
{
    Email = "user@example.com",
    FirstName = "John",
    LastName = "Doe",
    UserProperties = new Dictionary<string, string>
    {
        { "company", "Acme Inc" }
    }
};

var contact = await client.Contacts.PostAsync(contactRequest);

// Find a contact
var contact = await client.Contacts.GetAsync(requestConfiguration => 
{
    requestConfiguration.QueryParameters.Email = "user@example.com";
});

// Delete a contact
await client.Contacts.DeleteAsync(new ContactDeleteRequest 
{ 
    Email = "user@example.com" 
});
```

### Sending Events

```csharp
// Send an event to trigger emails in loops
var eventRequest = new EventRequest
{
    Email = "user@example.com",
    EventName = "purchase_completed",
    EventProperties = new Dictionary<string, string>
    {
        { "product_id", "123" },
        { "amount", "99.99" }
    }
};

var response = await client.Events.PostAsync(eventRequest);
```

### Transactional Emails

```csharp
// Send a transactional email
var transactionalRequest = new TransactionalRequest
{
    TransactionalId = "your-transactional-id",
    Email = "user@example.com",
    DataVariables = new Dictionary<string, string>
    {
        { "name", "John Doe" },
        { "order_id", "12345" }
    }
};

var response = await client.Transactional.PostAsync(transactionalRequest);

// List published transactional emails
var transactionalEmails = await client.Transactional.GetAsync();
```

### Managing Mailing Lists

```csharp
// List all mailing lists
var mailingLists = await client.Lists.GetAsync();
```

## Error Handling

The client throws appropriate exceptions for different error scenarios:

- `401 Unauthorized`: Invalid or missing API key
- `429 Too Many Requests`: Rate limit exceeded
- `400 Bad Request`: Invalid request parameters
- `404 Not Found`: Resource not found

Example error handling:

```csharp
try
{
    var response = await client.Contacts.PostAsync(contactRequest);
}
catch (ApiException ex) when (ex.StatusCode == 429)
{
    // Handle rate limiting
    await Task.Delay(1000); // Implement exponential backoff
    // Retry the request
}
catch (ApiException ex) when (ex.StatusCode == 401)
{
    // Handle invalid API key
    Console.WriteLine("Please check your API key");
}
```

## Additional Resources

- [Loops API Documentation](https://loops.so/docs/api-reference/intro)

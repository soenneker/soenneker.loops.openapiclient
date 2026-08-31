[![](https://img.shields.io/nuget/v/soenneker.loops.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.loops.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.loops.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.loops.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.loops.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.loops.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.loops.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.loops.openapiclient/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Loops.OpenApiClient

Call Loops endpoints through a Kiota-generated client with typed request builders and models.

## Install

```bash
dotnet add package Soenneker.Loops.OpenApiClient
```

## Create a client

```csharp
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Loops.OpenApiClient;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://app.loops.so/api/")
};
httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

var adapter = new HttpClientRequestAdapter(
    new AnonymousAuthenticationProvider(),
    httpClient: httpClient);

var client = new LoopsOpenApiClient(adapter);
```

Reuse the transport rather than constructing one per request, and dispose it with its owning application component. For application registration, lazy reuse, and coordinated ownership, use `Soenneker.Loops.ClientUtil`.

## Create a contact

```csharp
using Soenneker.Loops.OpenApiClient.Models;

var request = new ContactRequest
{
    Email = "person@example.com",
    FirstName = "Morgan",
    Subscribed = true
};

ContactSuccessResponse? created = await client.V1.Contacts.Create.PostAsync(
    request,
    cancellationToken: cancellationToken);
```

## Find a contact

```csharp
List<Contact>? matches = await client.V1.Contacts.Find.GetAsync(
    config => config.QueryParameters.Email = "person@example.com",
    cancellationToken);
```

The generated surface begins at `client.V1`. Follow its request builders for contacts, events, transactional email, lists, campaigns, workflows, and other resources. HTTP failures are surfaced through Kiota exceptions; nullable results indicate no response body.

This repository contains generated code. Put reusable helpers and behavior changes in a separate package so regeneration does not overwrite them.

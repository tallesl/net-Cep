# Cep

[![][build-img]][build]
[![][nuget-img]][nuget]

Sanitizes CEPs and scraps addresses from Correios website.

[build]:     https://ci.appveyor.com/project/TallesL/net-cep
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/net-cep?svg=true
[nuget]:     https://www.nuget.org/packages/Cep
[nuget-img]: https://badge.fury.io/nu/Cep.svg

## Sanitizing

Checks the given CEP for the following formats:

* `##.###-###`;
* `#####-###`;
* `########`.

Strips its mask and returns it.
It can also be used for validation, if the CEP is invalid it returns null.

```cs
using CepLibrary;

Cep.Sanitize("30130-010"); // returns "30130010"
Cep.Sanitize("whatever");  // returns null
```

## Scraping

Requests [buscacep.correios.com.br] and scraps a fresh address info.

```cs
using CepLibrary;

Cep.Scrap("30130010");

// returns the following:
//
// new Endereco
// {
//      Cep = "30130010",
//      Logradouro = "Praça Sete de Setembro",
//      Bairro = "Centro",
//      Localidade = "Belo Horizonte",
//      Uf = "MG"
// }
```

[buscacep.correios.com.br]: http://buscacep.correios.com.br
# Cep Utility

[![][build-img]][build]
[![][nuget-img]][nuget]

Sanitizes CEPs and scraps addresses from Correios website.

[build]:     https://ci.appveyor.com/project/TallesL/CepUtility
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/CepUtility

[nuget]:     http://badge.fury.io/nu/CepUtility
[nuget-img]: https://badge.fury.io/nu/CepUtility.png

## Sanitizing

Checks for CEP in the following formats:

* `##.###-###`;
* `#####-###`;
* `########`.

Strips its mask and returns it.
It can also be used for validation, if the CEP is invalid it returns null.

```cs
using CepUtility;

CepSanitizer.Sanitize("30130-010"); // returns "30130010"
CepSanitizer.Sanitize("lol"); // returns null
```

## Scraping

Requests [buscacep.correios.com.br] and scraps a fresh address info.

[buscacep.correios.com.br]: http://buscacep.correios.com.br

```cs
using CepUtility;

CepScraper.Scrap("30130010");

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

# ÍNDICE

 - [INTRODUÇÃO](#introdução)
 - [PRINCÍPIOS S.O.L.I.D.](#princípios-solid)
 	- [Single Responsiblity Principle](#single-responsiblity-principle)
 - [GLOSSÁRIO](#glossário)
 - [REFERÊNCIAS](#referências)

# INTRODUÇÃO

Costumo dizer que conhecer os diversos conceitos de arquitetura de software e design de código é mais importante do que tecnologias em si, e penso assim pelas seguintes razões:

1. Os conceitos costumam ser atemporáis;
1. A grande parte dos conceitos podem ser aplicados em qualquer tecnologia;
1. Aplicam-se tanto no back-end quanto no front-end;
1. Proporcionam a construção de aplicações com uma melhor organização, manutenibilidade, reastreabilidade, portabilidade e etc...

SOLID foi proposto por Robert C. (ou Uncle Bob) por volta do ano 2000 e trata-se de um acrônimo dos cinco princípios da programação orientada a objetos.

O repositório "solid-in-practice" tem como proposito explorar e mostrar algumas aplicações práticas dos conceitos em questão com mais de uma linguagem de programação.

# PRINCÍPIOS S.O.L.I.D.

Segue cada um dos cinco princípios e o seu propósito.

| Sigla | Nome |
| :------------ | :------------ |
| **S** | Single Responsiblity Principle |
| **O** | Open-Closed Principle |
| **L** | Liskov Substitution Principle |
| **I** | Interface Segregation Principle |
| **D** | Dependency Inversion Principle |

###  Single Responsiblity Principle

Uma classe deve ser especializada em um únido assunto e ter apenas uma responsabilidade.
Construir uma *God Class** é mais fácil e rápido, porem com o passar do tempo a manuteção desse código torna-se impraticável.

# GLOSSÁRIO

- ***God Class**: Na programação orientada a objetos, é uma classe que sabe demais ou faz demais.

# REFERÊNCIAS

- http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod
- https://www.c-sharpcorner.com/UploadFile/damubetha/solid-principles-in-C-Sharp/

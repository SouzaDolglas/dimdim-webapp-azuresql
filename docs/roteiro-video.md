# Roteiro do vídeo — 2º Checkpoint (Web App + Azure SQL)

**O que dá desconto se faltar:**
- vídeo abaixo de 720p ou **sem explicação falada** → −3
- **não mostrar o banco depois de cada operação** → −3
- não mostrar o Application Insights → −1,5

Duração alvo: **10 a 15 min**. O vídeo mostra o *how to* do README rodando de verdade.

Antes de gravar:
- fonte do terminal grande (Ctrl + `+` umas 3 vezes); fechar abas pessoais
- `export SQL_PASSWORD='...'` **antes** de começar a gravar (a senha não pode aparecer)
- deixar aberto: terminal na pasta do repositório, portal Azure, o site

---

## Bloco 1 — Abertura (1 min)

> "Oi, professor. Sou [nome], RM [RM], do grupo PataCloud. Este é o 2º checkpoint:
> uma aplicação web .NET do projeto DimDim, publicada no Azure App Service, gravando
> num Azure SQL Database e monitorada pelo Application Insights. Vou seguir o how to
> do README do nosso GitHub."

Mostrar o repositório no GitHub: README, pastas `scripts/`, `json/`, `src/`, desenho da arquitetura.

## Bloco 2 — Criação dos recursos pelo CLI (4 min)

```bash
az login
source scripts/00-variaveis.sh
./scripts/01-criar-grupo.sh
./scripts/02-criar-banco.sh          # SQL Server + banco + firewall + DDL
./scripts/03-criar-monitoramento.sh  # Log Analytics + Application Insights
./scripts/04-criar-webapp.sh         # plano + Web App + connection strings
```

Falar enquanto roda: "o banco é PaaS, não é container"; "o DDL cria as tabelas cliente e
conta, com chave estrangeira de conta para cliente"; "a senha e a string de conexão não
estão no código — vão como configuração do Web App".

Mostrar no portal o grupo `rg-564099-dimdim-webapp` com os recursos: SQL Server, banco,
plano, Web App, Application Insights, Log Analytics. Abrir Web App → **Variáveis de
ambiente** e mostrar que as conexões estão lá (sem clicar em "mostrar valor").

## Bloco 3 — Deploy (2 min)

```bash
./scripts/05-deploy.sh
```

> "O deploy é automatizado: o script compila, gera o zip e publica com az webapp deploy."

Abrir https://app-564099-dimdim.azurewebsites.net.

## Bloco 4 — CRUD com SELECT depois de cada operação (5 min) ⭐

Deixar lado a lado: o site e o **Editor de consultas** do banco no portal
(banco `dimdim` → Editor de consultas → login `dimdimadmin`).

| # | No site | No banco, logo depois |
|---|---|---|
| 1 | Clientes → Novo cliente → salvar | `SELECT * FROM cliente;` — aparece a linha |
| 2 | Editar o cliente (mudar e-mail) | `SELECT * FROM cliente;` — e-mail mudou |
| 3 | Contas → Nova conta para esse cliente | `SELECT * FROM conta;` — `cliente_id` aponta para ele |
| 4 | Editar a conta (tipo/saldo) | `SELECT * FROM conta;` — valores mudaram |
| 5 | Mostrar o JOIN | consulta 3 de `scripts/consultas.sql` |
| 6 | Excluir a conta | `SELECT * FROM conta;` — sumiu |
| 7 | Excluir o cliente | `SELECT * FROM cliente;` — sumiu |

Fale em cada passo: "inseri / alterei / excluí, e aqui no banco está a prova".

(Opcional) rodar `./scripts/06-testes-crud.sh` mostrando os JSON de POST/PUT/GET/DELETE
da API com o SELECT após cada operação.

## Bloco 5 — Monitoramento (2 min)

Portal → `appi-564099-dimdim`:
- **Live Metrics** — clicar no site e mostrar as requisições chegando em tempo real
- **Application Map** — Web App ligado ao banco SQL
- **Performance** e **Logs**:
  ```kusto
  dependencies | where type == "SQL" | project timestamp, data, duration, success
  ```
  "cada INSERT/UPDATE/DELETE que fizemos aparece aqui"

Banco `dimdim` → **Métricas** → DTU / conexões bem-sucedidas.

## Bloco 6 — Fechamento (30 s)

> "Então: aplicação web .NET no App Service, deploy pelo CLI com az webapp deploy, dados
> no Azure SQL com duas tabelas relacionadas e CRUD completo, e monitoramento pelo
> Application Insights. Tudo está no GitHub com o how to. Obrigado!"

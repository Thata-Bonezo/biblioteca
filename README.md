# 📚 Minha Biblioteca - Biblioteca Pessoal de Livros

O **Minha Biblioteca** é um aplicativo mobile completo desenvolvido em **.NET MAUI** com **C#** e **XAML**, utilizando a arquitetura **MVVM** e banco de dados **SQLite**. O aplicativo foi projetado com foco na experiência do usuário para gerenciamento e acompanhamento de hábitos de leitura.

---

## 🎯 Objetivo

Oferecer um aplicativo mobile moderno, prático e totalmente funcional para organizar e acompanhar a vida literária do usuário. Permite o cadastro de livros, controle detalhado de progresso de leitura (atualização de páginas), atribuição de notas de 1 a 5 estrelas, elaboração de resenhas, acompanhamento de metas de leitura anuais e visualização de estatísticas completas da biblioteca, mantendo todos os dados salvos localmente no dispositivo via **SQLite**.

---

## 🚀 Funcionalidades

- **Lista e Filtro de Livros:**
  - Busca em tempo real por título, autor ou gênero literário.
  - Filtro rápido por status: *Todos*, *Quero ler*, *Lendo* e *Lido*.
  - Exibição de cards com progresso percentual, barra de progresso e nota em estrelas.
  
- **Cadastro e Edição de Livros:**
  - Validação estrita de campos obrigatórios (Título, Autor e Total de Páginas).
  - Impedimento de informar página atual negativa ou superior ao total de páginas.
  - Atualização automática de status com base na página atual informada:
    - Página = 0: `Quero ler`
    - 0 < Página < Total: `Lendo`
    - Página = Total: `Lido` (registra a data de conclusão).

- **Detalhes e Progresso de Leitura:**
  - Atualização direta de página atual com feedback ao usuário.
  - Registro de data de início/cadastro e data de conclusão automática.

- **Avaliação e Resenha:**
  - Atribuição de notas de 1 a 5 estrelas.
  - Campo dedicado para resenha e anotações literárias.

- **Exclusão com Confirmação:**
  - Diálogo de confirmação para evitar exclusões acidentais.

- **Metas Anuais de Leitura:**
  - Definição de meta de quantidade de livros para o ano selecionado.
  - Dashboard de progresso com barra indicadora, percentual e mensagem motivacional.

- **Painel de Estatísticas:**
  - Indicadores numéricos: Total de livros, Total de páginas lidas, Nota média e Livros concluídos no ano atual.
  - Distribuição gráfica/quantitativa de livros por status.

- **Carga Inicial (Seed Data):**
  - Inserção automática de livros e meta de exemplo apenas quando o banco estiver vazio na primeira inicialização.

---

## 🛠️ Tecnologias Utilizadas

- **.NET 9 / .NET 8 SDK**
- **.NET MAUI** (Alvo principal: Android; suporte cross-platform)
- **C# 12**
- **XAML** com Compiled Bindings (`x:DataType`)
- **Arquitetura MVVM** com `CommunityToolkit.Mvvm` (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`)
- **SQLite** (`sqlite-net-pcl` + `SQLitePCLRaw.bundle_green`)
- **Injeção de Dependência** nativa registrada no `MauiProgram.cs`
- **Navegação Shell** (`AppShell.xaml`)

---

## 📁 Estrutura de Pastas

```text
MinhaBiblioteca/
├── Converters/              # Conversores de valor para bindings em XAML
│   ├── StatusToColorConverter.cs
│   ├── RatingToStarsConverter.cs
│   └── InverseBoolConverter.cs
├── Data/                    # Configurações e constantes do banco SQLite
│   └── Constants.cs
├── Models/                  # Modelos de dados e entidades do SQLite
│   ├── Livro.cs
│   ├── MetaLeitura.cs
│   ├── StatusLivro.cs
│   └── EstatisticasModel.cs
├── Services/                # Serviços assíncronos e injeção de dependência
│   ├── IDatabaseService.cs
│   ├── DatabaseService.cs
│   ├── IDialogService.cs
│   └── DialogService.cs
├── ViewModels/              # Lógica de apresentação em padrão MVVM estrito
│   ├── LivrosViewModel.cs
│   ├── LivroFormViewModel.cs
│   ├── LivroDetalheViewModel.cs
│   ├── MetasViewModel.cs
│   └── EstatisticasViewModel.cs
├── Views/                   # Telas em XAML e Code-behind sem lógica de negócio
│   ├── LivrosPage.xaml (.cs)
│   ├── LivroFormPage.xaml (.cs)
│   ├── LivroDetalhePage.xaml (.cs)
│   ├── MetasPage.xaml (.cs)
│   └── EstatisticasPage.xaml (.cs)
├── App.xaml (.cs)           # Inicializador do app
├── AppShell.xaml (.cs)      # Configuração de abas e rotas
├── MauiProgram.cs           # Registro de serviços, ViewModels e Views no container de DI
├── MinhaBiblioteca.csproj   # Configuração do projeto e dependências NuGet
└── README.md                # Documentação completa do projeto
```

---

## ⚙️ Como Executar o Projeto

### Pré-requisitos
1. **.NET SDK 9.0 ou 8.0** instalado.
2. Carga de trabalho **.NET MAUI** instalada (`dotnet workload install maui`).
3. Para compilar para Android: Android SDK e Emulador Android (ou dispositivo físico em modo de depuração USB).

### Passos
1. **Clonar ou abrir o repositório:**
   ```bash
   cd c:\Users\195222024\Downloads\wilton\biblioteca
   ```

2. **Restaurar os pacotes NuGet:**
   ```bash
   dotnet restore
   ```

3. **Compilar e executar no Android:**
   ```bash
   dotnet build -f net8.0-android
   ```
   *Ou para rodar diretamente em um emulador/dispositivo ativo:*
   ```bash
   dotnet build -t:Run -f net8.0-android
   ```

4. **Executar no Windows (desktop):**
   ```bash
   dotnet build -f net8.0-windows10.0.19041.0
   ```

---

## 🤖 Nota de Geração por IA

> **Observação:** Este projeto foi totalmente gerado com auxílio de Inteligência Artificial a partir de um único prompt detalhado, seguindo as melhores práticas de arquitetura de software, padrão MVVM estrito, injeção de dependência e persistência com SQLite em .NET MAUI.

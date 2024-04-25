## WebDrive

É o projeto que cuida de toda parte de abstração de Selenium com algumas features aprimoradas para a facilidade de desenvolvimento de automações Web.

A classe **SeleniumTool** junta funções do Selenium para tarefa de manipulação web com a instancia de navegador publica por toda classe. Então cada instância da classe manipula 1 navegador.

Funções e parâmetros

### _StartNavigation_ abre o navegador.

```c#
  StartNavigation()

  StartNavigation(List<string> arguments)

  StartNavigation(string url)

  StartNavigation(TypeBrowser typeBrowser, string url)

  StartNavigation(string url, bool isHidden)

  StartNavigation(TypeBrowser typeBrowser , string url, bool isHidden)

	StartNavigation(TypeBrowser typeBrowser , string url, bool isHidden, string downloadPath)
```

```c#
  StartNavigation()

  StartNavigation(List<string> arguments)
  StartNavigation(string url)

  StartNavigation(TypeBrowser typeBrowser, string url)

  StartNavigation(string url, bool isHidden)

  StartNavigation(TypeBrowser typeBrowser , string url, bool isHidden)

	StartNavigation(TypeBrowser typeBrowser , string url, bool isHidden, string downloadPath)
```

- **TypeBrowser:** TypeBrowser é enum que determina qual navegador vai ser executado (IE, Edge, Firefox, Chrome).
- **url:** Qual url que vai iniciar junto ao navegador.
- **isHidden:** Se navegador vai ser oculto (true) ou visível (false).
- **downloadPath:** Diretório onde será baixado os downloads do navegador.
- **arguments:** Argumentos de configuração para ser enviado ao navegador Chrome.

### _GoToUrl_ direciona o navegador á url.

```c#
  GoToUrl(string url)
```

- **url:** url que vai executar no navegador.

### _GetElement_ retorna o elemento a uma variável IWebElement.

```c#
  GetElement(string elementLocalXpath)

  GetElement(By by)
```

- **elementLocalXpath**: Xpath do elemento.
- **by**: tipo de localizador do elemento junto a string de localização.

### _GetElements_ retorna uma lista elemento a uma variável IList<IWebElement>.

```c#
  GetElements(string elementLocalXpath)

  GetElements (By by)
```

- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.

### _GetElementSelect_ retorna elemento de seleção a uma variável SelectElement.

- **element:** elemento IWebElement.
- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.

```c#
  GoToUrl(string url)
```

GetElementSelect(IWebElement element)
GetElementSelect(string elementLocalXpath)
GetElementSelect(By by)

### _GetElementContains_ retorna um elemento que contenha tal texto a uma variável IWebElement.

- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.
- **text:** Texto a procurar.

```c#
  GetElementContains(string elementLocalXpath, string text)
  GetElementContains (By by, string text)
```

### _Click_ simula um click em um elemento.

```c#
  Click (IWebElement element)
  Click(string elementLocalXpath)
  Click (By by)
```

- **element:** elemento IWebElement.
- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.

### _ClickJs_ simula um evento de click em Java Script no elemento.

```c#
 ClickJs(string elementLocalXpath)
 ClickJs(By by)
```

- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.

### **SendKeys** envia caracteres a um elemento.

```c#
  SendKeys(IWebElement element, string text)
  SendKeys(string elementLocalXpath, string text)
  SendKeys(By by, string text)
```

- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.
- **text:** Texto a ser enviado.

### _SendKeysTime_ envia caracteres a um elemento com um tempo (milésimo) de espera a cada caractere.

```c#
  SendKeysTime(string elementLocalXpath, string text, int TimeSleep)
  SendKeysTime(By by, string text, int TimeSleep)
```

- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.
- **text:** Texto a ser enviado.

### _SetValue_ seta caracteres na value de um elemento.

```c#
  SetValue(IWebElement element, string text)
  SetValue(string elementLocalXpath, string text)
  SetValue(By by, string text)

```

- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.
- **text:** Texto a ser enviado.

### _AwaitElement_ espera o elemento ser carregado por padrão 15 segundos, retorna Boolean sobre a consulta.

```c#
 AwaitElement(string elementLocalXpath)
 AwaitElement (By by, string text)
 AwaitElement (string elementLocalXpath, int timeSeconse)
 AwaitElement (By by, string text, int timeSeconse)
```

- **elementLocalXpath:** Xpath do elemento.
- **by:** tipo de localizador do elemento junto a string de localização.
- **timeSeconse:** tempo da espera do elemento.
- **text:** leva em consideração se algum componente filho teve determinado texto.

### _SwitchToFrame_ altera a instância um frame da página.

- **nFrame:** Altera para número do frame da página.
- **elementFrame:** Altera pelo o IWebElement desejado.
- **nameFrame**: Altera pelo o nome do frame desejado.

```c#
  SwitchToFrame(int nFrame)
  SwitchToFrame(IWebElement elementFrame)
  SwitchToFrame(string nameFrame)
```

## SwitchToWindow altera instância a janela do navegador.

```c#
  SwitchToWindow() #Altera para próxima janela do navegador
  SwitchToWindow(string windowName)
```

- **windowName:** nome da janela para qual a automação vai ser instanciada.

### _TakeBrowserScreenshot_ tira um print da tela do navegador e salva em diretório e retorna o objeto FileInfo do arquivo.

```c#
  TakeBrowserScreenshot(string path, string fileName)
```

- **path:** Diretório de armazenamento do print.
- **fileName:** ”nome do arquivo” + extensão (.jpg, .png, etc..).

### _AcceptAndGetAlertMessage_ aceita o alerta e retorna sua mensagem.

```c#
  AcceptAndGetAlertMessage()
```

### _QuitDriver_ fecha a instância do navegador.

```c#
 QuitDriver()
```

### Atualização

Ainda há features ser a aprimoradas nese documento, estou trabalhando para uma documentação mais robusta.

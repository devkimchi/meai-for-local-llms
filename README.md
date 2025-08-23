# [Microsoft.Extensions.AI](https://learn.microsoft.com/dotnet/ai/microsoft-extensions-ai) for Local LLMs

This provides sample codes that uses Microsoft.Extensions.AI for locally running LLMs through [Docker Model Runner](https://docs.docker.com/ai/model-runner/), [Foundry Local](https://learn.microsoft.com/azure/ai-foundry/foundry-local/what-is-foundry-local), [Hugging Face](https://huggingface.co/) and [Ollama](https://ollama.com/).

> This is a trim-down version of [<img src="https://github.com/aliencube/open-chat-playground/raw/main/assets/icon-transparent.svg" alt="OpenChat Playground" width="16" /> OpenChat Playground](https://github.com/aliencube/open-chat-playground), specific to dealing with locally running LLMs. If you want to see more language models supported including MaaS and other vendors, try out [<img src="https://github.com/aliencube/open-chat-playground/raw/main/assets/icon-transparent.svg" alt="OpenChat Playground" width="16" /> OpenChat Playground](https://github.com/aliencube/open-chat-playground) instead.

## Prerequisites

- [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio Code](https://code.visualstudio.com/) + [C# DevKit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) or [Visual Studio 2022 v17.14+](https://visualstudio.com/vs)
- [GitHub CLI](https://cli.github.com/)
- [PowerShell 7.5+](https://learn.microsoft.com/powershell/scripting/install/installing-powershell) 👉 Windows only
- [Docker Desktop](https://docs.docker.com/desktop/)
- [Foundry Local](https://learn.microsoft.com/azure/ai-foundry/foundry-local/get-started)
- [Ollama](https://ollama.com/download)

## Getting Started

### Get the repository ready

1. Login to GitHub.

    ```bash
    gh auth login
    ```

1. Check login status.

    ```bash
    gh auth status
    ```

1. Fork this repository to your account and clone the forked repository to your local machine.

    ```bash
    gh repo fork devkimchi/meai-for-local-llms --clone --default-branch-only
    ```

1. Navigate to the cloned repository.

    ```bash
    cd meai-for-local-llms
    ```

1. Get the repository root.

    ```bash
    # bash/zsh
    REPOSITORY_ROOT=$(git rev-parse --show-toplevel)
    ```

    ```powershell
    # PowerShell
    $REPOSITORY_ROOT = git rev-parse --show-toplevel
    ```

### Use GitHub Models

As a default, this app uses [GitHub Models](https://github.com/marketplace?type=models).

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

2. Add GitHub Personal Access Token (PAT) for GitHub Models connection. Make sure you should replace `{{YOUR_TOKEN}}` with your GitHub PAT.

    ```bash
    # bash/zsh
    dotnet user-secrets --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        set GitHubModels:Token "{{YOUR_TOKEN}}"
    ```

    ```bash
    # PowerShell
    dotnet user-secrets --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        set GitHubModels:Token "{{YOUR_TOKEN}}"
    ```

    > For more details about GitHub PAT, refer to the doc, [Managing your personal access tokens](https://docs.github.com/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens).

3. Run the app. The default language model is `openai/gpt-4o-mini`.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp
    ```

   If you want to change the language model, add the `--model` option with a preferred model name. You can find the language model from the [GitHub Models catalog page](https://github.com/marketplace?type=models).

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type GitHubModels --model <model-name>
    ```

4. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

### Use Docker Model Runner

1. Make sure Docker Desktop is up and running.

    ```bash
    docker info
    ```

1. Download language model, `ai/gpt-oss`, to your local machine.

    ```bash
    docker model pull ai/gpt-oss
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app. The default language model is `ai/gpt-oss`.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type DockerModelRunner
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type DockerModelRunner --model <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

### Use Foundry Local

1. Make sure Foundry Local is up and running.

    ```bash
    foundry service status
    ```

   If Foundry Local service is not up and running, run the following command:

    ```bash
    foundry service start
    ```

1. Download language model, `gpt-oss-20b`, to your local machine.

    ```bash
    foundry model download gpt-oss-20b
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app. The default language model is `gpt-oss-20b`.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type FoundryLocal
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type FoundryLocal --model <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

### Use Hugging Face

Models from Hugging Face can be run through Ollama server.

1. Make sure Ollama is up and running.

    ```bash
    ollama start
    ```

1. In a separate terminal, download language model, `LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF`, to your local machine.

    ```bash
    ollama pull hf.co/LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app. The default language model is `hf.co/LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF`.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type HuggingFace
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type HuggingFace --model <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

### Use Ollama

1. Make sure Ollama is up and running.

    ```bash
    ollama start
    ```

1. In a separate terminal, download language model, `gpt-oss`, to your local machine.

    ```bash
    ollama pull gpt-oss
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app. The default language model is `gpt-oss`.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type Ollama
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp -- --connector-type Ollama --model <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.
